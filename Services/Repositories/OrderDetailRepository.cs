using System;

namespace Services.Repositories
{
    public class OrderDetailRepository : Base.BaseRepository<Infra.OrderDetailDal>
    {
        #region "Constructor"
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderDetailRepository()
        {
        }
        #endregion

        #region "Insert item in order"
        /// <summary>
        /// Insert item in order
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        /// <exception cref="ServiceException.IntegrityException"></exception>
        public async System.Threading.Tasks.Task InsertItemInOrderAsync(System.Data.DataSet ds)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() => ObjDal.InsertItemInOrder(ds));
            }
            catch (Exception ex)
            {
                throw new ServiceException.IntegrityException(ex.Message);
            }
        }
        #endregion

        #region "Get itens of order"
        /// <summary>
        /// Get itens of order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <exception cref="ServiceException.IntegrityException"></exception>
        public async System.Threading.Tasks.Task<System.Data.DataSet> GetItensOfOrderAsync(int orderId)
        {
            try
            {
                var result = await System.Threading.Tasks.Task.FromResult(ObjDal.GetItensOfOrder(orderId));
                return result;
            }
            catch (Exception ex)
            {
                throw new ServiceException.IntegrityException(ex.Message);
            }
        }
        #endregion

        #region "Remove item from order"
        /// <summary>
        /// Remove item from order
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderDetailId"></param>
        /// <returns></returns>
        /// <exception cref="ServiceException.IntegrityException"></exception>
        public async System.Threading.Tasks.Task RemoveItemFromOrderAsync(int orderId, int orderDetailId)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() => ObjDal.RemoveItemFromOrder(orderId, orderDetailId));
            }
            catch (Exception ex)
            {
                throw new ServiceException.IntegrityException(ex.Message);
            }
        }
        #endregion
    }
}