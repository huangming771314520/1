using System;

namespace Model.Models
{
	/// <summary>
	/// PMS_DesignTaskResult表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class PMS_DesignTaskResult
	{

		/// <summary>
		/// 主键，设计结果ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 设计任务ID
		/// </summary>
		public int MainID { get; set; } 

		/// <summary>
		/// 合同编号
		/// </summary>
		public string ContractCode { get; set; } 

		/// <summary>
		/// 产品ID
		/// </summary>
		public string ProductID { get; set; } 

		/// <summary>
		/// 任务类型
		/// </summary>
		public int? TaskType { get; set; } 

		/// <summary>
		/// 任务描述
		/// </summary>
		public string TaskDescription { get; set; } 

		/// <summary>
		/// 零件图号
		/// </summary>
		public string PartCode { get; set; } 

		/// <summary>
		/// 实际完成日期
		/// </summary>
		public DateTime? ActualFinishTime { get; set; } 

		/// <summary>
		/// 零件版本号
		/// </summary>
		public int? VersionCode { get; set; } 

		/// <summary>
		/// 是否可用
		/// </summary>
		public int? IsEnable { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string CreatePerson { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateTime { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ModifyPerson { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public DateTime? ModifyTime { get; set; } 

	}
}
