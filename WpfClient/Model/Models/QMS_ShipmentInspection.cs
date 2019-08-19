using System;

namespace Model.Models
{
	/// <summary>
	/// QMS_ShipmentInspection表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class QMS_ShipmentInspection
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 检验单号
		/// </summary>
		public string BillCode { get; set; } 

		/// <summary>
		/// 合同编号
		/// </summary>
		public string ContractCode { get; set; } 

		/// <summary>
		/// 项目名称
		/// </summary>
		public string ProjectName { get; set; } 

		/// <summary>
		/// 产品名称
		/// </summary>
		public string ProductName { get; set; } 

		/// <summary>
		/// 产品图号
		/// </summary>
		public string ProductFigureNumber { get; set; } 

		/// <summary>
		/// 型号规格
		/// </summary>
		public string Model { get; set; } 

		/// <summary>
		/// 检验数量
		/// </summary>
		public int? CheckQuantity { get; set; } 

		/// <summary>
		/// 合格数量
		/// </summary>
		public int? QualifiedQuntity { get; set; } 

		/// <summary>
		/// 是否合格
		/// </summary>
		public string IsQualified { get; set; } 

		/// <summary>
		/// 单据状态
		/// </summary>
		public int? BillState { get; set; } 

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

	}
}
