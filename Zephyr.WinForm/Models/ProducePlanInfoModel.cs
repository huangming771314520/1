using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.WinForm.Models
{
    public class ProducePlanInfoModel
    {
        public bool result { get; set; }

        public string msg { get; set; }

        public List<DataModel> data { get; set; }

        public class DataModel
        {
            public GetProducePlanInfoModel GetProducePlanInfo { get; set; }

            public GetProjectDetailModel GetProjectDetail { get; set; }

            public GetProjectModel GetProject { get; set; }

            public GetCodeModel GetCode { get; set; }

            public GetProductProcessRouteModel GetProductProcessRoute { get; set; }

            public GetProductProcessRouteDetailModel GetProductProcessRouteDetail { get; set; }

            public GetProductProcessRouteEquipmentModel GetProductProcessRouteEquipment { get; set; }

            public class GetProducePlanInfoModel
            {
                public int ID { get; set; }
                public string ContractCode { get; set; }
                public int? ProjectDetailID { get; set; }
                public int? ProductPlanMainID { get; set; }
                public string PartCode { get; set; }
                public string ProcessCode { get; set; }
                public string ProcessLineCode { get; set; }
                public string WorkshopID { get; set; }
                public string WorkshopName { get; set; }
                public string EquipmentID { get; set; }
                public string EquipmentName { get; set; }
                public string WorkGroupID { get; set; }
                public string WorkGroupName { get; set; }
                public int? Quantity { get; set; }
                public int? ManHour { get; set; }
                public int? Unit { get; set; }
                public DateTime? EarliestStartTime { get; set; }
                public DateTime? LatestStartTime { get; set; }
                public DateTime? PlanedStartTime { get; set; }
                public DateTime? EarliestFinishTime { get; set; }
                public DateTime? LatestFinishTime { get; set; }
                public DateTime? PlanedFinishTime { get; set; }
                public DateTime? ActualStartTime { get; set; }
                public DateTime? ActualFinishTime { get; set; }
                public DateTime? FloatingHour { get; set; }
                public string PlanColor { get; set; }
                public int? PlanState { get; set; }
                public int? IsEnable { get; set; }
                public string CreatePerson { get; set; }
                public DateTime? CreateTime { get; set; }
                public string ModifyPerson { get; set; }
                public DateTime? ModifyTime { get; set; }

                public string PlanedStart { get; set; }

                public string ReName { get; set; }

                public string ApproveState { get; set; }

                public string ApprovePerson { get; set; }

                public DateTime? ApproveDate { get; set; }

                public string ApproveRemark { get; set; }

                public string ApsCode { get; set; }

                public string ProductPlanCode { get; set; }

                public string ProcessName { get; set; }
            }

            public class GetProjectModel
            {
                public int ProjectID { get; set; }
                public string ContractCode { get; set; }
                public string ProjectName { get; set; }
                public string ProjectForShort { get; set; }
                public int? IsEnable { get; set; }
                public string CreatePerson { get; set; }
                public DateTime? CreateTime { get; set; }
                public string ModifyPerson { get; set; }
                public DateTime? ModifyTime { get; set; }
                public DateTime? AdvancePaymentDate { get; set; }
                public int? Is0tSartProduct { get; set; }
                public string Remark { get; set; }
                public string ProductReport { get; set; }
                public DateTime? ContractReceiveTime { get; set; }
            }

            public class GetProjectDetailModel
            {
                public int ID { get; set; }
                public int? MainID { get; set; }
                public string ProductName { get; set; }
                public int? ProductType { get; set; }
                public string Model { get; set; }
                public string Specifications { get; set; }
                public int? Quantity { get; set; }
                public string FigureNumber { get; set; }
                public DateTime? DeliveryTime { get; set; }
                public string Remark { get; set; }
                public int? Urgent { get; set; }
                public int? ProjectState { get; set; }
                public int? IsEnable { get; set; }
                public string CreatePerson { get; set; }
                public DateTime? CreateTime { get; set; }
                public string ModifyPerson { get; set; }
                public DateTime? ModifyTime { get; set; }

                public string ProductUnit { get; set; }

                public string TotalWeight { get; set; }

                public string PlanPrice { get; set; }

                public int? CompleteQuantity { get; set; }

            }

            public class GetCodeModel
            {
                public string Code { get; set; }

                public string Value { get; set; }

                public string Text { get; set; }

                public string ParentCode { get; set; }

                public string Seq { get; set; }

                public bool? IsEnable { get; set; }

                public bool? IsDefault { get; set; }

                public string Description { get; set; }

                public string CodeTypeName { get; set; }

                public string CodeType { get; set; }

                public string CreatePerson { get; set; }

                public string CreateDate { get; set; }

                public string UpdatePerson { get; set; }

                public string UpdateDate { get; set; }
            }

            public class GetProductProcessRouteModel
            {
                public int ID { get; set; }
                public string ContractCode { get; set; }
                public string ProjectName { get; set; }
                public string PartCode { get; set; }
                public string ProcessCode { get; set; }
                public string ProcessName { get; set; }
                public string ProcessDesc { get; set; }
                public int? ProcessLineCode { get; set; }
                public string WorkshopID { get; set; }
                public string EquipmentID { get; set; }
                public string WorkGroupID { get; set; }
                public string WarehouseID { get; set; }
                public string WorkshopName { get; set; }
                public string EquipmentName { get; set; }
                public string WorkGroupName { get; set; }
                public string WarehouseName { get; set; }
                public int? ManHour { get; set; }
                public int Unit { get; set; }
                public string FigureCode { get; set; }
                public string Remark { get; set; }
                public int? IsEnable { get; set; }
                public int? IsInspectionReport { get; set; }
                public string CreatePerson { get; set; }
                public DateTime? CreateTime { get; set; }
                public string ModifyPerson { get; set; }
                public DateTime? ModifyTime { get; set; }

            }

            public class GetProductProcessRouteDetailModel
            {
                public int ID { get; set; }
                public int ProcessRouteID { get; set; }
                public string ToolCode { get; set; }
                public string ToolName { get; set; }
                public int ToolNum { get; set; }
                public int? IsEnable { get; set; }
                public string CreatePerson { get; set; }
                public DateTime? CreateTime { get; set; }
                public string ModifyPerson { get; set; }
                public DateTime? ModifyTime { get; set; }
            }

            public class GetProductProcessRouteEquipmentModel
            {
                public int ID { get; set; }
                public int ProcessRouteID { get; set; }
                public string EquipmentCode { get; set; }
                public string EquitmentName { get; set; }
                public string EquitmentType { get; set; }
                public string AffiliatedWorkshopID { get; set; }
                public string EquiptmentParms { get; set; }
                public string EquipmentState { get; set; }
                public int? IsEnable { get; set; }
                public string CreatePerson { get; set; }
                public DateTime? CreateTime { get; set; }
                public string ModifyPerson { get; set; }
                public DateTime? ModifyTime { get; set; }
            }
        }
    }
}
