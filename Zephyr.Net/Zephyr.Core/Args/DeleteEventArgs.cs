/*************************************************************************
 * 文件名称 ：DeleteEventArgs.cs                          
 * 描述说明 ：删除事件参数
 **************************************************************************/

using Zephyr.Data;

namespace Zephyr.Core
{
    public class DeleteEventArgs
    {
        public IDbContext db { get; set; }
        public ParamDeleteData data { get; set; }
        public int executeValue { get; set; }
    }
}
