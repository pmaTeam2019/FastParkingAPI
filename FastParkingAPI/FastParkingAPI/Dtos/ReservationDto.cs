using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FastParkingAPI.Dtos
{
    public class ReservationDto
    {
        public int Id { get; set; }
        [Required]
        public Nullable<int> SlotId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        [Required]
        public Nullable<int> OwnerId { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public Nullable<System.DateTime> StartReservationDate { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public Nullable<System.DateTime> EndReservationDate { get; set; }
        public Nullable<bool> isActive { get; set; }
        public bool ActiveField { get; set; }
        public virtual SlotDto Slot { get; set; }
    }
}