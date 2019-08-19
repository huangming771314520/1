using BlueprintUpload.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace BlueprintUpload.Helpers
{
    public class FtpHelper
    {

        public static bool UploadFile(string filePath, string ftpFileName = "")
        {
            bool result = true;

            FileInfo fileInfo = new FileInfo(filePath);

            if (string.IsNullOrEmpty(ftpFileName))
            {
                ftpFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "1".PadLeft(5, '0') + Path.GetExtension(filePath);
            }
            string ftpPath = $"ftp://{ConfigInfo.FtpSite}/{ConfigInfo.FtpSiteDicPath}/{ftpFileName}";

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(ftpPath));
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.UseBinary = true;
            request.UsePassive = false;
            request.KeepAlive = true;
            request.Credentials = new NetworkCredential(ConfigInfo.FtpUid, ConfigInfo.FtpPwd);
            request.ContentLength = fileInfo.Length;

            try
            {
                using (Stream rs = request.GetRequestStream())
                {
                    using (FileStream fs = fileInfo.OpenRead())
                    {
                        byte[] buffer = new byte[1024 * 4];
                        int count = fs.Read(buffer, 0, buffer.Length);
                        while (count > 0)
                        {
                            rs.Write(buffer, 0, count);
                            count = fs.Read(buffer, 0, buffer.Length);
                        }
                        fs.Close();
                    }
                }
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public static bool UploadMultiFile(List<string> filePaths, ref List<FileInfoEntity> files)
        {
            bool result = true;
            string temp = DateTime.Now.ToString("yyyyMMddHHmmss");

            for (var i = 0; i < filePaths.Count; i++)
            {
                string ftpFileName = temp + (i + 1).ToString().PadLeft(5, '0') + Path.GetExtension(filePaths[i]);
                string ftpPath = $"ftp://{ConfigInfo.FtpSite}/{ConfigInfo.FtpSiteDicPath}/{ftpFileName}";

                files.Add(new FileInfoEntity()
                {
                    FileName = ftpFileName,
                    DocName = Path.GetFileName(filePaths[i]),
                    FileAddress = ftpPath
                });

                if (!UploadFile(filePaths[i], ftpFileName))
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        public static ResultModel<List<FileInfoEntity>> UploadMultiFile(List<string> filePaths)
        {
            try
            {
                List<FileInfoEntity> files = new List<FileInfoEntity>();
                string temp = DateTime.Now.ToString("yyyyMMddHHmmss");

                for (var i = 0; i < filePaths.Count; i++)
                {
                    string ftpFileName = temp + (i + 1).ToString().PadLeft(5, '0') + Path.GetExtension(filePaths[i]);
                    string ftpPath = $"ftp://{ConfigInfo.FtpSite}/{ConfigInfo.FtpSiteDicPath}/{ftpFileName}";

                    files.Add(new FileInfoEntity()
                    {
                        FileName = ftpFileName,
                        DocName = Path.GetFileName(filePaths[i]),
                        FileAddress = ftpPath
                    });

                    FileInfo fileInfo = new FileInfo(filePaths[i]);
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(ftpPath));
                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    request.UseBinary = true;
                    request.UsePassive = false;
                    request.KeepAlive = true;
                    request.Credentials = new NetworkCredential(ConfigInfo.FtpUid, ConfigInfo.FtpPwd);
                    request.ContentLength = fileInfo.Length;

                    using (Stream rs = request.GetRequestStream())
                    {
                        using (FileStream fs = fileInfo.OpenRead())
                        {
                            byte[] buffer = new byte[1024 * 4];
                            int count = fs.Read(buffer, 0, buffer.Length);
                            while (count > 0)
                            {
                                rs.Write(buffer, 0, count);
                                count = fs.Read(buffer, 0, buffer.Length);
                            }
                            fs.Close();
                        }
                    }
                }

                return new ResultModel<List<FileInfoEntity>>()
                {
                    Result = true,
                    Data = files,
                    Msg = "文件上传完成！"
                };
            }
            catch (Exception ex)
            {
                return new ResultModel<List<FileInfoEntity>>()
                {
                    Result = false,
                    Data = new List<FileInfoEntity>(),
                    Msg = ex.Message
                };
            }
        }

    }
}
