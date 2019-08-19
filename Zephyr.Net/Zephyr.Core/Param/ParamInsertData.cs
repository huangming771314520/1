/*************************************************************************
 * 文件名称 ：ParamDeleteData.cs                          
 * 描述说明 ：接入参数数据
 * 
 * 
 * 创建信息 : create by shiruizhi@qq.com on 2013 (施睿智)
 * 修订信息 : 
**************************************************************************/

using System.Collections.Generic;

namespace Zephyr.Core
{
    public class ParamInsertData
    {
        public string From { get; set; }
        public Dictionary<string,object> Columns { get; set; }

        public ParamInsertData()
        {
            From = "";
            Columns = new Dictionary<string, object>();
        }
    }
}
