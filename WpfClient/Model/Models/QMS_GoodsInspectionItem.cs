using System;

namespace Model.Models
{
	/// <summary>
	/// QMS_GoodsInspectionItem表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class QMS_GoodsInspectionItem
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 主表ID
		/// </summary>
		public int? MainID { get; set; } 

		/// <summary>
		/// 单据编号
		/// </summary>
		public string BillCode { get; set; } 

		/// <summary>
		/// 质检项目编号
		/// </summary>
		public string InspectionItemCode { get; set; } 

		/// <summary>
		/// 质检项目名称
		/// </summary>
		public string InspectionItemName { get; set; } 

		/// <summary>
		/// 质检标准
		/// </summary>
		public string InspectionStandard { get; set; } 

		/// <summary>
		/// 检验结果
		/// </summary>
		public string InspectionResult { get; set; } 

		/// <summary>
		/// 是否合格
		/// </summary>
		public int? IsQualified { get; set; } 

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
		public string ModifyPerson { get; set; } 

		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime? ModifyTime { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string FileName { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string FileAddress { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string GoodsCheckItemCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string DocName { get; set; } 

	}
}
