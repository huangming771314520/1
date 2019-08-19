using System;

namespace Zephyr.Utils
{
    public interface ICacheWriter
    {
        void AddCache(string key, object value, DateTime expDate);
        void AddCache(string key, object value);
        object GetCache(string key);
        T GetCache<T>(string key);
        void SetCache(string key, object value, DateTime extDate);
        void SetCache(string key, object value);
        void RemoveCache(string key);
    }
}