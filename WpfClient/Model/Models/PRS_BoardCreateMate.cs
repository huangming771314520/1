using System;

namespace Model.Models
{
	/// <summary>
	/// PRS_BoardCreateMate表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class PRS_BoardCreateMate
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public long ID { get; set; } 

		/// <summary>
		/// 合同编号
		/// </summary>
		public string ContractCode { get; set; } 

		/// <summary>
		/// 零件编号
		/// </summary>
		public string InventoryCode { get; set; } 

		/// <summary>
		/// 零件名称
		/// </summary>
		public string InventoryName { get; set; } 

		/// <summary>
		/// 型号
		/// </summary>
		public string Model { get; set; } 

		/// <summary>
		/// 规格
		/// </summary>
		public string Specs { get; set; } 

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
		/// 修改人
		/// </summary>
		public string ModifyPerson { get; set; } 

		/// <summary>
		/// 修改时间
		/// </summary>
		public DateTime? ModifyTime { get; set; } 

		/// <summary>
		/// 要料数量
		/// </summary>
		public string InventoryNum { get; set; } 

		/// <summary>
		/// 单位
		/// </summary>
		public string Unit { get; set; } 

		/// <summary>
		/// 新零件编号
		/// </summary>
		public string New_InventoryCode { get; set; } 

		/// <summary>
		/// 新零件名称
		/// </summary>
		public string New_InventoryName { get; set; } 

		/// <summary>
		/// 新型号
		/// </summary>
		public string New_Model { get; set; } 

		/// <summary>
		/// 新规格
		/// </summary>
		public string New_Specs { get; set; } 

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark { get; set; } 

	}
}
