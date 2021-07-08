using System.Threading.Tasks;
using Elevel.Domain.Authentication;

namespace Elevel.Application.Interfaces
{
    public interface IUserService
    {
        Task<AuthenticationModel> GetTokenAsync(TokenRequestModel model);
    }
}