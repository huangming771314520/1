using MesWpfClient.Commands;

namespace MesWpfClient.Models
{
    public class WorkStepsModel : ViewModelBase
    {
        private int lineNum;

        /// <summary>
        /// 编号
        /// </summary>
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

        private string workStepsInfo;

        /// <summary>
        /// 工步描述
        /// </summary>
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
                return LineNum + " - " + WorkStepsName;
            }
        }
    }
}
