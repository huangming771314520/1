using System;

namespace Model.Models
{
	/// <summary>
	/// EQP_EquipmentMaintenancePlan表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class EQP_EquipmentMaintenancePlan
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string MaintenancePlanCode { get; set; } 

		/// <summary>
		/// 维护标准ID
		/// </summary>
		public int? EquipmentMaintenanceID { get; set; } 

		/// <summary>
		/// 设备ID
		/// </summary>
		public string EquipmentCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string EquipmentName { get; set; } 

		/// <summary>
		/// 维护类型
		/// </summary>
		public int? MaintenanceType { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string MaintenanceName { get; set; } 

		/// <summary>
		/// 计划开始时间
		/// </summary>
		public DateTime? PlanedStartTime { get; set; } 

		/// <summary>
		/// 计划结束时间
		/// </summary>
		public DateTime? PlanedFinishTime { get; set; } 

		/// <summary>
		/// 实际开始时间
		/// </summary>
		public DateTime? ActualStartTime { get; set; } 

		/// <summary>
		/// 实际结束时间
		/// </summary>
		public DateTime? ActualFinishTime { get; set; } 

		/// <summary>
		/// 计划维护内容
		/// </summary>
		public string PlanedContent { get; set; } 

		/// <summary>
		/// 实际维护内容描述
		/// </summary>
		public string ActualContent { get; set; } 

		/// <summary>
		/// 维护人
		/// </summary>
		public string MaintenanceMan { get; set; } 

		/// <summary>
		/// 维护计划状态
		/// </summary>
		public int? MaintenanceState { get; set; } 

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
