using System;

namespace Model.Models
{
	/// <summary>
	/// PRS_BD_StandardProcess表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class PRS_BD_StandardProcess
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 工序编号
		/// </summary>
		public string ProcessCode { get; set; } 

		/// <summary>
		/// 工序名称
		/// </summary>
		public string ProcessName { get; set; } 

		/// <summary>
		/// 说明
		/// </summary>
		public string Instracutions { get; set; } 

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
		/// 审批人
		/// </summary>
		public string ModifyPerson { get; set; } 

		/// <summary>
		/// 审批时间
		/// </summary>
		public DateTime? ModifyTime { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public int? ProcessType { get; set; } 

	}
}
