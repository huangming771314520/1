using System;

namespace Model.Models
{
	/// <summary>
	/// PRS_ProcessRouteModelDetail表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class PRS_ProcessRouteModelDetail
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public int? MainID { get; set; } 

		/// <summary>
		/// 标准工序编号
		/// </summary>
		public string ProcessCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ProcessName { get; set; } 

		/// <summary>
		/// 工序行号
		/// </summary>
		public int? ProcessLineCode { get; set; } 

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public int? IsEnable { get; set; } 

		/// <summary>
		/// 创建人
		/// </summary>
		public string CraetePerson { get; set; } 

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? CreateTime { get; set; } 

		/// <summary>
		/// 审批人
		/// </summary>
		public string ModifyPerson { get; set; } 

		/// <summary>
		/// 审批时间
		/// </summary>
		public DateTime? ModifyTime { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string DepartmentCode { get; set; } 

	}
}
