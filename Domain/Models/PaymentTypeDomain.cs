namespace Domain.Models
{
    public struct PaymentTypeDomain
    {
        #region "Constantes"
        public const string TABLE_NAME = "TablePayment";

        public const string COLUMN_PAYMENTE_NAME = "PaymentTypeName";
        public const string COLUMN_PAYMENTE_ID = "PaymentTypeId";
        #endregion

        #region "Create DataTable"
        public static System.Data.DataTable CreateDataTable()
        {
            System.Data.DataTable dataTable = new System.Data.DataTable(TABLE_NAME);
            dataTable.Columns.Add(new System.Data.DataColumn(COLUMN_PAYMENTE_ID, typeof(int)));
            dataTable.Columns.Add(new System.Data.DataColumn(COLUMN_PAYMENTE_NAME, typeof(string)));

            return dataTable;
        }
        #endregion

        #region "Create DataRow"
        public static System.Data.DataRow CreateDataRow(System.Data.DataTable table, int paymentId, string paymentName)
        {
            System.Data.DataRow row = table.NewRow();

            row[COLUMN_PAYMENTE_ID]   = paymentId;
            row[COLUMN_PAYMENTE_NAME] = paymentName;

            return row;
        }
        #endregion
    }
}
