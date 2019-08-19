using System;

namespace Model.Models
{
	/// <summary>
	/// APS_ProductPurchaseMain表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class APS_ProductPurchaseMain
	{

		/// <summary>
		/// 
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 请购单编码
		/// </summary>
		public string PurchaseDocumentCode { get; set; } 

		/// <summary>
		/// 项目ID
		/// </summary>
		public string ContractCode { get; set; } 

		/// <summary>
		/// 请购日期
		/// </summary>
		public DateTime? PurchaseDate { get; set; } 

		/// <summary>
		/// 仓库编号
		/// </summary>
		public string WarehouseID { get; set; } 

		/// <summary>
		/// 是否可用
		/// </summary>
		public int? IsEnable { get; set; } 

		/// <summary>
		/// 审核状态 1：未审核 2：已审核
		/// </summary>
		public int? BillState { get; set; } 

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
		public string ProductPlanCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string DepartmentCode { get; set; } 

		/// <summary>
		/// 生产请购类型
		/// </summary>
		public int? PurchaseType { get; set; } 

	}
}
