using System;

namespace Model.Models
{
	/// <summary>
	/// MES_PurchaseOrderDetail表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class MES_PurchaseOrderDetail
	{

		/// <summary>
		/// 
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 采购订单主表ID
		/// </summary>
		public int? MainID { get; set; } 

		/// <summary>
		/// 存货编码
		/// </summary>
		public string InventoryCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string InventoryName { get; set; } 

		/// <summary>
		/// 采购数量
		/// </summary>
		public int? OrderQuantity { get; set; } 

		/// <summary>
		/// 到货数量
		/// </summary>
		public int? GoodsQuantity { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string Model { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string Unit { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public double? UnitPrice { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public double? TotalPrice { get; set; } 

		/// <summary>
		/// 是否关闭
		/// </summary>
		public int? IsFinish { get; set; } 

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
		public string WarehouseID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string SupplierCode { get; set; } 

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
		/// 计划到货日期
		/// </summary>
		public int? CheckedQuantity { get; set; } 

		/// <summary>
		/// 计划时间
		/// </summary>
		public DateTime? PlanArrivelDate { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public int? MatchTableID { get; set; } 

	}
}
