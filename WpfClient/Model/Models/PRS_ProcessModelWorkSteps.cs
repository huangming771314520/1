using System;

namespace Model.Models
{
	/// <summary>
	/// PRS_ProcessModelWorkSteps表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class PRS_ProcessModelWorkSteps
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 工艺模型工序ID
		/// </summary>
		public int? MainID { get; set; } 

		/// <summary>
		/// 工步编号
		/// </summary>
		public int? WorkStepsLineCode { get; set; } 

		/// <summary>
		/// 工步名称
		/// </summary>
		public string WorkStepsName { get; set; } 

		/// <summary>
		/// 工步描述
		/// </summary>
		public string WorkStepsContent { get; set; } 

		/// <summary>
		/// 工时定额
		/// </summary>
		public int? ManHours { get; set; } 

		/// <summary>
		/// 是否有效
		/// </summary>
		public int? IsEnable { get; set; } 

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? CreateTime { get; set; } 

		/// <summary>
		/// 创建人
		/// </summary>
		public string CreatePerson { get; set; } 

		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime? ModifyTime { get; set; } 

		/// <summary>
		/// 更新人
		/// </summary>
		public string ModifyPerson { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public int? Unit { get; set; } 

	}
}
