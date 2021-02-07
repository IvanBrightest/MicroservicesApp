using Basket.API.Data.Interfaces;
using StackExchange.Redis;
using System;

namespace Basket.API.Data
{
    public class BasketContext : IBasketContext
    {
        private readonly ConnectionMultiplexer _connection;

        public BasketContext(ConnectionMultiplexer connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            Redis = _connection.GetDatabase();
        }

        public IDatabase Redis { get; }
    }
}