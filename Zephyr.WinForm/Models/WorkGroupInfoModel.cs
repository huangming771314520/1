using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.WinForm.Models
{
    public class WorkGroupInfoModel
    {
        public bool result { get; set; }

        public string msg { get; set; }

        public DataModel data { get; set; }

        public class DataModel
        {
            public GetWorkGroupInfoModel GetWorkGroupInfo { get; set; }

            public GetWarehouseModel GetWarehouse { get; set; }

            public class GetWorkGroupInfoModel
            {
                public int ID { get; set; }
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

            public class GetWarehouseModel
            {
                public int ID { get; set; }
                public string WarehouseCode { get; set; }
                public string WarehouseName { get; set; }
                public string UserCode { get; set; }
                public string StoreKeeper { get; set; }
                public int? WarehouseType { get; set; }
                public int? IsEnable { get; set; }
                public string CreatePerson { get; set; }
                public DateTime? CreateTime { get; set; }
                public string ModifyPerson { get; set; }
                public DateTime? ModifyTime { get; set; }
            }
        }
    }
}
