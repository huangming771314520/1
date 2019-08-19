using System;

namespace Model.Models
{
	/// <summary>
	/// SYS_Email表实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class SYS_Email
	{

		/// <summary>
		/// 
		/// </summary>
		public int ID { get; set; } 

		/// <summary>
		/// 邮件编码
		/// </summary>
		public string EmailCode { get; set; } 

		/// <summary>
		/// 主题
		/// </summary>
		public string EmailName { get; set; } 

		/// <summary>
		/// 发送人
		/// </summary>
		public string Sender { get; set; } 

		/// <summary>
		/// 抄送
		/// </summary>
		public string CarbonCopy { get; set; } 

		/// <summary>
		/// 主要内容
		/// </summary>
		public string Context { get; set; } 

		/// <summary>
		/// 发送时间
		/// </summary>
		public DateTime? SendTime { get; set; } 

		/// <summary>
		/// 
		/// </summary>
		public string Addressee { get; set; } 

		/// <summary>
		/// 是否已发送
		/// </summary>
		public int? IsSend { get; set; } 

	}
}
