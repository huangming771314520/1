using System;

namespace Model.Models
{
	/// <summary>
	/// MES_WorkshopBatchingDetail表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class MES_WorkshopBatchingDetail
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 零件编号
		/// </summary>
		public string PartCode { get; set; } 

		/// <summary>
		/// 零件图号
		/// </summary>
		public string PartFigureNumber { get; set; } 

		/// <summary>
		/// 零件名称
		/// </summary>
		public string PartName { get; set; } 

		/// <summary>
		/// 材质
		/// </summary>
		public string MaterialCode { get; set; } 

		/// <summary>
		/// 单台数量
		/// </summary>
		public int? PartQuantity { get; set; } 

		/// <summary>
		/// 台数
		/// </summary>
		public int? Quantity { get; set; } 

		/// <summary>
		/// 需求数量
		/// </summary>
		public int? RequirementNum { get; set; } 

		/// <summary>
		/// 领料数量
		/// </summary>
		public int? BatchingNum { get; set; } 

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
		/// 出库部门
		/// </summary>
		public string OutDeptCode { get; set; } 

		/// <summary>
		/// 所属配料单号
		/// </summary>
		public string BatchingCode { get; set; } 

		/// <summary>
		/// 父级零件编码
		/// </summary>
		public string ParentCode { get; set; } 

		/// <summary>
		/// 是否关键件
		/// </summary>
		public int? IsCrux { get; set; } 

	}
}
