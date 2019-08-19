using System;

namespace Model.Models
{
	/// <summary>
	/// Mes_BillBarCode表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class Mes_BillBarCode
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 单据类型
		/// </summary>
		public string BillType { get; set; } 

		/// <summary>
		/// 单据号
		/// </summary>
		public string BillCode { get; set; } 

		/// <summary>
		/// 单据条码
		/// </summary>
		public string BillBarcode { get; set; } 

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

	}
}
