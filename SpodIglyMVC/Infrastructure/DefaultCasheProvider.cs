using System;
using System.Web;
using System.Web.Caching;

namespace SpodIglyMVC.Infrastructure
{
    public class DefaultCasheProvider : ICasheProvider
    {
        private Cache Cache { get { return HttpContext.Current.Cache; } }

        public object Get(string key)
        {
            return Cache[key];
        }

        public void Invalidate(string key)
        {
            Cache.Remove(key);
        }

        public bool IsSet(string key)
        {
            return (Cache[key] != null);
        }

        public void Set(string key, object data, int casheTime)
        {
            DateTime expirationTime = DateTime.Now + TimeSpan.FromMinutes(casheTime);
            Cache.Insert(key, data, null, expirationTime, Cache.NoSlidingExpiration);
        }
    }
}