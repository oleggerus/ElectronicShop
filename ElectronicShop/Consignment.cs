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
    
    public partial class Consignment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Consignment()
        {
            this.StorehouseItems = new HashSet<StorehouseItem>();
        }
    
        public int Quantity { get; set; }
        public int ProviderId { get; set; }
        public int ItemId { get; set; }
        public double ProviderPrice { get; set; }
        public Nullable<System.DateTime> ArriveDate { get; set; }
        public int ConsignmentId { get; set; }
    
        public virtual Provider Provider { get; set; }
        public virtual Item Item { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StorehouseItem> StorehouseItems { get; set; }
    }
}