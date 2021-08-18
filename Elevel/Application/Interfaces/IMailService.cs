using Elevel.Application.Infrastructure.Configurations;
using System;
using System.Collections.Generic;

namespace Elevel.Application.Interfaces
{
    public interface IMailService
    {
        string SendMessage(Guid receiverId, string subject, string body);
        public string UsersEmailNotification(List<EmailFormConfiguration> emails);
    }
}
