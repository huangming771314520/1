using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Utils
{
    public class CacheHelper
    {
       // Spring.Net直接注入一个Cache的实现过来。
        public static ICacheWriter CacheWriter { get; set; }

        static CacheHelper()
        {
            CacheHelper.CacheWriter = new MemcacheWriter();
        }

        public static void AddCache(string key, object value,DateTime expDate)
        {
            //往缓存写：单机，分布式   观察者模式可以。修改一下配置，就能切换。
            CacheWriter.AddCache(key, value, expDate);
        }

        public static void AddCache(string key, object value)
        {
             CacheWriter.AddCache(key,value);
        }

        public static object GetCache(string key)
        {
            return CacheWriter.GetCache(key);
        }

        public static T GetCache<T>(string key)
        {
            return CacheWriter.GetCache<T>(key);
        }

        public static void SetCache(string key,object value,DateTime extTime)
        {
            CacheWriter.SetCache(key,value,extTime);
        }

        public static void SetCache(string key, object value)
        {
            CacheWriter.SetCache(key,value);
        }

        public static void RemoveCache(string key)
        {
            CacheWriter.RemoveCache(key);
        }
    }
}
