﻿/*************************************************************************
 * 文件名称 ：ParamInsert.cs                          
 * 描述说明 ：插入参数构建
 * 
 * 
 * 创建信息 : create by shiruizhi@qq.com on 2013 (施睿智)
 * 修订信息 : 
**************************************************************************/

using System.Collections.Generic;
using System.Dynamic;

namespace Zephyr.Core
{
    public class ParamInsert
    {
        protected ParamInsertData data;

        public dynamic this[string index]
        {
            get { return data.Columns[index]; }
            set { data.Columns[index] = value; }
        }

        public ParamInsert Insert(string tableName)
        {
            data.From = tableName;
            return this;
        }

        public ParamInsert Column(string columnName, object value)
        {
            data.Columns[columnName] = value;
            return this;
        }

        public ParamInsert()
        {
            data = new ParamInsertData();
        }

        public static ParamInsert Instance()
        {
            return new ParamInsert();
        }

        public dynamic GetDynamicValue()
        {
            var expando = (IDictionary<string, object>)new ExpandoObject();
            foreach (var c in this.data.Columns)
                expando.Add(c.Key, c.Value);

            return (ExpandoObject)expando;
        }

        public ParamInsertData GetData()
        {
            return data;
        }
    }
}
