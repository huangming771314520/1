using System;

namespace Model.Models
{
	/// <summary>
	/// MES_ProcessInspectionRequest表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class MES_ProcessInspectionRequest
	{

		/// <summary>
		/// 
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string BillCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ContractCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ProjectName { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ProductName { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ProductFigureNumber { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string PartCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string PartName { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string partFigure { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string MaterialCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public int? CheckQuantity { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public int? BillState { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public int? IsEnable { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string CreatePerson { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateTime { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ModifyPerson { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public DateTime? ModifyTime { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ProductPlanCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string OperatorCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string DepartmentCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string DepartmentName { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string Unit { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string EquipmentCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string EquipmentName { get; set; } 

		/// <summary>
		/// 产品工艺路线ID
		/// </summary>
		public int? ProductProcessRouteID { get; set; } 

	}
}
