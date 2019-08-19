using MesClient.API;
using MesClient.Commands;
using MesClient.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace MesClient.ViewModels
{
    public class OperatorRecordViewModel : ViewModelBase
    {
        public OperatorRecordViewModel()
        {
            BarCodeScanCommand = new DelegateCommand(obj => BarCodeScan(obj));
            NextCommand = new DelegateCommand(obj => Next(obj));
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


        private string barCode = string.Empty;

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

        //***************************************************************************************************//

        public DelegateCommand BarCodeScanCommand { get; set; }

        public DelegateCommand NextCommand { get; set; }


        public void BarCodeScan(object obj)
        {
            string barCode = obj.ToString();
            BarCode = "";

            if (string.IsNullOrEmpty(barCode))
            {
                return;
            }

            OperatorModel operatorModel = Operators.SingleOrDefault(item => item.BarCode.ToLower().Equals(barCode.ToLower()));
            if (operatorModel != null)
            {
                Operators.Remove(operatorModel);
                for (int i = 0; i < Operators.Count; i++)
                {
                    Operators[i].LineNum = i + 1;
                }
                return;
            }

            var result = ApiManager.GetUserInfoByBarCode(barCode);
            if (result.Result)
            {
                Operators.Add(new OperatorModel()
                {
                    LineNum = Operators.Count + 1,
                    UserName = result.Data.UserName,
                    BarCode = result.Data.BarCode
                });
            }
            else
            {
                MessageBox.Show(result.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        public void Next(object obj)
        {
            if (Operators.Count <= 0)
            {
                MessageBox.Show("请扫码！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //记录开始生产日志
            var logResult = ApiManager.SetProduceLog(EnumManager.ProduceLogTypeEnum.Start);

            //记录计划执行员工
            var operatorResult = ApiManager.SetProduceOperator(Operators.Select(item => item.BarCode).ToList(), Convert.ToInt32(logResult.Data));

            //录入计划实际开始时间
            var actualDateResult = ApiManager.SetActualProducePlanDate(EnumManager.ProducePlanStateEnum.Start);

            WindowHelper.ShowPagePlanExecute();
        }

    }

}
