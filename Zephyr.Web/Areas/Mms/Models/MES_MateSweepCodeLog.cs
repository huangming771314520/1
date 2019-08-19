using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class MES_MateSweepCodeLogService : ServiceBase<MES_MateSweepCodeLog>
    {

    }

    public class MES_MateSweepCodeLog : ModelBase
    {
        [PrimaryKey]
        [Identity]
        /// <summary>
		/// 主键ID
		/// </summary>
		public long ID { get; set; }

        /// <summary>
        /// 用户条码
        /// </summary>
        public string UserBarCode { get; set; }

        /// <summary>
        /// 生产任务编码
        /// </summary>
        public string ApsCode { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string MateBarCode { get; set; }

        /// <summary>
        /// 物料数量
        /// </summary>
        public int? MateQuantity { get; set; }

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
        public DateTime? ModifyTime { get; set; }
    }
}