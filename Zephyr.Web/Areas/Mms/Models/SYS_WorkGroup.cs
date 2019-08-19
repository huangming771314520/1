using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class SYS_WorkGroupService : ServiceBase<SYS_WorkGroup>
    {
        public dynamic GetWorkGroupByTCode(string teamCode)
        {
            string sql = string.Format(@"SELECT ID,TeamCode,TeamName,DepartID,DepartName,IsEnable FROM dbo.SYS_WorkGroup WHERE TeamCode = '{0}'", teamCode);
            using (var db = Db.Context("Mms"))
            {
                var list = db.Sql(sql).QueryMany<SYS_WorkGroup>();
                int count = list.Count;
                return new
                {
                    Count = count,
                    Data = new
                    {
                        ID = count > 0 ? list[0].ID : 0,
                        TeamCode = count > 0 ? list[0].TeamCode : string.Empty,
                        TeamName = count > 0 ? list[0].TeamName : string.Empty,
                        DepartID = count > 0 ? list[0].DepartID : string.Empty,
                        DepartName = count > 0 ? list[0].DepartName : string.Empty,
                        IsEnable = count > 0 ? list[0].IsEnable : null
                    }
                };
            }
        }

        public int GetDelete2(string id)
        {
            string sql = "update SYS_WorkGroup set IsEnable=0 where id=" + id;
            db.Sql(sql).Execute();
            sql = "update SYS_WorkGroupDetail set IsEnable=0 where MainID=" + id;
             db.Sql(sql).Execute();
             return 1;
        }
    }

    public class SYS_WorkGroup : ModelBase
    {
        [PrimaryKey]
        public int ID { get; set; }
        public string BillCode { get; set; }
        public string TeamCode { get; set; }
        public string TeamName { get; set; }
        public string DepartID { get; set; }
        public string DepartName { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}
