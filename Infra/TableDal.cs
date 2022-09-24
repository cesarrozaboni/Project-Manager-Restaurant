using System;
using System.Linq;

namespace Infra
{
    public class TableDal : Base.BaseInfra<Infra.DatabaseModel.RestauranteDBEntities>
    {

        #region "Constructor"
        /// <summary>
        /// Constructor
        /// </summary>
        public TableDal()
        {
        }
        #endregion

        #region "Get All Tables"
        /// <summary>
        /// Get All Dinner Tables
        /// </summary>
        /// <returns>dataset with values</returns>
        public System.Data.DataSet GetAllTable()
        {
            System.Data.DataTable dataTable = Domain.Models.TableDomain.CreateDataTable();

            foreach (Infra.DatabaseModel.DinnerTable item in RestauranteDb.DinnerTables)
            {
                System.Data.DataRow row = Domain.Models.TableDomain.CreateDataRow(dataTable, item.TableId, item.TableDescription);
                dataTable.Rows.Add(row);
            }
                
            
            System.Data.DataSet ds = new System.Data.DataSet();
            ds.Tables.Add(dataTable);

            return ds;
        }
        #endregion

        #region "Insert new table"
        /// <summary>
        /// Insert new table in database
        /// </summary>
        /// <param name="dataSet">dataset with parameters</param>
        /// <exception cref="InvalidOperationException">errors</exception>
        public void InsertNewTable(System.Data.DataSet dataSet)
        {
            int id             = (int) dataSet.Tables[Domain.Models.TableDomain.TABLE_NAME].Rows[0][Domain.Models.TableDomain.COLUMN_TABLE_ID];
            string description = (string)dataSet.Tables[Domain.Models.TableDomain.TABLE_NAME].Rows[0][Domain.Models.TableDomain.COLUMN_TABLE_DESCRIPTION];

            Infra.DatabaseModel.DinnerTable result = RestauranteDb.DinnerTables.SingleOrDefault(x => x.TableDescription.ToLower() == description.ToLower());

            if (result != null)
                throw new InvalidOperationException($"A mesa { description } ja foi cadastrada!");

            Infra.DatabaseModel.DinnerTable table = new Infra.DatabaseModel.DinnerTable();
            table.TableDescription = description;

            RestauranteDb.DinnerTables.Add(table);
            RestauranteDb.SaveChanges();
        }
        #endregion

        #region "Delete table"
        /// <summary>
        /// Delete table from database
        /// </summary>
        /// <param name="id">id to delete from database</param>
        /// <exception cref="ArgumentException">error</exception>
        public void DeleteTable(int id)
        {
            Infra.DatabaseModel.DinnerTable table = RestauranteDb.DinnerTables.SingleOrDefault(x => x.TableId == id);

            if (table == null)
                throw new ArgumentException("Mesa não encontrada!");

            RestauranteDb.DinnerTables.Remove(table);
            RestauranteDb.SaveChanges();
        }
        #endregion
    
    }
}
