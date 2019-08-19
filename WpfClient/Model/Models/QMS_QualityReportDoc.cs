using System;

namespace Model.Models
{
	/// <summary>
	/// QMS_QualityReportDoc表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class QMS_QualityReportDoc
	{

		/// <summary>
		/// 
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 单号
		/// </summary>
		public string BillCode { get; set; } 

		/// <summary>
		/// 文件类型
		/// </summary>
		public int? FileType { get; set; } 

		/// <summary>
		/// 文件名称
		/// </summary>
		public string FileName { get; set; } 

		/// <summary>
		/// 文件地址
		/// </summary>
		public string FileAddress { get; set; } 

		/// <summary>
		/// 是否可用
		/// </summary>
		public bool? IsEnable { get; set; } 

		/// <summary>
		/// 创建人
		/// </summary>
		public string CreatePerson { get; set; } 

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? CreateTime { get; set; } 

		/// <summary>
		/// 修改人
		/// </summary>
		public string ModifyPerson { get; set; } 

		/// <summary>
		/// 修改时间
		/// </summary>
		public DateTime? ModifyTime { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string DocName { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string SourceID { get; set; } 

	}
}
