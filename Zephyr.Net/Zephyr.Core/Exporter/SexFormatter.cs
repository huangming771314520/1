/*************************************************************************
 * 文件名称 ：SexFormatter.cs                          
 * 描述说明 ：格式化示例
 * 
 * 
 * 创建信息 : create by shiruizhi@qq.com on 2013 (施睿智)
 * 修订信息 : 
**************************************************************************/

using System;

namespace Zephyr.Core
{
    public class SexFormatter:IFormatter
    {
        public object Format(object value)
        {
            switch(Convert.ToString(value))
            {
                case "0":
                    return "纯爷们";
                case "1":
                    return "女汉子";
                default:
                    return "春哥";
            }
        }
    }
}
