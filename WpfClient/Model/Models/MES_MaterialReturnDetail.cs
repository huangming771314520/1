using System;

namespace Model.Models
{
	/// <summary>
	/// MES_MaterialReturnDetail表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class MES_MaterialReturnDetail
	{

		/// <summary>
		/// 
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public int? MainID { get; set; } 

		/// <summary>
		/// 合同编号
		/// </summary>
		public string ContractCode { get; set; } 

		/// <summary>
		/// 项目名称
		/// </summary>
		public string ProjectName { get; set; } 

		/// <summary>
		/// 存货编码
		/// </summary>
		public string InventoryCode { get; set; } 

		/// <summary>
		/// 存货名称
		/// </summary>
		public string InventoryName { get; set; } 

		/// <summary>
		/// 退料数量
		/// </summary>
		public int? ReturnQuantity { get; set; } 

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
		public string Unit { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string Model { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string Material { get; set; } 

	}
}
