using System;
using Microsoft.Extensions.Caching.Memory;

namespace DXC.Technology.Caching
{
    #region SlidingExpirationCache
    public static class SlidingExpirationCache
    {
        #region Static Fields

        /// <summary>
        /// Memory cache instance for sliding expiration.
        /// </summary>
        private static IMemoryCache slidingExpirationMemoryCacheInstance = null;

        /// <summary>
        /// Cache entry options for sliding expiration.
        /// </summary>
        private static readonly MemoryCacheEntryOptions userCacheEntryOptions = new MemoryCacheEntryOptions() { SlidingExpiration = TimeSpan.FromMinutes(5) };

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets the sliding expiration memory cache instance.
        /// </summary>
        private static IMemoryCache SlidingExpirationMemoryCache
        {
            get
            {
                if (slidingExpirationMemoryCacheInstance == null)
                {
                    slidingExpirationMemoryCacheInstance = new MemoryCache(new MemoryCacheOptions());
                }
                return slidingExpirationMemoryCacheInstance;
            }
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Adds an object to the sliding expiration cache.
        /// </summary>
        /// <typeparam name="T">Type of the object to cache.</typeparam>
        /// <param name="cacheKey">Cache key.</param>
        /// <param name="objectToCache">Object to cache.</param>
        public static void Add<T>(string cacheKey, T objectToCache)
        {
            SlidingExpirationMemoryCache.Set(cacheKey, objectToCache, userCacheEntryOptions);
        }

        /// <summary>
        /// Retrieves an object from the sliding expiration cache.
        /// </summary>
        /// <typeparam name="T">Type of the object to retrieve.</typeparam>
        /// <param name="cacheKey">Cache key.</param>
        /// <returns>Cached object.</returns>
        public static T Get<T>(string cacheKey)
        {
            SlidingExpirationMemoryCache.TryGetValue(cacheKey, out T cachedObject);
            return cachedObject;
        }

        /// <summary>
        /// Removes an object from the sliding expiration cache.
        /// </summary>
        /// <param name="cacheKey">Cache key.</param>
        public static void Remove(string cacheKey)
        {
            SlidingExpirationMemoryCache.Remove(cacheKey);
        }

        #endregion
    }
    #endregion

    #region AbsoluteExpirationCache
    public static class AbsoluteExpirationCache
    {
        #region Static Fields

        /// <summary>
        /// Memory cache instance for absolute expiration.
        /// </summary>
        private static IMemoryCache absoluteExpirationMemoryCacheInstance = null;

        /// <summary>
        /// Cache entry options for absolute expiration.
        /// </summary>
        private static readonly MemoryCacheEntryOptions userCacheEntryOptions = new MemoryCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(200) };

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets the absolute expiration memory cache instance.
        /// </summary>
        private static IMemoryCache AbsoluteExpirationMemoryCache
        {
            get
            {
                if (absoluteExpirationMemoryCacheInstance == null)
                {
                    absoluteExpirationMemoryCacheInstance = new MemoryCache(new MemoryCacheOptions());
                }
                return absoluteExpirationMemoryCacheInstance;
            }
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Removes an object from the absolute expiration cache.
        /// </summary>
        /// <param name="cacheKey">Cache key.</param>
        public static void Remove(string cacheKey)
        {
            AbsoluteExpirationMemoryCache.Remove(cacheKey);
        }

        /// <summary>
        /// Adds an object to the absolute expiration cache.
        /// </summary>
        /// <typeparam name="T">Type of the object to cache.</typeparam>
        /// <param name="cacheKey">Cache key.</param>
        /// <param name="objectToCache">Object to cache.</param>
        public static void Add<T>(string cacheKey, T objectToCache)
        {
            AbsoluteExpirationMemoryCache.Set(cacheKey, objectToCache, userCacheEntryOptions);
        }

        /// <summary>
        /// Retrieves an object from the absolute expiration cache.
        /// </summary>
        /// <typeparam name="T">Type of the object to retrieve.</typeparam>
        /// <param name="cacheKey">Cache key.</param>
        /// <returns>Cached object.</returns>
        public static T Get<T>(string cacheKey)
        {
            AbsoluteExpirationMemoryCache.TryGetValue(cacheKey, out T cachedObject);
            return cachedObject;
        }

        #endregion
    }

    #endregion
}