using System;

namespace Model.Models
{
	/// <summary>
	/// PMS_BN_ProjectPart表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class PMS_BN_ProjectPart
	{

		/// <summary>
		/// 
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 合同编码
		/// </summary>
		public string ContractCode { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public int? ProjectDetailID { get; set; } 

		/// <summary>
		/// 项目编号
		/// </summary>
		public string ProjectID { get; set; } 

		/// <summary>
		/// 项目名称
		/// </summary>
		public string ProjectName { get; set; } 

		/// <summary>
		/// 零件编号
		/// </summary>
		public string PartCode { get; set; } 

		/// <summary>
		/// 零件名称
		/// </summary>
		public string PartName { get; set; } 

		/// <summary>
		/// 文件名
		/// </summary>
		public string FileName { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string VersionCode { get; set; } 

		/// <summary>
		/// 文件地址
		/// </summary>
		public string FileAddress { get; set; } 

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
