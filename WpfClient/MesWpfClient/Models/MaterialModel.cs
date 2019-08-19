using MesWpfClient.Commands;

namespace MesWpfClient.Models
{
    public class MaterialModel : ViewModelBase
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

        private string partCode;

        /// <summary>
        /// 零件编码
        /// </summary>
        public string PartCode
        {
            get
            {
                return partCode;
            }
            set
            {
                partCode = value;
                OnPropertyChanged(nameof(PartCode));
            }
        }

        private string partName;

        /// <summary>
        /// 零件名称
        /// </summary>
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

        private string partFigureCode;

        /// <summary>
        /// 零件图号
        /// </summary>
        public string PartFigureCode
        {
            get
            {
                return partFigureCode;
            }
            set
            {
                partFigureCode = value;
                OnPropertyChanged(nameof(PartFigureCode));
            }
        }


        private string parentCode;

        /// <summary>
        /// 父级零件编码
        /// </summary>
        public string ParentCode
        {
            get
            {
                return parentCode;
            }
            set
            {
                parentCode = value;
                OnPropertyChanged(nameof(ParentCode));
            }
        }

        private int partQuantity;

        /// <summary>
        /// 零件数量
        /// </summary>
        public int PartQuantity
        {
            get
            {
                return partQuantity;
            }
            set
            {
                partQuantity = value;
                OnPropertyChanged(nameof(PartQuantity));
            }
        }

        private int partAlreadyScanQuantity;

        /// <summary>
        /// 零件实际扫码数量
        /// </summary>
        public int PartAlreadyScanQuantity
        {
            get
            {
                return partAlreadyScanQuantity;
            }
            set
            {
                partAlreadyScanQuantity = value;
                Proportion = $"{partAlreadyScanQuantity}/{partQuantity}";
                OnPropertyChanged(nameof(PartAlreadyScanQuantity));
            }
        }

        private string materialCode;

        /// <summary>
        /// 材料编码
        /// </summary>
        public string MaterialCode
        {
            get
            {
                return materialCode;
            }
            set
            {
                materialCode = value;
                OnPropertyChanged(nameof(MaterialCode));
            }
        }

        private string proportion = "0/0";

        /// <summary>
        /// 零件 已扫描数量 占比
        /// </summary>
        public string Proportion
        {
            get
            {
                return proportion;
            }
            set
            {
                proportion = value;
                OnPropertyChanged(nameof(Proportion));
            }
        }
    }
}
