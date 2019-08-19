using System;

namespace Model.Models
{
	/// <summary>
	/// APS_DispatchPlanMain表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class APS_DispatchPlanMain
	{

		/// <summary>
		/// 调度计划编号
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
		/// 计划已排程数量
		/// </summary>
		public int? PlanQuantity { get; set; } 

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
		/// 调度任务模型编号
		/// </summary>
		public string ModelCode { get; set; } 

		/// <summary>
		/// 是否可用
		/// </summary>
		public int? IsEnable { get; set; } 

	}
}
