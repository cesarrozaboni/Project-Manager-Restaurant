using System;

namespace Services.Repositories
{
    public class OrderRepository : Base.BaseRepository<Infra.OrderDal>
    {
        public OrderRepository()
        {
        }

        #region "Get order from table"
        /// <summary>
        /// Return current order from table
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        /// <exception cref="ServiceException.IntegrityException"></exception>
        public async System.Threading.Tasks.Task<System.Data.DataSet> GetOrderByTableAsync(int table)
        {
            try
            {
                var result = await System.Threading.Tasks.Task.FromResult(ObjDal.GetOrderByTable(table));
                return result;
            }
            catch (Exception ex)
            {
                throw new ServiceException.IntegrityException(ex.Message);
            }
            
        }
        #endregion

        #region "Add Order"
        /// <summary>
        /// Service to add new order
        /// </summary>
        /// <param name="ds">dataset with params</param>
        /// <returns>id from order</returns>
        /// <exception cref="ServiceException.IntegrityException"></exception>
        public async System.Threading.Tasks.Task<int> AddOrderAsync(System.Data.DataSet ds)
        {
            try
            {
                var result = await System.Threading.Tasks.Task.FromResult(ObjDal.AddOrder(ds));
                return result;
            }
            catch (Exception ex)
            {
                throw new ServiceException.IntegrityException(ex.Message);
            }           
        }
        #endregion

        #region "Create reserve"
        /// <summary>
        /// Create new reserve in table
        /// </summary>
        /// <param name="ds">dataset with params</param>
        /// <returns></returns>
        /// <exception cref="ServiceException.IntegrityException"></exception>
        public async System.Threading.Tasks.Task CreateReserveAsync(System.Data.DataSet ds)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() => ObjDal.CreateReserve(ds));
            }
            catch (Exception ex)
            {
                throw new ServiceException.IntegrityException(ex.Message);
            }
        }
        #endregion

        #region "Delete Reserve"
        /// <summary>
        /// Delete reserve from table
        /// </summary>
        /// <param name="table">number of table</param>
        /// <returns></returns>
        /// <exception cref="ServiceException.IntegrityException"></exception>
        public async System.Threading.Tasks.Task DeleteReserveAsync(int table)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() => ObjDal.DeleteReserve(table));
            }
            catch (Exception ex)
            {
                throw new ServiceException.IntegrityException(ex.Message);
            }
        }
        #endregion

        #region "Final Payment"
        /// <summary>
        /// Final payment
        /// </summary>
        /// <param name="table">number of table</param>
        /// <returns></returns>
        /// <exception cref="ServiceException.IntegrityException"></exception>
        public async System.Threading.Tasks.Task FinalPaymentAsync(int orderId, int paymentTypeId, decimal payment)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() => ObjDal.FinalPayment(orderId, paymentTypeId, payment));
            }
            catch (Exception ex)
            {
                throw new ServiceException.IntegrityException(ex.Message);
            }
        }
        #endregion
    }
}