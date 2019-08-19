using BLL.ExtraBLL;
using BLL.Helpers;
using MesWpfClient.API;
using MesWpfClient.Commands;
using MesWpfClient.Common;
using MesWpfClient.Helpers;
using MesWpfClient.Models;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace MesWpfClient.ViewModels
{
    public class ProducePlanViewModel : ViewModelBase
    {
        public ProducePlanViewModel()
        {
            Task.Run(() =>
            {
                ProducePlanInfoModel GetPPlanInfo = new ProducePlanInfoBLL().GetProducePlanInfo();
                if (!GetPPlanInfo.Result)
                {
                    MessageBox.Show(GetPPlanInfo.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Execute.OnUIThread(() =>
                {
                    ProducePlanList = GetProducePlansData();

                    if (ProducePlanList.Count > 0)
                    {
                        ProducePlanIndex = 0;
                    }

                    UserInfo = string.Format(@"姓名：{0}  工号：{1}", StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserName, StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserCode);
                });

            });

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


        private List<ProducePlanModel> producePlanList = new List<ProducePlanModel>();

        public List<ProducePlanModel> ProducePlanList
        {
            get
            {
                return producePlanList;
            }
            set
            {
                producePlanList = value;
                OnPropertyChanged(nameof(ProducePlanList));
            }
        }


        private int producePlanIndex = -1;

        public int ProducePlanIndex
        {
            get
            {
                return producePlanIndex;
            }
            set
            {
                producePlanIndex = value;
                OnPropertyChanged(nameof(ProducePlanIndex));
            }
        }

        //***********************************************************************************************************//

        public DelegateCommand SelectCommand { get; set; }

        public DelegateCommand ShutdownCommand { get; set; }

        public void Select(object obj)
        {
            try
            {
                LogHelper.WriteLog($"成功选择计划，索引为{ProducePlanIndex}");

                if (ProducePlanIndex.Equals(-1))
                {
                    return;
                }

                StoreInfoModel.ProducePlanInfo = StoreInfoModel.ProducePlanInfos[ProducePlanIndex];

                //下料
                if (StoreInfoModel.WorkGroupInfo.GetWorkGroup.DepartID.Equals("0218"/*"0209"*/))
                {
                    LogHelper.WriteLog($"当前计划为下料");

                    WindowHelper.ShowPageBlankingRecord();
                }
                //生产
                else
                {
                    LogHelper.WriteLog($"当前为普通生产计划");

                    //加载该生产计划所需物料详情
                    var getMaterialInfo = new MaterialInfoBLL().GetMaterialDetail();
                    if (!getMaterialInfo.Result)
                    {
                        MessageBox.Show(getMaterialInfo.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    LogHelper.WriteLog($"成功加载物料信息");

                    //下载图纸文件
                    var drawResult = new ProducePlanInfoBLL().DownLoadDrawing();
                    if (!drawResult.Result)
                    {
                        MessageBox.Show(drawResult.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                        //return;
                    }

                    LogHelper.WriteLog($"成功加载图纸信息");

                    WindowHelper.ShowOperatorStatistical();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog($"异常：{ex}");
            }
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

        private List<ProducePlanModel> GetProducePlansData()
        {
            List<ProducePlanModel> result = new List<ProducePlanModel>();

            foreach (var item in StoreInfoModel.ProducePlanInfos)
            {
                string color = string.Empty;
                switch (item.GetProjectProduceDetial.TicketStatus)
                {
                    case 1: color = ConfigInfoModel.ColorSetting.Instruct.ToString(); break;
                    case 2: color = ConfigInfoModel.ColorSetting.Notice.ToString(); break;
                    case 3: color = ConfigInfoModel.ColorSetting.Safety.ToString(); break;
                    case 4: color = ConfigInfoModel.ColorSetting.Ban.ToString(); break;
                    default: color = "#FF7BA4F4"; break;
                }

                string levelName = string.Empty;
                switch (item.GetProjectProduceDetial.TicketLevel)
                {
                    case 1: levelName = "一般"; break;
                    case 2: levelName = "重要"; break;
                    case 3: levelName = "必须完成"; break;
                    default: levelName = "未知"; break;
                }

                result.Add(new ProducePlanModel()
                {
                    ContractCode = item.GetProjectProduceDetial.ContractCode,//合同编号
                    ProjectName = item.GetProject.ProjectName,//工程项目
                    ProcessName = "工序名称：" + item.GetProductProcessRoute.ProcessName.Trim() + "(数量：" + item.GetProjectProduceDetial.WorkQuantity + ")",//工序名称
                    //PlanDate = item.GetProjectProduceDetial.PlanedStartTime == null ? (item.GetProjectProduceDetial.PlanedFinishTime == null ? "--------" :
                    //    Convert.ToDateTime(item.GetProjectProduceDetial.PlanedFinishTime).ToString("---- - MM:dd HH:mm:ss")) : (item.GetProjectProduceDetial.PlanedFinishTime == null ?
                    //    Convert.ToDateTime(item.GetProjectProduceDetial.PlanedStartTime).ToString("MM-dd HH:mm:ss - ----") : (Convert.ToDateTime(item.GetProjectProduceDetial.PlanedStartTime).ToString("MM-dd HH:mm:ss - ") +
                    //    Convert.ToDateTime(item.GetProjectProduceDetial.PlanedFinishTime).ToString("HH:mm:ss"))),
                    WorkStepsName = "工步名称：" + item.GetProjectProduceDetial.WorkStepsName.Trim(),
                    PlanDate = "计划时间：" + item.GetProjectProduceDetial.ApproveTime == null ? string.Empty : Convert.ToDateTime(item.GetProjectProduceDetial.ApproveTime.ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                    PartNameAndFigure = item.GetProcessBOM.PartName + "【" + item.GetProcessBOM.PartFigureCode + "】",
                    ApproveRemark = "备注：" + item.GetProjectProduceDetial.ApproveRemark ?? string.Empty,
                    TicketLevelName = "工票优先级：" + levelName,
                    Background = color
                });
            }

            return result;
        }

    }
}
