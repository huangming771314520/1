using BLL.Helpers;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ExtraBLL
{
    public class ProduceProceInfoBLL
    {
        public enum ProduceProceStateEnum
        {
            /// <summary>
            /// 完工
            /// </summary>
            CompleteTask = 0,
            /// <summary>
            /// 暂停
            /// </summary>
            PauseTask = 1,
            /// <summary>
            /// 取消
            /// </summary>
            Cancel = 2
        }

        public enum PauseResonEnum
        {
            /// <summary>
            /// 其他
            /// </summary>
            Other = 0,
            /// <summary>
            /// 休息或下班
            /// </summary>
            RestOrAfterWork = 1,
            /// <summary>
            /// 物料问题暂停
            /// </summary>
            MaterialProblem = 2,
            /// <summary>
            /// 质量问题暂停
            /// </summary>
            QualityProblem = 3
        }

        /// <summary>
        /// 生产状态
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="num"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        //public static ResultModel ProduceProceResult(int num = 0, ProduceProceStateEnum state = ProduceProceStateEnum.Cancel)
        //{
        //    try
        //    {
        //        switch (state)
        //        {
        //            case ProduceProceStateEnum.CompleteTask:
        //                //记录生产转序入库数量
        //                StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.Quantity = num;

        //                //录入计划实际结束时间
        //                ResultModel actualDateResult = new ProducePlanInfoBLL().ActualProducePlanDate(ProducePlanInfoBLL.ProducePlanStateEnum.End);

        //                //记录生产日志
        //                ResultModel logResult = new ProducePlanInfoBLL().ProduceLog(ProducePlanInfoBLL.ProduceLogTypeEnum.End);

        //                break;
        //            case ProduceProceStateEnum.PauseTask:
        //                new ProducePlanInfoBLL().ProduceLog(ProducePlanInfoBLL.ProduceLogTypeEnum.Pause);
        //                break;
        //            case ProduceProceStateEnum.Cancel:

        //                break;
        //            default: break;
        //        }

        //        return new ResultModel()
        //        {
        //            Result = true,
        //            Msg = @"成功！"
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResultModel()
        //        {
        //            Result = false,
        //            Msg = ex.Message
        //        };
        //    }
        //}

        public static ResultModel ProduceProceResult(int pauseReson = 0, int num = 0, ProduceProceStateEnum state = ProduceProceStateEnum.Cancel)
        {
            try
            {
                switch (state)
                {
                    case ProduceProceStateEnum.CompleteTask:
                        //记录生产转序入库数量
                        StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.WorkQuantity = num;

                        //录入计划实际结束时间
                        ResultModel actualDateResult = new ProducePlanInfoBLL().ActualProducePlanDate(ProducePlanInfoBLL.ProducePlanStateEnum.End);

                        //记录生产日志
                        ResultModel logResult = new ProducePlanInfoBLL().ProduceLog(ProducePlanInfoBLL.ProduceLogTypeEnum.End);

                        break;
                    case ProduceProceStateEnum.PauseTask:
                        new ProducePlanInfoBLL().ProduceLog(ProducePlanInfoBLL.ProduceLogTypeEnum.Pause, (PauseResonEnum)pauseReson);
                        break;
                    case ProduceProceStateEnum.Cancel:

                        break;
                    default: break;
                }

                return new ResultModel()
                {
                    Result = true,
                    Msg = @"成功！"
                };
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
        /// 判断当前生产计划是否是最后一道工序
        /// </summary>
        /// <returns></returns>
        public static ResultModel IsLastProcess()
        {
            try
            {
                string url = $"http://{ConfigInfoModel.API}/api/Mms/WinFormClient/GetIsLastProcess?ppdID={StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.ID}";

                return new ResultModel()
                {
                    Result = true,
                    Data = HttpHelper.GetTByUrl<ResultModel>(url).Data
                };
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
        /// 生成条码
        /// </summary>
        /// <param name="inventoryCode"></param>
        /// <returns></returns>
        public static ResultModel GenerateBarCode(string inventoryCode)
        {
            try
            {
                string url = $"http://{ConfigInfoModel.API}/api/Mms/WinFormClient/GetUpdatePartBarCodeByICode?inventoryCode={StoreInfoModel.MaterialInfo.GetProcessBom.InventoryCode}";

                return HttpHelper.GetTByUrl<ResultModel>(url);
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

    }
}
