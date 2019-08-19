using System;

namespace Model.Models
{
	/// <summary>
	/// Mes_BlankingResult表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class Mes_BlankingResult
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 下料方案ID
		/// </summary>
		public int? BlankingResultID { get; set; } 

		/// <summary>
		/// 是否有效
		/// </summary>
		public int? IsEnable { get; set; } 

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? CreateTime { get; set; } 

		/// <summary>
		/// 创建人
		/// </summary>
		public string CreatePerson { get; set; } 

		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime? ModifyTime { get; set; } 

		/// <summary>
		/// 更新人
		/// </summary>
		public string ModifyPerson { get; set; } 

		/// <summary>
		/// 下料结果尺寸
		/// </summary>
		public string BiankingSize { get; set; } 

		/// <summary>
		/// 实时库存ID
		/// </summary>
		public int? MainID { get; set; } 

		/// <summary>
		/// 下料序号
		/// </summary>
		public string BlankingCode { get; set; } 

		/// <summary>
		/// 是否已下料
		/// </summary>
		public int? IsBlanking { get; set; } 

		/// <summary>
		/// 筋板编号
		/// </summary>
		public string BoardCode { get; set; } 

		/// <summary>
		/// 所属方案明细
		/// </summary>
		public int? BlankingDetailID { get; set; } 

		/// <summary>
		/// 下料方式
		/// </summary>
		public int? BlankingType { get; set; } 

	}
}
