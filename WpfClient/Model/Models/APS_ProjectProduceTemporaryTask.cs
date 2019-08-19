using System;

namespace Model.Models
{
	/// <summary>
	/// APS_ProjectProduceTemporaryTask表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class APS_ProjectProduceTemporaryTask
	{

		/// <summary>
		/// 
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string TemporaryTaskDec { get; set; } 

		/// <summary>
		/// 合同号
		/// </summary>
		public string ContractCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public int? ProjectDetailID { get; set; } 

		/// <summary>
		/// 主计划ID
		/// </summary>
		public int? ProductPlanMainID { get; set; } 

		/// <summary>
		/// 零件编码
		/// </summary>
		public string PartCode { get; set; } 

		/// <summary>
		/// 工序编码
		/// </summary>
		public string ProcessCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ProcessLineCode { get; set; } 

		/// <summary>
		/// 车间ID
		/// </summary>
		public string WorkshopID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string WorkshopName { get; set; } 

		/// <summary>
		/// 设备ID
		/// </summary>
		public string EquipmentID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string EquipmentName { get; set; } 

		/// <summary>
		/// 班组ID
		/// </summary>
		public string WorkGroupID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string WorkGroupName { get; set; } 

		/// <summary>
		/// 生产数量
		/// </summary>
		public int? Quantity { get; set; } 

		/// <summary>
		/// 工时定额
		/// </summary>
		public int? ManHour { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public int? Unit { get; set; } 

		/// <summary>
		/// 最早开始时间
		/// </summary>
		public DateTime? EarliestStartTime { get; set; } 

		/// <summary>
		/// 最晚开始时间
		/// </summary>
		public DateTime? LatestStartTime { get; set; } 

		/// <summary>
		/// 计划开始时间
		/// </summary>
		public DateTime? PlanedStartTime { get; set; } 

		/// <summary>
		/// 最早结束时间
		/// </summary>
		public DateTime? EarliestFinishTime { get; set; } 

		/// <summary>
		/// 最晚结束时间
		/// </summary>
		public DateTime? LatestFinishTime { get; set; } 

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
		/// 浮动工时
		/// </summary>
		public DateTime? FloatingHour { get; set; } 

		/// <summary>
		/// 计划颜色
		/// </summary>
		public string PlanColor { get; set; } 

		/// <summary>
		/// 计划状态
		/// </summary>
		public int? PlanState { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public int? PlanType { get; set; } 

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
		/// 审批状态
		/// </summary>
		public string ApproveState { get; set; } 

		/// <summary>
		/// 审批人
		/// </summary>
		public string ApprovePerson { get; set; } 

		/// <summary>
		/// 审批时间
		/// </summary>
		public DateTime? ApproveDate { get; set; } 

		/// <summary>
		/// 审批备注
		/// </summary>
		public string ApproveRemark { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ApsCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string LaunchWorkshopID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string LaunchWorkshopName { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string BillCode { get; set; } 

	}
}
