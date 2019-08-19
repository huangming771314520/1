using System;

namespace Model.Models
{
	/// <summary>
	/// MES_OperatorStatistical表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class MES_OperatorStatistical
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public long ID { get; set; } 

		/// <summary>
		/// 主表ID
		/// </summary>
		public int MainID { get; set; } 

		/// <summary>
		/// 操作员编号
		/// </summary>
		public string OperatorCode { get; set; } 

		/// <summary>
		/// 操作员姓名
		/// </summary>
		public string OperatorName { get; set; } 

		/// <summary>
		/// 操作员条码
		/// </summary>
		public string OperatorBarCode { get; set; } 

	}
}
