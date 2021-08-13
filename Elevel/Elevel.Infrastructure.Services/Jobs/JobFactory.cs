using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace Elevel.Infrastructure.Services.Jobs
{
    public class JobFactory : IJobFactory
    {
        protected readonly IServiceScopeFactory _serviceScopeFactory;


        public JobFactory(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var job = scope.ServiceProvider.GetService(bundle.JobDetail.JobType) as IJob;
                return job;
            }

        }

        public void ReturnJob(IJob job)
        {
            //Do something if need
        }
    }
}
