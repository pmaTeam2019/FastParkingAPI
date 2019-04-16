using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FastParkingAPI.Dtos
{
    public class OwnerPaymentTypeDto
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        [Required]
        public int PaymentTypeId { get; set; }
        public Nullable<bool> ActiveField { get; set; }
        public virtual OwnerDto Owner { get; set; }
        public virtual PaymentTypeDto PaymentType { get; set; }
    }
}