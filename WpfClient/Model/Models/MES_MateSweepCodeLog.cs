using System;

namespace Model.Models
{
	/// <summary>
	/// MES_MateSweepCodeLog表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class MES_MateSweepCodeLog
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public long ID { get; set; } 

		/// <summary>
		/// 用户条码
		/// </summary>
		public string UserBarCode { get; set; } 

		/// <summary>
		/// 生产任务编码
		/// </summary>
		public string ApsCode { get; set; } 

		/// <summary>
		/// 物料条码
		/// </summary>
		public string MateBarCode { get; set; } 

		/// <summary>
		/// 物料数量
		/// </summary>
		public int? MateQuantity { get; set; } 

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
