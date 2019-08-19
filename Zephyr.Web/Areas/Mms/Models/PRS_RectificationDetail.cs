using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class PRS_RectificationDetailService : ServiceBase<PRS_RectificationDetail>
    {
        public int GetDeleteData(string id, out string msg)
        {
            msg = string.Empty;
            string sql = string.Format(@"Update PRS_RectificationDetail set IsEnable=0 where id={0}", id);
            int res = db.Sql(sql).Execute();
            if (res > 0)
            {
                msg = "操作成功";
                return res;
            }
            else
            {
                msg = "操作失败";
                return 0;
            }
        }
    }

    public class PRS_RectificationDetail : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ID { get; set; }
        public string MainID { get; set; }
        public string PartCode { get; set; }
        public string ChargePerson { get; set; }
        public string RectificationContent { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }
        public string ProcessDesc { get; set; }
        public int? ProcessLineCode { get; set; }
        public string WorkshopCode { get; set; }
        public string EquipmentCode { get; set; }
        public string WorkGroupCode { get; set; }
        public string WarehouseCode { get; set; }
        public string WorkshopName { get; set; }
        public string EquipmentName { get; set; }
        public string WorkGroupName { get; set; }
        public string WarehouseName { get; set; }
        public int? Quantity { get; set; } 
        public int? ManHour { get; set; }
        public int? Unit { get; set; }
        public string FigureCode { get; set; }
        public string ToolCode { get; set; }
        //alter table PRS_RectificationDetail add IsCheck int null
        public int? IsCheck { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string ApproveState { get; set; }
        public string ApprovePerson { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string ApproveRemark { get; set; }

        public string FileName { get; set; }

        public string FileAddress { get; set; }

        public string DocName { get; set; }
    }
}
