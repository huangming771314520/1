using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class PlanKittingService : ServiceBase<PlanKitting>
    {

        public bool GetUpdatePlanKittingConfig(int id, int assembling, int blanking, int machining, int welding)
        {
            string sql = string.Format(@"UPDATE dbo.PRS_Process_BOM SET Blanking = {0},Welding = {1},Machining = {2},Assembling = {3} WHERE ID = {4}", blanking, welding, machining, assembling, id);

            return db.Sql(sql).Execute() > 0;
        }

        public bool GetUpdatePlanKittingConfig(int id, string name, int value)
        {
            if (new string[] { "Blanking", "Welding", "Machining", "Assembling" }.Contains(name))
            {
                string sql = string.Format(@"UPDATE dbo.PRS_Process_BOM SET {0} = {1} WHERE ID = {2};", name, value, id);
                return db.Sql(sql).Execute() > 0;
            }
            else
            {
                throw new Exception(@"参数错误！");
            }
        }


    }

    public class PlanKitting : ModelBase
    {

    }
}