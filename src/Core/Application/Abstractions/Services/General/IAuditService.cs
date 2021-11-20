using StoreKit.Application.Wrapper;
using StoreKit.Shared.DTOs.General.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreKit.Application.Abstractions.Services.General
{
    public interface IAuditService : ITransientService
    {
        Task<IResult<IEnumerable<AuditResponse>>> GetUserTrailsAsync(Guid userId);
    }
}