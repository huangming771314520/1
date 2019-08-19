using System;

namespace Model.Models
{
	/// <summary>
	/// QMS_BN_InspectionEquipment表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class QMS_BN_InspectionEquipment
	{

		/// <summary>
		/// 质检设备ID
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 质检设备编号
		/// </summary>
		public string InspectionEquipmentCode { get; set; } 

		/// <summary>
		/// 质检设备名称
		/// </summary>
		public string InspectionEquipmentName { get; set; } 

		/// <summary>
		/// 质检设备状态
		/// </summary>
		public int? InspectionEquipmenState { get; set; } 

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
		public DateTime? MoodifyTime { get; set; } 

		/// <summary>
		/// 规格型号
		/// </summary>
		public string SpecModel { get; set; } 

		/// <summary>
		/// 制造厂名
		/// </summary>
		public string FactoryName { get; set; } 

		/// <summary>
		/// 出厂编号
		/// </summary>
		public string FactoryNumber { get; set; } 

		/// <summary>
		/// 设备编号
		/// </summary>
		public string EquipmentCode { get; set; } 

		/// <summary>
		/// 精度等级
		/// </summary>
		public string PrecisionLevel { get; set; } 

		/// <summary>
		/// 采购日期
		/// </summary>
		public DateTime? PurchaseDate { get; set; } 

	}
}
