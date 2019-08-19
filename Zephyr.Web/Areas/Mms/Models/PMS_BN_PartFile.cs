using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class PMS_BN_PartFileService : ServiceBase<PMS_BN_PartFile>
    {
        public List<SYS_BOM> GetBom(string parentCode)
        {
            List<SYS_BOM> boomlist = new List<SYS_BOM>();
            string sql = String.Format(@"select * from SYS_BOM where PartCode='{0}' ", parentCode);
            var list = db.Sql(sql).QueryMany<SYS_BOM>();
            while (list.Count != 0)
            {
                boomlist.AddRange(list);
                var part = "(";
                foreach (var item in list)
                {
                    part += "'" + item.PartCode + "',";
                }
                part = part.Remove(part.Length - 1, 1);
                part += ")";
                sql = String.Format(@"select * from SYS_BOM where ParentCode in {0} ", part);
                list = db.Sql(sql).QueryMany<SYS_BOM>();
            }
            return boomlist;

            //  string sql = String.Format(@"select * from SYS_BOM where ParentCode='{0}' or PartCode='{0}' ", parentCode);
            //return  db.Sql(sql).QueryMany<dynamic>();
        }
       
        public int PartFileSubmit(string ids, out string msg)
        {
            msg = "操作失败！";

            string sql = string.Format(@"update PMS_BN_PartFile set State='1' where ID in {0}", ids);
            var rowsAffected = db.Sql(sql).Execute();

            if (rowsAffected > 0)
            {
                msg = "操作成功！";
            }
            return rowsAffected;
        }
    }

    public class PMS_BN_PartFile : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ID { get; set; }
        public string ContractCode { get; set; }
        public string ProjectID { get; set; }
        public string ProjectDetailID { get; set; }
        public string ProjectName { get; set; }
        public string PPartCode { get; set; }
        public string PPartName { get; set; }
        public string PartCode { get; set; }
        public string PartName { get; set; }
        public string DocName { get; set; }
        public string FileName { get; set; }
        public string FileAddress { get; set; }
        public int State { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string FileType { get; set; }
    }
}
