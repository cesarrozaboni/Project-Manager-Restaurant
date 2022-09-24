namespace Domain.Models
{
    public struct ItemDomain
    {
        public const string TABLE_NAME = "Item";

        public const string COLUMN_ITEM_ID = "ItemId";
        public const string COLUMN_ITEM_NAME = "ItemName";
        public const string COLUMN_ITEM_PRICE = "ItemPrice";
        public const string COLUMN_ITEM_CATEGORY = "ItemCategory";


        public static System.Data.DataTable CreateDataTable()
        {
            System.Data.DataTable dataTable = new System.Data.DataTable(TABLE_NAME);

            dataTable.Columns.Add(new System.Data.DataColumn(COLUMN_ITEM_ID, typeof(int)));
            dataTable.Columns.Add(new System.Data.DataColumn(COLUMN_ITEM_NAME, typeof(string)));
            dataTable.Columns.Add(new System.Data.DataColumn(COLUMN_ITEM_PRICE, typeof(decimal)));
            dataTable.Columns.Add(new System.Data.DataColumn(COLUMN_ITEM_CATEGORY, typeof(string)));
            dataTable.PrimaryKey = new System.Data.DataColumn[] { dataTable.Columns[COLUMN_ITEM_ID] };
            
            return dataTable;
        }

        public static System.Data.DataRow CreateDataRow(System.Data.DataTable dataTable, int id, string name, decimal price, string itemCategory)
        {
            System.Data.DataRow row = dataTable.NewRow();

            row[COLUMN_ITEM_ID] = id;
            row[COLUMN_ITEM_NAME] = name;
            row[COLUMN_ITEM_PRICE] = price;
            row[COLUMN_ITEM_CATEGORY] = itemCategory;

            return row;
        }

    }
}
