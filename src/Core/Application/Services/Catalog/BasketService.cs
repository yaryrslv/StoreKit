using System;
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

        public async Task<Result<Guid>> CreateBasketAsync(CreateBasketRequest request)
        {
            var user = await _userService.GetAsync(request.UserId.ToString());
            if (user == null)
            {
                throw new EntityNotFoundException(string.Format(_localizer["user.notfound"], request.UserId));
            }
            bool basketExists = await _repository.ExistsAsync<Basket>(a => a.UserId == request.UserId);
            if (basketExists) throw new EntityAlreadyExistsException(string.Format(_localizer["basketwiththisuser.alreadyexists"], request.UserId));
            var basket = new Basket(request.UserId, request.Products);
            var basketId = await _repository.CreateAsync<Basket>(basket);
            await _repository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(basketId);
        }

        public async Task<Result<Guid>> UpdateBasketAsync(UpdateBasketRequest request, Guid id)
        {
            var basket = await _repository.GetByIdAsync<Basket>(id);
            if (basket == null) throw new EntityNotFoundException(string.Format(_localizer["basket.notfound"], id));
            var updatedBasket = basket.Update(request.UserId, request.Products);
            await _repository.UpdateAsync<Basket>(updatedBasket);
            await _repository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(id);
        }

        public async Task<Result<Guid>> DeleteBasketAsync(Guid id)
        {
            await _repository.RemoveByIdAsync<Basket>(id);
            await _repository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(id);
        }
    }
}