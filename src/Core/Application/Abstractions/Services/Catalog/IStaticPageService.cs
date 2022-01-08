using System;
using System.Threading.Tasks;
using StoreKit.Application.Wrapper;
using StoreKit.Domain.Entities.Catalog;
using StoreKit.Shared.DTOs.Catalog.StaticPage;

namespace StoreKit.Application.Abstractions.Services.Catalog;

public interface IStaticPageService : ITransientService
{
    Task<Guid> CreateStaticPageAsync(CreateStaticPageDto request);
    Task<StaticPage> GetStaticPageAsync(Guid id);
}