using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StoreKit.Application.Abstractions.Services.General
{
    public interface IJobService : ITransientService
    {
        string Enqueue(Expression<Func<Task>> methodCall);
    }
}