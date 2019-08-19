using System;

namespace Model.Models
{
	/// <summary>
	/// Mes_PurchaseProcessDetail表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class Mes_PurchaseProcessDetail
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 采购过程ID
		/// </summary>
		public int? MainID { get; set; } 

		/// <summary>
		/// 字段名
		/// </summary>
		public string Field { get; set; } 

		/// <summary>
		/// 字段值
		/// </summary>
		public string Value { get; set; } 

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark { get; set; } 

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
