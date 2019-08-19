using MesWpfClient.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesWpfClient.Models
{
    public class OperatorModel : ViewModelBase
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

        private string barCode;

        /// <summary>
        /// 编号
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

        private string userName;

        /// <summary>
        /// 编号
        /// </summary>
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }


    }
}
