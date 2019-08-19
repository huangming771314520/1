using System;

namespace Model.Models
{
	/// <summary>
	/// QMS_UnQualifiedPartHandling表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class QMS_UnQualifiedPartHandling
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 单据编号
		/// </summary>
		public string BillCode { get; set; } 

		/// <summary>
		/// 合同编号
		/// </summary>
		public string ContractCode { get; set; } 

		/// <summary>
		/// 项目名称
		/// </summary>
		public string ProjectName { get; set; } 

		/// <summary>
		/// 零件编号
		/// </summary>
		public string PartCode { get; set; } 

		/// <summary>
		/// 零件名称
		/// </summary>
		public string PartName { get; set; } 

		/// <summary>
		/// 型号规格
		/// </summary>
		public string Model { get; set; } 

		/// <summary>
		/// 不合格品数量
		/// </summary>
		public int? RejectQuantity { get; set; } 

		/// <summary>
		/// 工序编号
		/// </summary>
		public string ProcessCode { get; set; } 

		/// <summary>
		/// 工序名称
		/// </summary>
		public string ProcessName { get; set; } 

		/// <summary>
		/// 不合格品描述
		/// </summary>
		public string RejectDescription { get; set; } 

		/// <summary>
		/// 处理方式意见
		/// </summary>
		public int? HandlingType { get; set; } 

		/// <summary>
		/// 原因分析及预防措施
		/// </summary>
		public string AnalysisReason { get; set; } 

		/// <summary>
		/// 审核人
		/// </summary>
		public string ApprovedPerson { get; set; } 

		/// <summary>
		/// 审核日期
		/// </summary>
		public DateTime? ApprovedTime { get; set; } 

		/// <summary>
		/// 处理结果
		/// </summary>
		public int? HandlingResult { get; set; } 

		/// <summary>
		/// 检验员编码
		/// </summary>
		public string InspectorCode { get; set; } 

		/// <summary>
		/// 检验员名称
		/// </summary>
		public string InspectorName { get; set; } 

		/// <summary>
		/// 单据状态
		/// </summary>
		public int? ApproveState { get; set; } 

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

	}
}
