using System;

namespace Model.Models
{
	/// <summary>
	/// PMS_BN_ProjectDetail表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class PMS_BN_ProjectDetail
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 外键ID
		/// </summary>
		public int? MainID { get; set; } 

		/// <summary>
		/// 产品名称
		/// </summary>
		public string ProductName { get; set; } 

		/// <summary>
		/// 产品类型
		/// </summary>
		public int? ProductType { get; set; } 

		/// <summary>
		/// 型号
		/// </summary>
		public string Model { get; set; } 

		/// <summary>
		/// 规格
		/// </summary>
		public string Specifications { get; set; } 

		/// <summary>
		/// 产品数量
		/// </summary>
		public int? Quantity { get; set; } 

		/// <summary>
		/// 图号
		/// </summary>
		public string FigureNumber { get; set; } 

		/// <summary>
		/// 交货日期
		/// </summary>
		public DateTime? DeliveryTime { get; set; } 

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark { get; set; } 

		/// <summary>
		/// 紧急度 1：一般 2：重要 3：紧急
		/// </summary>
		public int? Urgent { get; set; } 

		/// <summary>
		/// 项目状态
		/// </summary>
		public int? ProjectState { get; set; } 

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
		public string ProductUnit { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string TotalWeight { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string PlanPrice { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public int? CompleteQuantity { get; set; } 

	}
}
