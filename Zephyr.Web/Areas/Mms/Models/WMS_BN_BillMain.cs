using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class WMS_BN_BillMainService : ServiceBase<WMS_BN_BillMain>
    {
        public string GetBillCodeByID(string ID)
        {
            var pQuery = ParamQuery.Instance()
                .Select("BillCode")
                .AndWhere("ID", ID);
            var list = base.GetModelList(pQuery);
            if (list.Count > 0)
            {
                return list[0].BillCode;
            }
            else
                return string.Empty;
        }

        public dynamic SetBillMainData(WMS_BN_BillMain model)
        {

            return new
            {
                result = true,
                id = string.Empty
            };
        }

    }

    public class WMS_BN_BillMain : ModelBase
    {

        [PrimaryKey]
        public int ID { get; set; }
        public string BillCode { get; set; }
        public int? BillType { get; set; }
        public string ContractCode { get; set; }
        public string ProjectName { get; set; }

        public string SalesmanCode { get; set; }
        public string Salesman { get; set; }
        public string DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string WarehouseCode { get; set; }

        public string WarehouseName { get; set; }
        public string PickPersonCode { get; set; }
        public string PickPerson { get; set; }
        public string OrderBillCode { get; set; }
        public string ApproveState { get; set; }
        public string ApprovePerson { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string ApproveRemark { get; set; }
        public string Remark { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? IsEnable { get; set; }
    }
}
