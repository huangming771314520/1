using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MesClient.Entitys
{
    public class WorkingTicketEntity
    {
        //项目

        public int ProjectID { get; set; }

        public string ContractCode { get; set; }

        public string ProjectName { get; set; }

        //产品

        public int ProductID { get; set; }

        public string ProductName { get; set; }

        //计划

        public int ProducePlanID { get; set; }

        public int Quantity { get; set; }

        public string ApsCode { get; set; }

        //工票

        public int WorkTicketID { get; set; }

        public string WorkTicketCode { get; set; }

        public string WorkStepsName { get; set; }

        public string WorkStepsContent { get; set; }

        public DateTime? ApproveTime { get; set; }

        public string ApproveRemark { get; set; }

        public int? TicketLevel { get; set; }

        public int? TicketStatus { get; set; }


        //工艺路线

        public int ProcessRouteID { get; set; }

        public string ProcessName { get; set; }

        //工艺bom

        public int ProcessBomID { get; set; }

        public string PartCode { get; set; }

        public string PartName { get; set; }

        public string FigureNo { get; set; }

    }
}
