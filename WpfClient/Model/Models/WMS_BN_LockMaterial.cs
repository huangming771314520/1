using System;

namespace Model.Models
{
	/// <summary>
	/// WMS_BN_LockMaterial表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class WMS_BN_LockMaterial
	{

		/// <summary>
		/// 存货锁定ID
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
		/// 锁定数量
		/// </summary>
		public double? LockQuantity { get; set; } 

		/// <summary>
		/// 仓库
		/// </summary>
		public string WarehouseCode { get; set; } 

		/// <summary>
		/// 仓库名称
		/// </summary>
		public string WarehouseName { get; set; } 

		/// <summary>
		/// 物料类别编号
		/// </summary>
		public int? MaterialCategoryNum { get; set; } 

		/// <summary>
		/// 物料类别名称
		/// </summary>
		public string MaterialCategoryName { get; set; } 

		/// <summary>
		/// 锁定状态
		/// </summary>
		public int? LockState { get; set; } 

		/// <summary>
		/// 锁定原因
		/// </summary>
		public string LockDescription { get; set; } 

		/// <summary>
		/// 解锁原因
		/// </summary>
		public string UnLockDescription { get; set; } 

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
