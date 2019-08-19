using System;

namespace Model.Models
{
	/// <summary>
	/// BD_FileAddress表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class BD_FileAddress
	{

		/// <summary>
		/// 
		/// </summary>
		public int Object_Id { get; set; } 

		/// <summary>
		/// 表名
		/// </summary>
		public string TableName { get; set; } 

		/// <summary>
		/// 表的主键
		/// </summary>
		public string TableId { get; set; } 

		/// <summary>
		/// 文件路径
		/// </summary>
		public string FileAddr { get; set; } 

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? Create_Time { get; set; } 

		/// <summary>
		/// 创建人
		/// </summary>
		public string Create_Person { get; set; } 

	}
}
