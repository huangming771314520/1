using System;

namespace Model.Models
{
	/// <summary>
	/// APS_ProductPurchaseDetail表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class APS_ProductPurchaseDetail
	{

		/// <summary>
		/// 
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 生产请购单主表ID
		/// </summary>
		public int? MainID { get; set; } 

		/// <summary>
		/// 存货编码
		/// </summary>
		public string InventoryCode { get; set; } 

		/// <summary>
		/// 单台数量
		/// </summary>
		public int? SingleProductRequestQuantity { get; set; } 

		/// <summary>
		/// 实需数量
		/// </summary>
		public decimal? TotalRequestQuantity { get; set; } 

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark { get; set; } 

		/// <summary>
		/// 请购状态
		/// </summary>
		public int? PurchaseState { get; set; } 

		/// <summary>
		/// 
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

		/// <summary>
		/// 
		/// </summary>
		public decimal? PurchaseQuantity { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public decimal? StockQuantity { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public decimal? RequestedQuantity { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string SaleMan { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string DepartmentCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string DepartmentName { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string WarehouseCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string WarehouseName { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string SupplierCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string SupplierName { get; set; } 

		/// <summary>
		/// 采购数量
		/// </summary>
		public decimal? OrderQuantity { get; set; } 

		/// <summary>
		/// 采购反馈
		/// </summary>
		public int? PurchaseFeedback { get; set; } 

		/// <summary>
		/// 采购备注
		/// </summary>
		public string PurchaseRemark { get; set; } 

		/// <summary>
		/// 采购员编码
		/// </summary>
		public string UserCode { get; set; } 

		/// <summary>
		/// 请购日期
		/// </summary>
		public DateTime? PurchaseDate { get; set; } 

		/// <summary>
		/// 计划到货日期
		/// </summary>
		public DateTime? PlanArrivelDate { get; set; } 

		/// <summary>
		/// 配套表ID
		/// </summary>
		public int? MatchTableID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string Unit { get; set; } 

	}
}
