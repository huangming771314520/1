using System;

namespace Model.Models
{
	/// <summary>
	/// SYS_BN_User表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class SYS_BN_User
	{

		/// <summary>
		/// 用户ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string UserCode { get; set; } 

		/// <summary>
		/// 用户名
		/// </summary>
		public string UserName { get; set; } 

		/// <summary>
		/// 部门编号
		/// </summary>
		public string DepartmentCode { get; set; } 

		/// <summary>
		/// 所属部门
		/// </summary>
		public string DepartmentName { get; set; } 

		/// <summary>
		/// 是否有效
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
		public string UserBarcode { get; set; } 

	}
}
