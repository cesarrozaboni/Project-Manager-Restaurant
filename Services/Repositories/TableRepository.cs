using Infra;
using Services.ServiceException;
using System;

namespace Services.Repositories
{
    public class TableRepository : Base.BaseRepository<TableDal>
    {
        #region "Constructor"
        /// <summary>
        /// Constructor
        /// </summary>
        public TableRepository()
        {
        }
        #endregion

        #region "Get All tables"
        /// <summary>
        /// Service to Get All table
        /// </summary>
        /// <returns>dataset with values</returns>
        /// <exception cref="IntegrityException">error</exception>
        public async System.Threading.Tasks.Task<System.Data.DataSet> GetAllTableAsync()
        {
            try
            {
                return await System.Threading.Tasks.Task.FromResult(ObjDal.GetAllTable());
            }
            catch (Exception ex)
            {
                throw new IntegrityException(ex.Message);
            }
        }
        #endregion

        #region "Insert new table"
        /// <summary>
        /// Service to Insert new table
        /// </summary>
        /// <param name="dataSet">dataSet with parameters</param>
        /// <exception cref="IntegrityException">error</exception>
        public async System.Threading.Tasks.Task InsertNewTableAsync(System.Data.DataSet dataSet)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() => ObjDal.InsertNewTable(dataSet));
            }
            catch (Exception ex)
            {
                throw new IntegrityException(ex.Message);
            }
        }
        #endregion

        #region "Delete table"
        /// <summary>
        /// Delete table from dataBase
        /// </summary>
        /// <param name="id">id to delete</param>
        /// <exception cref="IntegrityException">error</exception>
        public async System.Threading.Tasks.Task DeleteTableAsync(int id)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() => ObjDal.DeleteTable(id));
            }
            catch (Exception ex)
            {
                throw new IntegrityException(ex.Message);
            }
        }
        #endregion
    }
}