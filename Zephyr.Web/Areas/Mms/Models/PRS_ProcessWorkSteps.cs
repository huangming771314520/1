using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class PRS_ProcessWorkStepsService : ServiceBase<PRS_ProcessWorkSteps>
    {
        public void Insert(List<PRS_ProcessWorkSteps> model)
        {
            string sql = string.Format(@"delete PRS_ProcessWorkSteps where ProcessRouteID={0}", model[0].ProcessRouteID);
            db.Sql(sql).Execute();
            foreach (var item in model)
            {
               db.Insert<PRS_ProcessWorkSteps>("PRS_ProcessWorkSteps", item)
    .AutoMap(x => x.ID)
    .ExecuteReturnLastId<int>();
            }
           
        }
        public void Delete(int ProcessRouteID)
        {
            string sql = string.Format(@"delete PRS_ProcessWorkSteps where ProcessRouteID={0}", ProcessRouteID);
            db.Sql(sql).Execute();
        }
    }

    public class PRS_ProcessWorkSteps : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public int? ProcessRouteID { get; set; }
        public int? WorkStepsLineCode { get; set; }
        public string WorkStepsName { get; set; }
        public string WorkStepsContent { get; set; }
        public int? ManHours { get; set; }
        public int? IsEnable { get; set; }
        public int? Unit { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string ModifyPerson { get; set; }
    }
}
