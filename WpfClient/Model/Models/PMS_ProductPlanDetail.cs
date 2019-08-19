using System;

namespace Model.Models
{
	/// <summary>
	/// PMS_ProductPlanDetail表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class PMS_ProductPlanDetail
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 合同编号
		/// </summary>
		public string ContractCode { get; set; } 

		/// <summary>
		/// 产品编码
		/// </summary>
		public string ProductID { get; set; } 

		/// <summary>
		/// 投产产品数量
		/// </summary>
		public int? ProductQuantity { get; set; } 

		/// <summary>
		/// 计划完成日期
		/// </summary>
		public DateTime? PlanFinishDate { get; set; } 

		/// <summary>
		/// 单据状态
		/// </summary>
		public int? BillState { get; set; } 

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
		/// 
		/// </summary>
		public int? MainID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ProductPlanCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public int? CompleteQuantity { get; set; } 

	}
}
