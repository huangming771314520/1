using MesClient.Entitys;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MesClient.Helpers
{
    public class FtpHelper
    {
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool DownLoad(FileInfoEntity file)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(file.FileAddress);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.UseBinary = true;
                request.UsePassive = false;
                request.Credentials = new NetworkCredential(ConfigInfo.FtpUid, ConfigInfo.FtpPwd);
                request.KeepAlive = false;

                using (Stream stream = request.GetResponse().GetResponseStream())
                {
                    using (Stream fileStream = new FileStream(string.Format(@"{0}\{1}", ConfigInfo.DocDirName, file.FileName), FileMode.Create, FileAccess.Write))
                    {
                        byte[] bytes = new byte[1024 * 4];
                        int size = stream.Read(bytes, 0, bytes.Length);
                        while (size > 0)
                        {
                            fileStream.Write(bytes, 0, size);
                            size = stream.Read(bytes, 0, bytes.Length);
                        }
                        fileStream.Flush();
                        fileStream.Close();
                        stream.Close();

                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
