//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class OwnerPaymentType
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int PaymentTypeId { get; set; }
        public Nullable<bool> ActiveField { get; set; }
    
        public virtual Owner Owner { get; set; }
        public virtual PaymentType PaymentType { get; set; }
    }
}