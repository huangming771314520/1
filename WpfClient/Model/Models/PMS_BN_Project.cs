using System;

namespace Model.Models
{
	/// <summary>
	/// PMS_BN_Project表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class PMS_BN_Project
	{

		/// <summary>
		/// 项目ID
		/// </summary>
		public int ProjectID { get; set; } 

		/// <summary>
		/// 合同编码
		/// </summary>
		public string ContractCode { get; set; } 

		/// <summary>
		/// 项目名称
		/// </summary>
		public string ProjectName { get; set; } 

		/// <summary>
		/// 项目简称
		/// </summary>
		public string ProjectForShort { get; set; } 

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
		public DateTime? AdvancePaymentDate { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string Remark { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public int? Is0tSartProduct { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ProductReport { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public DateTime? ContractReceiveTime { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string FileAddress { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string FileName { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string DocName { get; set; } 

		/// <summary>
		/// 审核状态
		/// </summary>
		public string ProjectState { get; set; } 

	}
}
