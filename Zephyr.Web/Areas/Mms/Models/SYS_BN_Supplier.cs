using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class SYS_BN_SupplierService : ServiceBase<SYS_BN_Supplier>
    {
        protected override void OnAfterHandleExcel(ref DataTable dtSheet)
        {
            var service= new SYS_BN_SupplierService();
            foreach (DataRow item in dtSheet.Rows)
            {
                string dName = item["SupplierName"].ToString();
                string sql = string.Format(@"select * from SYS_BN_Supplier where SupplierName='{0}'", dName);


                string isenable = item["IsEnable"].ToString();
                if (isenable == "否")
                {
                    item["IsEnable"] = 0;
                }
                else
                    item["IsEnable"] = 1;

                var SupplierCode = item["SupplierCode"].ToString();
                var Query = ParamDelete.Instance().AndWhere("SupplierCode", SupplierCode);
                service.Delete(Query);
            }
            base.OnAfterHandleExcel(ref dtSheet);
        }
        public string GetSupplierCode()
        {
            string sql = "   select [SupplierCode] from [SYS_BN_Supplier] where [SupplierCode] = (select max([SupplierCode]) from [SYS_BN_Supplier])";
            return db.Sql(sql).QuerySingle<string>();
        }
    }


    public class SYS_BN_Supplier : ModelBase
    {
        [PrimaryKey]   
        public int ID { get; set; }

        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string SupplierForShort { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}
