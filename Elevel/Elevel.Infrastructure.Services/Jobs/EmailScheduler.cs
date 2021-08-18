using Elevel.Application.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl;
using System;
using System.Threading.Tasks;

namespace Elevel.Infrastructure.Services.Jobs
{
    public class EmailScheduler
    {
        public static async Task Start(IServiceProvider service, SchedulerConfigurations schedulerConfig)
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            scheduler.JobFactory = (JobFactory)service.GetService(typeof(JobFactory));
            await scheduler.Start();

            IJobDetail jobDetail = JobBuilder.Create<EmailJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("MailingTrigger", "default")
                .StartAt(DateBuilder.DateOf(
                    schedulerConfig.startHour,
                    schedulerConfig.startMinute,
                    schedulerConfig.startSecond,
                    schedulerConfig.startDay,
                    schedulerConfig.startMonth))
                //.StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInHours(24)
                    //.WithIntervalInMinutes(1)
                    .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(jobDetail, trigger);
        }
    }
}
