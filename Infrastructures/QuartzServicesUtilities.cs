using System;
using Quartz;

public static class QuartzServicesUtilities
{
    public static void StartJob<TJob>(IScheduler scheduler, string cron)
         where TJob : IJob
    {
        var jobName = typeof(TJob).FullName;

        var job = JobBuilder.Create<TJob>()
            .WithIdentity(jobName)
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithSchedule(CronScheduleBuilder.WeeklyOnDayAndHourAndMinute(DayOfWeek.Sunday, 06, 30)
                            .WithMisfireHandlingInstructionFireAndProceed()
            // .InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Africa/Lagos")))
            .InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time")))
            .ForJob(job)
            .Build();

        scheduler.ScheduleJob(job, trigger);
    }
}