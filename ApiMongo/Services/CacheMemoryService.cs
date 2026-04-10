using ApiMongo.Services.IService;
using Microsoft.Extensions.Caching.Memory;

namespace ApiMongo.Services
{
    public class CacheMemoryService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheMemoryService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T Get<T>(string key)
        {
            var cache = _memoryCache.Get<T>(key);
            return cache;
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public void Set<T>(string key, T content)
        {
            _memoryCache.Set(key, content);
        }
    }
}
