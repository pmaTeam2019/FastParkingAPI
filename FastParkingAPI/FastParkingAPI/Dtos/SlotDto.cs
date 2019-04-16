using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FastParkingAPI.Dtos
{
    public class SlotDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Identifier { get; set; }
        public int OwnerId { get; set; }
        [Required]
        public string Description { get; set; }
        public Nullable<bool> isAvailable { get; set; }
        public Nullable<bool> ActiveField { get; set; }
    }
}