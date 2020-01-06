using System;
using Quartz;
using Quartz.Spi;

namespace Inspiration_International.Services
{
    internal sealed class IntegrationJobFactory : IJobFactory
    {
        private readonly IServiceProvider _service;

        public IntegrationJobFactory(IServiceProvider service)
        {
            _service = service;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            Console.WriteLine("Inside JobFactory");
            var jobDetail = bundle.JobDetail;

            var job = _service.GetService(jobDetail.JobType) as IJob;
            return job;
        }
        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}