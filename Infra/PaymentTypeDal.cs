using Infra.Base;
using Infra.DatabaseModel;
using System;
using System.Linq;

namespace Infra
{
    public class PaymentTypeDal : BaseInfra<RestauranteDBEntities>
    {
        #region "Constructor
        /// <summary>
        /// constructor
        /// </summary>
        public PaymentTypeDal()
        {
        }
        #endregion

        #region "Get All payments"
        /// <summary>
        /// Get all payment from database
        /// </summary>
        /// <returns>dataset with values</returns>
        public System.Data.DataSet GetAllPaymentType()
        {
            System.Data.DataTable dataTable = Domain.Models.PaymentTypeDomain.CreateDataTable();
            
            foreach(var item in RestauranteDb.PaymentTypes)
            {
                System.Data.DataRow row = Domain.Models.PaymentTypeDomain.CreateDataRow(dataTable, item.PaymentTypeId, item.PaymentTypeName);
                dataTable.Rows.Add(row);
            }

            System.Data.DataSet dataSet = new System.Data.DataSet();
            dataSet.Tables.Add(dataTable);

            return dataSet;
        }
        #endregion

        #region "Insert new payment"
        /// <summary>
        /// Insert new payment in database
        /// </summary>
        /// <param name="ds">dataset with parameters</param>
        /// <exception cref="InvalidOperationException"></exception>
        public void InsertNewPayment(System.Data.DataSet ds)
        {
            int paymentId      = (int)ds.Tables[Domain.Models.PaymentTypeDomain.TABLE_NAME].Rows[0][Domain.Models.PaymentTypeDomain.COLUMN_PAYMENTE_ID];
            string paymentName = (string)ds.Tables[Domain.Models.PaymentTypeDomain.TABLE_NAME].Rows[0][Domain.Models.PaymentTypeDomain.COLUMN_PAYMENTE_NAME];

            PaymentType result = RestauranteDb.PaymentTypes.SingleOrDefault(x => x.PaymentTypeName.ToLower() == paymentName.ToLower());
            
            if (result != null)
                throw new InvalidOperationException($"O Pagamento { result.PaymentTypeName } ja foi cadastrado!");

            PaymentType payment = new PaymentType();
            payment.PaymentTypeName = paymentName;

            RestauranteDb.PaymentTypes.Add(payment);
            RestauranteDb.SaveChanges();
        }
        #endregion

        #region "Delete payment"
        /// <summary>
        /// Delete payment from database
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="ArgumentException"></exception>
        public void DeletePayment(int id)
        {
            PaymentType payment = RestauranteDb.PaymentTypes.SingleOrDefault(x => x.PaymentTypeId == id);

            if (payment == null)
                throw new ArgumentException("Modelo de pagamento não encontrado!");

            RestauranteDb.PaymentTypes.Remove(payment);
            RestauranteDb.SaveChanges();
        }
        #endregion
    }
}
