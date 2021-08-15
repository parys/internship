using Elevel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elevel.Application.Interfaces
{
    public interface IMailService
    {
        string SendMessage(string receiverEmail, string subject, string body);
        string UsersEmailNotification(List<string> receiverEmails, string subject, string body);
    }
}
