using System;

namespace Model.Models
{
	/// <summary>
	/// MES_OrderContract表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class MES_OrderContract
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 采购合同编号
		/// </summary>
		public string OrderContractCode { get; set; } 

		/// <summary>
		/// 采购合同名称
		/// </summary>
		public string OrderContractName { get; set; } 

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
		/// 文档名
		/// </summary>
		public string FileName { get; set; } 

		/// <summary>
		/// 文档地址
		/// </summary>
		public string FileAddress { get; set; } 

		/// <summary>
		/// 原文件名
		/// </summary>
		public string DocName { get; set; } 

		/// <summary>
		/// 单据状态
		/// </summary>
		public int? BillState { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string SupplierCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string SupplierName { get; set; } 

	}
}
