using MesWpfClient.Commands;

namespace MesWpfClient.Models
{
    public class ProducePlanModel : ViewModelBase
    {
        private string contractCode;

        /// <summary>
        /// 合同编号
        /// </summary>
        public string ContractCode
        {
            get
            {
                return contractCode;
            }
            set
            {
                contractCode = value;
                OnPropertyChanged(nameof(ContractCode));
            }
        }

        private string projectName;

        /// <summary>
        /// 项目名称/工程项目
        /// </summary>
        public string ProjectName
        {
            get
            {
                return projectName;
            }
            set
            {
                projectName = value;
                OnPropertyChanged(nameof(ProjectName));
            }
        }

        private string processName;

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get
            {
                return processName;
            }
            set
            {
                processName = value;
                OnPropertyChanged(nameof(ProcessName));
            }
        }

        private string planDate;

        /// <summary>
        /// 计划时间
        /// </summary>
        public string PlanDate
        {
            get
            {
                return planDate;
            }
            set
            {
                planDate = value;
                OnPropertyChanged(nameof(PlanDate));
            }
        }

        private string partNameAndFigure;

        /// <summary>
        /// 零件名称与图号
        /// </summary>
        public string PartNameAndFigure
        {
            get
            {
                return partNameAndFigure;
            }
            set
            {
                partNameAndFigure = value;
                OnPropertyChanged(nameof(PartNameAndFigure));
            }
        }

        private string workStepsName;

        /// <summary>
        /// 工步名称
        /// </summary>
        public string WorkStepsName
        {
            get
            {
                return workStepsName;
            }
            set
            {
                workStepsName = value;
                OnPropertyChanged(nameof(WorkStepsName));
            }
        }

        private string background;

        /// <summary>
        /// 背景颜色
        /// </summary>
        public string Background
        {
            get
            {
                return background;
            }
            set
            {
                background = value;
                OnPropertyChanged(nameof(Background));
            }
        }

        private string approveRemark;

        /// <summary>
        /// 备注
        /// </summary>
        public string ApproveRemark
        {
            get
            {
                return approveRemark;
            }
            set
            {
                approveRemark = value;
                OnPropertyChanged(nameof(ApproveRemark));
            }
        }

        private string ticketLevelName;

        /// <summary>
        /// 工票优先级
        /// </summary>
        public string TicketLevelName
        {
            get
            {
                return ticketLevelName;
            }
            set
            {
                ticketLevelName = value;
                OnPropertyChanged(nameof(TicketLevelName));
            }
        }

    }
}
