using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.WinForm.Models
{
    public class EquipmentMaintenancePlanModel
    {
        public bool result { get; set; }

        public string msg { get; set; }

        public DataModel data { get; set; }

        public class DataModel
        {
            public List<EMaintenancePlanModel> GetEMaintenancePlan { get; set; }

            public class EMaintenancePlanModel
            {
                public int ID { get; set; }

                public string MaintenancePlanCode { get; set; }

                public int? EquipmentMaintenanceID { get; set; }

                public string EquipmentCode { get; set; }

                public string EquipmentName { get; set; }

                public int? MaintenanceType { get; set; }

                public string MaintenanceName { get; set; }

                public DateTime? PlanedStartTime { get; set; }

                public DateTime? PlanedFinishTime { get; set; }

                public DateTime? ActualStartTime { get; set; }

                public DateTime? ActualFinishTime { get; set; }

                public string PlanedContent { get; set; }

                public string ActualContent { get; set; }

                public string MaintenanceMan { get; set; }

                public int? MaintenanceState { get; set; }

                public int? IsEnable { get; set; }

                public string CreatePerson { get; set; }

                public DateTime? CreateTime { get; set; }

                public string ModifyPerson { get; set; }

                public DateTime? ModifyTime { get; set; }
            }
        }
    }
}
