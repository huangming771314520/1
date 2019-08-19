using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memcached.ClientLibrary;

namespace Zephyr.Utils
{
    public class MemcacheWriter :ICacheWriter
    {
        private MemcachedClient memcachedClient;
        public MemcacheWriter()
        {
            //string[] servers = { "192.168.1.100:11211", "192.168.1.118:11211" };

            string strAppMemcachedServer= System.Configuration.ConfigurationManager.AppSettings["MemcachedServerList"];

            string[] servers = strAppMemcachedServer.Split(',');

            //初始化池
            SockIOPool pool = SockIOPool.GetInstance();
            pool.SetServers(servers);
            pool.InitConnections = 3;
            pool.MinConnections = 3;
            pool.MaxConnections = 5;
            pool.SocketConnectTimeout = 1000;
            pool.SocketTimeout = 3000;
            pool.MaintenanceSleep = 30;
            pool.Failover = true;
            pool.Nagle = false;
            pool.Initialize();
            //客户端实例
            MemcachedClient mc = new Memcached.ClientLibrary.MemcachedClient();
            mc.EnableCompression = false;

            memcachedClient = mc;
        }


        public void AddCache(string key, object value, DateTime expDate)
        {
            var result = memcachedClient.Add(key, value, expDate);
        }

        public void AddCache(string key, object value)
        {
            var result = memcachedClient.Add(key, value);
        }

        public object GetCache(string key)
        {
            var cache = memcachedClient.Get(key);
            return cache;
        }

        public T GetCache<T>(string key)
        {
            var cache = (T)memcachedClient.Get(key);
            return cache;
        }


        public void SetCache(string key, object value, DateTime extDate)
        {
            var result = memcachedClient.Set(key, value, extDate);
        }

        public void SetCache(string key, object value)
        {
            var result = memcachedClient.Set(key, value);
        }

        public void RemoveCache(string key)
        {
            var result = memcachedClient.Delete(key);
        }
    }
}
