using System;

namespace Model.Models
{
	/// <summary>
	/// MES_WorkshopBatching表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class MES_WorkshopBatching
	{

		/// <summary>
		/// 配料流水号
		/// </summary>
		public string BatchingCode { get; set; } 

		/// <summary>
		/// 根零件编号
		/// </summary>
		public string RootPartCode { get; set; } 

		/// <summary>
		/// 合同编号
		/// </summary>
		public string ContractCode { get; set; } 

		/// <summary>
		/// 产品ID
		/// </summary>
		public int? ProductID { get; set; } 

		/// <summary>
		/// 车间编号
		/// </summary>
		public string WorkshopCode { get; set; } 

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
		/// 当前选择的零件编号
		/// </summary>
		public string PartCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public int? IsEnable { get; set; } 

	}
}
