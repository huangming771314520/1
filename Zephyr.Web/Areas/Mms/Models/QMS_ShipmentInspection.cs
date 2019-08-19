using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class QMS_ShipmentInspectionService : ServiceBase<QMS_ShipmentInspection>
    {
        public int AuditBillCode(string BillCode, out string msg)
        {
            msg = string.Empty;
            var rowsAffected = 0;
            string sql = String.Format(@"  select BillState FROM QMS_ShipmentInspection  where BillCode='{0}'", BillCode);
            var state = db.Sql(sql).QuerySingle<int>();
            if (state == 2)
            {
                msg = "单据已审核";
                return 0;
            }
            else
            {
                sql = string.Format(@"update QMS_ShipmentInspection set BillState=2 where BillCode='{0}'", BillCode);
                rowsAffected = db.Sql(sql).Execute();
                if (rowsAffected > 0)
                {
                    msg = "检验记录审核成功";
                    return rowsAffected;

                }
                else
                {
                    msg = "检验记录审核失败";
                    return 0;
                }
            }
        }
    }

    public class QMS_ShipmentInspection : ModelBase
    {
        [PrimaryKey]   
        public int ID { get; set; }
        public string BillCode { get; set; }
        public string ContractCode { get; set; }
        public string ProjectName { get; set; }
        public string ProductName { get; set; }
        public string ProductFigureNumber { get; set; }
        public string Model { get; set; }
        public int? CheckQuantity { get; set; }
        public int? QualifiedQuntity { get; set; }
        public string IsQualified { get; set; }
        public int? BillState { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string DocName { get; set; }
    }
}
