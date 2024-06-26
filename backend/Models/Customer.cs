using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace EcommerceApp.Models
#nullable disable

{
    public class Customer
    {
        [Key]
        public int Id { set; get; }

        [MaxLength(100)]
        [Display(Name = "First Name")]

        public string FirstName { set; get; }

        [MaxLength(100)]
        [Display(Name = "Last Name")]

        public string LastName { set; get; }

        [MaxLength(100)]
        [Display(Name = "Full Address")]

        public string Address { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [ForeignKey("User")]
        [Display(Name = "Identity User")]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
