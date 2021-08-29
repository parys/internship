using AutoMapper;
using Elevel.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevel.Test.Infrastructure
{
    public abstract class BaseTestFixture : IDisposable
    {
        public ApplicationDbContext Context { get; set; }
        public IMapper Mapper{ get; set; }

        protected BaseTestFixture()
        {
            Context = ApplicationDbContextFactory.Create();
            Mapper = AutoMapperFactory.Create();
        }

        public void Dispose()
        {
            ApplicationDbContextFactory.Destroy(Context);
        }
    }
}
