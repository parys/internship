using Elevel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevel.Application.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
