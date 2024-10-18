using DataLayer.Constants.DBConnection;
using DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace DataLayer.Repository
{
    
    public class CacheDL : ICache
    {
        private readonly IDatabase _cacheDb;
        private readonly IConfiguration _configuration;

        public CacheDL(IConfiguration configuration)
        {
            _configuration = configuration;
            var redis = ConnectionMultiplexer.Connect(_configuration["Redis:ConnectionString"]);
            _cacheDb = redis.GetDatabase();
        }

      
        public T GetData<T>(string key)
        {
            var value=_cacheDb.StringGet(key);
            if(!string.IsNullOrEmpty(value))
            {
                return JsonSerializer.Deserialize<T>(value);
            }

            return default;
        }

        public object RemoveData(string key)
        {
            var _exist=_cacheDb.KeyExists(key);

            if (_exist)
            {
                return _cacheDb.KeyDelete(key);
            }
            return false;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expirtyTime = expirationTime.DateTime.Subtract(DateTime.Now);
            return _cacheDb.StringSet(key,JsonSerializer.Serialize(value),expirtyTime);
            
        }
    }
}
