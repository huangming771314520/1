using System;

namespace Model.Models
{
	/// <summary>
	/// QMS_BD_QualityCheckType表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class QMS_BD_QualityCheckType
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 质检类型编码
		/// </summary>
		public string InspectionTypeCode { get; set; } 

		/// <summary>
		/// 质检类型名称
		/// </summary>
		public string InspectionTypeName { get; set; } 

		/// <summary>
		/// 父级类型编码
		/// </summary>
		public string PInspectionType { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string PInspectionName { get; set; } 

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
