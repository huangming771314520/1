using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class EQP_FailureReleaseService : ServiceBase<EQP_FailureRelease>
    {
        public List<EQP_FailureRelease> GetLoadData() 
        {  
             
            string sql;
            sql = "select * from EQP_FailureRelease where FailureState=2;";
            List<EQP_FailureRelease> reportList = db.Sql(sql).QueryMany<EQP_FailureRelease>();
            return reportList;

        }
        public int ChangeFailureState(int? id, out string msg)
        {

            string sql;
            sql = "select * from EQP_FailureRelease where FailureState=2;";
            sql = string.Format(@"update EQP_FailureRelease set FailureState='2' where ID='{0}'", id);
            int res = db.Sql(sql).Execute();
            if (res>0)
            {
                msg = "操作失败";
                return res;
            }
            else{
                msg = "操作成功";
                 return res;
            }
           

        }

        public int RemoveFailureState(int id, out string msg)
        {
            string sql;
            int rowsAffected = 0;

            db.UseTransaction(true);
            sql = string.Format(@"update EQP_FailureRelease set FailureState='3' where ID='{0}'", id);
            string selectSql = string.Format(@" select b.* from EQP_FailureRelease a right join EQP_FailureReport b on a.FailureReportID=b.ID where a.ID='{0}'", id);
            var failureObj = db.Sql(selectSql).QuerySingle<EQP_FailureReport>();
            rowsAffected = db.Sql(sql).Execute();
            if (rowsAffected < 0)
            {
                db.Rollback();
                msg = "提交报告处理单失败";
                return rowsAffected;
            }
            sql = string.Format(@"update SYS_Equipment set EquipmentState='1' where ID='{0}'", failureObj.ID);
            rowsAffected = db.Sql(sql).Execute();
            if (rowsAffected < 0)
            {
                db.Rollback();
                msg = "更改设备状态失败";
                return rowsAffected;
            }

            msg = "操作成功！";
            db.Commit();
            return rowsAffected;



        }

    }

    public class EQP_FailureRelease : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public string FailureRemoveCode { get; set; }
        
        public int? FailureReportID { get; set; }
        public DateTime? ReleaseTime { get; set; }
        public string FailRemoveDescription { get; set; }
        public string FailRemoveMan { get; set; }
        public int? FailureState { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}
