using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]

    public class MES_SavantAndPBomService : ServiceBase<MES_SavantAndPBom>
    {

    }

    public class MES_SavantAndPBom : ModelBase
    {
        [PrimaryKey]
        [Identity]

        /// <summary>
        /// 
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 中间件编码
        /// </summary>
        public string SavantCode { get; set; }

        /// <summary>
        /// 工艺BOM主键ID
        /// </summary>
        public int? ProcessBomID { get; set; }

        /// <summary>
        /// 本次下料数量
        /// </summary>
        public int? BlankingNum { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }

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
    }
}