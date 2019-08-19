using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;

namespace Model
{
    public class MaterialInfoModel
    {
        public bool Result { get; set; }

        public string Msg { get; set; }

        public List<DataModel> Data { get; set; }

        public class DataModel
        {
            public MaterialDetailModel GetMaterialDetail { get; set; }

            public SYS_Part GetPart { get; set; }

            public PRS_Process_BOM GetProcessBom { get; set; }

            public class MaterialDetailModel
            {
                public string PartCode { get; set; }

                public string PartName { get; set; }

                public string PartFigureCode { get; set; }

                public string ParentCode { get; set; }

                public int PartQuantity { get; set; }

                public int PartAlreadyScanQuantity { get; set; }

                public string MaterialCode { get; set; }

                public bool IsOneSelf { get; set; }

                public bool IsZhuDuanJian { get; set; }
            }
        }
    }

    public class MaterialScanInfoModel
    {
        public bool Result { get; set; }

        public string Msg { get; set; }

        public DataModel Data { get; set; }

        public class DataModel
        {
            public SYS_Part GetPart { get; set; }

            public PRS_Process_BOM GetProcessBom { get; set; }
        }
    }
}
