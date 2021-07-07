using System.Threading.Tasks;
using Elevel.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Elevel.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; set; }

        Task<int> SaveChanges();
    }
}