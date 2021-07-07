using System.Threading.Tasks;
using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Elevel.Persistence.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public async Task<int> SaveChanges()
        {
            return await base.SaveChangesAsync();
        }
    }
}