using BLL.ExtraBLL;
using MesWpfClient.API;
using MesWpfClient.Commands;
using MesWpfClient.Common;
using MesWpfClient.Helpers;
using MesWpfClient.Models;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace MesWpfClient.ViewModels
{
    public class OperatorStatisticalViewModel : ViewModelBase
    {
        public OperatorStatisticalViewModel()
        {
            BarCodeList = new List<string>();

            BarCodeScanCommand = new DelegateCommand(obj => BarCodeScan(obj));
            NextCommand = new DelegateCommand(obj => Next(obj));
        }

        //***************************************************************************************************//

        private string barCode;

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

        private int selectOperatorsIndex = -1;

        public int SelectOperatorsIndex
        {
            get
            {
                return selectOperatorsIndex;
            }
            set
            {
                selectOperatorsIndex = value;
                OnPropertyChanged(nameof(SelectOperatorsIndex));
            }
        }

        //***************************************************************************************************//

        private ObservableCollection<OperatorModel> operators = new ObservableCollection<OperatorModel>();

        public ObservableCollection<OperatorModel> Operators
        {
            get
            {
                return operators;
            }
            set
            {
                operators = value;
                OnPropertyChanged(nameof(Operators));
            }
        }

        public List<string> BarCodeList { get; set; }

        //***************************************************************************************************//

        public DelegateCommand BarCodeScanCommand { get; set; }

        public DelegateCommand NextCommand { get; set; }


        public void BarCodeScan(object obj)
        {
            string barCode = obj.ToString();
            BarCode = "";

            if (BarCodeList.Contains(barCode))
            {
                BarCodeList.Remove(barCode);
                Operators.Remove(Operators.Single(item => item.BarCode.Equals(barCode)));
                for (int i = 0; i < Operators.Count; i++)
                {
                    Operators[i].LineNum = i + 1;
                }
                return;
            }

            var userResult = new BLL.ExtraBLL.WorkGroupInfoBLL().GetUserInfoByBarCode(barCode);
            if (userResult.Result)
            {
                Operators.Add(new OperatorModel()
                {
                    LineNum = Operators.Count + 1,
                    UserName = userResult.Data.UserName,
                    BarCode = userResult.Data.UserBarcode
                });
                BarCodeList.Add(barCode);
            }
            else
            {
                MessageBox.Show(userResult.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        public void Next(object obj)
        {
            if (BarCodeList.Count <= 0)
            {
                MessageBox.Show("请扫码！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //记录开始生产日志
            ResultModel logResult = new ProducePlanInfoBLL().ProduceLog(ProducePlanInfoBLL.ProduceLogTypeEnum.Start);
            LogHelper.WriteLog($"记录生产日志：{logResult.Result}");

            ResultModel operatorResult = new WorkGroupInfoBLL().SetProduceOperatorStatistical(BarCodeList, Convert.ToInt32(logResult.Data));
            LogHelper.WriteLog($"记录计划执行员工：{string.Join(",", BarCodeList)}");

            //录入计划实际开始时间
            ResultModel actualDateResult = new ProducePlanInfoBLL().ActualProducePlanDate(ProducePlanInfoBLL.ProducePlanStateEnum.Start);
            LogHelper.WriteLog($"记录计划实际开始时间：{actualDateResult.Result}");

            WindowHelper.ShowPageProduceProce();
        }


    }
}
