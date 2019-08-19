using System;

namespace Model.Models
{
	/// <summary>
	/// PRS_BlankingPlateDetail表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class PRS_BlankingPlateDetail
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 拼板方案ID
		/// </summary>
		public int? MainID { get; set; } 

		/// <summary>
		/// 筋板尺寸
		/// </summary>
		public string PlateSize { get; set; } 

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

		/// <summary>
		/// 重量
		/// </summary>
		public decimal? Weight { get; set; } 

		/// <summary>
		/// 线长
		/// </summary>
		public decimal? LineLength { get; set; } 

		/// <summary>
		/// 下料方式：数控，手工
		/// </summary>
		public int? BlankingType { get; set; } 

	}
}
