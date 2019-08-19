/*************************************************************************
 * 文件名称 ：UpdateEventArgs.cs                          
 * 描述说明 ：更新事件参数
 **************************************************************************/

using Zephyr.Data;

namespace Zephyr.Core
{
    public class UpdateEventArgs
    {
        public IDbContext db { get; set; }
        public ParamUpdateData data { get; set; }
        public int executeValue { get; set; }
    }
}
