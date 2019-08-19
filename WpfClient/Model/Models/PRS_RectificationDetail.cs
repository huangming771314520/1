using System;

namespace Model.Models
{
	/// <summary>
	/// PRS_RectificationDetail表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class PRS_RectificationDetail
	{

		/// <summary>
		/// 
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string MainID { get; set; } 

		/// <summary>
		/// 零件编号
		/// </summary>
		public string PartCode { get; set; } 

		/// <summary>
		/// 责任人
		/// </summary>
		public string ChargePerson { get; set; } 

		/// <summary>
		/// 整改内容
		/// </summary>
		public string RectificationContent { get; set; } 

		/// <summary>
		/// 工序编号
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
		/// 
		/// </summary>
		public int? ProcessLineCode { get; set; } 

		/// <summary>
		/// 车间编号
		/// </summary>
		public string WorkshopCode { get; set; } 

		/// <summary>
		/// 设备编号
		/// </summary>
		public string EquipmentCode { get; set; } 

		/// <summary>
		/// 班组编号
		/// </summary>
		public string WorkGroupCode { get; set; } 

		/// <summary>
		/// 仓库编号
		/// </summary>
		public string WarehouseCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string WorkshopName { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string EquipmentName { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string WorkGroupName { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string WarehouseName { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public int? Quantity { get; set; } 

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
		/// 工具编号
		/// </summary>
		public string ToolCode { get; set; } 

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
		/// 审批状态
		/// </summary>
		public string ApproveState { get; set; } 

		/// <summary>
		/// 审批人
		/// </summary>
		public string ApprovePerson { get; set; } 

		/// <summary>
		/// 审批时间
		/// </summary>
		public DateTime? ApproveDate { get; set; } 

		/// <summary>
		/// 审批备注
		/// </summary>
		public string ApproveRemark { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public int? IsCheck { get; set; } 

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
