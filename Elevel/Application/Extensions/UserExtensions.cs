using Elevel.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevel.Application.Extensions
{
    public static class UserExtensions
    {
        public static string GetUserNames(this User source)
        {
            return $"{source.FirstName} {source.LastName}";
        }
    }
}
