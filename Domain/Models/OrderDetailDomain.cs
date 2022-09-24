using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public struct OrderDetailDomain
    {
       
        public const string TABLE_NAME = "OrderDetail";

        public const string COLUMN_ORDER_DETAIL_ID = "OrderDetailId";
        public const string COLUMN_ORDER_DETAIL_ORDER_ID = "OrderId";
        public const string COLUMN_ORDER_DETAIL_ITEM_ID = "ItemId";
        public const string COLUMN_ORDER_DETAIL_ITEM_NAME = "ItemName";
        public const string COLUMN_ORDER_DETAIL_UNIT_PRICE = "UnitPrice";
        public const string COLUMN_ORDER_DETAIL_QUANTITY = "Quantity";
        public const string COLUMN_ORDER_DETAIL_DISCOUNT = "Discount";
        public const string COLUMN_ORDER_DETAIL_TOTAL = "Total";
        

        public static System.Data.DataTable CreateDataTable()
        {
            System.Data.DataTable dataTable = new System.Data.DataTable(TABLE_NAME);

            dataTable.Columns.Add(new System.Data.DataColumn(COLUMN_ORDER_DETAIL_ID, typeof(int)));
            dataTable.Columns.Add(new System.Data.DataColumn(COLUMN_ORDER_DETAIL_ORDER_ID, typeof(int)));
            dataTable.Columns.Add(new System.Data.DataColumn(COLUMN_ORDER_DETAIL_ITEM_ID, typeof(int)));
            dataTable.Columns.Add(new System.Data.DataColumn(COLUMN_ORDER_DETAIL_UNIT_PRICE, typeof(decimal)));
            dataTable.Columns.Add(new System.Data.DataColumn(COLUMN_ORDER_DETAIL_QUANTITY, typeof(decimal)));
            dataTable.Columns.Add(new System.Data.DataColumn(COLUMN_ORDER_DETAIL_DISCOUNT, typeof(decimal)));
            dataTable.Columns.Add(new System.Data.DataColumn(COLUMN_ORDER_DETAIL_TOTAL, typeof(decimal)));
            dataTable.Columns.Add(new System.Data.DataColumn(COLUMN_ORDER_DETAIL_ITEM_NAME, typeof(string)));
            //dataTable.PrimaryKey = new System.Data.DataColumn[] { dataTable.Columns[COLUMN_ORDER_DETAIL_ID] };

            return dataTable;
        }

        public static System.Data.DataRow CreateDataRow(System.Data.DataTable dataTable, int? id, int orderId, int itemId, decimal unitPrice,
            decimal? quantity, decimal discount, decimal total, string itemName = null)
        {
            System.Data.DataRow row = dataTable.NewRow();

            row[COLUMN_ORDER_DETAIL_ID] = (object) id ?? DBNull.Value;
            row[COLUMN_ORDER_DETAIL_ORDER_ID] = orderId;
            row[COLUMN_ORDER_DETAIL_ITEM_ID] = itemId;
            row[COLUMN_ORDER_DETAIL_UNIT_PRICE] = unitPrice;
            row[COLUMN_ORDER_DETAIL_QUANTITY] = quantity;
            row[COLUMN_ORDER_DETAIL_DISCOUNT] = discount;
            row[COLUMN_ORDER_DETAIL_TOTAL] = total;
            row[COLUMN_ORDER_DETAIL_ITEM_NAME] = (object)itemName ?? DBNull.Value;

            return row;
        }
    }
}
