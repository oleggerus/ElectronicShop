//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ElectronicShop
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sale
    {
        public int Quantity { get; set; }
        public int SaleId { get; set; }
        public int CheckId { get; set; }
        public System.DateTime SaleDate { get; set; }
        public Nullable<int> StorehouseItemId { get; set; }
    
        public virtual Check Check { get; set; }
        public virtual StorehouseItem StorehouseItem { get; set; }
    }
}
