using System;

namespace Model.Models
{
	/// <summary>
	/// PRS_BlankingRecord表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class PRS_BlankingRecord
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 合同编号
		/// </summary>
		public string ContractCode { get; set; } 

		/// <summary>
		/// 零件编码
		/// </summary>
		public string PartCode { get; set; } 

		/// <summary>
		/// 零件图号
		/// </summary>
		public string FigureCode { get; set; } 

		/// <summary>
		/// 零件名称
		/// </summary>
		public string PartName { get; set; } 

		/// <summary>
		/// 单件数量
		/// </summary>
		public int? SingleQuantity { get; set; } 

		/// <summary>
		/// 总数量
		/// </summary>
		public int? TotalQuantity { get; set; } 

		/// <summary>
		/// 材料
		/// </summary>
		public string MaterialCode { get; set; } 

		/// <summary>
		/// 到位尺寸
		/// </summary>
		public string InPlanceSize { get; set; } 

		/// <summary>
		/// 下料尺寸
		/// </summary>
		public string BlankingSize { get; set; } 

		/// <summary>
		/// 工艺流程
		/// </summary>
		public string Process { get; set; } 

		/// <summary>
		/// 批次号
		/// </summary>
		public string BatchNumber { get; set; } 

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark { get; set; } 

		/// <summary>
		/// 是否可用
		/// </summary>
		public int? IsEnable { get; set; } 

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? CreateTime { get; set; } 

		/// <summary>
		/// 创建人
		/// </summary>
		public string CreatePerson { get; set; } 

		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime? ModifyTime { get; set; } 

		/// <summary>
		/// 更新人
		/// </summary>
		public string ModifyPerson { get; set; } 

		/// <summary>
		/// 已下料数量
		/// </summary>
		public int? BlankedQuantity { get; set; } 

		/// <summary>
		/// 未下料数量
		/// </summary>
		public int? NoBlankingQuantity { get; set; } 

		/// <summary>
		/// 物料编码
		/// </summary>
		public string InventoryCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public int? BillState { get; set; } 

	}
}
