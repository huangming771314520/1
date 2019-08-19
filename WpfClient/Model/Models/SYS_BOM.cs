using System;

namespace Model.Models
{
	/// <summary>
	/// SYS_BOM表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class SYS_BOM
	{

		/// <summary>
		/// ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 零件图号
		/// </summary>
		public string PartFigureCode { get; set; } 

		/// <summary>
		/// 零件编码
		/// </summary>
		public string PartCode { get; set; } 

		/// <summary>
		/// 零件名称
		/// </summary>
		public string PartName { get; set; } 

		/// <summary>
		/// 库存编号
		/// </summary>
		public string InventoryCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string InventoryName { get; set; } 

		/// <summary>
		/// 零件数量
		/// </summary>
		public int? PartQuantity { get; set; } 

		/// <summary>
		/// 父级编码
		/// </summary>
		public string ParentCode { get; set; } 

		/// <summary>
		/// 重量
		/// </summary>
		public string Weight { get; set; } 

		/// <summary>
		/// 总重量
		/// </summary>
		public string Totalweight { get; set; } 

		/// <summary>
		/// 材料编码
		/// </summary>
		public string MaterialCode { get; set; } 

		/// <summary>
		/// 材料库存编号
		/// </summary>
		public string MaterialInventoryCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string MaterialInventoryName { get; set; } 

		/// <summary>
		/// 材料数量
		/// </summary>
		public int? MaterialQuantity { get; set; } 

		/// <summary>
		/// 版本编号
		/// </summary>
		public string VersionCode { get; set; } 

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
		public string IsSelfMade { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string PartType { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string Remark { get; set; } 

		/// <summary>
		/// 排序
		/// </summary>
		public int? Sort { get; set; } 

	}
}
