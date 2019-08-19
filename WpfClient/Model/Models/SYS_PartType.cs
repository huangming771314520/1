using System;

namespace Model.Models
{
	/// <summary>
	/// SYS_PartType表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class SYS_PartType
	{

		/// <summary>
		/// 
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 零件类型编码
		/// </summary>
		public string PartTypeCode { get; set; } 

		/// <summary>
		/// 类型名称
		/// </summary>
		public string TypeName { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string PPartTypeCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string PTypeName { get; set; } 

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
