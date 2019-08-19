using System;

namespace Model.Models
{
	/// <summary>
	/// LoginLog表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class LoginLog
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public long ID { get; set; } 

		/// <summary>
		/// token
		/// </summary>
		public string Token { get; set; } 

		/// <summary>
		/// 类型  0:CS端  1:BS端
		/// </summary>
		public int Type { get; set; } 

		/// <summary>
		/// 登录人编号
		/// </summary>
		public string UserCode { get; set; } 

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreateDate { get; set; } 

	}
}
