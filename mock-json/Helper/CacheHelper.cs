using Microsoft.Extensions.Caching.Memory;
using System;

namespace mock_json.Helper
{
    public class CacheHelper
    {
        private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        private static string nullValue = Guid.NewGuid().ToString();

        public static void Set(string cacheKey, string toSet)
          => _cache.Set(cacheKey, toSet ?? nullValue);

        public static string Get(string cacheKey)
        {
            var isInCache = _cache.TryGetValue(cacheKey, out string cachedVal);
            if (!isInCache) return null;

            return cachedVal == nullValue ? null : cachedVal;
        }
    }
}
