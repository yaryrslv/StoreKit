using StoreKit.Application.Abstractions.Repositories;
using StoreKit.Application.Abstractions.Services.Catalog;
using StoreKit.Application.Exceptions;
using StoreKit.Application.Specifications;
using StoreKit.Application.Wrapper;
using StoreKit.Domain.Entities.Catalog;
using Mapster;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using StoreKit.Application.Services.LocalStorage;
using StoreKit.Infrastructure.Services.LocalStorage;
using StoreKit.Shared.DTOs.Catalog.Product;

namespace StoreKit.Application.Services.Catalog
{
    public class ProductsService : IProductService
    {
        private readonly IStringLocalizer<ProductsService> _localizer;
        private readonly IImageService _imageService;
        private readonly IRepositoryAsync _repository;
        private readonly StaticFileSettings _staticFileSettings;

        public ProductsService(IRepositoryAsync repository, IStringLocalizer<ProductsService> localizer, IImageService imageService, IOptions<StaticFileSettings> staticFileSettings)
        {
            _repository = repository;
            _localizer = localizer;
            _imageService = imageService;
            _staticFileSettings = staticFileSettings.Value;
        }

        public async Task<Result<Guid>> CreateProductAsync(CreateProductRequest request)
        {
            bool productExists = await _repository.ExistsAsync<Product>(a => a.Name == request.Name);
            if (productExists) throw new EntityAlreadyExistsException(string.Format(_localizer["product.alreadyexists"], request.Name));

            var category = await _repository.GetByIdAsync<Category>(request.CategoryId);
            if (category == null) throw new EntityNotFoundException(string.Format(_localizer["category.notfound"], request.CategoryId));

            //string productImagePath = await _imageService.Save(imageStream, Guid.NewGuid().ToString());
            var product = new Product(request.Name, request.Description, null, category.Id, request.Tags, request.Prices);
            var productId = await _repository.CreateAsync(product);
            await _repository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(productId);
        }

        public async Task<Result<Guid>> UpdateProductAsync(UpdateProductRequest request, Guid id)
        {
            var product = await _repository.GetByIdAsync<Product>(id);
            if (product == null) throw new EntityNotFoundException(string.Format(_localizer["product.notfound"], id));

            var category = await _repository.GetByIdAsync<Product>(request.CategoryId);
            if (category == null) throw new EntityNotFoundException(string.Format(_localizer["category.notfound"], request.CategoryId));

            string productImagePath = string.Empty;
            if (request.ImageStream != null)
            {
                productImagePath = await _imageService.Save(request.ImageStream, Guid.NewGuid().ToString());
            }

            var updatedProduct = product.Update(request.Name, request.Description, productImagePath, category.Id, request.Tags, request.Prices);
            await _repository.UpdateAsync(updatedProduct);
            await _repository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(id);
        }

        public async Task<Result<Guid>> DeleteProductAsync(Guid id)
        {
            var product = await _repository.GetByIdAsync<Product>(id);
            await _repository.RemoveByIdAsync<Product>(id);
            _imageService.Delete(product.ImagePath, _staticFileSettings);
            await _repository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(id);
        }

        public async Task<Result<ProductDetailsDto>> GetProductDetailsAsync(Guid id)
        {
            var spec = new BaseSpecification<Product>();
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
            var products = await _repository.GetSearchResultsAsync<Product, ProductDto>(filter.PageNumber, filter.PageSize, filter.OrderBy, filter.Keyword);
            if (filter.CategoryId != null && filter.CategoryId != Guid.Empty)
            {
                var findProducts = products.Data.Where(i => i.CategoryId == filter.CategoryId).ToList();
                products = new PaginatedResult<ProductDto>(products.Succeeded, findProducts, products.Messages, products.Data.Count, products.CurrentPage, products.PageSize);
            }
            if (filter.Tags?.Any() == true)
            {
                var findProducts = products.Data.Where(i =>
                {
                    var intersectedTags = i.Tags.Select(t => (t.Name+t.Value).GetHashCode())
                        .Intersect(filter.Tags.Select(t => (t.Name+t.Value).GetHashCode())).ToList();
                    if (intersectedTags.Count > 0)
                    {
                        return true;
                    }
                    return false;
                }).ToList();
                if (findProducts.Count > 0)
                {
                    products = new PaginatedResult<ProductDto>(products.Succeeded, findProducts, products.Messages, products.Data.Count, products.CurrentPage, products.PageSize);
                }
            }
            if (filter.Prices is {Count: > 0})
            {
                var findProducts = products.Data.Where(i =>
                {
                    var intersectedPrices = i.Prices.Select(t => (t.Type+t.Price).GetHashCode())
                        .Intersect(filter.Prices.Select(t => (t.Type+t.Price).GetHashCode())).ToList();
                    if (intersectedPrices.Count > 0)
                    {
                        return true;
                    }
                    return false;
                }).ToList();
                if (findProducts.Count > 0)
                {
                    products = new PaginatedResult<ProductDto>(products.Succeeded, findProducts, products.Messages, products.Data.Count, products.CurrentPage, products.PageSize);
                }
            }

            return new PaginatedResult<ProductDto>(products.Succeeded, products.Data, products.Messages, products.Data.Count, products.CurrentPage, products.PageSize);
        }
    }
}