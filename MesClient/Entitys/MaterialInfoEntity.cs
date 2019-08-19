using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace MesClient.Entitys
{
    public class MaterialInfoEntity
    {
        public OneSelfModel MaterialOneSelf { get; set; }

        public ChildModel MaterialChild { get; set; }

        public class OneSelfModel
        {
            public bool IsCastingOrForging { get; set; }

            public MaterialInfoModel MaterialInfo { get; set; }
        }

        public class ChildModel
        {
            public List<MaterialInfoModel> MaterialInfos { get; set; }
        }

        public class MaterialInfoModel
        {
            public string PartCode { get; set; }

            public string PartName { get; set; }

            public string FigureNo { get; set; }

            public int TotalQuantity { get; set; }

            public int AlreadyScanQuantity { get; set; }

            public string MaterialCode { get; set; }

            public ExtraInfoModel ExtraInfo { get; set; }

            public class ExtraInfoModel
            {
                public PartModel PartInfo { get; set; }

                public BomModel BomInfo { get; set; }

                public class PartModel
                {
                    /// <summary>
                    /// 零件信息ID
                    /// </summary>
                    public int ID { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public string InventoryCode { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public string InventoryName { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public int? MaxStock { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public int? MinStock { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public string Weight { get; set; }

                    /// <summary>
                    /// 零件编码
                    /// </summary>
                    public string PartCode { get; set; }

                    /// <summary>
                    /// 零件名称
                    /// </summary>
                    public string PartName { get; set; }

                    /// <summary>
                    /// 零件类型ID
                    /// </summary>
                    public string PartTypeID { get; set; }

                    /// <summary>
                    /// 规格
                    /// </summary>
                    public string Spec { get; set; }

                    /// <summary>
                    /// 型号
                    /// </summary>
                    public string Model { get; set; }

                    /// <summary>
                    /// 安全库存
                    /// </summary>
                    public int? SafetyStock { get; set; }

                    /// <summary>
                    /// 请购提前期
                    /// </summary>
                    public int? PurchaseAdvanceTime { get; set; }

                    /// <summary>
                    /// 经济批量
                    /// </summary>
                    public int? EconomicBatchQuantity { get; set; }

                    /// <summary>
                    /// 最小包装量
                    /// </summary>
                    public int? MinPackQuantity { get; set; }

                    /// <summary>
                    /// 紧急采购批量
                    /// </summary>
                    public int? EnconanmicOrderQuantity { get; set; }

                    /// <summary>
                    /// 数量单位
                    /// </summary>
                    public string QuantityUnit { get; set; }

                    /// <summary>
                    /// 图号
                    /// </summary>
                    public string FigureCode { get; set; }

                    /// <summary>
                    /// 质量批号
                    /// </summary>
                    public string QualityCode { get; set; }

                    /// <summary>
                    /// 对应条码
                    /// </summary>
                    public string CorrespondingBarcode { get; set; }

                    /// <summary>
                    /// 是否自制件
                    /// </summary>
                    public bool? IsSelfMade { get; set; }

                    /// <summary>
                    /// 是否采购供应
                    /// </summary>
                    public bool? IsSupplyMade { get; set; }

                    /// <summary>
                    /// 是否铸锻配套
                    /// </summary>
                    public bool? IsCastforgeMatch { get; set; }

                    /// <summary>
                    /// 是否外协焊接
                    /// </summary>
                    public bool? IsOutsouceWeiding { get; set; }

                    /// <summary>
                    /// 是否电液配套
                    /// </summary>
                    public bool? IsElectroHydraulicMatch { get; set; }

                    /// <summary>
                    /// 是否门市配套
                    /// </summary>
                    public bool? IsMarketMatch { get; set; }

                    /// <summary>
                    /// 仓库ID
                    /// </summary>
                    public string WarehouseCode { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public string WarehouseName { get; set; }

                    /// <summary>
                    /// 是否可用
                    /// </summary>
                    public int? IsEnable { get; set; }

                    /// <summary>
                    /// 创建人
                    /// </summary>
                    public string CreatePerson { get; set; }

                    /// <summary>
                    /// 创建时间
                    /// </summary>
                    public DateTime? CreateTime { get; set; }

                    /// <summary>
                    /// 更新人
                    /// </summary>
                    public string ModifyPerson { get; set; }

                    /// <summary>
                    /// 更新时间
                    /// </summary>
                    public DateTime? ModifyTime { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public string SimHash { get; set; }
                }

                public class BomModel
                {
                    /// <summary>
                    /// 
                    /// </summary>
                    public int ID { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public string PartFigureCode { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public string PartCode { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public string PartName { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public string InventoryCode { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public string InventoryName { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public int? PartQuantity { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public string ParentCode { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public string Weight { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public string Totalweight { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public string MaterialCode { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public string MaterialInventoryCode { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public string MaterialInventoryName { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public int? MaterialQuantity { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public string IsSelfMade { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public int? IsEnable { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public string CreatePerson { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public DateTime? CreateTime { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public string ModifyPerson { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public DateTime? ModifyTime { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public string ApproveState { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public string ApprovePerson { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public string ApproveDate { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public string ApproveRemark { get; set; }

                    /// <summary>
                    /// 定料材料
                    /// </summary>
                    public string SetMateName { get; set; }

                    /// <summary>
                    /// 定料数量
                    /// </summary>
                    public double? SetMateNum { get; set; }

                    /// <summary>
                    /// 到位尺寸
                    /// </summary>
                    public string InPlanceSize { get; set; }

                    /// <summary>
                    /// 下料尺寸
                    /// </summary>
                    public string BlankingSize { get; set; }

                    /// <summary>
                    /// 材料类型:板材、棒材、其他
                    /// </summary>
                    public int? MateType { get; set; }

                    /// <summary>
                    /// 下料方式:数控切割、锯、手工切割
                    /// </summary>
                    public int? BlankingType { get; set; }

                    /// <summary>
                    /// 规格_新
                    /// </summary>
                    public string New_Specs { get; set; }

                    /// <summary>
                    /// 型号_新
                    /// </summary>
                    public string New_Model { get; set; }

                    /// <summary>
                    /// 名称_新
                    /// </summary>
                    public string New_PartName { get; set; }

                    /// <summary>
                    /// 材料参数：厚度或直径
                    /// </summary>
                    public string MateParamValue { get; set; }

                    /// <summary>
                    /// 合同编号
                    /// </summary>
                    public string ContractCode { get; set; }

                    /// <summary>
                    /// 产品ID
                    /// </summary>
                    public int? ProductID { get; set; }

                    /// <summary>
                    /// 新存货编码
                    /// </summary>
                    public string New_InventoryCode { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public int Blanking { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public int Welding { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public int Machining { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public int Assembling { get; set; }

                    /// <summary>
                    /// 下料总数
                    /// </summary>
                    public int? BlankingTotal { get; set; }

                    /// <summary>
                    /// 下料数量
                    /// </summary>
                    public int? BlankingNum { get; set; }

                    /// <summary>
                    /// 外购数量
                    /// </summary>
                    public int? PurchaseNum { get; set; }

                    /// <summary>
                    /// 改制数量
                    /// </summary>
                    public int? RestructNum { get; set; }

                    /// <summary>
                    /// 排序
                    /// </summary>
                    public int? Sort { get; set; }

                    /// <summary>
                    /// 是否关键件
                    /// </summary>
                    public int? IsCrux { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    public int? PartQuantityAll { get; set; }
                }
            }
        }
    }

    public class ScanMaterialInfoEntity
    {
        public string PartCode { get; set; }

        public string PartName { get; set; }

        public string FigureNo { get; set; }
    }

}
