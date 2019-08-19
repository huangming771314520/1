using System;

namespace Model.Models
{
	/// <summary>
	/// SYS_BN_Department表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class SYS_BN_Department
	{

		/// <summary>
		/// 部门ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 部门名称
		/// </summary>
		public string DepartmentName { get; set; } 

		/// <summary>
		/// 父级部门
		/// </summary>
		public string DepartmentCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ParentCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ParentName { get; set; } 

		/// <summary>
		/// 是否为车间
		/// </summary>
		public int? IsWorkshop { get; set; } 

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
		/// 工艺类型
		/// </summary>
		public string ProcessType { get; set; } 

	}
}
