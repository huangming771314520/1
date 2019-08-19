using System;

namespace Model.Models
{
	/// <summary>
	/// WMS_BN_BillDetail表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class WMS_BN_BillDetail
	{

		/// <summary>
		/// 出入库明细表ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 单号
		/// </summary>
		public string BillCode { get; set; } 

		/// <summary>
		/// 请购单号
		/// </summary>
		public string OrderBillCode { get; set; } 

		/// <summary>
		/// 存货编码
		/// </summary>
		public string InventoryCode { get; set; } 

		/// <summary>
		/// 物料名称
		/// </summary>
		public string InventoryName { get; set; } 

		/// <summary>
		/// 型号规格
		/// </summary>
		public string Specs { get; set; } 

		/// <summary>
		/// 单位
		/// </summary>
		public string Unit { get; set; } 

		/// <summary>
		/// 物料数量
		/// </summary>
		public double? MateNum { get; set; } 

		/// <summary>
		/// 确认数量
		/// </summary>
		public double? ConfirmNum { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public double? UnitPrice { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public double? TotalPrice { get; set; } 

		/// <summary>
		/// 箱号
		/// </summary>
		public string PackageCode { get; set; } 

		/// <summary>
		/// 批次号（质检批号）
		/// </summary>
		public string BatchCode { get; set; } 

		/// <summary>
		/// 上级单据号
		/// </summary>
		public string PBillCode { get; set; } 

		/// <summary>
		/// 责任单位（工废料废使用）
		/// </summary>
		public string AccountabilityCode { get; set; } 

		/// <summary>
		/// 备注信息
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
		public int? IsEnable { get; set; } 

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

	}
}
