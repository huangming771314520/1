using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Newtonsoft.Json;

namespace BLL.ExtraBLL
{
    public class EquipmentInfoBLL
    {
        /// <summary>
        /// 获取某计划中使用设备的所有保养计划
        /// </summary>
        /// <returns></returns>
        public EquipmentInfoModel GetEquipmentInfo()
        {
            string url = string.Format(@"http://{0}/api/Mms/WinFormClient/GetEquipmentMaintainByTCode?teamCode={1}", ConfigInfoModel.API, StoreInfoModel.WorkGroupInfo.GetWorkGroup.TeamCode);
            var result = Helpers.HttpHelper.GetTByUrl<EquipmentInfoModel>(url);
            StoreInfoModel.EquipmentInfos = result.Data;
            return result;
        }

        /// <summary>
        /// 设备保养计划
        /// </summary>
        /// <param name="type">0:开始保养 1:结束保养</param>
        /// <returns></returns>
        public ResultModel EQPMaintenancePlan(int type)
        {
            if (new int[] { 0, 1 }.Contains(type))
            {
                string url = string.Format(@"http://{0}/api/Mms/WinFormClient/PostEQPMaintenancePlan", ConfigInfoModel.API);
                object parameter = new
                {
                    EQPMaintenancePlanData = new
                    {
                        ID = StoreInfoModel.EquipmentInfo.GetEquipmentMaintenancePlan.ID,
                        MaintenanceMan = StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserName
                    },
                    EQPData = new
                    {
                        ID = StoreInfoModel.EquipmentInfo.GetEquipment.ID
                    },
                    Type = type
                };
                return Helpers.HttpHelper.PostTByUrl<ResultModel>(url, parameter);
            }
            else
            {
                return new ResultModel()
                {
                    Result = false,
                    Msg = @"type参数错误！"
                };
            }
        }


    }
}
