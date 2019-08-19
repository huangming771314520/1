using System;

namespace Model.Models
{
	/// <summary>
	/// MES_WorkshopPurchaseMain表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class MES_WorkshopPurchaseMain
	{

		/// <summary>
		/// 
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 车间请购单编码
		/// </summary>
		public string WorkshopPurchaseCode { get; set; } 

		/// <summary>
		/// 车间ID
		/// </summary>
		public string WorkshopID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string WorkshopName { get; set; } 

		/// <summary>
		/// 请购状态
		/// </summary>
		public int? PurchaseStatus { get; set; } 

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
		public string ContractCode { get; set; } 

	}
}
