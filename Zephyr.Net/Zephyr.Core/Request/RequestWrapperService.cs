/*************************************************************************
 * 文件名称 ：RequestWrapperService.cs                          
 * 描述说明 ：请求包装 服务
 * 
 * 
 * 创建信息 : create by shiruizhi@qq.com on 2013 (施睿智)
 * 修订信息 : 
**************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Zephyr.Core
{
    public partial class RequestWrapper
    {
        public ServiceBase GetService()
        {
            var module = GetXmlNodeValue("module"); //var area = "";
            return new ServiceBase(module);
        }
    }
}