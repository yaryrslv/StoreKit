using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using StoreKit.Application.Abstractions.Repositories;
using StoreKit.Application.Abstractions.Services.Catalog;
using StoreKit.Application.Abstractions.Services.Identity;
using StoreKit.Application.Exceptions;
using StoreKit.Application.Specifications;
using StoreKit.Application.Wrapper;
using StoreKit.Domain.Entities.Catalog;
using StoreKit.Shared.DTOs.Catalog.Basket;

namespace StoreKit.Application.Services.Catalog
{
    public class BasketService : IBasketService
    {
        private readonly IStringLocalizer<BasketService> _localizer;
        private readonly IUserService _userService;
        private readonly IRepositoryAsync _repository;

        public BasketService(IRepositoryAsync repository, IStringLocalizer<BasketService> localizer, IUserService userService)
        {
            _repository = repository;
            _localizer = localizer;
            _userService = userService;
        }

        public async Task<Result<BasketDetailsDto>> GetBasketDetailsAsync(Guid id)
        {
            var spec = new BaseSpecification<Basket>();
            var basket = await _repository.GetByIdAsync<Basket, BasketDetailsDto>(id, spec);
            if (basket == null) throw new EntityNotFoundException(string.Format(_localizer["basket.notfound"], id));
            return await Result<BasketDetailsDto>.SuccessAsync(basket);
        }

        public async Task<BasketDetailsDto> GetBasketDetailsByUserIdAsync(Guid id)
        {
            var basketsByUserId = await _repository.GetListAsync<Basket>(i => i.UserId == id);
            if (basketsByUserId.All(i => i.UserId != id))
            {
                throw new EntityNotFoundException(string.Format(_localizer["basket.notfound"], id));
            }
            var basketByUserId = basketsByUserId.First(i => i.UserId == id);
            return new BasketDetailsDto()
            {
                Id = basketByUserId.Id,
                UserId = basketByUserId.UserId,
                Products = basketByUserId.Products
            };
        }

        public async Task<Result<Guid>> CreateBasketAsync(CreateBasketRequest request, Guid userId)
        {
            var user = await _userService.GetAsync(userId.ToString());
            if (user == null)
            {
                throw new EntityNotFoundException(string.Format(_localizer["user.notfound"], userId));
            }
            bool basketExists = await _repository.ExistsAsync<Basket>(a => a.UserId == userId);
            if (basketExists) throw new EntityAlreadyExistsException(string.Format(_localizer["basketwiththisuser.alreadyexists"], userId));
            var products = new List<ProductInBasket>();
            foreach (var requestedProduct in request.Products)
            {
                var product = await _repository.GetByIdAsync<Product>(requestedProduct.Id);
                if (product != null && product.Prices.Select(i => i.Type).Contains(requestedProduct.PriceType))
                {
                    var price = product.Prices.First(i => i.Type == requestedProduct.PriceType);
                    products.Add(new ProductInBasket(product.Name, product.Description, product.ImagePath, price.Type, price.Price));
                }
            }
            var basket = new Basket(userId, products);
            var basketId = await _repository.CreateAsync<Basket>(basket);
            await _repository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(basketId);
        }

        public async Task<Result<Guid>> UpdateBasketAsync(UpdateBasketRequest request, Guid userId)
        {
            var user = await _userService.GetAsync(userId.ToString());
            if (user == null)
            {
                throw new EntityNotFoundException(string.Format(_localizer["user.notfound"], userId));
            }
            bool basketExists = await _repository.ExistsAsync<Basket>(a => a.UserId == userId);
            if (!basketExists)
            {
                var newBasket = new Basket(userId, new List<ProductInBasket>());
                await _repository.CreateAsync<Basket>(newBasket);
            }
            var basketList = await _repository.GetListAsync<Basket>(a => a.UserId == userId);
            var basket = basketList.FirstOrDefault();
            if (basket == null) throw new EntityNotFoundException(string.Format(_localizer["basketbyuser.notfound"], userId));
            var products = new List<ProductInBasket>();
            foreach (var requestedProduct in request.Products)
            {
                var product = await _repository.GetByIdAsync<Product>(requestedProduct.Id);
                if (product != null && product.Prices.Select(i => i.Type).Contains(requestedProduct.PriceType))
                {
                    var price = product.Prices.First(i => i.Type == requestedProduct.PriceType);
                    products.Add(new ProductInBasket(product.Name, product.Description, product.ImagePath, price.Type, price.Price));
                }
            }
            var updatedBasket = basket.Update(userId, products);
            await _repository.UpdateAsync<Basket>(updatedBasket);
            await _repository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(basket.Id);
        }

        public async Task<Result<Guid>> DeleteBasketAsync(Guid id)
        {
            await _repository.RemoveByIdAsync<Basket>(id);
            await _repository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(id);
        }
    }
}