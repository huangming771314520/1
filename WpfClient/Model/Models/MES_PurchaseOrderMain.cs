using System;

namespace Model.Models
{
	/// <summary>
	/// MES_PurchaseOrderMain表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class MES_PurchaseOrderMain
	{

		/// <summary>
		/// 
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 单号
		/// </summary>
		public string BillCode { get; set; } 

		/// <summary>
		/// 请购单编码
		/// </summary>
		public string ProductPurchaseCode { get; set; } 

		/// <summary>
		/// 项目ID
		/// </summary>
		public string ContractCode { get; set; } 

		/// <summary>
		/// 采购日期
		/// </summary>
		public DateTime? OrderDate { get; set; } 

		/// <summary>
		/// 仓库ID
		/// </summary>
		public string WarehouseID { get; set; } 

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
		/// 
		/// </summary>
		public string UserCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string SaleMan { get; set; } 

		/// <summary>
		/// 
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

		/// <summary>
		/// 
		/// </summary>
		public string PurchaseContract { get; set; } 

	}
}
