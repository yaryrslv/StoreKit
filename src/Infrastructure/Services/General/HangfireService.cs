using StoreKit.Application.Abstractions.Services.General;
using Hangfire;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StoreKit.Infrastructure.Services.General
{
    public class HangfireService : IJobService
    {
        public string Enqueue(Expression<Func<Task>> methodCall)
        {
            return BackgroundJob.Enqueue(methodCall);
        }
    }
}