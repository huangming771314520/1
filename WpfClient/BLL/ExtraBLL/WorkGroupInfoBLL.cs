using Model;
using Model.Models;
using System.Collections.Generic;

namespace BLL.ExtraBLL
{
    public class WorkGroupInfoBLL
    {
        /// <summary>
        /// 根据用户编码查详情
        /// </summary>
        /// <param name="userCode"></param>
        public WorkGroupInfoModel GetWorkGroupInfo(string barCode)
        {
            string url = string.Format(@"http://{0}/api/Mms/WinFormClient/GetWorkGroupInfoByUCode?barCode={1}", ConfigInfoModel.API, barCode);
            var result = Helpers.HttpHelper.GetTByUrl<WorkGroupInfoModel>(url);
            if (result.Result)
            {
                StoreInfoModel.UserBarCode = barCode;
                StoreInfoModel.WorkGroupInfo = result.Data;
                StoreInfoModel.Token = result.Data.GetToken;
                StoreInfoModel.UserCode = result.Data.GetWorkGroupDetail.UserCode;
                StoreInfoModel.UserName = result.Data.GetWorkGroupDetail.UserName;
            }
            return result;
        }

        /// <summary>
        /// 根据条码 查用户信息
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public ResultModel<SYS_BN_User> GetUserInfoByBarCode(string barCode)
        {
            string url = string.Format(@"http://{0}/api/Mms/WinFormClient/GetUserInfoByBarCode?barCode={1}", ConfigInfoModel.API, barCode);
            return Helpers.HttpHelper.GetTByUrl<ResultModel<SYS_BN_User>>(url);
        }

        /// <summary>
        /// 记录计划执行操作人
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ResultModel SetProduceOperatorStatistical(List<string> barCodes, int logID)
        {
            string url = string.Format(@"http://{0}/api/Mms/WinFormClient/PostProduceOperatorStatistical", ConfigInfoModel.API);
            return Helpers.HttpHelper.PostTByUrl<ResultModel>(url, new
            {
                LogID = logID,
                BarCodes = string.Join(",", barCodes)
            });
        }

    }
}
