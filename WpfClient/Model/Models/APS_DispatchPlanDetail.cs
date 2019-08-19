using System;

namespace Model.Models
{
	/// <summary>
	/// APS_DispatchPlanDetail表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class APS_DispatchPlanDetail
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 任务名称
		/// </summary>
		public string TaskName { get; set; } 

		/// <summary>
		/// 所属调度计划编号
		/// </summary>
		public string PlanCode { get; set; } 

		/// <summary>
		/// 最早开始时间
		/// </summary>
		public DateTime? EarliestStartTime { get; set; } 

		/// <summary>
		/// 最早结束时间
		/// </summary>
		public DateTime? EarliestFinishTime { get; set; } 

		/// <summary>
		/// 最晚开始时间
		/// </summary>
		public DateTime? LatestStartTime { get; set; } 

		/// <summary>
		/// 最晚结束时间
		/// </summary>
		public DateTime? LatestFinishTime { get; set; } 

		/// <summary>
		/// 计划开始时间
		/// </summary>
		public DateTime? PlanStartTime { get; set; } 

		/// <summary>
		/// 计划结束时间
		/// </summary>
		public DateTime? PlanFinishTime { get; set; } 

		/// <summary>
		/// 浮动工时
		/// </summary>
		public int? FloatHour { get; set; } 

		/// <summary>
		/// 工时定额
		/// </summary>
		public int? WorkHour { get; set; } 

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
		/// 排序
		/// </summary>
		public int? Seq { get; set; } 

	}
}
