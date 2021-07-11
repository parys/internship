using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Elevel.Domain.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public ICollection<Test> Tests { get; set; } 
    }
}