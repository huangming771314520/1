using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Areas;
using System.IO;
using System.Text;
using System.Collections;
using System.Web.UI.WebControls;
using System.Data;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Utils;
using System.Xml.Linq;
using Zephyr.Areas.CommonWrap;
using System.Net.Http;
using System.Net;
using System.Configuration;
using Microsoft.Win32;

namespace Zephyr.Areas.Mms.Controllers
{
    [MvcMenuFilter(false)]
    public class COM_UploadFileManageController : Controller
    {
        //
        // GET: /Mms/COM_UploadFileManage/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index_Ftp()
        {
            return View();
        }

        public FileContentResult DownLoad_Ftp(string filepath)
        {
            string ftpUid = ConfigurationManager.AppSettings["FtpUser"].ToString();
            string ftpPwd = ConfigurationManager.AppSettings["FtpPassword"].ToString();

            try
            {
                //Uri uri = new Uri(filepath);
                //string url = $"ftp://{ftpUid}:{ftpPwd}@{uri.Host}{uri.AbsolutePath}";

                WebClient request = new WebClient();
                request.Credentials = new NetworkCredential(ftpUid, ftpPwd);

                byte[] bytes = request.DownloadData(filepath);

                string contentType = "application/x-" + Path.GetExtension(filepath).Substring(1);
                RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(Path.GetExtension(filepath));
                if (regKey != null && regKey.GetValue("Content Type") != null)
                {
                    contentType = regKey.GetValue("Content Type").ToString();
                }
                return new FileContentResult(bytes, contentType);
            }
            catch
            {
                return null;
            }

            #region 弃用的方法
            /*
            string FtpUser = ConfigurationManager.AppSettings["FtpUser"];
            string FtpPassword = ConfigurationManager.AppSettings["FtpPassword"];
            Uri serverUri = new Uri(filepath);
            WebClient request = new WebClient();
            request.Credentials = new NetworkCredential(FtpUser, FtpPassword);

            try
            {
                byte[] newFileData = null;
                //var model = new PRS_ProcessFigureService().GetModel(ParamQuery.Instance().AndWhere("FileAddress", filepath));
                //if (model.FileContent.Count() > 0)
                //{
                //    newFileData = model.FileContent;
                //}
                newFileData = request.DownloadData(serverUri.ToString()); //从FTP服务器下载文件
                string fileString = System.Text.Encoding.UTF8.GetString(newFileData);
                string fileType = filepath.Substring(filepath.LastIndexOf('.'));
                string contenttype = "application/";
                if (fileType == ".text")   //根据文件类型判断类型头
                {
                    contenttype = "text/plain";
                }
                else if (fileType == ".doc" || fileType == ".docx")
                {
                    contenttype += "msword";
                }
                else if (fileType == ".xls" || fileType == ".xlsx" || fileType == ".ppt" || fileType == ".pptx")
                {
                    contenttype = contenttype + "x-" + fileType.Substring(1);
                }
                else if (fileType == ".jpg" || fileType == "bmp" || fileType == "jpeg" || fileType == "gif" || fileType == "png")
                {
                    contenttype = "image/" + fileType.Substring(1);
                }
                else
                {
                    contenttype += "x-" + fileType.Substring(1);

                }
                return File(newFileData, contenttype);
            }
            catch (WebException e)
            {
                // 若服务器未开启或者文件不存在抛出异常
                Response.Write("<script languge='javascript'>alert('源文件已经被删除或者文件服务器未开启！'); window.close(); </script>");
                return null;
            }
            */
            #endregion

        }

        public void MyDownloadFile(string filepath)
        {
            try
            {
                string ftpUid = ConfigurationManager.AppSettings["FtpUser"].ToString();
                string ftpPwd = ConfigurationManager.AppSettings["FtpPassword"].ToString();

                WebClient request = new WebClient();
                request.Credentials = new NetworkCredential(ftpUid, ftpPwd);

                byte[] bytes = request.DownloadData(filepath);

                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + Path.GetFileName(filepath));
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();
            }
            catch
            {

            }
        }

    }

    public class COM_UploadFileManageApiController : ApiController
    {
        public dynamic GetFileUrl(string BindTableName, string BindCode, string FileType)
        {
            var FileList = new SYS_FileManageService().GetModelList
                (
                ParamQuery.Instance()
                .AndWhere("BindTableName", BindTableName)
                .AndWhere("BindCode", BindCode)
                .AndWhere("FileType", FileType)
                );
            return FileList;
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage PostUpload(string BindTableName, string BindCode)
        {
            HttpPostedFile fileData = HttpContext.Current.Request.Files[0];
            string filePath = HttpContext.Current.Server.MapPath("~/Upload/" + BindTableName + "/"); //获取需要存放的服务器文件夹地址
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fileName = Path.GetFileName(fileData.FileName);      //本地上传文件名称
            string fileExtension = Path.GetExtension(fileName).Substring(1);     //本地上传文件拓展名
            string saveName = "[" + DateTime.Now.ToString("yyyyMMddhhmmss") + "]" + fileName; //时间戳前缀+本地上传文件名称
            string UploadUrl = string.Format("{0}\\{1}", filePath, saveName);
            fileData.SaveAs(UploadUrl);

            new SYS_FileManageService().Delete(
                ParamDelete.Instance()
                .From("SYS_FileManage")
                .AndWhere("BindTableName", BindTableName)
                .AndWhere("BindCode", BindCode)
                .AndWhere("FileType", 1)
                );

            new SYS_FileManageService().Insert(
                ParamInsert.Instance()
                .Insert("SYS_FileManage")
                .Column("BindTableName", BindTableName)
                .Column("BindCode", BindCode)
                .Column("FileName", fileName)
                .Column("FileType", 1)
                .Column("FileSuffix", fileExtension)
                .Column("FileAddress", UploadUrl)
                .Column("CreatePerson", "")
                .Column("CreateTime", DateTime.Now)
                );
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// FTP上传文件
        /// </summary>
        /// <param name="Savepath">服务器用于保存的文件夹路径，不是服务器根路径,例如： "/UploadDocumentsSave/"</param>
        /// <param name="localpath">本地路径</param>
        /// <param name="filetype">文件类型，例如: ".rte"</param>
        public void PostFTPUpload(string docName, string path)
        {
            //path:PRS_ProcessFigure/
            //fileName:2019070309511121870.png
            HttpPostedFile fileData = HttpContext.Current.Request.Files[0];
            var filestream = fileData.InputStream;
            //var filename = fileData.FileName;
            var filename = docName + Path.GetExtension(fileData.FileName);

            string FtpServer = "ftp://" + ConfigurationManager.AppSettings["FtpServer"] + ":" + ConfigurationManager.AppSettings["FtpPort"];
            string FtpFolder = path.Substring(0, path.Length - 1);
            if (!string.IsNullOrEmpty(FtpFolder))
            {
                FtpServer = FtpServer + "/" + FtpFolder;
            }
            string FtpUrl = string.Format("{0}/{1}", FtpServer, filename);
            FtpHelper.UploadFTPWebRequest(FtpUrl, FtpServer, filestream);
        }


    }
}
