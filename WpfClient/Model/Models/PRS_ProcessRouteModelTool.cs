using System;

namespace Model.Models
{
	/// <summary>
	/// PRS_ProcessRouteModelTool表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class PRS_ProcessRouteModelTool
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 工艺路线模型ID
		/// </summary>
		public int? ProcessRouteID { get; set; } 

		/// <summary>
		/// 工具编码
		/// </summary>
		public string ToolCode { get; set; } 

		/// <summary>
		/// 工具名称
		/// </summary>
		public string ToolName { get; set; } 

		/// <summary>
		/// 工具数量
		/// </summary>
		public int? ToolNum { get; set; } 

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
		/// 工艺类型
		/// </summary>
		public int? ProcessType { get; set; } 

	}
}
