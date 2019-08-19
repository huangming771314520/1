using System;

namespace Model.Models
{
	/// <summary>
	/// QMS_BN_InspectionEquipmentQualification表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class QMS_BN_InspectionEquipmentQualification
	{

		/// <summary>
		/// 质检设备资质ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 质检设备编码
		/// </summary>
		public string InspectionEquipmentCode { get; set; } 

		/// <summary>
		/// 质检设备名称
		/// </summary>
		public string InspectionEquipmentName { get; set; } 

		/// <summary>
		/// 资质编码
		/// </summary>
		public string QualificationCode { get; set; } 

		/// <summary>
		/// 资质名称
		/// </summary>
		public string QualificationName { get; set; } 

		/// <summary>
		/// 证书编号
		/// </summary>
		public string CertificateCode { get; set; } 

		/// <summary>
		/// 有效开始日期
		/// </summary>
		public DateTime? EffectiveBeginDate { get; set; } 

		/// <summary>
		/// 有效结束日期
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
		public DateTime? ModifyTiime { get; set; } 

	}
}
