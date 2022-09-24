using Services.ServiceException;
using System;
using System.Data;
using System.Linq;

namespace Services.Repositories
{
    public class ItemRepository : Base.BaseRepository<Infra.ItemDal>
    {
        #region "Constructor"
        /// <summary>
        /// constructor
        /// </summary>
        public ItemRepository()
        {
        }
        #endregion

        #region "Get Item by Id
        /// <summary>
        /// Service to Get Item by id
        /// </summary>
        /// <param name="id">id to search</param>
        /// <returns><see cref="DataSet"/> with values filled</returns>
        /// <exception cref="IntegrityException">error</exception>
        public async System.Threading.Tasks.Task<System.Data.DataSet> GetItemByIdAsync(int id)
        {
            try
            {
                return await System.Threading.Tasks.Task.FromResult(ObjDal.GetItemById(id));
            }
            catch (Exception ex)
            {
                throw new IntegrityException(ex.Message);
            }
        }
        #endregion

        #region "Insert new item"
        /// <summary>
        /// Repository to insert new item
        /// </summary>
        /// <param name="ds">dataset with parameters</param>
        /// <exception cref="IntegrityException">error</exception>
        public async System.Threading.Tasks.Task InsertNewItemAsync(System.Data.DataSet ds)
        {
            try 
            { 
                await System.Threading.Tasks.Task.Run(() => ObjDal.InsertNewItem(ds));
            }
            catch (Exception ex)
            {
                throw new IntegrityException(ex.Message);
            }
        }
        #endregion

        #region "Update item"
        /// <summary>
        /// Service to Update item
        /// </summary>
        /// <param name="ds">dataset with parameters</param>
        /// <exception cref="IntegrityException"></exception>
        public async System.Threading.Tasks.Task UpdateItemAsync(System.Data.DataSet ds)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() => ObjDal.UpdateItem(ds));
            }
            catch (Exception ex)
            {
                throw new IntegrityException(ex.Message);
            }
        }
        #endregion

        #region "delete item"
        /// <summary>
        /// Service to delete item
        /// </summary>
        /// <param name="id">id to delete</param>
        /// <exception cref="IntegrityException">error</exception>
        public async System.Threading.Tasks.Task DeleteItemAsync(int id)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() => ObjDal.DeleteItem(id));
            }
            catch (Exception ex)
            {
                throw new IntegrityException(ex.Message);
            }
        }
        #endregion

        #region "Get unit price"
        /// <summary>
        /// Get Unit price from item
        /// </summary>
        /// <param name="id">id to search</param>
        /// <returns>value decimal</returns>
        /// <exception cref="IntegrityException">error</exception>
        public async System.Threading.Tasks.Task<decimal> GetItemUnitPriceAsync(int id)
        {
            try
            {
                return await System.Threading.Tasks.Task.FromResult(ObjDal.GetItemUnitPrice(id));
            }
            catch (Exception ex)
            {
                throw new IntegrityException(ex.Message);
            }
        }
        #endregion

        #region "Get all itens"
        /// <summary>
        /// Get all itens
        /// </summary>
        /// <returns>List datarow</returns>
        public async System.Threading.Tasks.Task<System.Collections.Generic.List<System.Data.DataRow>> GetAllItemsAsync()
        {
            var result = await System.Threading.Tasks.Task.FromResult(
                ObjDal.GetAllItems().Tables[Domain.Models.ItemDomain.TABLE_NAME]
                .AsEnumerable()
                .OrderBy(x => x[Domain.Models.ItemDomain.COLUMN_ITEM_NAME])
                .ToList());

            return result;
        }
        #endregion
    }
}