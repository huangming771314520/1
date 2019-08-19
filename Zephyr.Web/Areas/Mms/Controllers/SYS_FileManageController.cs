using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Mms.Controllers
{
    public class SYS_FileManageController : Controller
    {

    }

    public class SYS_FileManageApiController : ApiController
    {
        #region 图纸申请 文件断点续传

        public dynamic GetResumFile(string fileName)
        {
            try
            {
                ResultModel<string> result = WinFormClientService.GetFileUpLoadPath(new
                {
                    RootPath = "SYS_DrawingApplication",
                    FileType = Path.GetExtension(fileName)
                });

                var saveFilePath = result.Data + fileName;
                long length = 0;

                if (File.Exists(saveFilePath))
                {
                    using (FileStream fs = File.OpenWrite(saveFilePath))
                    {
                        length = fs.Length;
                    }
                }

                return new ResultModel()
                {
                    Result = true,
                    Data = length
                };
            }
            catch (Exception ex)
            {
                return new ResultModel()
                {
                    Result = false,
                    Msg = ex.ToString()
                };
            }
        }

        public dynamic PostRsume()
        {
            try
            {
                var file = HttpContext.Current.Request.InputStream;
                var fileName = HttpContext.Current.Request.QueryString["filename"];

                ResultModel<string> result = WinFormClientService.GetFileUpLoadPath(new
                {
                    RootPath = "SYS_DrawingApplication",
                    FileType = Path.GetExtension(fileName)
                });

                return new SYS_FileManageService().SaveAs(result.Data + fileName, file);
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

        public dynamic PostSetFileManageData(dynamic data)
        {
            try
            {
                string BindCode = data["BindCode"];
                string FileName = data["FileName"];
                string MD5Code = data["MD5Code"];

                ResultModel<string> result = WinFormClientService.GetFileUpLoadPath(new
                {
                    RootPath = "SYS_DrawingApplication",
                    FileType = Path.GetExtension(FileName)
                });

                return new SYS_FileManageService().InsertData(new SYS_FileManage()
                {
                    BindTableName = nameof(SYS_DrawingApplication),
                    BindCode = BindCode,
                    FileName = Path.GetFileNameWithoutExtension(FileName),
                    FileType = 0,
                    FileSuffix = Path.GetExtension(FileName).Replace(".", string.Empty),
                    FileAddress = result.Data + MD5Code + Path.GetExtension(FileName),
                    MD5Code = MD5Code,

                    CreateTime = DateTime.Now
                });

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

        #endregion
    }
}