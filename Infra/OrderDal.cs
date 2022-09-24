using System;
using System.Data;
using System.Linq;

namespace Infra
{
    public class OrderDal : Base.BaseInfra<DatabaseModel.RestauranteDBEntities>
    {
        #region "Constructor"
        public OrderDal()
        {
        }
        #endregion

        #region "Get order from table"
        /// <summary>
        /// Get Order by table
        /// </summary>
        /// <param name="table">number of table</param>
        /// <returns>dataSet with values or null</returns>
        public System.Data.DataSet GetOrderByTable(int table)
        {
            DatabaseModel.Orders order =  RestauranteDb.Orders.SingleOrDefault(x => x.TableNumber == table && x.Status != 3);

            if (order == null)
                return null;

            System.Data.DataTable dataTable = Domain.Models.OrderDomain.CreateDataTable();
            System.Data.DataRow row = Domain.Models.OrderDomain.CreateDataRow(dataTable, order.OrderId, 
                                                                             order.PaymentTypeId, 
                                                                             order.TableNumber,
                                                                             order.OrderNumber, 
                                                                             order.OrderDate, 
                                                                             order.FinalTotal, 
                                                                             order.CustomerName, 
                                                                             order.Status);

            dataTable.Rows.Add(row);

            System.Data.DataSet dataSet = new System.Data.DataSet();
            dataSet.Tables.Add(dataTable);

            return dataSet;
        }
        #endregion

        #region "Create reserve"
        /// <summary>
        /// Create new reserve
        /// </summary>
        /// <param name="ds">dataSet with values</param>
        /// <exception cref="InvalidOperationException"></exception>
        public void CreateReserve(System.Data.DataSet ds)
        {
            int table       = (int)    ds.Tables[Domain.Models.OrderDomain.TABLE_NAME].Rows[0][Domain.Models.OrderDomain.COLUMN_ORDERS_TABLE];
            string customer = (string) ds.Tables[Domain.Models.OrderDomain.TABLE_NAME].Rows[0][Domain.Models.OrderDomain.COLUMN_ORDERS_CUSTOMER];

            var result = RestauranteDb.Orders.SingleOrDefault(x => x.TableNumber == table && x.Status == 0);

            if (result != null)
                throw new InvalidOperationException("A mesa não esta disponivel");

            ds.Tables[Domain.Models.OrderDomain.TABLE_NAME].Rows[0][Domain.Models.OrderDomain.COLUMN_ORDERS_STATUS] = 1;
            AddOrder(ds);
        }
        #endregion

        #region "Cancel reservation"
        /// <summary>
        /// Cancel reservation from table
        /// </summary>
        /// <param name="table">number from table to cancel</param>
        /// <exception cref="InvalidOperationException"></exception>
        public void DeleteReserve(int table)
        {
            var result = RestauranteDb.Orders.SingleOrDefault(x => x.TableNumber == table && x.Status == 1);

            if (result == null || result.TableNumber == null)
                throw new InvalidOperationException("A mesa não esta reservada");

            RestauranteDb.Orders.Remove(result);
            RestauranteDb.SaveChanges();
        }
        #endregion

        #region "Add order"
        /// <summary>
        /// Add new order in database
        /// </summary>
        /// <param name="ds"></param>
        /// <returns>order id</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public int AddOrder(DataSet ds)
        {
            int table       = (int)    ds.Tables[0].Rows[0][Domain.Models.OrderDomain.COLUMN_ORDERS_TABLE];
            string customer = (string) ds.Tables[0].Rows[0][Domain.Models.OrderDomain.COLUMN_ORDERS_CUSTOMER];

            var result = RestauranteDb.Orders.SingleOrDefault(x => x.TableNumber == table && x.Status == 0);

            if (result != null)
                throw new InvalidOperationException("A mesa não esta disponivel");

            DatabaseModel.Orders objOrder = new DatabaseModel.Orders();
            objOrder.CustomerName = customer;
            objOrder.OrderDate    = DateTime.Now;
            objOrder.OrderNumber  = string.Format("{0:ddmmmyyyyhhmmss}", DateTime.Now);
            objOrder.TableNumber  = table;
            objOrder.Status       = (int)ds.Tables[0].Rows[0][Domain.Models.OrderDomain.COLUMN_ORDERS_STATUS];

            RestauranteDb.Orders.Add(objOrder);
            RestauranteDb.SaveChanges();
            
            return objOrder.OrderId;
        }
        #endregion

        #region "Final payment"
        /// <summary>
        /// Final payment
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="paymentType"></param>
        /// <param name="payment"></param>
        /// <exception cref="ArgumentException"></exception>
        public void FinalPayment(int orderId, int paymentType, decimal payment)
        {
            DatabaseModel.Orders order = RestauranteDb.Orders.SingleOrDefault(x => x.OrderId == orderId);
            if (order == null)
                throw new ArgumentException("Pedido não encontrado para realizar pagamento!");

            order.FinalTotal = payment;
            order.Status = 3;
            order.PaymentTypeId = paymentType;

            RestauranteDb.SaveChanges();
        }
        #endregion
    }
}
