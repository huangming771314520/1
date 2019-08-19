using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class MES_WorkshopPurchaseMainService : ServiceBase<MES_WorkshopPurchaseMain>
    {
        public int GetDelete2(string id)
        {
            string sql = "update MES_WorkshopPurchaseMain set IsEnable=0 where id=" + id;
            db.Sql(sql).Execute();
            return 1;
        }


    }

    public class MES_WorkshopPurchaseMain : ModelBase
    {
        [PrimaryKey]
        public int ID { get; set; }
        public string WorkshopPurchaseCode { get; set; }
        public string WorkshopID { get; set; }
        public string WorkshopName { get; set; }
        public int? PurchaseStatus { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string ApproveState { get; set; }
        public string ApprovePerson { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string ApproveRemark { get; set; }
        public string ContractCode { get; set; }

    }
}
