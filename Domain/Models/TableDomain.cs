namespace Domain.Models
{
    public struct TableDomain
    {
        public const string TABLE_NAME               = "Table_Dinner";
        public const string COLUMN_TABLE_ID          = "TableId";
        public const string COLUMN_TABLE_DESCRIPTION = "TableDescription";
        

        public static System.Data.DataTable CreateDataTable()
        {
            System.Data.DataTable dataTable = new System.Data.DataTable(TABLE_NAME);
            dataTable.Columns.Add(new System.Data.DataColumn(COLUMN_TABLE_ID, typeof(int)));
            dataTable.Columns.Add(new System.Data.DataColumn(COLUMN_TABLE_DESCRIPTION, typeof(string)));

            return dataTable;
        }

        public static System.Data.DataRow CreateDataRow(System.Data.DataTable table, int id, string description)
        {
            System.Data.DataRow row = table.NewRow();
            row[COLUMN_TABLE_ID] = id;
            row[COLUMN_TABLE_DESCRIPTION] = description;

            return row;
        }
    }

    
}
