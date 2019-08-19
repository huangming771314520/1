using System;

namespace Model.Models
{
	/// <summary>
	/// MES_MaterialPickMain表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class MES_MaterialPickMain
	{

		/// <summary>
		/// 
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 单号
		/// </summary>
		public string BillCode { get; set; } 

		/// <summary>
		/// 部门编码
		/// </summary>
		public string DepartmentID { get; set; } 

		/// <summary>
		/// 部门名称
		/// </summary>
		public string DepartmentName { get; set; } 

		/// <summary>
		/// 领料日期
		/// </summary>
		public DateTime? PickingDate { get; set; } 

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
		public int? BillState { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string WarehouseCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string WarehouseName { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ContractCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ProjectName { get; set; } 

		/// <summary>
		/// 产品表ID
		/// </summary>
		public int? ProductID { get; set; } 

		/// <summary>
		/// 产品名称
		/// </summary>
		public string ProductName { get; set; } 

	}
}
