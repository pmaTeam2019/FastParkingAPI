﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FastParkingAPI.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class FastParkingEntities : DbContext
    {
        public FastParkingEntities()
            : base("name=FastParkingEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CalculationType> CalculationTypes { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<OwnerCalculationType> OwnerCalculationTypes { get; set; }
        public virtual DbSet<OwnerPaymentType> OwnerPaymentTypes { get; set; }
        public virtual DbSet<Owner> Owners { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<PaymentType> PaymentTypes { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<Slot> Slots { get; set; }
    }
}
