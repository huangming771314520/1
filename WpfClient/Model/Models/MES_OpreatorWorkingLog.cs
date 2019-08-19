using System;

namespace Model.Models
{
	/// <summary>
	/// MES_OpreatorWorkingLog表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class MES_OpreatorWorkingLog
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 生产任务编码
		/// </summary>
		public string APSCode { get; set; } 

		/// <summary>
		/// 加工开始时间
		/// </summary>
		public DateTime? BegainTime { get; set; } 

		/// <summary>
		/// 加工结束时间
		/// </summary>
		public DateTime? FinishTime { get; set; } 

		/// <summary>
		/// 加工时间
		/// </summary>
		public int? WorkingHour { get; set; } 

		/// <summary>
		/// 操作工编码
		/// </summary>
		public string OpreateCode { get; set; } 

		/// <summary>
		/// 操作工
		/// </summary>
		public string OperatePerson { get; set; } 

		/// <summary>
		/// 是否有效
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
		/// 暂停类型 1：暂停 2：完工
		/// </summary>
		public string PauseType { get; set; } 

		/// <summary>
		/// 完工数量
		/// </summary>
		public decimal? FinishQuantity { get; set; } 

		/// <summary>
		/// 审核数量
		/// </summary>
		public decimal? AuditQuantity { get; set; } 

		/// <summary>
		/// 审核状态 0:未审核  1:已审核
		/// </summary>
		public int AuditState { get; set; } 

		/// <summary>
		/// 暂停原因：1.休息或下班 2.物料问题暂停 3.质量问题暂停
		/// </summary>
		public int? PauseReson { get; set; } 

	}
}
