using System;

namespace Model.Models
{
	/// <summary>
	/// Mes_BlankingBoard表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class Mes_BlankingBoard
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 合同编号
		/// </summary>
		public string ContractCode { get; set; } 

		/// <summary>
		/// 零件编码
		/// </summary>
		public string PartCode { get; set; } 

		/// <summary>
		/// 板材物料编码
		/// </summary>
		public string BoardInventoryCode { get; set; } 

		/// <summary>
		/// 板材
		/// </summary>
		public string BoardInventoryName { get; set; } 

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
		/// 文件路径
		/// </summary>
		public string FileAddress { get; set; } 

		/// <summary>
		/// 文件名称
		/// </summary>
		public string FileName { get; set; } 

		/// <summary>
		/// 文档名称
		/// </summary>
		public string DocName { get; set; } 

		/// <summary>
		/// 程序号
		/// </summary>
		public string ProgramCode { get; set; } 

	}
}
