using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FastParkingAPI.Dtos
{
    public class OwnerDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }
        [Required]
        [MaxLength(250)]
        public string Address { get; set; }
        public Nullable<int> SlotsQuantity { get; set; }
        public Nullable<bool> isAvailable { get; set; }
        [Required]
        [MaxLength(11)]
        public string Ruc { get; set; }
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Birthday { get; set; }
        public string Description { get; set; }
        [Required]
        [MaxLength(200)]
        public string Email { get; set; }
        [Required]
        [MaxLength(200)]
        public string Password { get; set; }
    }
}