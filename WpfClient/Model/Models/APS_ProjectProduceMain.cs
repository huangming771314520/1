using System;

namespace Model.Models
{
	/// <summary>
	/// APS_ProjectProduceMain表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class APS_ProjectProduceMain
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
		/// 产品ID
		/// </summary>
		public int? ProductID { get; set; } 

		/// <summary>
		/// 合同数量
		/// </summary>
		public int? ContractQuantity { get; set; } 

		/// <summary>
		/// 排产数量
		/// </summary>
		public int? ProduceQuantity { get; set; } 

		/// <summary>
		/// 单据状态
		/// </summary>
		public int? BillState { get; set; } 

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
		/// 更新人
		/// </summary>
		public string ModifyPerson { get; set; } 

		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime? ModifyTime { get; set; } 

		/// <summary>
		/// 设计任务编码
		/// </summary>
		public string DesignTaskCode { get; set; } 

		/// <summary>
		/// 根零件编号
		/// </summary>
		public string PartCode { get; set; } 

		/// <summary>
		/// 生产任务编号
		/// </summary>
		public string ProductPlanCode { get; set; } 

	}
}
