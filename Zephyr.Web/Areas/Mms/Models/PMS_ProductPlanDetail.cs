using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class PMS_ProductPlanDetailService : ServiceBase<PMS_ProductPlanDetail>
    {
        public int Insert(PMS_ProductPlanDetail model)
        {
            var rowsAffected = model.ID = db.Insert<PMS_ProductPlanDetail>("PMS_ProductPlanDetail", model)
     .AutoMap(x => x.ID)
     .ExecuteReturnLastId<int>();
            return rowsAffected;
        }
        public int Update(PMS_ProductPlanDetail model)
        {
            string sql = String.Format(@"select * from PMS_ProductPlanDetail where ID='{0}' ", model.ID);
            var single = db.Sql(sql).QuerySingle<PMS_ProductPlanDetail>();
            model.CreatePerson = single.CreatePerson;
            model.CreateTime = single.CreateTime;
            model.ModifyPerson = MmsHelper.GetUserCode();
            model.ModifyTime = DateTime.Now;
            var rowsAffected = db.Update<PMS_ProductPlanDetail>("PMS_ProductPlanDetail", model)
     .AutoMap(x => x.ID)
     .Where(x => x.ID)
     .Execute();
            return rowsAffected;
        }
    }

    public class PMS_ProductPlanDetail : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public int MainID { get; set; }
        public string ContractCode { get; set; }
        public string ProductID { get; set; }
        public int? ProductQuantity { get; set; }
        public DateTime? PlanFinishDate { get; set; }
        public int? BillState { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string ProductPlanCode { get; set; }
        public int? CompleteQuantity { get;set; }
    }
}
