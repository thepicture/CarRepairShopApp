﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CarRepairShopApp.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CarRepairShopBaseEntities : DbContext
    {
        public CarRepairShopBaseEntities()
            : base("name=CarRepairShopBaseEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Auto> Auto { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Contract> Contract { get; set; }
        public virtual DbSet<Master> Master { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Phone> Phone { get; set; }
        public virtual DbSet<PhotoOfService> PhotoOfService { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Service> Service { get; set; }
        public virtual DbSet<ServiceOfModel> ServiceOfModel { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TypeOfCar> TypeOfCar { get; set; }
        public virtual DbSet<User> User { get; set; }
    }
}
