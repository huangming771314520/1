using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class PRS_BD_StandardProcessService : ServiceBase<PRS_BD_StandardProcess>
    {
       public int PostChangeStandardProcess(int? id,out string msg)
        {
            string sql = string.Format(@"update  PRS_BD_StandardProcess set IsEnable=0 where id={0}", id);
            int res = db.Sql(sql).Execute();
            msg = res > 0 ? "删除成功" : "删除失败";
            return res;
        }
       public string GetMaxCode()
       {
           string sql = @"select isnull(Max(ProcessCode), 0)+1 as maxCode from PRS_BD_StandardProcess";
           string code = db.Sql(sql).QuerySingle<string>();
           if(int.Parse(code)<10)
           {
               code = "00" + code;
           }
           else if(int.Parse(code)<100)
           {
               code = "0" + code;
           }
         
           return code;
       }
    }

    public class PRS_BD_StandardProcess : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }
        public string Instracutions { get; set; }
        public int? IsEnable { get; set; }
        public int? ProcessType { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}
