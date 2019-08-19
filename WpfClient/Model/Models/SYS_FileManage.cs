using System;

namespace Model.Models
{
	/// <summary>
	/// SYS_FileManage表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class SYS_FileManage
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public long ID { get; set; } 

		/// <summary>
		/// 绑定所属表名
		/// </summary>
		public string BindTableName { get; set; } 

		/// <summary>
		/// 绑定所属业务ID
		/// </summary>
		public string BindCode { get; set; } 

		/// <summary>
		/// 文件名称
		/// </summary>
		public string FileName { get; set; } 

		/// <summary>
		/// 文件分类：根据业务需分类字段
		/// </summary>
		public int? FileType { get; set; } 

		/// <summary>
		/// 文件后缀
		/// </summary>
		public string FileSuffix { get; set; } 

		/// <summary>
		/// 文件上传地址
		/// </summary>
		public string FileAddress { get; set; } 

		/// <summary>
		/// 文件的唯一码
		/// </summary>
		public string MD5Code { get; set; } 

		/// <summary>
		/// 上传人
		/// </summary>
		public string CreatePerson { get; set; } 

		/// <summary>
		/// 上传时间
		/// </summary>
		public DateTime? CreateTime { get; set; } 

	}
}
