using System;

namespace Model.Models
{
	/// <summary>
	/// PRS_BlankingResult表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class PRS_BlankingResult
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 下料结果尺寸
		/// </summary>
		public string ResultSize { get; set; } 

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark { get; set; } 

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
		public string MdifyPerson { get; set; } 

		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime? ModifyTime { get; set; } 

		/// <summary>
		/// 零件下料数量
		/// </summary>
		public int? PartBlankingQuntity { get; set; } 

		/// <summary>
		/// 中间件编码
		/// </summary>
		public string SavantCode { get; set; } 

	}
}
