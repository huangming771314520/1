/*************************************************************************
 * 文件名称 ：ZException.cs                          
 * 描述说明 ：框架错误定义
 * 
 * 创建信息 : create by shiruizhi@qq.com on 2012-11-10 (施睿智)
 * 修订信息 : 
 **************************************************************************/

using System;

namespace Zephyr.Core
{
	public class ZException : Exception
	{
		public ZException(string message): base(message){}
        public ZException(string message, Exception innerException) : base(message, innerException) { }
	}
}
