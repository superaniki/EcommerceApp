using EcommerceApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace EcommerceApp.Models;

public class ApplicationUser : IdentityUser
{
    public virtual Customer Customer { get; set; }
}