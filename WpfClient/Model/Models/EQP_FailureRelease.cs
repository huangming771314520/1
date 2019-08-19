using System;

namespace Model.Models
{
	/// <summary>
	/// EQP_FailureRelease表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class EQP_FailureRelease
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 故障处理单号
		/// </summary>
		public string FailureRemoveCode { get; set; } 

		/// <summary>
		/// 故障报告ID
		/// </summary>
		public int? FailureReportID { get; set; } 

		/// <summary>
		/// 解除时间
		/// </summary>
		public DateTime? ReleaseTime { get; set; } 

		/// <summary>
		/// 排障方法描述
		/// </summary>
		public string FailRemoveDescription { get; set; } 

		/// <summary>
		/// 排障人
		/// </summary>
		public string FailRemoveMan { get; set; } 

		/// <summary>
		/// 故障状态
		/// </summary>
		public int? FailureState { get; set; } 

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
