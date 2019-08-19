using System;

namespace Model.Models
{
	/// <summary>
	/// PRS_DesignChangeRequest表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class PRS_DesignChangeRequest
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 申请单号
		/// </summary>
		public string RequestCode { get; set; } 

		/// <summary>
		/// 合同编号
		/// </summary>
		public string ContractCode { get; set; } 

		/// <summary>
		/// 图号
		/// </summary>
		public string FigureNumber { get; set; } 

		/// <summary>
		/// 原因
		/// </summary>
		public string Reason { get; set; } 

		/// <summary>
		/// 更改内容
		/// </summary>
		public string ChangeContent { get; set; } 

		/// <summary>
		/// 请求单状态
		/// </summary>
		public int? RequestState { get; set; } 

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
		/// 
		/// </summary>
		public string FileName { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string FileAddress { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string DocName { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ProductID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string TypeID { get; set; } 

	}
}
