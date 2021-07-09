using System;
using Microsoft.AspNetCore.Identity;

namespace Elevel.Domain.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTimeOffset CreationDate { get; set; }
    }
}