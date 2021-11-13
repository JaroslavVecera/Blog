using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blog
{
    public class ApplicationUser : Microsoft.AspNetCore.Identity.IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}