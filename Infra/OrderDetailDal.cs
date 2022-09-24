using System;
using System.Linq;

namespace Infra
{
    public class OrderDetailDal : Base.BaseInfra<DatabaseModel.RestauranteDBEntities>
    {
        #region "Construtor"
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderDetailDal()
        {
        }
        #endregion

        #region "Insert item in order"
        /// <summary>
        /// Insert item in order
        /// </summary>
        /// <param name="ds">dataset with values</param>
        public void InsertItemInOrder(System.Data.DataSet ds)
        {
            int order = (int)ds.Tables[0].Rows[0][Domain.Models.OrderDetailDomain.COLUMN_ORDER_DETAIL_ORDER_ID];
            DatabaseModel.Orders objOrder = RestauranteDb.Orders.SingleOrDefault(x => x.OrderId == order);

            DatabaseModel.OrderDetails objOrderDetail = new DatabaseModel.OrderDetails();
            objOrderDetail.Orders    = objOrder;
            objOrderDetail.Discount  = (decimal)ds.Tables[0].Rows[0][Domain.Models.OrderDetailDomain.COLUMN_ORDER_DETAIL_DISCOUNT];
            objOrderDetail.ItemId    = (int)ds.Tables[0].Rows[0][Domain.Models.OrderDetailDomain.COLUMN_ORDER_DETAIL_ITEM_ID];
            objOrderDetail.Total     = (decimal)ds.Tables[0].Rows[0][Domain.Models.OrderDetailDomain.COLUMN_ORDER_DETAIL_TOTAL];
            objOrderDetail.UnitPrice = (decimal)ds.Tables[0].Rows[0][Domain.Models.OrderDetailDomain.COLUMN_ORDER_DETAIL_UNIT_PRICE];
            objOrderDetail.Quantity  = (decimal)ds.Tables[0].Rows[0][Domain.Models.OrderDetailDomain.COLUMN_ORDER_DETAIL_QUANTITY];
                        
            RestauranteDb.OrderDetails.Add(objOrderDetail);
            RestauranteDb.SaveChanges();

            DatabaseModel.Transaction objTransaction = new DatabaseModel.Transaction();
            objTransaction.ItemId   = (int)ds.Tables[0].Rows[0][Domain.Models.OrderDetailDomain.COLUMN_ORDER_DETAIL_ITEM_ID];
            objTransaction.Quantity = (-1) * (decimal)ds.Tables[0].Rows[0][Domain.Models.OrderDetailDomain.COLUMN_ORDER_DETAIL_QUANTITY];
            objTransaction.TransactionDate = DateTime.Now;
            objTransaction.TypeId   = 2;
            RestauranteDb.Transactions.Add(objTransaction);
            RestauranteDb.SaveChanges();
        }
        #endregion

        #region "Get itens of order"
        /// <summary>
        /// Get itens from order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public System.Data.DataSet GetItensOfOrder(int orderId)
        {
            System.Collections.Generic.List<DatabaseModel.OrderDetails> result = RestauranteDb.OrderDetails.Where(x => x.Orders.OrderId == orderId && x.Orders.Status != 3).ToList();

            if (result.Count == 0)
                return null;

            System.Data.DataTable dt = Domain.Models.OrderDetailDomain.CreateDataTable();
            foreach (DatabaseModel.OrderDetails item in result)
            {
                string itemName = RestauranteDb.Items.SingleOrDefault(x => x.ItemId == item.ItemId).ItemName;
                System.Data.DataRow row = Domain.Models.OrderDetailDomain.CreateDataRow(dt, item.OrderDetailId, item.Orders.OrderId, item.ItemId,
                item.UnitPrice, item.Quantity, item.Discount, item.Total, itemName);

                dt.Rows.Add(row);
            }

            System.Data.DataSet ds = new System.Data.DataSet();
            ds.Tables.Add(dt);

            return ds;
        }
        #endregion

        #region "Remove item from order"
        /// <summary>
        /// Remove item from order
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderDetailId"></param>
        /// <exception cref="ArgumentException"></exception>
        public void RemoveItemFromOrder(int orderId, int orderDetailId)
        {
            DatabaseModel.Orders orders = new DatabaseModel.Orders();
            orders = RestauranteDb.Orders.SingleOrDefault(x => x.OrderId == orderId);

            var result = RestauranteDb.OrderDetails.SingleOrDefault(x => x.Orders.OrderId == orders.OrderId && x.OrderDetailId == orderDetailId);
            if (result == null)
                throw new ArgumentException("Não foi possivel remover o item informado");

            RestauranteDb.OrderDetails.Remove(result);
            RestauranteDb.SaveChanges();
        }
        #endregion
    }
}
