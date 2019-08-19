using System;

namespace Model.Models
{
	/// <summary>
	/// PMS_BN_ProjectPlanDetailUpdate表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class PMS_BN_ProjectPlanDetailUpdate
	{

		/// <summary>
		/// 项目计划明细变更ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 项目计划明细ID
		/// </summary>
		public int? ProjectDetailID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ContractCode { get; set; } 

		/// <summary>
		/// 考核节点类型
		/// </summary>
		public int? PlanType { get; set; } 

		/// <summary>
		/// 考核节点日期
		/// </summary>
		public DateTime? PlanDate { get; set; } 

		/// <summary>
		/// 变更后考核节点日期
		/// </summary>
		public DateTime? UpdatedPlanDate { get; set; } 

		/// <summary>
		/// 实际完成日期
		/// </summary>
		public DateTime? FinishDate { get; set; } 

		/// <summary>
		/// 变更后实际完成日期
		/// </summary>
		public DateTime? UpdatedFinishDate { get; set; } 

		/// <summary>
		/// 是否有效
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
		/// 更新人人
		/// </summary>
		public string ModifyPerson { get; set; } 

		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime? ModifyTime { get; set; } 

	}
}
