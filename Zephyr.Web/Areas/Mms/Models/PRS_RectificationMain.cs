using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class PRS_RectificationMainService : ServiceBase<PRS_RectificationMain>
    {
        public string GetContractCodeByID(string ID)
        {
            var pQuery = ParamQuery.Instance()
                .Select("ContractCode")
                .AndWhere("ID", ID);
            var list = base.GetModelList(pQuery);
            if (list.Count > 0)
            {
                return list[0].ContractCode;
            }
            else
                return string.Empty;
        }
    }

    public class PRS_RectificationMain : ModelBase
    {
        [PrimaryKey]
        public int ID { get; set; }
        public string BillCode { get; set; }
        public string ContractCode { get; set; }
        public string ProjectName { get; set; }
        public int? ProjectDetailID { get; set; }
        public string ProductName { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? BillState { get; set; }
    }
}
