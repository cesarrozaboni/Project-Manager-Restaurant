using Infra.DatabaseModel;
using System;
using System.Linq;

namespace Infra
{
    public class ItemDal : Base.BaseInfra<RestauranteDBEntities>
    {
        #region "Constructor"
        /// <summary>
        /// constructor
        /// </summary>
        public ItemDal()
        {
        }
        #endregion

        #region "Get Item id"
        /// <summary>
        /// Get Item by id from database
        /// </summary>
        /// <param name="id">id to search</param>
        /// <returns><see cref="DataSet"/> with values filled</returns>
        /// <exception cref="ArgumentException"></exception>
        public System.Data.DataSet GetItemById(int id)
        {
            Items result = RestauranteDb.Items.SingleOrDefault(x => x.ItemId.Equals(id));
            
            if (result is null)
                throw new ArgumentException("O item solicitado não foi encontrado!");

            System.Data.DataTable dataTable = Domain.Models.ItemDomain.CreateDataTable();
            System.Data.DataRow row         = Domain.Models.ItemDomain.CreateDataRow(dataTable, result.ItemId, result.ItemName, result.ItemPrice, result.ItemCategory);
            dataTable.Rows.Add(row);

            System.Data.DataSet dataSet = new System.Data.DataSet();
            dataSet.Tables.Add(dataTable);

            return dataSet;
        }
        #endregion

        #region "Get all items
        /// <summary>
        /// Get all itens from database
        /// </summary>
        /// <returns>dataset with values</returns>
        public System.Data.DataSet GetAllItems()
        {
            System.Data.DataTable dataTable = Domain.Models.ItemDomain.CreateDataTable();

            foreach (Items result in RestauranteDb.Items)
            {
                System.Data.DataRow row = Domain.Models.ItemDomain.CreateDataRow(dataTable, result.ItemId, result.ItemName, result.ItemPrice, result.ItemCategory);
                dataTable.Rows.Add(row);
            }

            System.Data.DataSet ds = new System.Data.DataSet();
            ds.Tables.Add(dataTable);

            return ds;
        }
        #endregion

        #region "Insert new item"
        /// <summary>
        /// Insert new item in database
        /// </summary>
        /// <param name="ds">dataset with parametes</param>
        /// <exception cref="InvalidOperationException">error</exception>
        public void InsertNewItem(System.Data.DataSet ds)
        {
            int id          = (int) ds.Tables[Domain.Models.ItemDomain.TABLE_NAME].Rows[0][Domain.Models.ItemDomain.COLUMN_ITEM_ID];
            string name     = (string)ds.Tables[Domain.Models.ItemDomain.TABLE_NAME].Rows[0][Domain.Models.ItemDomain.COLUMN_ITEM_NAME];
            decimal price   = (decimal)ds.Tables[Domain.Models.ItemDomain.TABLE_NAME].Rows[0][Domain.Models.ItemDomain.COLUMN_ITEM_PRICE];
            string category = (string)ds.Tables[Domain.Models.ItemDomain.TABLE_NAME].Rows[0][Domain.Models.ItemDomain.COLUMN_ITEM_CATEGORY];

            if (RestauranteDb.Items.Any(x => x.ItemName == name))
                throw new InvalidOperationException("O Produto informado já foi cadastrado");

            Items item = new Items();
            item.ItemName     = name;
            item.ItemPrice    = price;
            item.ItemId       = id;
            item.ItemCategory = category;
            RestauranteDb.Items.Add(item);
            RestauranteDb.SaveChanges();
        }
        #endregion

        #region "Update item"
        /// <summary>
        /// Update item in dataBase
        /// </summary>
        /// <param name="ds">dataset with parameters</param>
        /// <exception cref="ArgumentException">error</exception>
        public void UpdateItem(System.Data.DataSet ds)
        {
            int id          = (int) ds.Tables[Domain.Models.ItemDomain.TABLE_NAME].Rows[0][Domain.Models.ItemDomain.COLUMN_ITEM_ID];
            string name     = (string)ds.Tables[Domain.Models.ItemDomain.TABLE_NAME].Rows[0][Domain.Models.ItemDomain.COLUMN_ITEM_NAME];
            decimal price   = (decimal)ds.Tables[Domain.Models.ItemDomain.TABLE_NAME].Rows[0][Domain.Models.ItemDomain.COLUMN_ITEM_PRICE];
            string category = (string)ds.Tables[Domain.Models.ItemDomain.TABLE_NAME].Rows[0][Domain.Models.ItemDomain.COLUMN_ITEM_CATEGORY];

            Items result = RestauranteDb.Items.SingleOrDefault(x => x.ItemId == id);

            if (result == null)
                throw new ArgumentException("Produto não encontrado!");
            
            result.ItemName = name;
            result.ItemPrice = price;
            result.ItemCategory = category;
            RestauranteDb.SaveChanges();
        }
        #endregion

        #region "Delete item"
        /// <summary>
        /// Delete item from dataBase
        /// </summary>
        /// <param name="id">id to delete</param>
        /// <exception cref="ArgumentException">error</exception>
        public void DeleteItem(int id)
        {
            Items result = RestauranteDb.Items.SingleOrDefault(x => x.ItemId == id);
            if (result == null)
                throw new ArgumentException("Produto não encontrado!");

            RestauranteDb.Items.Remove(result);
            RestauranteDb.SaveChanges();
        }
        #endregion

        #region "Get unit price"
        /// <summary>
        /// Get price from item
        /// </summary>
        /// <param name="itemId">id to search</param>
        /// <returns></returns>
        public decimal GetItemUnitPrice(int itemId)
        {
            decimal unitPrice = RestauranteDb.Items.Single(model => model.ItemId == itemId).ItemPrice;
            return unitPrice;
        }
        #endregion

    }
}
