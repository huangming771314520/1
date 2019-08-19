using MesClient.Entitys;
using MesClient.Helpers;
using O2S.Components.PDFRender4NET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using static MesClient.EnumManager;

namespace MesClient.API
{
    public class ApiManager
    {
        private static readonly string uri = $"http://{ConfigInfo.API}/api/Mms/Client/";

        /// <summary>
        /// 获取客户端与服务器连接状态
        /// </summary>
        /// <returns></returns>
        public static ResultModel GetConnectionStatus()
        {
            try
            {
                string url = uri + "GetTestConnectionStatus";
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
                string url = uri + "GetUserLoginStatus?uCode=" + StoreInfo.UserCode + "&token=" + StoreInfo.Token;
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
        /// 登录，并查用户详细信息
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public static ResultModel GetUserDetailInfoByBarCode(string barCode)
        {
            string url = uri + "GetUserDetailInfoByBarCode?barCode=" + barCode;
            var result = HttpHelper.GetTByUrl<ResultModel<UserDetailInfoEntity>>(url);
            if (result.Result)
            {
                StoreInfo.Token = result.Data.Token;
                StoreInfo.UserCode = result.Data.UserCode;
                StoreInfo.UserName = result.Data.UserName;
                StoreInfo.BarCode = result.Data.BarCode;
                StoreInfo.UserExtraInfo = result.Data.ExtraInfo;
            }
            return new ResultModel()
            {
                Result = result.Result,
                Msg = result.Msg
            };
        }

        /// <summary>
        /// 根据条码查用户信息
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public static ResultModel<UserInfoEntity> GetUserInfoByBarCode(string barCode)
        {
            string url = uri + "GetUserInfoByBarCode?barCode=" + barCode;
            return HttpHelper.GetTByUrl<ResultModel<UserInfoEntity>>(url);
        }

        /// <summary>
        /// 根据班组编号查生产计划
        /// </summary>
        /// <returns></returns>
        public static ResultModel GetWorkingTicketData()
        {
            string url = uri + "GetWorkingTicketDataByTCode?teamCode=" + StoreInfo.UserExtraInfo.TeamCode;
            var result = HttpHelper.GetTByUrl<ResultModel<List<WorkingTicketEntity>>>(url);
            if (result.Result)
            {
                StoreInfo.WorkingTickets = result.Data;
            }
            return new ResultModel()
            {
                Result = result.Result,
                Msg = result.Msg
            };
        }

        /// <summary>
        /// 根据工票ID查物料
        /// </summary>
        /// <returns></returns>
        public static ResultModel GetMaterialInfo()
        {
            string url = uri + "GetMaterialInfoByWTicketID?wTicketID=" + StoreInfo.CurrentWorkingTicket.WorkTicketID;
            var result = HttpHelper.GetTByUrl<ResultModel<MaterialInfoEntity>>(url);
            if (result.Result)
            {
                StoreInfo.MaterialOneSelf = result.Data.MaterialOneSelf;
                StoreInfo.MaterialChild = result.Data.MaterialChild;
            }
            return new ResultModel()
            {
                Result = result.Result,
                Msg = result.Msg
            };
        }

        /// <summary>
        /// 根据工票ID查下料物料
        /// </summary>
        /// <param name="wTicketID"></param>
        /// <returns></returns>
        public static ResultModel GetBlankingMaterialInfo()
        {
            string url = uri + "GetBlankingMaterialInfoByWTicketID?wTicketID=" + StoreInfo.CurrentWorkingTicket.WorkTicketID;
            var result = HttpHelper.GetTByUrl<ResultModel<BlankingMaterialInfoEntity>>(url);
            if (result.Result)
            {
                StoreInfo.BlankingMaterial = result.Data;
            }
            return new ResultModel()
            {
                Result = result.Result,
                Msg = result.Msg
            };
        }

        /// <summary>
        /// 获取生产计划所需图纸文件
        /// </summary>
        /// <returns></returns>
        public static ResultModel DownLoadPlanNeedFile()
        {
            string url = uri + "GetPlanNeedFile?pRouteID=" + StoreInfo.CurrentWorkingTicket.ProcessRouteID + "&productID=" + StoreInfo.CurrentWorkingTicket.ProductID;
            var result = HttpHelper.GetTByUrl<ResultModel<List<FileInfoEntity>>>(url);
            if (result.Result)
            {
                int errNum = 0;
                string[] fileType = new string[] { ".jpg", ".png", ".dwg", ".pdf", };
                StoreInfo.CurrentPlanFiles.Clear();
                foreach (var item in result.Data)
                {
                    if (fileType.Contains(Path.GetExtension(item.FileName).ToLower()))
                    {
                        if (!File.Exists($"{ConfigInfo.DocDirName}\\{item.FileName}"))
                        {
                            if (!FtpHelper.DownLoad(item))
                            {
                                errNum++;
                                continue;
                            }
                        }

                        if (Path.GetExtension(item.FileName).ToLower().Equals(".pdf"))
                        {
                            using (FileStream fs = new FileStream(string.Format(@"{0}\{1}", ConfigInfo.DocDirName, item.FileName), FileMode.Open))
                            {
                                PDFFile pdfFile = PDFFile.Open(fs);
                                for (int i = 0; i < pdfFile.PageCount; i++)
                                {
                                    string filePath = $"{ConfigInfo.DocDirName}\\{Path.GetFileNameWithoutExtension(item.FileName)}_{(i + 1)}.png";
                                    if (!File.Exists(filePath))
                                    {
                                        Bitmap pageImage = pdfFile.GetPageImage(i, 56 * 10);
                                        pageImage.Save(filePath, ImageFormat.Png);
                                    }

                                    StoreInfo.CurrentPlanFiles.Add(new FileInfoEntity()
                                    {
                                        DocName = item.DocName,
                                        FileName = Path.GetFileName(filePath),
                                        FileAddress = filePath,
                                        WebAddressType = WebAddressTypeEnum.Local
                                    });
                                }
                                pdfFile.Dispose();
                            }
                        }
                        else
                        {
                            StoreInfo.CurrentPlanFiles.Add(item);
                        }

                    }
                }
                bool isOK = errNum.Equals(0);
                return new ResultModel()
                {
                    Result = isOK,
                    Msg = isOK ? @"下载成功！" : $"{errNum}个文件下载失败！"
                };
            }
            return new ResultModel()
            {
                Result = result.Result,
                Msg = result.Msg
            };
        }

        /// <summary>
        /// 记录生产日志
        /// </summary>
        /// <param name="lType"></param>
        /// <returns></returns>
        public static ResultModel SetProduceLog(ProduceLogTypeEnum lType, int quantity = 0, PauseResonEnum pReson = PauseResonEnum.Other)
        {
            string url = uri + "PostProduceLog";
            object parameter = new
            {
                WorkTicketCode = StoreInfo.CurrentWorkingTicket.WorkTicketCode,
                UserCode = StoreInfo.UserCode,
                UserName = StoreInfo.UserName,
                Quantity = quantity,
                Type = (int)lType,
                PauseReson = (int)pReson
            };
            return HttpHelper.PostTByUrl<ResultModel>(url, parameter);
        }

        /// <summary>
        /// 录入生产实际开始/结束时间
        /// </summary>
        /// <returns></returns>
        public static ResultModel SetActualProducePlanDate(ProducePlanStateEnum state)
        {
            string url = uri + "GetActualProducePlanDate?planID=" + StoreInfo.CurrentWorkingTicket.ProducePlanID + "&state=" + (int)state;
            return HttpHelper.GetTByUrl<ResultModel>(url);
        }

        /// <summary>
        /// 记录计划执行操作人
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ResultModel SetProduceOperator(List<string> barCodes, int logID)
        {
            string url = uri + "PostProduceOperator";
            return HttpHelper.PostTByUrl<ResultModel>(url, new
            {
                LogID = logID,
                BarCodes = string.Join(",", barCodes)
            });
        }

        /// <summary>
        /// 物料扫码出库
        /// </summary>
        /// <param name="partCode"></param>
        /// <returns></returns>
        public static ResultModel MaterialScanOutput(string partCode)
        {
            string url = uri + "PostMaterialOutput";
            var materialInfo = StoreInfo.MaterialChild.MaterialInfos.Single(item => item.PartCode.Equals(partCode));

            return HttpHelper.PostTByUrl<ResultModel>(url, new
            {
                mainData = new
                {
                    ContractCode = StoreInfo.CurrentWorkingTicket.ContractCode,
                    ProjectName = StoreInfo.CurrentWorkingTicket.ProjectName,
                    WarehouseCode = StoreInfo.UserExtraInfo.WarehouseCode,
                    WarehouseName = StoreInfo.UserExtraInfo.WarehouseName,
                    ApproveState = 2
                },
                detailData = new
                {
                    InventoryCode = materialInfo.ExtraInfo.PartInfo.InventoryCode,
                    InventoryName = materialInfo.ExtraInfo.PartInfo.InventoryName,
                    Specs = materialInfo.ExtraInfo.PartInfo.Model,
                    Unit = materialInfo.ExtraInfo.PartInfo.QuantityUnit,
                    MateNum = 1,
                    ConfirmNum = 1,
                    PBillCode = StoreInfo.CurrentWorkingTicket.WorkTicketCode
                },
                userData = new
                {
                    UserCode = StoreInfo.UserCode,
                    UserName = StoreInfo.UserName
                }
            });
        }

        /// <summary>
        /// 记录扫码日志
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public static ResultModel SaveBarCodeScanLog(string barCode, int quantity = 1)
        {
            string url = uri + "PostSaveBarCodeScanLog";

            return HttpHelper.PostTByUrl<ResultModel>(url, new
            {
                UserBarCode = StoreInfo.BarCode,
                PartBarCode = barCode,
                ApsCode = StoreInfo.CurrentWorkingTicket.WorkTicketCode,
                MateQuantity = quantity,
                UserName = StoreInfo.UserName
            });
        }

        /// <summary>
        /// 根据条码 查PartCode、FigureNo
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public static ResultModel GetPartCodeAndFigureNoByBarCode(string barCode)
        {
            string url = uri + "GetPartCodeAndFigureNoByBarCode?barCode=" + barCode;

            return HttpHelper.GetTByUrl<ResultModel>(url);
        }

        /// <summary>
        /// 判断当前生产计划是否是最后一道工序
        /// </summary>
        /// <returns></returns>
        public static ResultModel IsLastProcess()
        {
            string url = uri + "GetIsLastProcess?planID=" + StoreInfo.CurrentWorkingTicket.ProducePlanID;

            return HttpHelper.GetTByUrl<ResultModel>(url);
        }

        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="inventoryCode"></param>
        /// <returns></returns>
        public static ResultModel GenerateBarCode()
        {
            string url = uri + "GetUpdatePartBarCode?partID=" + StoreInfo.MaterialOneSelf.MaterialInfo.ExtraInfo.PartInfo.ID;

            return HttpHelper.GetTByUrl<ResultModel>(url);
        }



    }
}
