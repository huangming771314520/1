using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class QMS_BN_InspectionEquipmentService : ServiceBase<QMS_BN_InspectionEquipment>
    {
        public string GetInspectionEquipmentCodeByID(string ID)
        {
            var pQuery = ParamQuery.Instance()
                .Select("InspectionEquipmentCode")
                .AndWhere("ID", ID);
            var list = base.GetModelList(pQuery);
            if (list.Count > 0)
            {
                return list[0].InspectionEquipmentCode;
            }
            else
                return string.Empty;
        }

        public string GetInspectionEquipmentNameByID(string ID)
        {
            var pQuery = ParamQuery.Instance()
                .Select("InspectionEquipmentName")
                .AndWhere("ID", ID);
            var list = base.GetModelList(pQuery);
            if (list.Count > 0)
            {
                return list[0].InspectionEquipmentName;
            }
            else
                return string.Empty;
        }


    }

    public class QMS_BN_InspectionEquipment : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ID { get; set; }
        public string InspectionEquipmentCode { get; set; }
        public string InspectionEquipmentName { get; set; }
        public int? InspectionEquipmenState { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? MoodifyTime { get; set; }

        public string SpecModel { get; set; }

        public string FactoryName { get; set; }

        public string FactoryNumber { get; set; }

        public string EquipmentCode { get; set; }

        public string PrecisionLevel { get; set; }

        public DateTime? PurchaseDate { get; set; }
    }
}
