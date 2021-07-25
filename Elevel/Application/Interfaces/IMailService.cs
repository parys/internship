using Elevel.Domain.Models;
using System.Threading.Tasks;

namespace Elevel.Application.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
