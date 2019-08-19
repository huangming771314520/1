using System;

namespace Model.Models
{
	/// <summary>
	/// MES_WorkshopPurchaseDetail表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class MES_WorkshopPurchaseDetail
	{

		/// <summary>
		/// 
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 车间请购单主表ID
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
		/// 请购数量
		/// </summary>
		public int? PurchaseQuantity { get; set; } 

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
		public string Model { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string Remark { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string Spec { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string WriteModel { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string MeterialName { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string WriteSpec { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string New_InventoryCode { get; set; } 

	}
}
