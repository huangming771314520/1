using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class QMS_BD_CheckItemsService : ServiceBase<QMS_BD_CheckItems>
    {
        public List<dynamic> GetCheckItemTree(string inspectionTypeCode)
        {
            using (var db = Db.Context("Mms"))
            {
                //string sql = string.Format("select CheckItemCode as id,InspectionTypeCode as pid,CheckItemCode+' '+CheckItemName as text from QMS_BD_CheckItems where CheckItemCode='{0}' or InspectionTypeCode='{0}'", inspectionTypeCode);
                string sql = "select CheckItemCode as id,InspectionTypeCode as pid,CheckItemCode+' '+CheckItemName as text from QMS_BD_CheckItems";

                List<dynamic> list = db.Sql(sql).QueryMany<dynamic>();
                return list;
            }
        }
       
    }

    public class QMS_BD_CheckItems : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public string CheckItemCode { get; set; }
        public string CheckItemName { get; set; }
        public string InspectionTypeCode { get; set; }
        public string InspectionTypeName { get; set; }
        public string Remark { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}
