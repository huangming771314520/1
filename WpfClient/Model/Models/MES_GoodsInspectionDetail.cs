using System;

namespace Model.Models
{
	/// <summary>
	/// MES_GoodsInspectionDetail表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class MES_GoodsInspectionDetail
	{

		/// <summary>
		/// 
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 到货报检单主表ID
		/// </summary>
		public int? MainID { get; set; } 

		/// <summary>
		/// 存货编码
		/// </summary>
		public string InventoryCode { get; set; } 

		/// <summary>
		/// 存货名称
		/// </summary>
		public string InVentoryName { get; set; } 

		/// <summary>
		/// 型号规格
		/// </summary>
		public string Model { get; set; } 

		/// <summary>
		/// 到货数量
		/// </summary>
		public int? ArrivalQuatity { get; set; } 

		/// <summary>
		/// 物料单位
		/// </summary>
		public string Unit { get; set; } 

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
		public string Material { get; set; } 

	}
}
