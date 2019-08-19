using MesClient.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesClient.Models
{
    public class MaterialScanModel : ViewModelBase
    {
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

        private int demandScanQuantity;

        /// <summary>
        /// 需求数量
        /// </summary>
        public int DemandScanQuantity
        {
            get
            {
                return demandScanQuantity;
            }
            set
            {
                demandScanQuantity = value;
                OnPropertyChanged(nameof(DemandScanQuantity));
            }
        }

        private int alreadyScanQuantity;

        /// <summary>
        /// 扫码数量
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
                OnPropertyChanged(nameof(AlreadyScanQuantity));
            }
        }

        private string barCode;

        /// <summary>
        /// 条形码
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

        private int isCrux;

        /// <summary>
        /// 是否关键件
        /// </summary>
        public int IsCrux
        {
            get
            {
                return isCrux;
            }
            set
            {
                isCrux = value;
                OnPropertyChanged(nameof(IsCrux));
            }
        }

    }
}
