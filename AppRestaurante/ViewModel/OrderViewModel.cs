using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppRestaurante.ViewModel
{
    public class OrderViewModel
    {
        public int TableNumber { get; set; }
        public int OrderId { get; set; }
        public int PaymentTypeId { get; set; }
        public string CustomerName { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        public decimal FinalTotal { get; set; }
        public IEnumerable<OrderDetailViewModel> ListOfOrderDetailViewModel { get; set; }
    }
}