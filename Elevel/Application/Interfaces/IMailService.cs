using Elevel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elevel.Application.Interfaces
{
    public interface IMailService
    {
        string SendMessage(Guid receiverId, string subject, string body);
        public Task SendNotificationsToHrsAndUsersAsync();
    }
}
