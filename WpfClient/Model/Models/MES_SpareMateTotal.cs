using System;

namespace Model.Models
{
	/// <summary>
	/// MES_SpareMateTotal表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class MES_SpareMateTotal
	{

		/// <summary>
		/// 
		/// </summary>
		public long ID { get; set; } 

		/// <summary>
		/// 所属工艺BOM
		/// </summary>
		public int? ProcessBomID { get; set; } 

		/// <summary>
		/// 本次下料数量
		/// </summary>
		public int? BlankingNum { get; set; } 

	}
}
