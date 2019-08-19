using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.WinForm.Models
{
    public class MaterialDetailModel
    {
        public bool result { get; set; }

        public string msg { get; set; }

        public List<DataModel> data { get; set; }

        public class DataModel
        {
            public GetMaterialDetailModel GetMaterialDetail { get; set; }

            public GetPartModel GetPart { get; set; }

            public class GetMaterialDetailModel
            {
                public string PartCode { get; set; }

                public string PartName { get; set; }

                public string ParentCode { get; set; }

                public int PartQuantity { get; set; }

                public string MaterialCode { get; set; }
            }

            public class GetPartModel
            {
                public int ID { get; set; }
                public string InventoryCode { get; set; }
                public string InventoryName { get; set; }
                public string PartCode { get; set; }
                public string PartName { get; set; }
                public int? MinStock { get; set; }
                public int? MaxStock { get; set; }
                public int? PartTypeID { get; set; }
                public string Spec { get; set; }
                public string Model { get; set; }
                public string Weight { get; set; }
                public int? SafetyStock { get; set; }
                public int? PurchaseAdvanceTime { get; set; }
                public int? EconomicBatchQuantity { get; set; }
                public int? MinPackQuantity { get; set; }
                public int? EnconanmicOrderQuantity { get; set; }
                public string QuantityUnit { get; set; }
                public string FigureCode { get; set; }
                public string QualityCode { get; set; }
                public string CorrespondingBarcode { get; set; }
                public bool? IsSelfMade { get; set; }
                public bool? IsSupplyMade { get; set; }
                public bool? IsCastforgeMatch { get; set; }
                public bool? IsOutsouceWeiding { get; set; }
                public bool? IsElectroHydraulicMatch { get; set; }
                public bool? IsMarketMatch { get; set; }
                public string WarehouseCode { get; set; }
                public string WarehouseName { get; set; }
                public int? IsEnable { get; set; }
                public string CreatePerson { get; set; }
                public DateTime? CreateTime { get; set; }
                public string ModifyPerson { get; set; }
                public DateTime? ModifyTime { get; set; }
            }
        }
    }
}
