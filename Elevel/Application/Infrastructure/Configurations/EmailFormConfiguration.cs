using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevel.Application.Infrastructure.Configurations
{
    public class EmailFormConfiguration
    {
        public List<MailboxAddress> ReceiverEmails { get; set; } = new List<MailboxAddress>();
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
