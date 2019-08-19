/*************************************************************************
 * 文件名称 ：ParamWhere.cs                          
 * 描述说明 ：参数条件定义
 * 
 * 
 * 创建信息 : create by shiruizhi@qq.com on 2013 (施睿智)
 * 修订信息 : 
**************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace Zephyr.Core
{
    public class ParamWhere
    {
        public WhereData Data { get; set; }
        public Func<WhereData,string> Compare { get; set; }
    }

    public class WhereData
    {
        public string AndOr { get; set; }
        public string Column { get; set; }
        public object Value { get; set; }
        public object[] Extend { get; set; }
    }
}
