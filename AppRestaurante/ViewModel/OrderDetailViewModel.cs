using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppRestaurante.ViewModel
{
    [Serializable]
    public class OrderDetailViewModel
    {
        public int TableNumber { get; set; }
        public string CustomerName { get; set; }
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public string UnitPrice { get; set; }
        public string Quantity { get; set; }
        public string Discount { get; set; }
        public string Total { get; set; }
    }
}