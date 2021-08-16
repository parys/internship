using Elevel.Domain.Models;
using System;
using System.Collections.Generic;

namespace Elevel.Application.Interfaces
{
    public interface IMailService
    {
        string SendMessage(Guid receiverId, string subject, string body);
        string UsersEmailNotification(List<Guid> receiverIds, string subject, string body);

        string MissedDeadlineEmailNotification(List<Test> tests, string subject, string body);
        void Connect();
        void Disconnect();
    }
}
