using System;

namespace Model.Models
{
	/// <summary>
	/// WMS_BN_BillMain表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class WMS_BN_BillMain
	{

		/// <summary>
		/// 主表ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 单据编号
		/// </summary>
		public string BillCode { get; set; } 

		/// <summary>
		/// 单据类型
		/// </summary>
		public int? BillType { get; set; } 

		/// <summary>
		/// 合同编号
		/// </summary>
		public string ContractCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ProjectName { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string SalesmanCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string Salesman { get; set; } 

		/// <summary>
		/// 部门编号
		/// </summary>
		public string DepartmentID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string DepartmentName { get; set; } 

		/// <summary>
		/// 供应商编号
		/// </summary>
		public string SupplierCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string SupplierName { get; set; } 

		/// <summary>
		/// 仓库编号
		/// </summary>
		public string WarehouseCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string WarehouseName { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string PickPersonCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string PickPerson { get; set; } 

		/// <summary>
		/// 审批状态
		/// </summary>
		public string ApproveState { get; set; } 

		/// <summary>
		/// 审批人
		/// </summary>
		public string ApprovePerson { get; set; } 

		/// <summary>
		/// 审批时间
		/// </summary>
		public DateTime? ApproveDate { get; set; } 

		/// <summary>
		/// 审批备注
		/// </summary>
		public string ApproveRemark { get; set; } 

		/// <summary>
		/// 请购单号
		/// </summary>
		public string OrderBillCode { get; set; } 

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark { get; set; } 

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
		public int? IsEnable { get; set; } 

	}
}
