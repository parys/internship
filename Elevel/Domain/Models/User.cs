using System;

namespace Elevel.Domain.Models
{
    public class User : BaseDataModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PasswordSalt { get; set; }

        public string PasswordHash { get; set; }

        public DateTime CreationDate { get; set; }

        public Guid RoleId { get; set; }

        public Role Role { get; set; }

    }
}