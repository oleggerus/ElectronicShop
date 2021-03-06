﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ShopContext : DbContext
    {
        public ShopContext()
            : base("name=ShopContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Adress> Adresses { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Check> Checks { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Consignment> Consignments { get; set; }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<CustomerDiscount> CustomerDiscounts { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }
        public virtual DbSet<DiscountToItem> DiscountToItems { get; set; }
        public virtual DbSet<EmployeeRole> EmployeeRoles { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Producer> Producers { get; set; }
        public virtual DbSet<Provider> Providers { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<StorehouseEmployee> StorehouseEmployees { get; set; }
        public virtual DbSet<StorehouseItem> StorehouseItems { get; set; }
        public virtual DbSet<Storehouse> Storehouses { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
    }
}
