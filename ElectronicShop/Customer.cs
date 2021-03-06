﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel;

namespace ElectronicShop
{
    using System;
    using System.Collections.Generic;

    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            this.Checks = new HashSet<Check>();
        }

        [DisplayName("Прізвище покупця")]

        public string Surname { get; set; }

        [DisplayName("Ім'я")]
        public string Name { get; set; }
        [DisplayName("Дата реєстрації")]
        public System.DateTime RegistrationDate { get; set; }
        [DisplayName("Телефон")]
        public string Phone { get; set; }
        [DisplayName("Тип знижки")]
        public Nullable<int> CustDiscId { get; set; }
        [DisplayName("Номер покупця")]
        public int CustomerId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Check> Checks { get; set; }
        [DisplayName("Знижка (%)")]
        public virtual CustomerDiscount CustomerDiscount { get; set; }
    }
}
