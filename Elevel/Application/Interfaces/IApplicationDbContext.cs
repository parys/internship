using System.Threading;
using System.Threading.Tasks;
using Elevel.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Elevel.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Answer> Answers { get; set; }
        DbSet<TestQuestion> TestQuestions { get; set; }
        DbSet<Question> Questions { get; set; }
        DbSet<Test> Tests { get; set; }
        DbSet<Topic> Topics { get; set; }
        DbSet<Audition> Auditions { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}