using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace PreviewClient.ViewModels
{
    public class newProduceProceViewModel : ViewModelBase
    {
        public newProduceProceViewModel()
        {
            //projectName = "啥啥啥项目";
            //workSteps = new List<WorkStepModel>()
            //{
            //    new WorkStepModel (){LineNum=1,WorkStepsName="洗",WorkStepsInfo="洗白白洗白白"},
            //    new WorkStepModel (){LineNum=2,WorkStepsName="吹",WorkStepsInfo="吹呀吹呀"},
            //    new WorkStepModel (){LineNum=3,WorkStepsName="烫",WorkStepsInfo="开始烫咯"},
            //    new WorkStepModel (){LineNum=4,WorkStepsName="烘",WorkStepsInfo="烘干咯"},
            //};
            //workStep = new WorkStepModel() { LineNum = 2, WorkStepsName = "吹", WorkStepsInfo = "吹呀吹呀" };

            workSteps = new List<WorkStepModel>();
            workStep = new WorkStepModel();
        }



        private string projectName;

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

        private WorkStepModel workStep;

        public WorkStepModel WorkStep
        {
            get
            {
                return workStep;
            }
            set
            {
                workStep = value;
                OnPropertyChanged(nameof(WorkStep));
            }
        }

        private List<WorkStepModel> workSteps;

        public List<WorkStepModel> WorkSteps
        {
            get
            {
                return workSteps;
            }
            set
            {
                workSteps = value;
                OnPropertyChanged(nameof(WorkSteps));
            }
        }

    }

    public class WorkStepModel : ViewModelBase
    {
        private int lineNum;

        public int LineNum
        {
            get
            {
                return lineNum;
            }
            set
            {
                lineNum = value;
                OnPropertyChanged(nameof(LineNum));
            }
        }

        private string workStepsName;

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

        private string workStepsInfo;

        public string WorkStepsInfo
        {
            get
            {
                return workStepsInfo;
            }
            set
            {
                workStepsInfo = value;
                OnPropertyChanged(nameof(WorkStepsInfo));
            }
        }

        public string WorkStepsShowTxt
        {
            get
            {
                return lineNum + " - " + workStepsName;
            }
        }

    }

}
