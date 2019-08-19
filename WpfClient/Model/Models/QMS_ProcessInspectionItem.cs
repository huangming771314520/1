using System;

namespace Model.Models
{
	/// <summary>
	/// QMS_ProcessInspectionItem表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class QMS_ProcessInspectionItem
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 主表ID
		/// </summary>
		public int? MainID { get; set; } 

		/// <summary>
		/// 检验记录
		/// </summary>
		public string CheckRecord { get; set; } 

		/// <summary>
		/// 质检员
		/// </summary>
		public string Inspector { get; set; } 

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
		public string FileName { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string FileAddress { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string InspectionItemCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string InspectionItemName { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string InspectionStandard { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string InspectionCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ProcessCheckItemCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string DocName { get; set; } 

	}
}
