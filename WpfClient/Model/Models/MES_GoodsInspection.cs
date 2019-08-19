using System;

namespace Model.Models
{
	/// <summary>
	/// MES_GoodsInspection表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class MES_GoodsInspection
	{

		/// <summary>
		/// 
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 报检单号
		/// </summary>
		public string BillCode { get; set; } 

		/// <summary>
		/// 采购订单编号
		/// </summary>
		public string PurchaseOrderCode { get; set; } 

		/// <summary>
		/// 合同编号
		/// </summary>
		public string ContractCode { get; set; } 

		/// <summary>
		/// 项目名称
		/// </summary>
		public string ProjectName { get; set; } 

		/// <summary>
		/// 供应商编码
		/// </summary>
		public string SupplierCode { get; set; } 

		/// <summary>
		/// 供应商名称
		/// </summary>
		public string SupplierName { get; set; } 

		/// <summary>
		/// 部门编码
		/// </summary>
		public string DepartmentID { get; set; } 

		/// <summary>
		/// 部门名称
		/// </summary>
		public string DepartmentName { get; set; } 

		/// <summary>
		/// 业务员编码
		/// </summary>
		public string UserCode { get; set; } 

		/// <summary>
		/// 业务员
		/// </summary>
		public string SaleMan { get; set; } 

		/// <summary>
		/// 到货日期
		/// </summary>
		public DateTime? ArrivalDate { get; set; } 

		/// <summary>
		/// 仓库编码
		/// </summary>
		public string WarehouseID { get; set; } 

		/// <summary>
		/// 仓库名称
		/// </summary>
		public string WarehouseName { get; set; } 

		/// <summary>
		/// 单据状态
		/// </summary>
		public int? BillState { get; set; } 

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
		/// 修改人
		/// </summary>
		public string ModifyPerson { get; set; } 

		/// <summary>
		/// 修改时间
		/// </summary>
		public DateTime? ModifyTime { get; set; } 

	}
}
