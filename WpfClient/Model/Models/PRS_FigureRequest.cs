using System;

namespace Model.Models
{
	/// <summary>
	/// PRS_FigureRequest表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class PRS_FigureRequest
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 单据编号
		/// </summary>
		public string BillCode { get; set; } 

		/// <summary>
		/// 合同编号
		/// </summary>
		public string ContractCode { get; set; } 

		/// <summary>
		/// 产品ID
		/// </summary>
		public int? ProductID { get; set; } 

		/// <summary>
		/// 零件编码
		/// </summary>
		public string PartCode { get; set; } 

		/// <summary>
		/// 零件图号
		/// </summary>
		public string FigureCode { get; set; } 

		/// <summary>
		/// 申请理由
		/// </summary>
		public string RequestReason { get; set; } 

		/// <summary>
		/// 申请状态
		/// </summary>
		public int? RequestState { get; set; } 

		/// <summary>
		/// 文档名称
		/// </summary>
		public string DocName { get; set; } 

		/// <summary>
		/// 文件名称
		/// </summary>
		public string FileName { get; set; } 

		/// <summary>
		/// 文件地址
		/// </summary>
		public string FileAddress { get; set; } 

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

	}
}
