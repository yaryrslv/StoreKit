using System;
using System.Threading.Tasks;
using StoreKit.Application.Abstractions.Repositories;
using StoreKit.Application.Abstractions.Services.Catalog;
using StoreKit.Application.Specifications;
using StoreKit.Application.Wrapper;
using StoreKit.Domain.Entities.Catalog;
using StoreKit.Shared.DTOs.Catalog;
using StoreKit.Shared.DTOs.Catalog.StaticPage;

namespace StoreKit.Application.Services.Catalog;

public class StaticPageService : IStaticPageService
{
    private readonly IRepositoryAsync _repository;

    public StaticPageService(IRepositoryAsync repository)
    {
        _repository = repository;
    }
    public async Task<Guid> CreateStaticPageAsync(CreateStaticPageDto request)
    {
        var staticPage = new StaticPage{Body = request.Body};
        var guid = await _repository.CreateAsync(staticPage);
        await _repository.SaveChangesAsync();
        return guid;
    }

    public Task<StaticPage> GetStaticPageAsync(Guid id)
    {
        return _repository.GetByIdAsync<StaticPage>(id);
    }
}