using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;
using Zephyr.Utils;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class SYS_BN_UserService : ServiceBase<SYS_BN_User>
    {
        protected override void OnAfterHandleExcel(ref DataTable dtSheet)
        {
            var service = new SYS_BN_UserService();

            dtSheet.Columns.Add("CreatePerson", typeof(string));
            dtSheet.Columns.Add("CreateTime", typeof(DateTime));
            dtSheet.Columns.Add("ModifyPerson", typeof(string));
            dtSheet.Columns.Add("ModifyTime", typeof(DateTime));
            dtSheet.Columns.Add("UserBarcode", typeof(string));

            string maxBarCode = db.Sql(@"SELECT ISNULL(MAX(UserBarcode),'USER00000000') FROM dbo.SYS_BN_User WHERE UserBarcode IS NOT NULL AND LTRIM(RTRIM(UserBarcode)) <> ''").QuerySingle<string>();
            long maxNum = Convert.ToInt64(maxBarCode.Substring(4));

            var userCodeAndBarcodeList = db.Sql(@"SELECT UserCode,UserBarcode FROM dbo.SYS_BN_User WHERE UserBarcode IS NOT NULL AND LTRIM(RTRIM(UserBarcode)) <> ''").QueryMany<dynamic>();
            Dictionary<string, string> dicUserCodeAndBarcode = new Dictionary<string, string>();
            userCodeAndBarcodeList.ForEach(item => { dicUserCodeAndBarcode.Add(item.UserCode, item.UserBarcode); });

            var userName = MmsHelper.GetUserName();
            var newDT = DateTime.Now;

            foreach (DataRow item in dtSheet.Rows)
            {
                string dName = item["DepartmentName"].ToString();
                string sql = string.Format(@"select DepartmentCode from SYS_BN_Department where DepartmentName='{0}'", dName);
                string hID = db.Sql(sql).QuerySingle<string>();
                item["DepartmentCode"] = hID;

                string isenable = item["IsEnable"].ToString();
                if (isenable == "否")
                {
                    item["IsEnable"] = 0;
                }
                else
                {
                    item["IsEnable"] = 1;
                }

                var UserCode = item["UserCode"].ToString();

                item["CreatePerson"] = userName;
                item["CreateTime"] = newDT;
                item["ModifyPerson"] = userName;
                item["ModifyTime"] = newDT;

                if (dicUserCodeAndBarcode.ContainsKey(UserCode))
                {
                    item["UserBarcode"] = dicUserCodeAndBarcode[UserCode];
                }
                else
                {
                    Interlocked.Increment(ref maxNum);
                    item["UserBarcode"] = "USER" + maxNum.ToString().PadLeft(8, '0');
                }

                var Query = ParamDelete.Instance().AndWhere("UserCode", UserCode);
                service.Delete(Query);
            }
            base.OnAfterHandleExcel(ref dtSheet);


        }
        public dynamic GetDepartment(string user)
        {
            var pQuery = ParamQuery.Instance()
                .Select("DepartmentCode,DepartmentName").AndWhere("IsEnable ", 1).AndWhere("UserCode", user);

            return base.GetDynamic(pQuery);
        }
        public string GetUserCode()
        {
            string sql = "  select [UserCode] from [SYS_BN_User] where [UserCode] = (select max([UserCode]) from [SYS_BN_User])";
            return db.Sql(sql).QuerySingle<string>();
        }

        public dynamic GetDepartmentInfo(string userCode)
        {
            string sql = string.Format(@"select SYS_BN_Department.* from SYS_BN_User inner join SYS_BN_Department on SYS_BN_User.DepartmentCode=SYS_BN_Department.DepartmentCode where UserCode='{0}'", userCode);
            return db.Sql(sql).QuerySingle<dynamic>();
        }
    }

    public class SYS_BN_User : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ID { get; set; }
        public string UserCode { get; set; }
        public string UserBarcode { get; set; }
        public string UserName { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}
