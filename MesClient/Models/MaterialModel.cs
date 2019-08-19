using MesClient.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesClient.Models
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

        private string barCode;

        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode
        {
            get
            {
                return barCode;
            }
            set
            {
                barCode = value;
                OnPropertyChanged(nameof(BarCode));
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

        private int totalQuantity;

        /// <summary>
        /// 零件数量
        /// </summary>
        public int TotalQuantity
        {
            get
            {
                return totalQuantity;
            }
            set
            {
                totalQuantity = value;
                OnPropertyChanged(nameof(TotalQuantity));
            }
        }

        private int alreadyScanQuantity;

        /// <summary>
        /// 零件实际扫码数量
        /// </summary>
        public int AlreadyScanQuantity
        {
            get
            {
                return alreadyScanQuantity;
            }
            set
            {
                alreadyScanQuantity = value;
                Proportion = $"{alreadyScanQuantity}/{totalQuantity}";
                OnPropertyChanged(nameof(AlreadyScanQuantity));
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
