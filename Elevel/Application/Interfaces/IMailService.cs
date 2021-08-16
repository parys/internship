using Elevel.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elevel.Application.Interfaces
{
    public interface IMailService
    {
        public string SendMessage(UserManager<User> userManager, Guid receiverId, string subject, string body);
        public string UsersEmailNotification(UserManager<User> userManager, List<Guid> receiverIds, string subject, string body);
    }
}
