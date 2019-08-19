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
    public class LoginLogService : ServiceBase<LoginLog>
    {
        public ResultModel Insert(string uCode, ref string token, int type = 0)
        {
            using (var db = Db.Context("Mms"))
            {
                token = WinFormClientService.GetMD5WithString((DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1))).TotalSeconds.ToString());
                LoginLog model = new LoginLog()
                {
                    Token = token,
                    Type = type,
                    UserCode = uCode,
                    CreateDate = DateTime.Now
                };

                string sql = WinFormClientService.GetInsertSQL(model);
                int count = db.Sql(sql).Execute();

                return new ResultModel()
                {
                    Result = count > 0
                };
            }
        }

        public string GetTokenByUCode(string uCode, int type = 0)
        {
            using (var db = Db.Context("Mms"))
            {
                var loginLog = db.Sql(string.Format(@"select top 1 * from LoginLog where [Type] = {0} and UserCode = '{1}' order by CreateDate desc", type, uCode)).QuerySingle<LoginLog>();
                return loginLog == null ? string.Empty : loginLog.Token;
            }
        }

    }

    public class LoginLog : ModelBase
    {
        [PrimaryKey]
        [Identity]

        /// <summary>
		/// 主键ID
		/// </summary>
		public long ID { get; set; }

        /// <summary>
        /// token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 类型  0:CS端  1:BS端
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 登录人编号
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}