using System;

namespace Model.Models
{
	/// <summary>
	/// MES_WorkTicketMate表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class MES_WorkTicketMate
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public long ID { get; set; } 

		/// <summary>
		/// 车间工票编码
		/// </summary>
		public string WorkTicketCode { get; set; } 

		/// <summary>
		/// 物料编码
		/// </summary>
		public string InventoryCode { get; set; } 

		/// <summary>
		/// 物料名称
		/// </summary>
		public string InventoryName { get; set; } 

		/// <summary>
		/// 需求数量
		/// </summary>
		public int? RequiredQuantity { get; set; } 

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
		/// 车间编码
		/// </summary>
		public string WorhshopCode { get; set; } 

		/// <summary>
		/// 车间名称
		/// </summary>
		public string WorkshopName { get; set; } 

		/// <summary>
		/// 零件编码
		/// </summary>
		public string PartCode { get; set; } 

		/// <summary>
		/// 父级零件编码
		/// </summary>
		public string ParentCode { get; set; } 

		/// <summary>
		/// 是否关键件
		/// </summary>
		public int? IsCrux { get; set; } 

	}
}
