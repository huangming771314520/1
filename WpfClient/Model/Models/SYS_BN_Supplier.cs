using System;

namespace Model.Models
{
	/// <summary>
	/// SYS_BN_Supplier表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class SYS_BN_Supplier
	{

		/// <summary>
		/// 供应ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string SupplierCode { get; set; } 

		/// <summary>
		/// 供应商名称
		/// </summary>
		public string SupplierName { get; set; } 

		/// <summary>
		/// 供应商简称
		/// </summary>
		public string SupplierForShort { get; set; } 

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
