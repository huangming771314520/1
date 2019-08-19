using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;
using Zephyr.Utils;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class WMS_BN_TakeStockDetailService : ServiceBase<WMS_BN_TakeStockDetail>
    {
        protected override void OnAfterHandleExcel(ref System.Data.DataTable dtSheet)
        {
            foreach (DataRow item in dtSheet.Rows)
            {
                string dName = item["WarehouseName"].ToString();
                string sql = string.Format(@"select ID from SYS_BN_Warehouse where WarehouseName='{0}'", dName);
                int hID = db.Sql(sql).QuerySingle<int>();
                if (hID == 0)
                {
                    MmsHelper.ThrowHttpExceptionWhen(true, "仓库名：" + dName + "错误，请检查数据", "");
                    return;
                }
                item["WarehouseCode"] = hID;
            }
            base.OnAfterHandleExcel(ref dtSheet);
        }
    }

    public class WMS_BN_TakeStockDetail : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ID { get; set; }
        //public int? StockMianId { get; set; }
        public string BillCode { get; set; }
        public string InventoryCode { get; set; }
        public string InventoryName { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        public double? RealNum { get; set; }
        public double? TakeStockNum { get; set; }
        public double? DValue { get; set; }
        public string BatchCode { get; set; }
        //public int? IsCanReceive { get; set; }
        public string Remark { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? IsEnable { get; set; }
    }
}
