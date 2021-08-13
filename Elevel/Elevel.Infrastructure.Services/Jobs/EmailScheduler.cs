using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Quartz;
using Quartz.Impl;
using System;

namespace Elevel.Infrastructure.Services.Jobs
{
    public class EmailScheduler
    {
        public static async void Start(IServiceProvider service, IApplicationDbContext context, UserManager<User> userManager)
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            scheduler.JobFactory = (JobFactory)service.GetService(typeof(JobFactory));
            await scheduler.Start();

            IJobDetail jobDetail = JobBuilder.Create<EmailJob>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("MailingTrigger", "default")
                .StartAt(DateBuilder.DateOf(12,30,0,12,8))
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
