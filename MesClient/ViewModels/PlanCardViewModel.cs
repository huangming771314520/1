using MesClient.API;
using MesClient.Commands;
using MesClient.Common;
using MesClient.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MesClient.ViewModels
{
    public class PlanCardViewModel : ViewModelBase
    {
        public PlanCardViewModel()
        {
            var result = ApiManager.GetWorkingTicketData();
            if (!result.Result)
            {
                MessageBox.Show(result.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            foreach (var item in StoreInfo.WorkingTickets)
            {
                PlanCardList.Add(new PlanCardModel()
                {
                    ContractCode = item.ContractCode.Trim(),
                    ProjectName = item.ProjectName.Trim(),
                    ProcessName = $"工序名称：{item.ProcessName.Trim()}  【数量：{item.Quantity}】",
                    WorkStepsName = $"工步名称：{(item.WorkStepsName ?? "").Trim()}",
                    PlanDate = item.ApproveTime == null ? string.Empty : Convert.ToDateTime(item.ApproveTime).ToString("yyyy-MM-dd HH:mm:ss"),
                    PartNameAndFigure = $"{item.PartName.Trim()}【{item.FigureNo.Trim()}】",
                    ApproveRemark = $"备注：{(item.ApproveRemark ?? "").Trim()}",
                    TicketLevelName = $"优先级：{GetLevelName(item.TicketLevel)}",
                    Background = GetBackGroundColor(item.TicketStatus)
                });
            }

            if (PlanCardList.Count > 0)
            {
                SelectIndex = 0;
            }

            UserInfo = string.Format(@"姓名：{0}  工号：{1}", StoreInfo.UserCode, StoreInfo.UserName);

            SelectCommand = new DelegateCommand(obj => Select(obj));
            ShutdownCommand = new DelegateCommand(obj => Shutdown(obj));
        }

        //***********************************************************************************************************//

        private string userInfo;

        public string UserInfo
        {
            get
            {
                return userInfo;
            }
            set
            {
                userInfo = value;
                OnPropertyChanged(nameof(UserInfo));
            }
        }


        private ObservableCollection<PlanCardModel> planCardList = new ObservableCollection<PlanCardModel>();

        public ObservableCollection<PlanCardModel> PlanCardList
        {
            get
            {
                return planCardList;
            }
            set
            {
                planCardList = value;
                OnPropertyChanged(nameof(PlanCardList));
            }
        }


        private int selectIndex = -1;

        public int SelectIndex
        {
            get
            {
                return selectIndex;
            }
            set
            {
                selectIndex = value;
                OnPropertyChanged(nameof(SelectIndex));
            }
        }

        //***********************************************************************************************************//

        public DelegateCommand SelectCommand { get; set; }

        public DelegateCommand ShutdownCommand { get; set; }

        public void Select(object obj)
        {
            if (SelectIndex.Equals(-1))
            {
                return;
            }

            StoreInfo.CurrentWorkingTicket = StoreInfo.WorkingTickets[SelectIndex];

            //下料
            if (StoreInfo.UserExtraInfo.DepartID.Equals("0218"/*"0209"*/))
            {
                WindowHelper.WorkShopType = EnumManager.WorkShopTypeEnum.BlankingShop;

                //加载物料
                var resultA = ApiManager.GetBlankingMaterialInfo();
                if (!resultA.Result)
                {
                    MessageBox.Show(resultA.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            //生产
            else
            {
                WindowHelper.WorkShopType = EnumManager.WorkShopTypeEnum.ProduceShop;

                //加载物料
                var resultA = ApiManager.GetMaterialInfo();
                if (!resultA.Result)
                {
                    MessageBox.Show(resultA.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            //下载图纸文件
            var resultB = ApiManager.DownLoadPlanNeedFile();
            if (!resultB.Result)
            {
                MessageBox.Show(resultB.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            WindowHelper.ShowPageOperatorRecord();
        }

        public void Shutdown(object obj)
        {
            if (MessageBox.Show("是否关机", "警告", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                ProcessStartInfo ps = new ProcessStartInfo();
                ps.FileName = "shutdown.exe";
                ps.Arguments = "-s -t 0";
                Process.Start(ps);
            }
        }

        //***********************************************************************************************************//

        /// <summary>
        /// 根据状态获取背景颜色
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private string GetBackGroundColor(int? state)
        {
            string color = string.Empty;
            switch (state)
            {
                case 1: color = ConfigInfo.ColorSetting.Instruct.ToString(); break;
                case 2: color = ConfigInfo.ColorSetting.Notice.ToString(); break;
                case 3: color = ConfigInfo.ColorSetting.Safety.ToString(); break;
                case 4: color = ConfigInfo.ColorSetting.Ban.ToString(); break;
                default: color = "#FF7BA4F4"; break;
            }
            return color;
        }

        /// <summary>
        /// 根据等级获取紧急程度
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private string GetLevelName(int? state)
        {
            string levelName = string.Empty;
            switch (state)
            {
                case 1: levelName = "一般"; break;
                case 2: levelName = "重要"; break;
                case 3: levelName = "必须完成"; break;
                default: levelName = "未知"; break;
            }
            return levelName;
        }

    }
}
