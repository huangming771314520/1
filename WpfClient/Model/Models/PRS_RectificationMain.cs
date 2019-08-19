using System;

namespace Model.Models
{
	/// <summary>
	/// PRS_RectificationMain表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class PRS_RectificationMain
	{

		/// <summary>
		/// 
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 整改单号
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
		/// 
		/// </summary>
		public int? ProjectDetailID { get; set; } 

		/// <summary>
		/// 产品名称
		/// </summary>
		public string ProductName { get; set; } 

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
