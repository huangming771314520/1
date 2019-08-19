using System;

namespace Model.Models
{
	/// <summary>
	/// Mes_PurchaseProcess表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class Mes_PurchaseProcess
	{

		/// <summary>
		/// 
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 合同编号
		/// </summary>
		public string ContractCode { get; set; } 

		/// <summary>
		/// 产品ID
		/// </summary>
		public int? ProductID { get; set; } 

		/// <summary>
		/// 采购员编码
		/// </summary>
		public string UserCode { get; set; } 

		/// <summary>
		/// 采购员名称
		/// </summary>
		public string SaleMan { get; set; } 

		/// <summary>
		/// 采购物料编码
		/// </summary>
		public string InventoryCode { get; set; } 

		/// <summary>
		/// 采购物料名称
		/// </summary>
		public string InventoryName { get; set; } 

		/// <summary>
		/// 型号规格
		/// </summary>
		public string Model { get; set; } 

		/// <summary>
		/// 材质
		/// </summary>
		public string MaterialCode { get; set; } 

		/// <summary>
		/// 采购数量
		/// </summary>
		public int? Quantity { get; set; } 

		/// <summary>
		/// 到货数量
		/// </summary>
		public int? ArrivalQuantity { get; set; } 

		/// <summary>
		/// 供应商编码
		/// </summary>
		public string SupplierCode { get; set; } 

		/// <summary>
		/// 采购订单编码
		/// </summary>
		public string PurchaseCode { get; set; } 

		/// <summary>
		/// 计划到货日期
		/// </summary>
		public DateTime? PlanArrivelDate { get; set; } 

		/// <summary>
		/// 采购日期
		/// </summary>
		public DateTime? PrchaseDate { get; set; } 

		/// <summary>
		/// 领里日期
		/// </summary>
		public DateTime? PickMaterialDate { get; set; } 

		/// <summary>
		/// 交货日期
		/// </summary>
		public DateTime? FinishDate { get; set; } 

		/// <summary>
		/// 实际交货日期
		/// </summary>
		public DateTime? ActualFinishDate { get; set; } 

		/// <summary>
		/// 当前状态
		/// </summary>
		public string CurrentState { get; set; } 

		/// <summary>
		/// 不合格数量
		/// </summary>
		public string UnqualityQuantity { get; set; } 

		/// <summary>
		/// 整改次数
		/// </summary>
		public int? RectificationNumber { get; set; } 

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark { get; set; } 

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
		/// 签约时间
		/// </summary>
		public DateTime? SigningTime { get; set; } 

		/// <summary>
		/// 开票金额
		/// </summary>
		public decimal? InvoiceAmount { get; set; } 

		/// <summary>
		/// 开票时间
		/// </summary>
		public DateTime? InvoiceTime { get; set; } 

		/// <summary>
		/// 领料状态
		/// </summary>
		public int? PickMaterialState { get; set; } 

		/// <summary>
		/// 实际到货状态
		/// </summary>
		public int? ActualFinishState { get; set; } 

		/// <summary>
		/// 开票状态
		/// </summary>
		public int? InvoiceState { get; set; } 

		/// <summary>
		/// 不合格数量状态
		/// </summary>
		public int? UnqualityQuantityState { get; set; } 

		/// <summary>
		/// 整改次数状态
		/// </summary>
		public int? RectificationState { get; set; } 

	}
}
