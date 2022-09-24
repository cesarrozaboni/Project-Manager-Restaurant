using System;

namespace Domain.Models
{
    public struct OrderDomain
    {
        #region "Constantes"
        public const string TABLE_NAME = "Order";

        public const string COLUMN_ORDERS_ID              = "OrderId";
        public const string COLUMN_ORDERS_PAYMENT_TYPE_ID = "PaymentTypeId";
        public const string COLUMN_ORDERS_TABLE           = "TableNumber";
        public const string COLUMN_ORDERS_NUMBER          = "OrderNumber";
        public const string COLUMN_ORDERS_DATE            = "OrderDate";
        public const string COLUMN_ORDERS_FINAL_TOTAL     = "FinalTotal";
        public const string COLUMN_ORDERS_CUSTOMER        = "CustomerName";
        public const string COLUMN_ORDERS_STATUS          = "Status";
        #endregion

        #region "Create data table"
        public static System.Data.DataTable CreateDataTable()
        {
            System.Data.DataTable dataTable = new System.Data.DataTable(TABLE_NAME);

            dataTable.Columns.Add(new System.Data.DataColumn(COLUMN_ORDERS_ID,              typeof(int)));
            dataTable.Columns.Add(new System.Data.DataColumn(COLUMN_ORDERS_PAYMENT_TYPE_ID, typeof(int)));
            dataTable.Columns.Add(new System.Data.DataColumn(COLUMN_ORDERS_TABLE,           typeof(int)));
            dataTable.Columns.Add(new System.Data.DataColumn(COLUMN_ORDERS_NUMBER,          typeof(string)));
            dataTable.Columns.Add(new System.Data.DataColumn(COLUMN_ORDERS_DATE,            typeof(DateTime)));
            dataTable.Columns.Add(new System.Data.DataColumn(COLUMN_ORDERS_FINAL_TOTAL,     typeof(decimal)));
            dataTable.Columns.Add(new System.Data.DataColumn(COLUMN_ORDERS_CUSTOMER,        typeof(string)));
            dataTable.Columns.Add(new System.Data.DataColumn(COLUMN_ORDERS_STATUS,          typeof(int)));

            dataTable.PrimaryKey = new System.Data.DataColumn[] { dataTable.Columns[COLUMN_ORDERS_ID] };
            return dataTable;
        }
        #endregion

        #region "Create data row"
        public static System.Data.DataRow CreateDataRow(System.Data.DataTable dataTable, int id, int? paymentTypeId, int? table, string orderNumber,
                                                         DateTime? orderDate, decimal? finalTotal, string customer, int? status)
        {
            System.Data.DataRow row = dataTable.NewRow();

            row[COLUMN_ORDERS_ID]              = id;
            row[COLUMN_ORDERS_PAYMENT_TYPE_ID] = (object)paymentTypeId ?? DBNull.Value;
            row[COLUMN_ORDERS_TABLE]           = table;
            row[COLUMN_ORDERS_NUMBER]          = orderNumber;
            row[COLUMN_ORDERS_DATE]            = orderDate;
            row[COLUMN_ORDERS_FINAL_TOTAL]     = (object)finalTotal ?? DBNull.Value;
            row[COLUMN_ORDERS_CUSTOMER]        = customer;
            row[COLUMN_ORDERS_STATUS]          = status;

            return row;
        }
        #endregion

    }
}
