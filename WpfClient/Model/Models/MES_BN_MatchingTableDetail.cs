using System;

namespace Model.Models
{
	/// <summary>
	/// MES_BN_MatchingTableDetail表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class MES_BN_MatchingTableDetail
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
		/// 
		/// </summary>
		public int? ProjectDetailID { get; set; } 

		/// <summary>
		/// 项目名称
		/// </summary>
		public string ProjectName { get; set; } 

		/// <summary>
		/// 父零件编号
		/// </summary>
		public string PPartCode { get; set; } 

		/// <summary>
		/// 父零件名称
		/// </summary>
		public string PPartName { get; set; } 

		/// <summary>
		/// 子零件编号
		/// </summary>
		public string PartCode { get; set; } 

		/// <summary>
		/// 子零件名称
		/// </summary>
		public string PartName { get; set; } 

		/// <summary>
		/// 需求数量
		/// </summary>
		public int? NeedQuantity { get; set; } 

		/// <summary>
		/// 配套类型
		/// </summary>
		public string Type { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string TypeName { get; set; } 

		/// <summary>
		/// 是否包料
		/// </summary>
		public int? IsMaterial { get; set; } 

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
		public int? BomQuantity { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ProductPlanCode { get; set; } 

		/// <summary>
		/// 设计任务编码
		/// </summary>
		public string DesignTaskCode { get; set; } 

	}
}
