using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FastParkingAPI.Dtos
{

    public class CustomerDto
    {

        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(200)]
        public string Address { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Birthday { get; set; }
        [MaxLength(8)]
        public string Dni { get; set; }
        [MaxLength(11)]
        public string Ruc { get; set; }
        [MaxLength(100)]
        public string FullName { get; set; }
        public bool ActiveField { get; set; }
        [Required]
        [MaxLength(200)]
        public string Email { get; set; }
        [Required]
        [MaxLength(200)]
        public string Password { get; set; }
    }
}