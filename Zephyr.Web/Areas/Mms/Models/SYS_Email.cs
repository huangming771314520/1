using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class SYS_EmailService : ServiceBase<SYS_Email>
    {
        public List<dynamic> GetEmailTree()
        {
            using (var db = Db.Context("Mms"))
            {
                //string sql = string.Format("select CheckItemCode as id,InspectionTypeCode as pid,CheckItemCode+' '+CheckItemName as text from QMS_BD_CheckItems where CheckItemCode='{0}' or InspectionTypeCode='{0}'", inspectionTypeCode);
                string sql = "select UserCode as id,0 as pid,UserCode+' '+UserName as text from SYS_BN_User ";

                List<dynamic> list = db.Sql(sql).QueryMany<dynamic>();
                return list;
            }
        }

        public List<dynamic> GetUserCode(string code)
        {
            using (var db = Db.Context("Mms"))
            {
                //string sql = string.Format("select CheckItemCode as id,InspectionTypeCode as pid,CheckItemCode+' '+CheckItemName as text from QMS_BD_CheckItems where CheckItemCode='{0}' or InspectionTypeCode='{0}'", inspectionTypeCode);
                string sql = string.Format("select UserCode ,UserName ,DepartmentCode,DepartmentName  from SYS_BN_User where UserCode={0}or  UserName={0}", code);

                List<dynamic> list = db.Sql(sql).QueryMany<dynamic>();
                return list;
            }
        }
    }


    public class SYS_Email : ModelBase
    {
        [PrimaryKey]   
        public int ID { get; set; }
        public string EmailCode { get; set; }
        public string EmailName { get; set; }
        public string Sender { get; set; }
        public string CarbonCopy { get; set; }
        public string Context { get; set; }
        public DateTime? SendTime { get; set; }
        public string Addressee { get; set; }
        public int IsSend { get; set; }
    }
}
