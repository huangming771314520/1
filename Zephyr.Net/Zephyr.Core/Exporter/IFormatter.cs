/*************************************************************************
 * 文件名称 ：IFormatter.cs                          
 * 描述说明 ：字段格式化接口
 * 
 * 
 * 创建信息 : create by shiruizhi@qq.com on 2013 (施睿智)
 * 修订信息 : 
**************************************************************************/

namespace Zephyr.Core
{
    public interface IFormatter
    {
        object Format(object value);
    }
}
