using Basket.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Repositories.Interfaces
{
    public interface IBasketRepository
    {
        Task<BasketCart> GetBasketCart(string userName);
        Task<BasketCart> UpdateBasketCart(BasketCart basket);
        Task<bool> DeleteBasketCart(string userName);
    }
}
