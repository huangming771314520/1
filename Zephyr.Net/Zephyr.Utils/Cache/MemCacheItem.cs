using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Utils
{
    [Serializable]
    public class MemCacheItem
    {
        public string AppKey { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public DateTime InvalidTime { get; set; }
    }
}
