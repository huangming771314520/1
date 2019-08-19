using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class MES_ProcessInspectionRequestService : ServiceBase<MES_ProcessInspectionRequest>
    {
       
    }

    public class MES_ProcessInspectionRequest : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        /// <summary>
        /// 产品工艺路线ID
        /// </summary>
        public int ProductProcessRouteID { get; set; }
        public string BillCode { get; set; }
        public string ContractCode { get; set; }
        public string ProjectName { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string ProductName { get; set; }
        public string ProductFigureNumber { get; set; }
        public string PartCode { get; set; }
        public string PartName { get; set; }
        public string partFigure { get; set; }
        public string MaterialCode { get; set; }
        public int? CheckQuantity { get; set; }
        public string Unit { get; set; }
        public int? BillState { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string OperatorCode { get; set; }
        //public string DepartmentCode { get; set; }
        //public string DepartmentName { get; set; }
        public string EquipmentCode { get; set; }
        public string EquipmentName { get; set; }
        //public string Unit { get; set; }
        public string ProductPlanCode { get; set; }
    }
}
