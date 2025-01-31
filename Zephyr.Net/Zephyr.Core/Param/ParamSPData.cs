﻿/*************************************************************************
 * 文件名称 ：ParamSPData.cs                          
 * 描述说明 ：SP参数数据
 * 
 * 
 * 创建信息 : create by shiruizhi@qq.com on 2013 (施睿智)
 * 修订信息 : 
**************************************************************************/

using System.Collections.Generic;
using Zephyr.Data;

namespace Zephyr.Core
{
    public class ParamSPData
    {
        public string Name { get; set; }
        public Dictionary<string,object> Parameter { get; set; }
        public Dictionary<string, DataTypes> ParameterOut { get; set; }

        public ParamSPData()
        {
            Name = string.Empty;
            Parameter = new Dictionary<string, object>();
            ParameterOut = new Dictionary<string, DataTypes>();
        }
    }
}
