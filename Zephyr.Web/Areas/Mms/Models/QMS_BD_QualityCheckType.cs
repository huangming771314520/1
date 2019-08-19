using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class QMS_BD_QualityCheckTypeService : ServiceBase<QMS_BD_QualityCheckType>
    {
        public List<dynamic> GetInspectionTypeTree()
        {
            using (var db = Db.Context("Mms"))
            {
                //string sql = string.Format("select CheckItemCode as id,InspectionTypeCode as pid,CheckItemCode+' '+CheckItemName as text from QMS_BD_CheckItems where CheckItemCode='{0}' or InspectionTypeCode='{0}'", inspectionTypeCode);
                string sql = "select InspectionTypeCode as id,PInspectionType as pid,InspectionTypeCode+' '+InspectionTypeName as text from QMS_BD_QualityCheckType " ;

                List<dynamic> list = db.Sql(sql).QueryMany<dynamic>();
                return list;
            }
        }

        public List<dynamic> GetInspectionTypeByCode(string code)
        {
            using (var db = Db.Context("Mms"))
            {
                //string sql = string.Format("select CheckItemCode as id,InspectionTypeCode as pid,CheckItemCode+' '+CheckItemName as text from QMS_BD_CheckItems where CheckItemCode='{0}' or InspectionTypeCode='{0}'", inspectionTypeCode);
                string sql = string.Format("select ID,InspectionTypeCode ,PInspectionType ,InspectionTypeCode,PInspectionName,InspectionTypeName,IsEnable  from QMS_BD_QualityCheckType where InspectionTypeCode={0}or  PInspectionType={0}", code);

                List<dynamic> list = db.Sql(sql).QueryMany<dynamic>();
                return list;
            }
        }

       
    }

    public class QMS_BD_QualityCheckType : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public string InspectionTypeCode { get; set; }
        public string InspectionTypeName { get; set; }
        public string PInspectionType { get; set; }
        public string PInspectionName { get; set; }
        
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}
