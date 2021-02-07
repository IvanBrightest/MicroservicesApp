using Basket.API.Data.Interfaces;
using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IBasketContext _context;

        public BasketRepository(IBasketContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<BasketCart> GetBasketCart(string userName)
        {
            RedisValue basket = await _context.Redis.StringGetAsync(userName);

            if (basket.IsNullOrEmpty) return null;

            return JsonConvert.DeserializeObject<BasketCart>(basket);
        }
        public async Task<BasketCart> UpdateBasketCart(BasketCart basket)
        {
            bool updatedBasket = await _context.Redis.StringSetAsync(basket.UserName, JsonConvert.SerializeObject(basket));

            if (!updatedBasket) return null;

            return await GetBasketCart(basket.UserName);

        }
        public async Task<bool> DeleteBasketCart(string userName)
        {
            return await _context.Redis.KeyDeleteAsync(userName);
        }
    }
}
