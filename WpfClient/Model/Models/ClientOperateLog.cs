using System;

namespace Model.Models
{
	/// <summary>
	/// ClientOperateLog表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class ClientOperateLog
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public long ID { get; set; } 

		/// <summary>
		/// IP地址
		/// </summary>
		public string AddressIP { get; set; } 

		/// <summary>
		/// 操作内容
		/// </summary>
		public string Content { get; set; } 

		/// <summary>
		/// 创建人编号
		/// </summary>
		public string CreatePersonCode { get; set; } 

		/// <summary>
		/// 创建人姓名
		/// </summary>
		public string CreatePersonName { get; set; } 

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreateDate { get; set; } 

	}
}
