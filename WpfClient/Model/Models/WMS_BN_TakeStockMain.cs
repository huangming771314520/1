using System;

namespace Model.Models
{
	/// <summary>
	/// WMS_BN_TakeStockMain表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class WMS_BN_TakeStockMain
	{

		/// <summary>
		/// 
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 盘点单号
		/// </summary>
		public string BillCode { get; set; } 

		/// <summary>
		/// 盘点年份
		/// </summary>
		public int? TakeStockYear { get; set; } 

		/// <summary>
		/// 盘点月份
		/// </summary>
		public int? TakeStockMonth { get; set; } 

		/// <summary>
		/// 盘点类型
		/// </summary>
		public int? TakeStockType { get; set; } 

		/// <summary>
		/// 单据状态
		/// </summary>
		public int? BillState { get; set; } 

		/// <summary>
		/// 盘点备注
		/// </summary>
		public string Remark { get; set; } 

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
		public int? IsEnable { get; set; } 

	}
}
