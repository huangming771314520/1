using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;
using System.Linq;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Areas.Mms.Controllers;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Xml;
using System.IO;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class ClientOperateLogService : ServiceBase<ClientOperateLog>
    {
        public ResultModel SetClientOperateLog(ClientOperateLog model)
        {
            using (var db = Db.Context("Mms"))
            {
                string sql = WinFormClientService.GetInsertSQL(model);
                int count = db.Sql(sql).Execute();

                return new ResultModel()
                {
                    Result = count > 0,
                    Msg = count > 0 ? null : "写入操作日志失败！"
                };
            }
        }

        public ResultModel SetClientOperateLog(List<ClientOperateLog> models)
        {
            using (var db = Db.Context("Mms"))
            {
                StringBuilder sb = new StringBuilder();

                foreach (var item in models)
                {
                    sb.Append(WinFormClientService.GetInsertSQL(item));
                }

                string sql = sb.ToString();
                int count = db.Sql(sql).Execute();

                return new ResultModel()
                {
                    Result = count > 0,
                    Msg = count > 0 ? null : "写入操作日志失败！"
                };
            }
        }




    }

    public class ClientOperateLog : ModelBase
    {
        [PrimaryKey]
        [Identity]

        /// <summary>
		/// 主键ID
		/// </summary>
		public long ID { get; set; }

        /// <summary>
		/// IP地址
		/// </summary>
		public string AddressIP { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 创建人编号
        /// </summary>
        public string CreatePersonCode { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string CreatePersonName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}