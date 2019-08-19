using System;

namespace Model.Models
{
	/// <summary>
	/// QMS_RejectPartReport表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class QMS_RejectPartReport
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
		/// 零件编码
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
		/// 工序编号
		/// </summary>
		public string ProcessCode { get; set; } 

		/// <summary>
		/// 工序名称
		/// </summary>
		public string ProcessName { get; set; } 

		/// <summary>
		/// 不合格数量
		/// </summary>
		public int? RejectQuantity { get; set; } 

		/// <summary>
		/// 工作班组编码
		/// </summary>
		public string WorkTeamCode { get; set; } 

		/// <summary>
		/// 工作班组名称
		/// </summary>
		public string WorkTeamName { get; set; } 

		/// <summary>
		/// 技术要求
		/// </summary>
		public string TechCommand { get; set; } 

		/// <summary>
		/// 检验记录
		/// </summary>
		public string InspectionRecord { get; set; } 

		/// <summary>
		/// 领导批示
		/// </summary>
		public string LeaderRemark { get; set; } 

		/// <summary>
		/// 质检编码
		/// </summary>
		public string InspectorCode { get; set; } 

		/// <summary>
		/// 质检名称
		/// </summary>
		public string InspectorName { get; set; } 

		/// <summary>
		/// 
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
