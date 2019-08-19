using System;

namespace Model.Models
{
	/// <summary>
	/// QMS_BN_InspectorQualification表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class QMS_BN_InspectorQualification
	{

		/// <summary>
		/// 质检员资质ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 质检员用户编码
		/// </summary>
		public string UserCode { get; set; } 

		/// <summary>
		/// 质检员名字
		/// </summary>
		public string InspectorName { get; set; } 

		/// <summary>
		/// 资质编码
		/// </summary>
		public string QualificationCode { get; set; } 

		/// <summary>
		/// 证书编号
		/// </summary>
		public string CertificateCode { get; set; } 

		/// <summary>
		/// 有效开始时间
		/// </summary>
		public DateTime? EffectiveBegainDate { get; set; } 

		/// <summary>
		/// 有效结束时间
		/// </summary>
		public DateTime? EffectiveEndDate { get; set; } 

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
		/// 更新时间
		/// </summary>
		public DateTime? ModiftTime { get; set; } 

	}
}
