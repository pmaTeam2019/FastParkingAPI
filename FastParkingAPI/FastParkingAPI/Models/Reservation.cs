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
    
    public partial class Reservation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reservation()
        {
            this.Payments = new HashSet<Payment>();
        }
    
        public int Id { get; set; }
        public Nullable<int> SlotId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> OwnerId { get; set; }
        public Nullable<System.DateTime> StartReservationDate { get; set; }
        public Nullable<System.DateTime> EndReservationDate { get; set; }
        public Nullable<bool> isActive { get; set; }
        public bool ActiveField { get; set; }
    
        public virtual Customer Customer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual Slot Slot { get; set; }
    }
}
