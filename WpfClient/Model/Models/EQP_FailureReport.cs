using System;

namespace Model.Models
{
	/// <summary>
	/// EQP_FailureReport表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class EQP_FailureReport
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 故障报告编号
		/// </summary>
		public string FailureReportCode { get; set; } 

		/// <summary>
		/// 设备ID
		/// </summary>
		public int? EquipmentID { get; set; } 

		/// <summary>
		/// 设备名称
		/// </summary>
		public string EquipmentName { get; set; } 

		/// <summary>
		/// 车间ID
		/// </summary>
		public int? WorkshopID { get; set; } 

		/// <summary>
		/// 所在车间
		/// </summary>
		public string WorkshopName { get; set; } 

		/// <summary>
		/// 故障时间
		/// </summary>
		public DateTime? FailTime { get; set; } 

		/// <summary>
		/// 故障描述
		/// </summary>
		public string FailDescription { get; set; } 

		/// <summary>
		/// 报障人
		/// </summary>
		public string ReportPerson { get; set; } 

		/// <summary>
		/// 报告状态
		/// </summary>
		public int? FailReportState { get; set; } 

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
