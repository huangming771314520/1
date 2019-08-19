using System;

namespace Model.Models
{
	/// <summary>
	/// SYS_MaterialBatch表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class SYS_MaterialBatch
	{

		/// <summary>
		/// 
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 物料条码
		/// </summary>
		public string MateBarCode { get; set; } 

		/// <summary>
		/// 存货编码
		/// </summary>
		public string InventoryCode { get; set; } 

		/// <summary>
		/// 存货名称
		/// </summary>
		public string InventoryName { get; set; } 

		/// <summary>
		/// 零件编码
		/// </summary>
		public string PartCode { get; set; } 

		/// <summary>
		/// 零件图号
		/// </summary>
		public string PartFigureCode { get; set; } 

		/// <summary>
		/// 零件名称
		/// </summary>
		public string PartName { get; set; } 

		/// <summary>
		/// 合同
		/// </summary>
		public string ContractCode { get; set; } 

		/// <summary>
		/// 产品
		/// </summary>
		public int ProductID { get; set; } 

		/// <summary>
		/// 材质
		/// </summary>
		public string MaterialCode { get; set; } 

		/// <summary>
		/// 厂内批次
		/// </summary>
		public string InnerFactoryBatch { get; set; } 

		/// <summary>
		/// 厂外批次
		/// </summary>
		public string OuterFactoryBatch { get; set; } 

	}
}
