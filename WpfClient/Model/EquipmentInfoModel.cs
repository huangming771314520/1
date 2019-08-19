using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;


namespace Model
{
    public class EquipmentInfoModel
    {
        public bool Result { get; set; }

        public string Msg { get; set; }

        public List<DataModel> Data { get; set; }

        public class DataModel
        {
            public SYS_Equipment GetEquipment { get; set; }

            public List<SYS_EquipmentDetail> GetEquipmentDetail { get; set; }

            public EQP_EquipmentMaintenancePlan GetEquipmentMaintenancePlan { get; set; }
        }
    }
}
