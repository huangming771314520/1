using System;

namespace Model.Models
{
	/// <summary>
	/// MES_BN_ProductProcessRoute表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class MES_BN_ProductProcessRoute
	{

		/// <summary>
		/// 
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 合同编码
		/// </summary>
		public string ContractCode { get; set; } 

		/// <summary>
		/// 项目名称
		/// </summary>
		public string ProjectName { get; set; } 

		/// <summary>
		/// 零件编码
		/// </summary>
		public string PartCode { get; set; } 

		/// <summary>
		/// 工序编码
		/// </summary>
		public string ProcessCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ProcessName { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ProcessDesc { get; set; } 

		/// <summary>
		/// 工序行号
		/// </summary>
		public int? ProcessLineCode { get; set; } 

		/// <summary>
		/// 作业车间ID
		/// </summary>
		public string WorkshopID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string WorkshopName { get; set; } 

		/// <summary>
		/// 设备ID
		/// </summary>
		public string EquipmentID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string EquipmentName { get; set; } 

		/// <summary>
		/// 班组ID
		/// </summary>
		public string WorkGroupID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string WorkGroupName { get; set; } 

		/// <summary>
		/// 仓库ID
		/// </summary>
		public string WarehouseID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string WarehouseName { get; set; } 

		/// <summary>
		/// 工时定额
		/// </summary>
		public int? ManHour { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public int? Unit { get; set; } 

		/// <summary>
		/// 图号
		/// </summary>
		public string FigureCode { get; set; } 

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark { get; set; } 

		/// <summary>
		/// 是否可用
		/// </summary>
		public int? IsEnable { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public int? IsInspectionReport { get; set; } 

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
		public string ApproveState { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ApprovePerson { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public DateTime? ApproveDate { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ApproveRemark { get; set; } 

		/// <summary>
		/// 工艺类型
		/// </summary>
		public string ProcessModelType { get; set; } 

	}
}
