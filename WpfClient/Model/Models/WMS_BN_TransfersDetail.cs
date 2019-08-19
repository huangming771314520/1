using System;

namespace Model.Models
{
	/// <summary>
	/// WMS_BN_TransfersDetail表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class WMS_BN_TransfersDetail
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 盘点单号
		/// </summary>
		public string BillCode { get; set; } 

		/// <summary>
		/// 存货编码
		/// </summary>
		public string InventoryCode { get; set; } 

		/// <summary>
		/// 存货名称
		/// </summary>
		public string InventoryName { get; set; } 

		/// <summary>
		/// 单位
		/// </summary>
		public string Unit { get; set; } 

		/// <summary>
		/// 物料需求数量数量
		/// </summary>
		public int? MateNum { get; set; } 

		/// <summary>
		/// 确认数量
		/// </summary>
		public int? ConfirmNum { get; set; } 

		/// <summary>
		/// 质检批次号
		/// </summary>
		public string BatchCode { get; set; } 

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark { get; set; } 

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
		public string Model { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string UnitPrice { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string TotalPrice { get; set; } 

	}
}
