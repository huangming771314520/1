using System;

namespace Model.Models
{
	/// <summary>
	/// PRS_ProcessFigure表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class PRS_ProcessFigure
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public long ID { get; set; } 

		/// <summary>
		/// 合同号
		/// </summary>
		public string ContractCode { get; set; } 

		/// <summary>
		/// 产品ID
		/// </summary>
		public int? ProductID { get; set; } 

		/// <summary>
		/// 零件编号
		/// </summary>
		public string PartCode { get; set; } 

		/// <summary>
		/// 文档名称
		/// </summary>
		public string DocName { get; set; } 

		/// <summary>
		/// 文件名称
		/// </summary>
		public string FileName { get; set; } 

		/// <summary>
		/// 文件路径
		/// </summary>
		public string FileAddress { get; set; } 

		/// <summary>
		/// 是否启用
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
		public byte[] FileContent { get; set; } 

	}
}
