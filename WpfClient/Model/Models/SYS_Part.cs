using System;

namespace Model.Models
{
	/// <summary>
	/// SYS_Part表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class SYS_Part
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
}
