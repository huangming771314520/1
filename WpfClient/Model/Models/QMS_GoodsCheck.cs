using System;

namespace Model.Models
{
	/// <summary>
	/// QMS_GoodsCheck表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class QMS_GoodsCheck
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 报检单号
		/// </summary>
		public string PBillCode { get; set; } 

		/// <summary>
		/// 检验单号
		/// </summary>
		public string BillCode { get; set; } 

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
		/// 
		/// </summary>
		public string DepartmentID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string DepartmentName { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string WarehouseID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string WarehouseName { get; set; } 

		/// <summary>
		/// 生产单位
		/// </summary>
		public string ProductionUnits { get; set; } 

		/// <summary>
		/// 物料编码
		/// </summary>
		public string InventoryCode { get; set; } 

		/// <summary>
		/// 物料名称
		/// </summary>
		public string InventoryName { get; set; } 

		/// <summary>
		/// 型号规格
		/// </summary>
		public string Model { get; set; } 

		/// <summary>
		/// 到货日期
		/// </summary>
		public DateTime? ArrivalDate { get; set; } 

		/// <summary>
		/// 报检数量
		/// </summary>
		public int? CheckQuantity { get; set; } 

		/// <summary>
		/// 合格数量
		/// </summary>
		public int? QualifiedQuantity { get; set; } 

		/// <summary>
		/// 单位
		/// </summary>
		public string Unit { get; set; } 

		/// <summary>
		/// 采购员编码
		/// </summary>
		public string SalesmanCode { get; set; } 

		/// <summary>
		/// 采购员
		/// </summary>
		public string Salesman { get; set; } 

		/// <summary>
		/// 批次号
		/// </summary>
		public string BatchCode { get; set; } 

		/// <summary>
		/// 质检单据状态
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
		public string Material { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public int? InspectionQuantity { get; set; } 

		/// <summary>
		/// 厂外批次
		/// </summary>
		public string OuterFactoryBatch { get; set; } 

	}
}
