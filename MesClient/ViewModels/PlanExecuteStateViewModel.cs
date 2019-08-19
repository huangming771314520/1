using MesClient.Commands;
using MesClient.Views.WinViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesClient.ViewModels
{
    public class PlanExecuteStateViewModel : ViewModelBase
    {
        public PlanExecuteStateViewModel(WindowPlanExecuteState _window)
        {
            num = StoreInfo.MaterialOneSelf == null ? 0 : StoreInfo.MaterialOneSelf.MaterialInfo.TotalQuantity;

            CompleteCommand = new DelegateCommand(new Action<object>(obj =>
            {
                _window.SelectState = 0;
                _window.SelectNum = Num;
                _window.Close();
            }));
            StopCommand = new DelegateCommand(new Action<object>(obj =>
            {
                _window.SelectState = 1;
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
                _window.PauseReson = pauseReson;
                _window.Close();
            }));
            CancelCommand = new DelegateCommand(new Action<object>(obj =>
            {
                _window.SelectState = 2;
                _window.Close();
            }));
            UpNumCommand = new DelegateCommand(new Action<object>(obj => { Num++; }));
            DownNumCommand = new DelegateCommand(new Action<object>(obj =>
            {
                if (Num > 0)
                {
                    Num--;
                }
            }));
            RdbChangeCommand = new DelegateCommand(obj => RdbChange(obj));

            _window.btnCancel.Focus();
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
