using System;

namespace Model.Models
{
	/// <summary>
	/// APS_BN_DispatchModelDetail表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class APS_BN_DispatchModelDetail
	{

		/// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 任务名称
		/// </summary>
		public string Name { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public int? IsEnable { get; set; } 

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark { get; set; } 

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
		/// 外键ID
		/// </summary>
		public int? MainID { get; set; } 

		/// <summary>
		/// 任务周期
		/// </summary>
		public int? TaskCycle { get; set; } 

		/// <summary>
		/// 排序字段
		/// </summary>
		public int? Seq { get; set; } 

	}
}
