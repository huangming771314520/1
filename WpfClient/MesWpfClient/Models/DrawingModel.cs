using MesWpfClient.Commands;

namespace MesWpfClient.Models
{
    public class DrawingModel : ViewModelBase
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

        private string drawName;

        /// <summary>
        /// 图纸名称
        /// </summary>
        public string DrawName
        {
            get
            {
                return drawName;
            }
            set
            {
                drawName = value;
                OnPropertyChanged(nameof(DrawName));
            }
        }

        private string drawPath;

        /// <summary>
        /// 图纸路径
        /// </summary>
        public string DrawPath
        {
            get
            {
                return drawPath;
            }
            set
            {
                drawPath = value;
                OnPropertyChanged(nameof(DrawPath));
            }
        }

    }
}
