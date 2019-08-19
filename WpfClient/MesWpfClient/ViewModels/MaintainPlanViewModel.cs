using MesWpfClient.API;
using MesWpfClient.Commands;
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
using System.Windows.Media.Media3D;

namespace MesWpfClient.ViewModels
{
    public class MaintainPlanViewModel : ViewModelBase
    {
        public MaintainPlanViewModel()
        {
            Task.Run(() =>
            {
                MaintainPlanList = GetMaintainPlanData();
                MaintainPlanList.Insert(0, new MaintainPlanModel()
                {
                    EquipmentName = @"",
                    MaintainInfo = @"            返 回",
                    PlanedContent = @" ******************* ",
                    PlanDate = @""
                });

            });

            SelectCommand = new DelegateCommand(obj => Select(obj));
        }


        public string TitleInfo
        {
            get
            {
                return @"设备保养计划";
            }
        }

        private ObservableCollection<MaintainPlanModel> maintainPlanList = new ObservableCollection<MaintainPlanModel>();

        public ObservableCollection<MaintainPlanModel> MaintainPlanList
        {
            get
            {
                return maintainPlanList;
            }
            set
            {
                maintainPlanList = value;
                OnPropertyChanged(nameof(MaintainPlanList));
            }
        }

        private int maintainPlanIndex = -1;

        public int MaintainPlanIndex
        {
            get
            {
                return maintainPlanIndex;
            }
            set
            {
                maintainPlanIndex = value;
                OnPropertyChanged(nameof(MaintainPlanIndex));
            }
        }

        public DelegateCommand SelectCommand { get; set; }

        public void Select(object obj)
        {
            if (MaintainPlanIndex.Equals(-1))
            {
                return;
            }

            if (MaintainPlanIndex.Equals(0))
            {
                WindowHelper.ShowPageProducePlan();
            }
            else
            {
                MaintainPlanList.RemoveAt(MaintainPlanIndex);

                /*StoreInfoModel.EquipmentInfo = StoreInfoModel.EquipmentInfos[MaintainPlanIndex - 1];

                //是否正在保养，是将结束
                if (StoreInfoModel.EquipmentInfo.GetEquipmentMaintenancePlan.ActualStartTime != null)
                {
                    ResultModel result = new BLL.ExtraBLL.EquipmentInfoBLL().EQPMaintenancePlan(1);
                    //结束保养成功
                    if (result.Result)
                    {
                        StoreInfoModel.EquipmentInfos.RemoveAt(MaintainPlanIndex - 1);
                    }
                    else
                    {
                        MessageBox.Show(result.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                //是否正在保养，否将开始
                else
                {
                    ResultModel result = new BLL.ExtraBLL.EquipmentInfoBLL().EQPMaintenancePlan(0);
                    //开始保养成功
                    if (result.Result)
                    {

                    }
                    else
                    {
                        MessageBox.Show(result.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }*/
            }

        }


        public ObservableCollection<MaintainPlanModel> GetMaintainPlanData()
        {
            ObservableCollection<MaintainPlanModel> result = new ObservableCollection<MaintainPlanModel>();

            //foreach (var item in StoreInfoModel.EquipmentInfos)
            //{
            //    result.Add(new MaintainPlanModel()
            //    {
            //        EquipmentName = item.GetEquipment.EquipmentName,
            //        MaintainInfo = item.GetEquipmentMaintenancePlan.MaintenanceName + "(" + (item.GetEquipmentMaintenancePlan.ActualStartTime == null ? "未开始" : "已开始") + ")",
            //        PlanedContent = item.GetEquipmentMaintenancePlan.PlanedContent,
            //        PlanDate = item.GetEquipmentMaintenancePlan.PlanedStartTime == null ? "" : Convert.ToDateTime(item.GetEquipmentMaintenancePlan.PlanedStartTime).ToString("yyyy-MM-dd"),
            //    });
            //}

            for (int i = 0; i < 50; i++)
            {
                result.Add(new MaintainPlanModel()
                {
                    EquipmentName = "设备" + (i + 1),
                    MaintainInfo = "大猪蹄子",
                    PlanedContent = "测试内容",
                    PlanDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                });
            }

            return result;
        }

    }
}
