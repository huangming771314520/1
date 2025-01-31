﻿/*************************************************************************
 * 文件名称 ：RequestWrapper.cs                          
 * 描述说明 ：请求包装
 * 
 * 
 * 创建信息 : create by shiruizhi@qq.com on 2013 (施睿智)
 * 修订信息 : 
**************************************************************************/

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

namespace Zephyr.Core
{
    public partial class RequestWrapper
    {
        private List<string> ignores = new List<string> { null, "page", "rows", "sort", "order", "format", "FLUENTDATA_ROWNUMBER", "workOrderType" };

        private string ignoreStartWith = "_";
        public NameValueCollection request { get; set; }

        private string settingXml = "<settings></settings>";

        public bool IsLoadedSettings { get { return !string.IsNullOrEmpty(settingXml); } }

        public string this[string name]
        {
            get
            {
                var field = string.Empty;
                if (name.IndexOf(".") >= 0) field = name.Split('.')[1];
                return request[field] ?? request[getAliasName(name)];
            }
            set
            {
                if (name.IndexOf(".") >= 0) name = name.Split('.')[1];
                request[name] = value;
            }
        }

        #region 构造函数
        public RequestWrapper(NameValueCollection query)
        {
            this.SetRequestData(query);
        }

        public RequestWrapper()
        {
            this.SetRequestData(new NameValueCollection());
        }

        public static RequestWrapper Instance()
        {
            return new RequestWrapper();
        }

        public static RequestWrapper InstanceFromRequest()
        {
            var request = new NameValueCollection(HttpContext.Current.Request.QueryString);
            return new RequestWrapper(request);
        }
        #endregion
    }
}