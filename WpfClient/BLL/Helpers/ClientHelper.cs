using Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace BLL.Helpers
{
    public class ClientHelper
    {
        /// <summary>
        /// 获取客户端与服务器连接状态
        /// </summary>
        /// <returns></returns>
        public static ResultModel GetConnectionStatus()
        {
            try
            {
                string url = string.Format(@"http://{0}/api/Mms/WinFormClient/GetTestConnectionStatus", ConfigInfoModel.API);
                return HttpHelper.GetTByUrl<ResultModel>(url, 1000 * 5);
            }
            catch (Exception ex)
            {
                return new ResultModel()
                {
                    Result = false,
                    Msg = ex.Message
                };
            }
        }

        /// <summary>
        /// 获取登录状态
        /// </summary>
        /// <returns></returns>
        public static ResultModel GetUsetLoginStatus()
        {
            try
            {
                string url = string.Format(@"http://{0}/api/Mms/WinFormClient/GetUserLoginStatus?uCode={1}&token={2}", ConfigInfoModel.API, StoreInfoModel.UserCode, StoreInfoModel.Token);
                return HttpHelper.GetTByUrl<ResultModel>(url, 1000 * 60);
            }
            catch (Exception ex)
            {
                return new ResultModel()
                {
                    Result = false,
                    Msg = string.Format(@"错误！\n详情：{0}", ex.Message)
                };
            }
        }

        /// <summary>
        /// 记录客户端操作日志
        /// </summary>
        /// <param name="content">操作内容</param>
        /// <param name="equipmentID">操作设备ID</param>
        /// <returns></returns>
        public static ResultModel RecordOperateLog(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return new ResultModel()
                {
                    Result = false,
                    Msg = @"你让我些什么呢？"
                };
            }

            var data = new List<Model.Models.ClientOperateLog>() {
                new Model.Models.ClientOperateLog()
                {
                    AddressIP=StoreInfoModel.LocalAddressIP,
                    Content = content,
                    CreatePersonCode = StoreInfoModel.UserCode,
                    CreatePersonName = StoreInfoModel.UserName,
                    CreateDate = DateTime.Now
                }
            };

            string url = string.Format(@"http://" + ConfigInfoModel.API + "/api/Mms/WinFormClient/PostSetOperateLog");
            return HttpHelper.PostTByUrl<ResultModel>(url, data);
        }

        /// <summary>
        /// 记录客户端操作日志
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="equipmentID"></param>
        /// <returns></returns>
        public static ResultModel RecordOperateLog(List<string> contents)
        {
            if (contents.Count <= 0)
            {
                return new ResultModel()
                {
                    Result = false,
                    Msg = @"你让我些什么呢？"
                };
            }

            List<Model.Models.ClientOperateLog> models = new List<Model.Models.ClientOperateLog>();
            contents.ForEach(item =>
            {
                models.Add(new Model.Models.ClientOperateLog()
                {
                    AddressIP = StoreInfoModel.LocalAddressIP,
                    Content = item,
                    CreatePersonCode = StoreInfoModel.UserCode,
                    CreatePersonName = StoreInfoModel.UserName,
                    CreateDate = DateTime.Now
                });
            });

            string url = string.Format(@"http://" + ConfigInfoModel.API + "/api/Mms/WinFormClient/PostSetOperateLog");
            return HttpHelper.PostTByUrl<ResultModel>(url, models);
        }

        /// <summary>
        /// 记录客户端操作日志
        /// </summary>
        /// <param name="models"></param>
        /// <param name="equipmentID"></param>
        /// <returns></returns>
        public static ResultModel RecordOperateLog(List<Model.Models.ClientOperateLog> models)
        {
            if (models.Count <= 0)
            {
                return new ResultModel()
                {
                    Result = false,
                    Msg = @"你让我些什么呢？"
                };
            }

            models.ForEach(item =>
            {
                if (item.AddressIP == null)
                {
                    item.AddressIP = StoreInfoModel.LocalAddressIP;
                }
                if (item.CreatePersonCode == null)
                {
                    item.CreatePersonCode = StoreInfoModel.UserCode;
                }
                if (item.CreatePersonName == null)
                {
                    item.CreatePersonName = StoreInfoModel.UserName;
                }
                if (item.CreateDate.Equals(DateTime.MinValue))
                {
                    item.CreateDate = DateTime.Now;
                }
            });

            string url = string.Format(@"http://" + ConfigInfoModel.API + "/api/Mms/WinFormClient/PostSetOperateLog");
            return HttpHelper.PostTByUrl<ResultModel>(url, models);
        }

    }
}
