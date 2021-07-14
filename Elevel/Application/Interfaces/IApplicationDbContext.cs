using System.Threading.Tasks;
using Elevel.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Elevel.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Audition> Auditions { get; set; }
        Task<int> SaveChanges();
    }
}