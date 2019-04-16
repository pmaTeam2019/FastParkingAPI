using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FastParkingAPI.Dtos
{
    public class OwnerCalculationTypeDto
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        [Required]
        public int CalculationTypeId { get; set; }
        public Nullable<bool> ActiveField { get; set; }
        [Required]
        public Nullable<double> Price { get; set; }

        public virtual CalculationTypeDto CalculationType { get; set; }
        public virtual OwnerDto Owner { get; set; }
    }
}