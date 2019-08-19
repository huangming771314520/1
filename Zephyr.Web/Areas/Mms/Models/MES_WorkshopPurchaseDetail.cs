using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class MES_WorkshopPurchaseDetailService : ServiceBase<MES_WorkshopPurchaseDetail>
    {
        public List<dynamic> GetWorkshopPurchaseDetail(string mainID)
        {
            string sql = String.Format(@"select InventoryCode,PurchaseQuantity as TotalRequestQuantity from MES_WorkshopPurchaseDetail where mainid='{0}'", mainID);
            return db.Sql(sql).QueryMany<dynamic>();
        }
        public int AuditBillCode(string BillCode, out string msg)
        {
            msg = string.Empty;

            var rowsAffected = 0;
            string sql = String.Format(@"  select ApproveState FROM MES_WorkshopPurchaseMain  where WorkshopPurchaseCode='{0}'", BillCode);
            var state = db.Sql(sql).QuerySingle<string>();
            if (state == "2")
            {
                msg = "单据已审核";
                return 0;
            }
            else
            {
                sql = string.Format(@"update MES_WorkshopPurchaseMain set ApproveState='2',ApprovePerson='{1}',ApproveDate='{2}' where WorkshopPurchaseCode='{0}'", BillCode, MmsHelper.GetUserCode(), DateTime.Now);
                rowsAffected = db.Sql(sql).Execute();
                if (rowsAffected > 0)
                {
                    msg = "单据审核成功";
                    return rowsAffected;
                }
                else
                {
                    msg = "单据审核失败";
                    return 0;
                }
            }
        }

    }

    public class MES_WorkshopPurchaseDetail : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ID { get; set; }
        public int MainID { get; set; }
        public string InventoryCode { get; set; }
        public string Spec { get; set; }
        public string Model { get; set; }
        public string InventoryName { get; set; }
        public string MeterialName { get; set; }
        public string WriteModel { get; set; }
        public string WriteSpec { get; set; }
        public int PurchaseQuantity { get; set; }
        public string Remark { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string New_InventoryCode { get; set; }
    }
}
