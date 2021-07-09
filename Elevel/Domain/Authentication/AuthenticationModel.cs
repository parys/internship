using System.Collections.Generic;

namespace Elevel.Domain.Authentication
{
    public class AuthenticationModel
    {
        public string Message { get; set; }

        public bool IsAuthenticated { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public List<string> Roles { get; set; }

        public string Token { get; set; }
    }
}