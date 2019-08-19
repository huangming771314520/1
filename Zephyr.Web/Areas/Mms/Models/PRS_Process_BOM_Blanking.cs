using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;
using System.Linq;
using System.Dynamic;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Utils.ExcelLibrary.BinaryFileFormat;
using System.IO;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class PRS_Process_BOM_BlankingService : ServiceBase<PRS_Process_BOM_Blanking>
    {
      
    }

    public class PRS_Process_BOM_Blanking : ModelBase
    {
        [Identity]
        [PrimaryKey]

        /// <summary>
		/// 
		/// </summary>
		public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PartFigureCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PartCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PartName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string InventoryCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string InventoryName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? PartQuantity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ParentCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Weight { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Totalweight { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MaterialInventoryCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MaterialInventoryName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? MaterialQuantity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string IsSelfMade { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? IsEnable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CreatePerson { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ModifyPerson { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ApproveState { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ApprovePerson { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ApproveDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ApproveRemark { get; set; }

        /// <summary>
        /// 定料材料
        /// </summary>
        public string SetMateName { get; set; }

        /// <summary>
        /// 定料数量
        /// </summary>
        public double? SetMateNum { get; set; }

        /// <summary>
        /// 到位尺寸
        /// </summary>
        public string InPlanceSize { get; set; }

        /// <summary>
        /// 下料尺寸
        /// </summary>
        public string BlankingSize { get; set; }

        /// <summary>
        /// 材料类型:板材、棒材、其他
        /// </summary>
        public int? MateType { get; set; }

        /// <summary>
        /// 下料方式:数控切割、锯、手工切割
        /// </summary>
        public int? BlankingType { get; set; }

        /// <summary>
        /// 规格_新
        /// </summary>
        public string New_Specs { get; set; }

        /// <summary>
        /// 型号_新
        /// </summary>
        public string New_Model { get; set; }

        /// <summary>
        /// 名称_新
        /// </summary>
        public string New_PartName { get; set; }

        /// <summary>
        /// 材料参数：厚度或直径
        /// </summary>
        public string MateParamValue { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public int? ProductID { get; set; }

        /// <summary>
        /// 零件编号(新)
        /// </summary>
        public string New_InventoryCode { get; set; }

        /// <summary>
        /// 工艺下料
        /// </summary>
        public int? Blanking { get; set; }

        /// <summary>
        /// 工艺焊接
        /// </summary>
        public int? Welding { get; set; }

        /// <summary>
        /// 工艺机加
        /// </summary>
        public int? Machining { get; set; }

        /// <summary>
        /// 工艺装配
        /// </summary>
        public int? Assembling { get; set; }

        /// <summary>
        /// 需求下料数量
        /// </summary>
        public int? BlankingTotal { get; set; }

        /// <summary>
        /// 下料数量
        /// </summary>
        public int? BlankingNum { get; set; }

        /// <summary> 
		/// 外购数量 
		/// </summary> 
		public int? PurchaseNum { get; set; }

        /// <summary> 
        /// 改制数量 
        /// </summary> 
        public int? RestructNum { get; set; }

        public int? Sort { get; set; }
        public int? IsCrux { get; set; }

        public int? PartQuantityAll { get; set; }
    }
}
