using System;

namespace Model.Models
{
	/// <summary>
	/// WMS_BN_RealStock表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class WMS_BN_RealStock
	{

		/// <summary>
		/// 真实库存ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 存货编码
		/// </summary>
		public string InventoryCode { get; set; } 

		/// <summary>
		/// 存货名称
		/// </summary>
		public string InventoryName { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string Model { get; set; } 

		/// <summary>
		/// 真实库存
		/// </summary>
		public double? RealStock { get; set; } 

		/// <summary>
		/// 总库存
		/// </summary>
		public double? TotalStock { get; set; } 

		/// <summary>
		/// 锁定库存
		/// </summary>
		public double? LockStock { get; set; } 

		/// <summary>
		/// 仓库编号
		/// </summary>
		public string WarehouseCode { get; set; } 

		/// <summary>
		/// 仓库名称
		/// </summary>
		public string WarehouseName { get; set; } 

		/// <summary>
		/// 批次号
		/// </summary>
		public string BatchCode { get; set; } 

		/// <summary>
		/// 单位
		/// </summary>
		public string Unit { get; set; } 

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
