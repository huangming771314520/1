using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class WMS_BN_BillDetailService : ServiceBase<WMS_BN_BillDetail>
    {
       //GetBillDetail
        public dynamic GetBillDetail(string BillCode)
        {
            //dynamic DetailList = ServiceBase.Instance().GetDynamicList(ParamQuery.Instance().AndWhere("BillCode", BillCode).Select("*"));
            //return DetailList;
            string sql = string.Format(@"select * from WMS_BN_BillDetail where IsEnable=1 and BillCode='{0}'", BillCode);
            dynamic list = db.Sql(sql).QueryMany<dynamic>();
            return list;
        }


        public dynamic GetIsExit(string BillCode)
        {
            //dynamic DetailList = ServiceBase.Instance().GetDynamicList(ParamQuery.Instance().AndWhere("BillCode", BillCode).Select("*"));
            //return DetailList;
            string sql = string.Format(@"select * from WMS_BN_BillDetail where IsEnable=1 and PBillCode='{0}'", BillCode);
            List<WMS_BN_BillDetail> list = db.Sql(sql).QueryMany<WMS_BN_BillDetail>();
            return list.Count;
        }
    }

    public class WMS_BN_BillDetail : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public string BillCode { get; set; }
        public string OrderBillCode { get; set; }
        public string InventoryCode { get; set; }
        public string InventoryName { get; set; }
        public string Specs { get; set; }
        public string Unit { get; set; }
        public double? MateNum { get; set; }
        public double? ConfirmNum { get; set; }
        public double? UnitPrice { get; set; }
        public double? TotalPrice { get; set; }
        public string PackageCode { get; set; }
        public string BatchCode { get; set; }
        public string PBillCode { get; set; }
        public string AccountabilityCode { get; set; }
        public string Remark { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? IsEnable { get; set; }
        public string FileName { get; set; }
        public string FileAddress { get; set; }
        public string DocName { get; set; }
    }
}
