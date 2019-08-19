using System;

namespace Model.Models
{
	/// <summary>
	/// EQP_EquipmentMaintenance表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class EQP_EquipmentMaintenance
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string MaintenanceCode { get; set; } 

		/// <summary>
		/// 设备编号
		/// </summary>
		public string EquipmentCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string EquipmentName { get; set; } 

		/// <summary>
		/// 所在车间
		/// </summary>
		public int? WorkshopID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string WorkshopName { get; set; } 

		/// <summary>
		/// 维护类型：1：周检 2：月检 3：年修
		/// </summary>
		public int? MaintenanceType { get; set; } 

		/// <summary>
		/// 维护标准
		/// </summary>
		public string MaintenanceStandard { get; set; } 

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

	}
}
