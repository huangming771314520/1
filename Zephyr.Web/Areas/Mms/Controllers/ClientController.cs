using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Models;

namespace Zephyr.Areas.Mms.Controllers
{
    public class ClientController
    {

    }

    public class ClientApiController : ApiController
    {

        #region 

        /// <summary>
        /// 获取客户端与服务器连接状态
        /// </summary>
        /// <returns></returns>
        public dynamic GetTestConnectionStatus()
        {
            return new ResultModel()
            {
                Result = true,
                Msg = @"连接成功!"
            };
        }

        /// <summary>
        /// 获取客户端当前登录状态是否失效
        /// </summary>
        /// <param name="uCode"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public dynamic GetUserLoginStatus(string uCode, string token)
        {
            //延时 30 秒 返回结果
            ClientService service = new ClientService();
            for (int i = 0; i < 30; i++)
            {
                if (service.GetTokenByUCode(uCode).Equals(token))
                {
                    Thread.Sleep(1000 * 1);
                }
                else
                {
                    return new
                    {
                        Result = false,
                        Msg = @"当前账户已在其他客户端登录！"
                    };
                }
            }
            return new
            {
                Result = true
            };
        }

        #endregion

        #region 用户信息

        /// <summary>
        /// 登录，并查用户详细信息
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public dynamic GetUserDetailInfoByBarCode(string barCode)
        {
            return new ClientService().GetUserDetailInfoByBarCode(barCode);
        }

        /// <summary>
        /// 根据条码查用户信息
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public dynamic GetUserInfoByBarCode(string barCode)
        {
            return new ClientService().GetUserInfoByBarCode(barCode);
        }

        #endregion

        /// <summary>
        /// 根据班组编号查生产计划
        /// </summary>
        /// <param name="teamCode"></param>
        /// <returns></returns>
        public dynamic GetWorkingTicketDataByTCode(string teamCode)
        {
            return new ClientService().GetWorkingTicketDataByTCode(teamCode);
        }

        /// <summary>
        /// 根据工票ID查物料
        /// </summary>
        /// <param name="wTicketID"></param>
        /// <returns></returns>
        public dynamic GetMaterialInfoByWTicketID(int wTicketID)
        {
            return new ClientService().GetMaterialInfoByWTicketID(wTicketID);
        }

        /// <summary>
        /// 根据工票ID查下料物料
        /// </summary>
        /// <param name="wTicketID"></param>
        /// <returns></returns>
        public ResultModel GetBlankingMaterialInfoByWTicketID(int wTicketID)
        {
            return new ClientService().GetBlankingMaterialInfoByWTicketID(wTicketID);
        }

        /// <summary>
        /// 获取生产计划所需图纸文件
        /// </summary>
        /// <param name="pRouteID"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        public dynamic GetPlanNeedFile(int pRouteID, int productID)
        {
            return new ClientService().GetPlanNeedFile(pRouteID, productID);
        }

        /// <summary>
        /// 记录生产计划开始暂停日志
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public dynamic PostProduceLog(dynamic data)
        {
            return new ClientService().SetProduceLog(data);
        }

        /// <summary>
        /// 录入生产实际开始/结束时间
        /// </summary>
        /// <returns></returns>
        public dynamic GetActualProducePlanDate(int planID, int state)
        {
            return new ClientService().SetActualProducePlanDate(planID, state);
        }

        /// <summary>
        /// 记录计划执行操作人
        /// </summary>
        /// <returns></returns>
        public dynamic PostProduceOperator(dynamic data)
        {
            return new ClientService().SetProduceOperator(data);
        }

        /// <summary>
        /// 物料扫码出库
        /// </summary>
        /// <returns></returns>
        public dynamic PostMaterialOutput(dynamic data)
        {
            return new ClientService().MaterialOutput(data);
        }

        /// <summary>
        /// 记录扫码日志
        /// </summary>
        /// <returns></returns>
        public dynamic PostSaveBarCodeScanLog(dynamic data)
        {
            return new ClientService().SaveBarCodeScanLog(data);
        }

        /// <summary>
        /// 根据条码 查PartCode、FigureNo
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public dynamic GetPartCodeAndFigureNoByBarCode(string barCode)
        {
            return new ClientService().GetPartCodeAndFigureNoByBarCode(barCode);
        }

        /// <summary>
        /// 判断某生产计划是否是最后一工序
        /// </summary>
        /// <param name="planID"></param>
        /// <returns></returns>
        public dynamic GetIsLastProcess(int planID)
        {
            return new ClientService().GetIsLastProcess(planID);
        }

        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="partID"></param>
        /// <returns></returns>
        public dynamic GetUpdatePartBarCode(string partID)
        {
            return new ClientService().GenerateBarCode(partID);
        }


    }
}