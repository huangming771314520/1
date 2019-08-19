/*************************************************************************
 * 文件名称 ：ServiceBaseLog.cs                          
 * 描述说明 ：定义数据服务基类中的日志处理
 * 
 * 创建信息 : create by shiruizhi@qq.com on 2012-11-10 (施睿智)
 * 修订信息 : 
 **************************************************************************/

using System;
using log4net;
using Newtonsoft.Json;

namespace Zephyr.Core
{
    public partial class ServiceBase<T> where T : ModelBase, new()
    {
        protected static ILog Log = LogManager.GetLogger(String.Format("Service{0}", typeof(T).Name));

        protected static void Logger(string function, Action tryHandle, Action<Exception> catchHandle = null, Action finallyHandle = null)
        {
            LogHelper.Logger( Log, function, ErrorHandle.Throw, tryHandle, catchHandle, finallyHandle);
        }

        protected static void Logger(string function, ErrorHandle errorHandleType, Action tryHandle, Action<Exception> catchHandle = null, Action finallyHandle = null)
        {
            LogHelper.Logger( Log, function, errorHandleType, tryHandle, catchHandle, finallyHandle);
        }

        public void Logger(string position,string target,string type,object message) 
        {
            using (var context = Db.Context().UseSharedConnection(true))
            {
                var user = FormsAuth.GetUserData();
                context.Insert("sys_log")
                    .Column("UserCode", user.UserCode)
                    .Column("UserName", user.UserName)
                    .Column("Position", position)
                    .Column("Target", target)
                    .Column("Type", type)
                    .Column("Message", JsonConvert.SerializeObject(message.ToString().Replace("'","&")))
                    .Column("Date", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"))
                    .Execute();
//                string sql = string.Empty;
//                sql = string.Format(@"insert into [Zephyr.Sys].[dbo].[sys_log](UserCode,UserName,Position,Target,Type,Message,Date)
//                values('{0}','{1}','{2}','{3}','{4}','{5}',getdate())",user.UserCode,user.UserName,position,target,type,message);
//                int i = 0;
//                i = context.Insert(sql).Execute();
            }
        }
    }
}
