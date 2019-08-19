using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class PMS_DesignTaskDetailService : ServiceBase<PMS_DesignTaskDetail>
    {
        public int Insert(PMS_DesignTaskDetail model)
        {
            var rowsAffected = model.ID = db.Insert<PMS_DesignTaskDetail>("PMS_DesignTaskDetail", model)
     .AutoMap(x => x.ID)
     .ExecuteReturnLastId<int>();
            return rowsAffected;
        }
        public int Update(PMS_DesignTaskDetail model)
        {
            string sql = String.Format(@"select * from PMS_DesignTaskDetail where ID='{0}' ", model.ID);
            var single = db.Sql(sql).QuerySingle<PMS_DesignTaskDetail>();
            model.CreatePerson = single.CreatePerson;
            model.CreateTime = single.CreateTime;
            model.ModifyPerson = MmsHelper.GetUserCode();
            model.ModifyTime = DateTime.Now;

            var rowsAffected = db.Update<PMS_DesignTaskDetail>("PMS_DesignTaskDetail", model)
     .AutoMap(x => x.ID)
     .Where(x => x.ID)
     .Execute();
            return rowsAffected;
        }
    }

    public class PMS_DesignTaskDetail : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ID { get; set; }
        public int MainID { get; set; }
        public string DesignTaskCode { get; set; }
        public string ContractCode { get; set; }
        public string ProductID { get; set; }
        public string PartCode { get; set; }
        /// <summary>
        /// 设计任务类型
        /// </summary>
        public int? TaskType { get; set; }
        /// <summary>
        /// 任务类型 1：新建项目设计任务 2：已有项目设计更改申请
        /// </summary>
        public int? DesignType { get; set; }
        public string VersionCode { get; set; }
        public string TaskDescription { get; set; }
        public DateTime? TaskStartDate { get; set; }
        public DateTime? TaskFinishDate { get; set; }
        public DateTime? ActualFinishTime { get; set; }
        //public string PartCode { get; set; }
        public int? BillState { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string BillCode { get; set; }
        public string DesignDepartment { get; set; }
    }
}
