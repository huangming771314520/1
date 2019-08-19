using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class APS_DispatchPlanMainService : ServiceBase<APS_DispatchPlanMain>
    {
        public int GetPlanedQuantity(string contractCode, string ProductID, string mainID)
        {
            string sql = String.Format(@"SELECT SUM(PlanQuantity) FROM APS_DispatchPlanMain WHERE ContractCode='{0}' AND ProductID='{1}'", contractCode, ProductID);
            string sql1 = string.Format(@"SELECT Quantity FROM dbo.PMS_BN_ProjectDetail WHERE MainID='{0}' and id='{1}'", mainID, ProductID);
            int totalQuantity = db.Sql(sql1).QuerySingle<int>();
            int planedQuantity = db.Sql(sql).QuerySingle<int>();

            return totalQuantity - planedQuantity;
        }


    }

    public class APS_DispatchPlanMain : ModelBase
    {
        
   
        

        [PrimaryKey]
        public string PlanCode { get; set; }
        public string ContractCode { get; set; }
        public int? ProductID { get; set; }
        public int? PlanQuantity { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string ModelCode { get; set; }
    }
}
