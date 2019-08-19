using MesWpfClient.Commands;

namespace MesWpfClient.Models
{
    public class BlankingRecordLeftModel : ViewModelBase
    {
        private long id;

        public long ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        private string contractCode;

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

        private string figureCode;
        public string FigureCode
        {
            get
            {
                return figureCode;
            }
            set
            {
                figureCode = value;
                OnPropertyChanged(nameof(FigureCode));
            }
        }

        private string partName;
        public string PartName
        {
            get
            {
                return partName;
            }
            set
            {
                partName = value;
                OnPropertyChanged(nameof(PartName));
            }
        }

        private string blankingSize;

        public string BlankingSize
        {
            get
            {
                return blankingSize;
            }
            set
            {
                blankingSize = value;
                OnPropertyChanged(nameof(BlankingSize));
            }
        }

        private string biankingSize;
        public string BiankingSize
        {
            get
            {
                return biankingSize;
            }
            set
            {
                biankingSize = value;
                OnPropertyChanged(nameof(BiankingSize));
            }
        }

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

        public string BackGroundColor
        {
            get
            {
                return LineNum % 2 == 0 ? "#FFFBFBFB" : "#FFFFFFFF";
            }
        }
    }


    public class BlankingRecordRightModel : ViewModelBase
    {
        private long id;

        public long ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        private string biankingSize;
        public string BiankingSize
        {
            get
            {
                return biankingSize;
            }
            set
            {
                biankingSize = value;
                OnPropertyChanged(nameof(BiankingSize));
            }
        }

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

        public string BackGroundColor
        {
            get
            {
                return LineNum % 2 == 0 ? "#FFFBFBFB" : "#FFFFFFFF";
            }
        }
    }
}
