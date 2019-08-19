using System;

namespace Model.Models
{
	/// <summary>
	/// PMS_ProductTask表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class PMS_ProductTask
	{

		/// <summary>
		/// 
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 合同编码
		/// </summary>
		public string ContractCode { get; set; } 

		/// <summary>
		/// 产品ID
		/// </summary>
		public string ProductID { get; set; } 

		/// <summary>
		/// 产品任务编码
		/// </summary>
		public string ProductTaskCode { get; set; } 

		/// <summary>
		/// 任务类型
		/// </summary>
		public int? TaskType { get; set; } 

		/// <summary>
		/// 任务开始时间
		/// </summary>
		public DateTime? TaskStartDate { get; set; } 

		/// <summary>
		/// 任务结束时间
		/// </summary>
		public DateTime? TaskFinishDate { get; set; } 

		/// <summary>
		/// 任务周期
		/// </summary>
		public int? TaskCycle { get; set; } 

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

	}
}
