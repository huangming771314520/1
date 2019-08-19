using System;

namespace Model.Models
{
	/// <summary>
	/// SYS_WorkGroupDetail表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class SYS_WorkGroupDetail
	{

		/// <summary>
		/// 
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public int? MainID { get; set; } 

		/// <summary>
		/// 员工编码
		/// </summary>
		public string UserCode { get; set; } 

		/// <summary>
		/// 员工名称
		/// </summary>
		public string UserName { get; set; } 

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
