//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Infra.DatabaseModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderDetails
    {
        public int OrderDetailId { get; set; }
        public int ItemId { get; set; }
        public decimal UnitPrice { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
    
        public virtual Orders Orders { get; set; }
    }
}
