using System;

namespace Model.Models
{
	/// <summary>
	/// PRS_ProcessRouteModelEquipment表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class PRS_ProcessRouteModelEquipment
	{

		/// <summary>
		/// 
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public int? MainID { get; set; } 

		/// <summary>
		/// 设备编码
		/// </summary>
		public string EquipmentCode { get; set; } 

		/// <summary>
		/// 设备类型
		/// </summary>
		public string EquitmentType { get; set; } 

		/// <summary>
		/// 所属车间
		/// </summary>
		public string AffiliatedWorkshopID { get; set; } 

		/// <summary>
		/// 设备参数
		/// </summary>
		public string EquiptmentParms { get; set; } 

		/// <summary>
		/// 设备状态
		/// </summary>
		public string EquipmentState { get; set; } 

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
		public string EquipmentName { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string epqID { get; set; } 

	}
}
