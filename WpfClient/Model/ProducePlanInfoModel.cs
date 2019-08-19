using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;

namespace Model
{
    public class ProducePlanInfoModel
    {
        public bool Result { get; set; }

        public string Msg { get; set; }

        public List<DataModel> Data { get; set; }

        public class DataModel
        {
            public PMS_BN_Project GetProject { get; set; }

            public PMS_BN_ProjectDetail GetProjectDetail { get; set; }

            public MES_WorkingTicket_Extend GetProjectProduceDetial { get; set; }

            public MES_BN_ProductProcessRoute GetProductProcessRoute { get; set; }

            public MES_BN_ProductProcessRouteDetail GetProductProcessRouteDetail { get; set; }

            public MES_BN_ProductProcessRouteEquipment GetProductProcessRouteEquipment { get; set; }

            public Sys_Code GetCode { get; set; }

            public List<PMS_BN_PartFile> GetPartFiles { get; set; }

            public List<QMS_QualityReportDoc> GetQualityReportDocs { get; set; }

            public List<PRS_ProcessFigure> GetProcessFigures { get; set; }

            public PRS_Process_BOM GetProcessBOM { get; set; }

            public List<PRS_ProcessWorkSteps> GetProcessWorkSteps { get; set; }
        }

    }
}
