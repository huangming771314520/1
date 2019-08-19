using System;

namespace Model.Models
{
	/// <summary>
	/// WMS_BN_TransfersMain表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class WMS_BN_TransfersMain
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 调拨单号
		/// </summary>
		public string BillCode { get; set; } 

		/// <summary>
		/// 调出仓库ID
		/// </summary>
		public string OutWarehouseCode { get; set; } 

		/// <summary>
		/// 调出仓库
		/// </summary>
		public string OutWarehouseName { get; set; } 

		/// <summary>
		/// 调入仓库ID
		/// </summary>
		public string InWarehouseCode { get; set; } 

		/// <summary>
		/// 调入仓库
		/// </summary>
		public string InWarehouseName { get; set; } 

		/// <summary>
		/// 审批状态
		/// </summary>
		public int? ApproveState { get; set; } 

		/// <summary>
		/// 审批人
		/// </summary>
		public string ApprovePerson { get; set; } 

		/// <summary>
		/// 审批时间
		/// </summary>
		public DateTime? ApproveTime { get; set; } 

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark { get; set; } 

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
		public string ContractCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ProjectName { get; set; } 

	}
}
