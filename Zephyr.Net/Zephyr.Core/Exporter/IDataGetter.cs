/*************************************************************************
 * 文件名称 ：IDataGetter.cs                          
 * 描述说明 ：取得数据接口
 * 
 * 
 * 创建信息 : create by shiruizhi@qq.com on 2013 (施睿智)
 * 修订信息 : 
**************************************************************************/

using System.Web;

namespace Zephyr.Core
{
    public interface IDataGetter
    {
        object GetData(HttpContext context);
    }
}
