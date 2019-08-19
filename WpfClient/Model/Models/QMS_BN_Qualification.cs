using System;

namespace Model.Models
{
	/// <summary>
	/// QMS_BN_Qualification表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class QMS_BN_Qualification
	{

		/// <summary>
		/// 检验资质ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 资质编码
		/// </summary>
		public string QualificationCode { get; set; } 

		/// <summary>
		/// 资质名称
		/// </summary>
		public string QualificationName { get; set; } 

		/// <summary>
		/// 检定周期
		/// </summary>
		public int? IdentificationCycle { get; set; } 

		/// <summary>
		/// 资质类型
		/// </summary>
		public int? QualificationState { get; set; } 

		/// <summary>
		/// 是否有效
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
		/// 更新信息
		/// </summary>
		public DateTime? ModifyTime { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public DateTime? InspectionBeginDate { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public DateTime? InspectionFinishDate { get; set; } 

	}
}
