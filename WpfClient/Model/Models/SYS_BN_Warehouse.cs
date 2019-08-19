using System;

namespace Model.Models
{
	/// <summary>
	/// SYS_BN_Warehouse表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class SYS_BN_Warehouse
	{

		/// <summary>
		/// 仓库ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 仓库编码
		/// </summary>
		public string WarehouseCode { get; set; } 

		/// <summary>
		/// 仓库名称
		/// </summary>
		public string WarehouseName { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string UserCode { get; set; } 

		/// <summary>
		/// 库管员
		/// </summary>
		public string StoreKeeper { get; set; } 

		/// <summary>
		/// 仓库类别
		/// </summary>
		public int? WarehouseType { get; set; } 

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

	}
}
