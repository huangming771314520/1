using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Zephyr.Areas;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class MES_OperatorStatisticalService : ServiceBase<MES_OperatorStatistical>
    {
        /// <summary>
        /// 记录计划执行操作人
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public dynamic SetProduceOperatorStatistical(List<string> barCodes, int logID)
        {
            try
            {
                var barCodeList = barCodes.Select(item => { return "'" + item + "'"; }).ToList();
                var userList = db.Sql(string.Format(@"SELECT * FROM dbo.SYS_BN_User WHERE IsEnable = 1 AND UserBarcode IN ({0})", string.Join(",", barCodeList))).QueryMany<SYS_BN_User>();

                StringBuilder sb = new StringBuilder();

                foreach (var item in barCodes)
                {
                    var userModel = userList.Single(i => i.UserBarcode.Equals(item));

                    sb.Append(WinFormClientService.GetInsertSQL(new MES_OperatorStatistical()
                    {
                        MainID = logID,
                        OperatorCode = userModel.UserCode,
                        OperatorName = userModel.UserName,
                        OperatorBarCode = userModel.UserBarcode
                    }));
                }

                bool result = db.Sql(sb.ToString()).Execute() > 0;

                return new ResultModel()
                {
                    Result = result,
                    Msg = result ? "保存成功！" : "保存失败！"
                };
            }
            catch (Exception ex)
            {
                return new ResultModel()
                {
                    Result = false,
                    Msg = ex.Message
                };
            }
        }

    }

    public class MES_OperatorStatistical : ModelBase
    {
        [PrimaryKey]
        [Identity]

        /// <summary>
		/// 主键ID
		/// </summary>
		public long ID { get; set; }

        /// <summary>
        /// 主表ID
        /// </summary>
        public int MainID { get; set; }

        /// <summary>
        /// 操作员编号
        /// </summary>
        public string OperatorCode { get; set; }

        /// <summary>
        /// 操作员姓名
        /// </summary>
        public string OperatorName { get; set; }

        /// <summary>
        /// 操作员条码
        /// </summary>
        public string OperatorBarCode { get; set; }
    }
}