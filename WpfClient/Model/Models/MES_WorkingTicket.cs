using System;

namespace Model.Models
{
	/// <summary>
	/// MES_WorkingTicket表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class MES_WorkingTicket
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public long ID { get; set; } 

		/// <summary>
		/// 工票编码
		/// </summary>
		public string WorkTicketCode { get; set; } 

		/// <summary>
		/// 计划编码
		/// </summary>
		public string ApsCode { get; set; } 

		/// <summary>
		/// 工步ID
		/// </summary>
		public long? WorkStepsID { get; set; } 

		/// <summary>
		/// 是否可用
		/// </summary>
		public int? IsEnable { get; set; } 

		/// <summary>
		/// 车间编码
		/// </summary>
		public string WorkshopCode { get; set; } 

		/// <summary>
		/// 车间名称
		/// </summary>
		public string WorkshopName { get; set; } 

		/// <summary>
		/// 班组编码
		/// </summary>
		public string TeamCode { get; set; } 

		/// <summary>
		/// 班组名称
		/// </summary>
		public string TeamName { get; set; } 

		/// <summary>
		/// 创建人
		/// </summary>
		public DateTime? CreateTime { get; set; } 

		/// <summary>
		/// 创建时间
		/// </summary>
		public string CreatePerson { get; set; } 

		/// <summary>
		/// 更新人
		/// </summary>
		public DateTime? ModifyTime { get; set; } 

		/// <summary>
		/// 更新时间
		/// </summary>
		public string ModifyPerson { get; set; } 

		/// <summary>
		/// 实际开始时间
		/// </summary>
		public DateTime? ActualStartTime { get; set; } 

		/// <summary>
		/// 实际结束时间
		/// </summary>
		public DateTime? ActualFinishTime { get; set; } 

		/// <summary>
		/// 审核状态
		/// </summary>
		public int? ApproveState { get; set; } 

		/// <summary>
		/// 审核人
		/// </summary>
		public string ApprovePerson { get; set; } 

		/// <summary>
		/// 审核时间
		/// </summary>
		public DateTime? ApproveTime { get; set; } 

		/// <summary>
		/// 零件编码
		/// </summary>
		public string PartCode { get; set; } 

		/// <summary>
		/// 派工数量
		/// </summary>
		public int? WorkQuantity { get; set; } 

		/// <summary>
		/// 工序编码
		/// </summary>
		public string ProcessCode { get; set; } 

		/// <summary>
		/// 工序名称
		/// </summary>
		public string ProcessName { get; set; } 

		/// <summary>
		/// 工步名称
		/// </summary>
		public string WorkStepsName { get; set; } 

		/// <summary>
		/// 工步行号
		/// </summary>
		public int? WorkStepsLineCode { get; set; } 

		/// <summary>
		/// 工步描述
		/// </summary>
		public string WorkStepsContent { get; set; } 

		/// <summary>
		/// 设备编码
		/// </summary>
		public string EquipmentCode { get; set; } 

		/// <summary>
		/// 设备名称
		/// </summary>
		public string EquipmentName { get; set; } 

		/// <summary>
		/// 转序目标
		/// </summary>
		public string TurnTargetName { get; set; } 

		/// <summary>
		/// 转序目标编码
		/// </summary>
		public string TurnTargetCode { get; set; } 

		/// <summary>
		/// 工票优先级：1.一般 2.重要 3.必须完成
		/// </summary>
		public int? TicketLevel { get; set; } 

		/// <summary>
		/// 下发备注
		/// </summary>
		public string ApproveRemark { get; set; } 

		/// <summary>
		/// 工票状态：1.未执行 2.已开工 3.完工 4.已开工未完成
		/// </summary>
		public int? TicketStatus { get; set; } 

	}
}
