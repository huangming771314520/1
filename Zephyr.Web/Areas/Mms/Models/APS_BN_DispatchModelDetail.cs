using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class APS_BN_DispatchModelDetailService : ServiceBase<APS_BN_DispatchModelDetail>
    {
        public int AuditBillCode(string ID, out string msg)
        {
            msg = string.Empty;

            var rowsAffected = 0;
            string sql = String.Format(@"  select BillState FROM PRS_ProcessRouteModelMain  where ID='{0}'", ID);
            var state = db.Sql(sql).QuerySingle<int>();
            if (state == 1)
            {
                msg = "单据已审核";
                return 0;
            }
            else
            {
                sql = string.Format(@"update PRS_ProcessRouteModelMain set BillState=1 where ID='{0}'", ID);
                rowsAffected = db.Sql(sql).Execute();
                if (rowsAffected > 0)
                {
                    msg = "审核成功";
                    return rowsAffected;
                }
                else
                {
                    msg = "审核失败";
                    return 0;
                }
            }
        }
    }

    public class APS_BN_DispatchModelDetail : ModelBase
    {
        [PrimaryKey]   
        public int ID { get; set; }
        public string Name { get; set; }
        public int? IsEnable { get; set; }
        public string Remark { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? MainID { get; set; }
        public int? TaskCycle { get; set; }
        public int? Seq { get; set; }
    }
}
