using System;

namespace Model.Models
{
	/// <summary>
	/// PRS_ProcessRouteModelMain表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class PRS_ProcessRouteModelMain
	{

		/// <summary>
		/// 
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ProcessRouteCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ProcessRouteName { get; set; } 

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark { get; set; } 

		/// <summary>
		/// 单据状态 1：未审核 2：已审核
		/// </summary>
		public int? BillState { get; set; } 

		/// <summary>
		/// 是否可用
		/// </summary>
		public int? IsEnable { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string CraetePerson { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateTime { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ModifyPerson { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public DateTime? ModifyTime { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string ContractCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string PartCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string PartFigure { get; set; } 

		/// <summary>
		/// 工艺类型
		/// </summary>
		public string ProcessModelType { get; set; } 

	}
}
