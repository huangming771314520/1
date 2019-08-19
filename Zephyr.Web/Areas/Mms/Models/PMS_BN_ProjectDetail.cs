using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class PMS_BN_ProjectDetailService : ServiceBase<PMS_BN_ProjectDetail>
    {
        public int Insert(PMS_BN_ProjectDetail model)
        {
            var rowsAffected = model.ID = db.Insert<PMS_BN_ProjectDetail>("PMS_BN_ProjectDetail", model)
     .AutoMap(x => x.ID)
     .ExecuteReturnLastId<int>();
            return rowsAffected;
        }
        public int Update(PMS_BN_ProjectDetail model)
        {
            string sql = String.Format(@"select * from PMS_BN_ProjectDetail where ID='{0}' ", model.ID);
            var single = db.Sql(sql).QuerySingle<PMS_BN_ProjectDetail>();
            model.CreatePerson = single.CreatePerson;
            model.CreateTime = single.CreateTime;
            model.ModifyPerson = MmsHelper.GetUserCode();
            model.ModifyTime = DateTime.Now;

            var rowsAffected = db.Update<PMS_BN_ProjectDetail>("PMS_BN_ProjectDetail", model)
     .AutoMap(x => x.ID)
     .Where(x => x.ID)
     .Execute();
            return model.ID;
        }
    }

    public class PMS_BN_ProjectDetail : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ID { get; set; }
        public int? MainID { get; set; }
        public string ProductName { get; set; }
        public int? ProductType { get; set; }
        public string Model { get; set; }
        public string Specifications { get; set; }
        public int? Quantity { get; set; }
        public string FigureNumber { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public string Remark { get; set; }
        public int? Urgent { get; set; }
        public int? ProjectState { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }

        public string ProductUnit { get; set; }

        public string TotalWeight { get; set; }

        public string PlanPrice { get; set; }

        public int? CompleteQuantity { get; set; }

    }
}
