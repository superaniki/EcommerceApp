using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceApp.Models
{
    public class Customer
    {
        [Key]
        public int Id { set; get; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "First Name")]

        public string FirstName { set; get; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Last Name")]

        public string LastName { set; get; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { set; get; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Full Adress")]

        public string Address { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }

    }
}
