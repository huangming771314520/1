using MesWpfClient.Commands;
using MesWpfClient.Views.WinViews;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesWpfClient.ViewModels
{
    public class ProceStateViewModel : ViewModelBase
    {
        private WindowProceState window;

        public ProceStateViewModel(WindowProceState _window)
        {
            window = _window;
            num = StoreInfoModel.MaterialInfo.GetMaterialDetail.PartQuantity;

            CompleteCommand = new DelegateCommand(obj => Complete(obj));
            StopCommand = new DelegateCommand(obj => Stop(obj));
            CancelCommand = new DelegateCommand(obj => Cancel(obj));
            UpNumCommand = new DelegateCommand(obj => UpNum(obj));
            DownNumCommand = new DelegateCommand(obj => DownNum(obj));
            RdbChangeCommand = new DelegateCommand(obj => RdbChange(obj));

            window.btnCancel.Focus();
        }

        //******************************************************************************************************************//

        private int num;
        public int Num
        {
            get
            {
                return num;
            }
            set
            {
                num = value;
                OnPropertyChanged(nameof(Num));
            }
        }

        private bool rdbA = true;
        private bool rdbB = false;
        private bool rdbC = false;

        public bool RdbA
        {
            get
            {
                return rdbA;
            }
            set
            {
                rdbA = value;
                OnPropertyChanged(nameof(RdbA));
            }
        }
        public bool RdbB
        {
            get
            {
                return rdbB;
            }
            set
            {
                rdbB = value;
                OnPropertyChanged(nameof(RdbB));
            }
        }
        public bool RdbC
        {
            get
            {
                return rdbC;
            }
            set
            {
                rdbC = value;
                OnPropertyChanged(nameof(RdbC));
            }
        }

        //******************************************************************************************************************//

        public DelegateCommand CompleteCommand { get; set; }

        public DelegateCommand StopCommand { get; set; }

        public DelegateCommand CancelCommand { get; set; }

        public DelegateCommand UpNumCommand { get; set; }

        public DelegateCommand DownNumCommand { get; set; }

        public DelegateCommand RdbChangeCommand { get; set; }

        public void Complete(object obj)
        {
            window.SelectState = 0;
            window.SelectNum = Num;

            window.Close();
        }

        public void Stop(object obj)
        {
            window.SelectState = 1;

            int pauseReson;
            if (RdbA)
            {
                pauseReson = 1;
            }
            else if (RdbB)
            {
                pauseReson = 2;
            }
            else if (RdbC)
            {
                pauseReson = 3;
            }
            else
            {
                pauseReson = 0;
            }

            window.PauseReson = pauseReson;

            window.Close();
        }

        public void Cancel(object obj)
        {
            window.SelectState = 2;

            window.Close();
        }

        public void UpNum(object obj)
        {
            Num++;
        }

        public void DownNum(object obj)
        {
            if (Num > 0)
            {
                Num--;
            }
        }

        public void RdbChange(object obj)
        {
            switch (obj.ToString())
            {
                case "Up":
                    if (RdbA)
                    {
                        RdbA = false;
                        RdbB = false;
                        RdbC = true;
                    }
                    else if (RdbB)
                    {
                        RdbA = true;
                        RdbB = false;
                        RdbC = false;
                    }
                    else
                    {
                        RdbA = false;
                        RdbB = true;
                        RdbC = false;
                    }
                    break;
                case "Down":
                    if (RdbA)
                    {
                        RdbA = false;
                        RdbB = true;
                        RdbC = false;
                    }
                    else if (RdbB)
                    {
                        RdbA = false;
                        RdbB = false;
                        RdbC = true;
                    }
                    else
                    {
                        RdbA = true;
                        RdbB = false;
                        RdbC = false;
                    }
                    break;
                default: break;
            }
        }

    }
}
