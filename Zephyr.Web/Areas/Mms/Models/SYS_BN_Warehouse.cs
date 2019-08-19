using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class SYS_BN_WarehouseService : ServiceBase<SYS_BN_Warehouse>
    {
        protected override void OnAfterHandleExcel(ref DataTable dtSheet)
        {
            var service = new SYS_BN_WarehouseService();
            foreach (DataRow item in dtSheet.Rows)
            {

                string dName = item["StoreKeeper"].ToString();
                string sql = string.Format(@"SELECT UserCode FROM dbo.SYS_BN_User WHERE IsEnable = 1 AND DepartmentCode = '0107' AND UserName = '{0}';", dName);
                string hID = db.Sql(sql).QuerySingle<string>();
                item["UserCode"] = hID;

                string isenable = item["IsEnable"].ToString();
                if (isenable == "否")
                {
                    item["IsEnable"] = 0;
                }
                else
                    item["IsEnable"] = 1;

                var WarehouseCode = item["WarehouseCode"].ToString();
                var Query = ParamDelete.Instance().AndWhere("WarehouseCode", WarehouseCode);
                service.Delete(Query);
            }
            base.OnAfterHandleExcel(ref dtSheet);


        }
        public int GetWarehouseIDByCode(string ID)
        {
            var pQuery = ParamQuery.Instance()
                .Select("ID")
                .AndWhere("UserCode", ID);
            var list = base.GetModelList(pQuery);
            if (list.Count > 0)
            {
                return list[0].ID;
            }
            else
                return 0;
        }
        public dynamic GetWarehouseByCode(string ID)
        {
            var pQuery = ParamQuery.Instance()
                .Select("WarehouseCode,WarehouseName")
                .AndWhere("UserCode", ID);
            var list = base.GetModelList(pQuery);
            if (list.Count > 0)
            {
                return list[0];
            }
            else
                return new SYS_BN_Warehouse(); ;
        }
        public string GetWarehouseCode()
        {
            string sql = "select [WarehouseCode] from [SYS_BN_Warehouse] where [WarehouseCode] = (select max([WarehouseCode]) from [SYS_BN_Warehouse])";
            return db.Sql(sql).QuerySingle<string>();
        }
        public string GetWarehouseCodeByCode(string ID)
        {
            var pQuery = ParamQuery.Instance()
                .Select("WarehouseCode,WarehouseName")
                .AndWhere("UserCode", ID);
            var list = base.GetModelList(pQuery);
            if (list.Count > 0)
            {
                return list[0].WarehouseCode;
            }
            else
                return "0";
        }
    }

    public class SYS_BN_Warehouse : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ID { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        public string UserCode { get; set; }
        public string StoreKeeper { get; set; }
        public int? WarehouseType { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}
