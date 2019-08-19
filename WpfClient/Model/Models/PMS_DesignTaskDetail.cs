using System;

namespace Model.Models
{
	/// <summary>
	/// PMS_DesignTaskDetail表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class PMS_DesignTaskDetail
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 设计任务明细编码
		/// </summary>
		public string DesignTaskCode { get; set; } 

		/// <summary>
		/// 合同编码
		/// </summary>
		public string ContractCode { get; set; } 

		/// <summary>
		/// 产品编码
		/// </summary>
		public string ProductID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public int? TaskType { get; set; } 

		/// <summary>
		/// 任务描述
		/// </summary>
		public string TaskDescription { get; set; } 

		/// <summary>
		/// 任务开始时间
		/// </summary>
		public DateTime? TaskStartDate { get; set; } 

		/// <summary>
		/// 任务结束时间
		/// </summary>
		public DateTime? TaskFinishDate { get; set; } 

		/// <summary>
		/// 单据状态
		/// </summary>
		public int? BillState { get; set; } 

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
		public int? MainID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string PartCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public DateTime? ActualFinishTime { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public int? VersionCode { get; set; } 

		/// <summary>
		/// 任务类型 1：新建项目设计任务 2：设计更改申请任务
		/// </summary>
		public int? DesignType { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string BillCode { get; set; } 

		/// <summary>
		/// 设计部门
		/// </summary>
		public string DesignDepartment { get; set; } 

	}
}
