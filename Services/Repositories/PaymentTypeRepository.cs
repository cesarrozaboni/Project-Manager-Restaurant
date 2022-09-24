using Services.Repositories.Base;
using Services.ServiceException;
using System;

namespace Services.Repositories
{
    public class PaymentTypeRepository : BaseRepository<Infra.PaymentTypeDal>
    {
        #region "Constructor
        /// <summary>
        /// Empty constructor
        /// </summary>
        public PaymentTypeRepository()
        {
        }
        #endregion

        #region "Get All payments"
        /// <summary>
        /// Service to Get all payment
        /// </summary>
        /// <returns>dataset with values</returns>
        /// <exception cref="IntegrityException">error</exception>
        public System.Threading.Tasks.Task<System.Data.DataSet> GetAllPaymentTypeAsync()
        {
            try
            {
                return System.Threading.Tasks.Task.FromResult(ObjDal.GetAllPaymentType());
            }
            catch(Exception ex)
            {
                throw new IntegrityException(ex.Message);
            }
        }
        #endregion

        #region "Insert new payment"
        /// <summary>
        /// Insert new payment in database
        /// </summary>
        /// <param name="ds">dataset with parameters</param>
        /// <exception cref="IntegrityException">error</exception>
        public async System.Threading.Tasks.Task InsertNewPaymentAsync(System.Data.DataSet ds)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() => ObjDal.InsertNewPayment(ds));
            }
            catch (Exception ex)
            {
                throw new IntegrityException(ex.Message);
            }
        }
        #endregion

        #region "Delete payment"
        /// <summary>
        /// Service to delete payment
        /// </summary>
        /// <param name="id">id to delete</param>
        /// <exception cref="IntegrityException">error</exception>
        public async System.Threading.Tasks.Task DeletePaymentAsync(int id)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() => ObjDal.DeletePayment(id));
            }
            catch (Exception ex)
            {
                throw new IntegrityException(ex.Message);
            }          
        }
        #endregion
    }
}