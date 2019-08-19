using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Model;
using System.Net;
using BLL.Helpers;

namespace BLL.ExtraBLL
{
    public class ProducePlanInfoBLL
    {
        public enum ProduceLogTypeEnum
        {
            /// <summary>
            /// 开始
            /// </summary>
            Start = 0,
            /// <summary>
            /// 暂停
            /// </summary>
            Pause = 1,
            /// <summary>
            /// 结束
            /// </summary>
            End = 2
        }

        public enum ProducePlanStateEnum
        {
            /// <summary>
            /// 开始
            /// </summary>
            Start = 0,
            /// <summary>
            /// 结束
            /// </summary>
            End = 1
        }

        /// <summary>
        /// 获取所有任务计划
        /// </summary>
        /// <returns></returns>
        public ProducePlanInfoModel GetProducePlanInfo()
        {
            string url = string.Format(@"http://{0}/api/Mms/WinFormClient/GetProducePlanInfoByTCode?teamCode={1}", ConfigInfoModel.API, StoreInfoModel.WorkGroupInfo.GetWorkGroup.TeamCode);
            var result = Helpers.HttpHelper.GetTByUrl<ProducePlanInfoModel>(url);
            StoreInfoModel.ProducePlanInfos = result.Data;
            return result;
        }

        /// <summary>
        /// 获取某计划中用到的所有图纸
        /// </summary>
        public ResultModel DownLoadDrawing()
        {
            string dirPath = AppDomain.CurrentDomain.BaseDirectory + ConfigInfoModel.DocDirName;
            Directory.CreateDirectory(dirPath);

            //int numA = 0;
            int numB = 0;
            int numC = 0;

            #region
            /*
            foreach (var item in StoreInfoModel.ProducePlanInfo.GetPartFiles)
            {
                try
                {
                    string url = string.Format(@"http://" + ConfigInfoModel.API + "/api/Mms/WinFormClient/GetDrawingWebPathByID?id={0}", item.ID);

                    string json = Helpers.HttpHelper.GetJSON(url);
                    dynamic dynamicObject = JsonConvert.DeserializeObject<dynamic>(json);

                    if (Convert.ToBoolean(dynamicObject["Result"]))
                    {
                        string webPath = dynamicObject["Data"];

                        Stream stream = System.Net.WebRequest.Create(webPath).GetResponse().GetResponseStream();
                        Stream fileStream = new FileStream(string.Format(@"{0}\{1}", ConfigInfoModel.DocDirName, item.FileName), FileMode.Create, FileAccess.Write);

                        byte[] bArr = new byte[1024];
                        int size = stream.Read(bArr, 0, bArr.Length);
                        while (size > 0)
                        {
                            fileStream.Write(bArr, 0, size);
                            size = stream.Read(bArr, 0, bArr.Length);
                        }
                        fileStream.Flush();
                        fileStream.Close();
                        stream.Close();
                    }
                    else
                    {
                        numA++;
                    }
                }
                catch (Exception ex)
                {
                    string errMsg = ex.ToString();
                    numA++;
                }
            }
            */
            #endregion

            foreach (var item in StoreInfoModel.ProducePlanInfo.GetQualityReportDocs)
            {
                try
                {
                    if (File.Exists(Path.Combine(dirPath, item.FileName)))
                    {
                        continue;
                    }

                    string url = item.FileAddress;
                    FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(url);
                    ftpWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                    ftpWebRequest.UseBinary = true;
                    ftpWebRequest.UsePassive = false;
                    ftpWebRequest.Credentials = new NetworkCredential(ConfigInfoModel.FtpUid, ConfigInfoModel.FtpPwd);
                    ftpWebRequest.KeepAlive = false;

                    Stream stream = ftpWebRequest.GetResponse().GetResponseStream();
                    Stream fileStream = new FileStream(string.Format(@"{0}\{1}", ConfigInfoModel.DocDirName, item.FileName), FileMode.Create, FileAccess.Write);

                    byte[] bArr = new byte[1024];
                    int size = stream.Read(bArr, 0, bArr.Length);
                    while (size > 0)
                    {
                        fileStream.Write(bArr, 0, size);
                        size = stream.Read(bArr, 0, bArr.Length);
                    }
                    fileStream.Flush();
                    fileStream.Close();
                    stream.Close();

                    //string url = string.Format(@"http://" + ConfigInfoModel.API + "/api/Mms/WinFormClient/GetDocWebPathByID?id={0}", item.ID);

                    //string json = Helpers.HttpHelper.GetJSON(url);
                    //dynamic dynamicObject = JsonConvert.DeserializeObject<dynamic>(json);

                    //if (Convert.ToBoolean(dynamicObject["Result"]))
                    //{
                    //    string webPath = dynamicObject["Data"];

                    //    Stream stream = System.Net.WebRequest.Create(webPath).GetResponse().GetResponseStream();
                    //    Stream fileStream = new FileStream(string.Format(@"{0}\{1}", ConfigInfoModel.DocDirName, item.FileName), FileMode.Create, FileAccess.Write);

                    //    byte[] bArr = new byte[1024];
                    //    int size = stream.Read(bArr, 0, bArr.Length);
                    //    while (size > 0)
                    //    {
                    //        fileStream.Write(bArr, 0, size);
                    //        size = stream.Read(bArr, 0, bArr.Length);
                    //    }
                    //    fileStream.Flush();
                    //    fileStream.Close();
                    //    stream.Close();
                    //}
                    //else
                    //{
                    //    numB++;
                    //}
                }
                catch (Exception ex)
                {
                    string errMsg = ex.ToString();
                    numB++;
                }
            }

            foreach (var item in StoreInfoModel.ProducePlanInfo.GetProcessFigures)
            {
                try
                {
                    if (File.Exists(Path.Combine(dirPath, item.FileName)))
                    {
                        continue;
                    }

                    string url = item.FileAddress;
                    FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(url);
                    ftpWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                    ftpWebRequest.UseBinary = true;
                    ftpWebRequest.UsePassive = false;
                    ftpWebRequest.Credentials = new NetworkCredential(ConfigInfoModel.FtpUid, ConfigInfoModel.FtpPwd);
                    ftpWebRequest.KeepAlive = false;

                    Stream stream = ftpWebRequest.GetResponse().GetResponseStream();
                    Stream fileStream = new FileStream(string.Format(@"{0}\{1}", ConfigInfoModel.DocDirName, item.FileName), FileMode.Create, FileAccess.Write);

                    byte[] bArr = new byte[1024];
                    int size = stream.Read(bArr, 0, bArr.Length);
                    while (size > 0)
                    {
                        fileStream.Write(bArr, 0, size);
                        size = stream.Read(bArr, 0, bArr.Length);
                    }
                    fileStream.Flush();
                    fileStream.Close();
                    stream.Close();

                    //string url = string.Format(@"http://" + ConfigInfoModel.API + "/api/Mms/WinFormClient/GetFigureWebPathByID?id={0}", item.ID);
                    //
                    //string json = Helpers.HttpHelper.GetJSON(url);
                    //dynamic dynamicObject = JsonConvert.DeserializeObject<dynamic>(json);
                    //
                    //if (Convert.ToBoolean(dynamicObject["Result"]))
                    //{
                    //    string webPath = dynamicObject["Data"];
                    //
                    //    Stream stream = System.Net.WebRequest.Create(webPath).GetResponse().GetResponseStream();
                    //    Stream fileStream = new FileStream(string.Format(@"{0}\{1}", ConfigInfoModel.DocDirName, item.FileName), FileMode.Create, FileAccess.Write);
                    //
                    //    byte[] bArr = new byte[1024];
                    //    int size = stream.Read(bArr, 0, bArr.Length);
                    //    while (size > 0)
                    //    {
                    //        fileStream.Write(bArr, 0, size);
                    //        size = stream.Read(bArr, 0, bArr.Length);
                    //    }
                    //    fileStream.Flush();
                    //    fileStream.Close();
                    //    stream.Close();
                    //}
                    //else
                    //{
                    //    numC++;
                    //}

                }
                catch (Exception ex)
                {
                    string errMsg = ex.ToString();
                    numC++;
                }
            }

            bool result = numB.Equals(0) && numC.Equals(0);

            return new ResultModel()
            {
                Result = result,
                Msg = !result ? string.Format(@"{0}个文件下载失败！", numB + numC) : null
            };
        }


        /// <summary>
        /// 记录生产日志
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ResultModel ProduceLog(ProduceLogTypeEnum type, ProduceProceInfoBLL.PauseResonEnum pauseReson = ProduceProceInfoBLL.PauseResonEnum.Other)
        {
            string url = string.Format(@"http://{0}/api/Mms/WinFormClient/PostProduceLog", ConfigInfoModel.API);
            object parameter = new
            {
                //APSCode = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.ApsCode,
                APSCode = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.WorkTicketCode,
                CreatePerson = StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserName,
                OperatePerson = StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserName,
                OpreateCode = StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserCode,
                ModifyPerson = StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserName,
                //FinishQuantity = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.Quantity,
                FinishQuantity = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.WorkQuantity,
                Type = (int)type,
                PauseReson = (int)pauseReson
            };
            return Helpers.HttpHelper.PostTByUrl<ResultModel>(url, parameter);
        }


        /// <summary>
        /// 录入生产实际开始/结束时间
        /// </summary>
        /// <param name="state">0:实际开始时间 1:实际结束时间</param>
        /// <returns></returns>
        public ResultModel ActualProducePlanDate(ProducePlanStateEnum state)
        {
            string url = string.Format("http://{0}/api/Mms/WinFormClient/GetProducePlanActualTime?ppdID={1}&type={2}",
                        ConfigInfoModel.API, StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.ID, (int)state);

            return Helpers.HttpHelper.GetTByUrl<ResultModel>(url);
        }


    }
}
