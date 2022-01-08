using System;
using System.Threading.Tasks;
using StoreKit.Application.Wrapper;
using StoreKit.Shared.DTOs.Catalog.StaticPage;

namespace StoreKit.Application.Abstractions.Services.Catalog
{
    public interface IStaticPageService : ITransientService
    {
        Task<Guid> CreateStaticPageAsync(CreateStaticPageRequest request);
        Task<Result<StaticPageDetailsDto>> GetStaticPageDetailsAsync(Guid id);
        Task<Result<Guid>> UpdateStaticPageAsync(UpdateStaticPageRequest request, Guid id);
        Task<Result<Guid>> DeleteStaticPageAsync(Guid id);
    }
}