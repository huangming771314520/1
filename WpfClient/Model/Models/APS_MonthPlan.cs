using System;

namespace Model.Models
{
	/// <summary>
	/// APS_MonthPlan表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class APS_MonthPlan
	{

		/// <summary>
		/// 计划编号
		/// </summary>
		public string PlanCode { get; set; } 

		/// <summary>
		/// 合同编号
		/// </summary>
		public string ContractCode { get; set; } 

		/// <summary>
		/// 产品ID
		/// </summary>
		public int? ProductID { get; set; } 

		/// <summary>
		/// 工程项目
		/// </summary>
		public string ProjectName { get; set; } 

		/// <summary>
		/// 产值
		/// </summary>
		public decimal? ProductiveValue { get; set; } 

		/// <summary>
		/// 产品名称
		/// </summary>
		public string ProductName { get; set; } 

		/// <summary>
		/// 型号
		/// </summary>
		public string Model { get; set; } 

		/// <summary>
		/// 规格
		/// </summary>
		public string Specifications { get; set; } 

		/// <summary>
		/// 生产数量
		/// </summary>
		public int? Quantity { get; set; } 

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark { get; set; } 

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
		/// 计划开始时间
		/// </summary>
		public DateTime? PlanStartTime { get; set; } 

		/// <summary>
		/// 计划结束时间
		/// </summary>
		public DateTime? PlanFinishTime { get; set; } 

		/// <summary>
		/// 月份
		/// </summary>
		public int? PlanMonth { get; set; } 

		/// <summary>
		/// 1.入库计划 2.试压 3.预试压
		/// </summary>
		public int? PlanType { get; set; } 

	}
}
