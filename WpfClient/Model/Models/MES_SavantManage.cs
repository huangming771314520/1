using System;

namespace Model.Models
{
	/// <summary>
	/// MES_SavantManage表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class MES_SavantManage
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public long ID { get; set; } 

		/// <summary>
		/// 中间件编码
		/// </summary>
		public string SavantCode { get; set; } 

		/// <summary>
		/// 工艺BOM主键ID
		/// </summary>
		public int? ProcessBomID { get; set; } 

		/// <summary>
		/// 备料尺寸
		/// </summary>
		public string SpareMateSize { get; set; } 

		/// <summary>
		/// 备料数量
		/// </summary>
		public int? SpareMateNum { get; set; } 

		/// <summary>
		/// 切分数量
		/// </summary>
		public int? SplitNum { get; set; } 

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark { get; set; } 

		/// <summary>
		/// 是否删除
		/// </summary>
		public int? IsEnable { get; set; } 

		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime? ModifyTime { get; set; } 

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

	}
}
