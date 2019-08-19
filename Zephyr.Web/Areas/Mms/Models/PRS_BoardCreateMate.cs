using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class PRS_BoardCreateMateService : ServiceBase<PRS_BoardCreateMate>
    {
       
    }

    public class PRS_BoardCreateMate : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public long ID { get; set; }
        public string ContractCode { get; set; }
        public string InventoryCode { get; set; }
        public string InventoryName { get; set; }
        public string Model { get; set; }
        public string Specs { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string InventoryNum { get; set; }

        /// <summary>
		/// 单位
		/// </summary>
		public string Unit { get; set; }

        /// <summary>
        /// 新零件编号
        /// </summary>
        public string New_InventoryCode { get; set; }

        /// <summary>
        /// 新零件名称
        /// </summary>
        public string New_InventoryName { get; set; }

        /// <summary>
        /// 新型号
        /// </summary>
        public string New_Model { get; set; }

        /// <summary>
        /// 新规格
        /// </summary>
        public string New_Specs { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
