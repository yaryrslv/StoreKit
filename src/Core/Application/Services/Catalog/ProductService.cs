using StoreKit.Application.Abstractions.Repositories;
using StoreKit.Application.Abstractions.Services.Catalog;
using StoreKit.Application.Abstractions.Services.General;
using StoreKit.Application.Exceptions;
using StoreKit.Application.Specifications;
using StoreKit.Application.Wrapper;
using StoreKit.Domain.Entities.Catalog;
using StoreKit.Domain.Enums;
using StoreKit.Shared.DTOs.Catalog;
using Mapster;
using Microsoft.Extensions.Localization;
using System;
using System.Threading.Tasks;

namespace StoreKit.Application.Services.Catalog
{
    public class ProductsService : IProductService
    {
        private readonly IStringLocalizer<ProductsService> _localizer;
        private readonly IFileStorageService _file;
        private readonly IRepositoryAsync _repository;

        public ProductsService(IRepositoryAsync repository, IStringLocalizer<ProductsService> localizer, IFileStorageService file)
        {
            _repository = repository;
            _localizer = localizer;
            _file = file;
        }

        public async Task<Result<Guid>> CreateProductAsync(CreateProductRequest request)
        {
            var productExists = await _repository.ExistsAsync<Product>(a => a.Name == request.Name);
            if (productExists) throw new EntityAlreadyExistsException(string.Format(_localizer["product.alreadyexists"], request.Name));
            string productImagePath = await _file.UploadAsync<Product>(request.Image, FileType.Image);
            var product = new Product(request.Name, request.Description, request.Rate, productImagePath, request.Tags);
            var productId = await _repository.CreateAsync<Product>(product);
            await _repository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(productId);
        }

        public async Task<Result<Guid>> UpdateProductAsync(UpdateProductRequest request, Guid id)
        {
            var product = await _repository.GetByIdAsync<Product>(id, null);
            if (product == null) throw new EntityNotFoundException(string.Format(_localizer["product.notfound"], id));
            string productImagePath = string.Empty;
            if (request.Image != null) productImagePath = await _file.UploadAsync<Product>(request.Image, FileType.Image);
            var updatedProduct = product.Update(request.Name, request.Description, request.Rate, productImagePath, request.Tags);
            await _repository.UpdateAsync<Product>(updatedProduct);
            await _repository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(id);
        }

        public async Task<Result<Guid>> DeleteProductAsync(Guid id)
        {
            await _repository.RemoveByIdAsync<Product>(id);
            await _repository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(id);
        }

        public async Task<Result<ProductDetailsDto>> GetProductDetailsAsync(Guid id)
        {
            var spec = new BaseSpecification<Product>();
            spec.Includes.Add(a => a.Tags);
            var product = await _repository.GetByIdAsync<Product, ProductDetailsDto>(id, spec);
            return await Result<ProductDetailsDto>.SuccessAsync(product);
        }

        public async Task<Result<ProductDto>> GetByIdUsingDapperAsync(Guid id)
        {
            var product = await _repository.QueryFirstOrDefaultAsync<Product>($"SELECT * FROM public.\"Products\" WHERE \"Id\"  = '{id}' AND \"TenantKey\"='@tenantKey'");
            var mappedProduct = product.Adapt<ProductDto>();
            return await Result<ProductDto>.SuccessAsync(mappedProduct);
        }

        public async Task<PaginatedResult<ProductDto>> SearchAsync(ProductListFilter filter)
        {
            var products = await _repository.GetSearchResultsAsync<Product, ProductDto>(filter.PageNumber, filter.PageSize, filter.OrderBy, filter.AdvancedSearch, filter.Keyword);
            return products;
        }
    }
}