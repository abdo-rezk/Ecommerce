using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastrucure.Data
{
    public class BasketRepository : IBasketRepository
    {
        // exist in redis
        private readonly StackExchange.Redis.IDatabase database;
        public BasketRepository(IConnectionMultiplexer Redis)
        {
            database = Redis.GetDatabase();
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
             var data =await database.StringGetAsync(basketId);
                return (data.IsNullOrEmpty)?null: JsonSerializer.Deserialize<CustomerBasket>(data);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var created =await database.StringSetAsync( basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));
            if (!created)
                return null;
            return await GetBasketAsync(basket.Id);
           
        }
    }
}
