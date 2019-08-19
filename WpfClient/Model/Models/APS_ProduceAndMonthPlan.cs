using System;

namespace Model.Models
{
	/// <summary>
	/// APS_ProduceAndMonthPlan表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class APS_ProduceAndMonthPlan
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public long ID { get; set; } 

		/// <summary>
		/// 生产计划编号
		/// </summary>
		public string ProducePlanCode { get; set; } 

		/// <summary>
		/// 月度计划编号
		/// </summary>
		public string MonthPlanCode { get; set; } 

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? CreateTime { get; set; } 

		/// <summary>
		/// 创建人
		/// </summary>
		public string CreatePerson { get; set; } 

	}
}
