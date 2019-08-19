using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Zephyr.Areas.CommonWrap
{
    public class FtpHelper
    {
        private static FtpWebRequest reqFTP;
        private static string FtpUser = ConfigurationManager.AppSettings["FtpUser"];
        private static string FtpPassword = ConfigurationManager.AppSettings["FtpPassword"];
        private static bool UsePassive = Convert.ToBoolean(ConfigurationManager.AppSettings["UsePassive"]);
        private static void Connect(String path)//连接ftp  
        {
            // 根据uri创建FtpWebRequest对象  
            WebRequest req = FtpWebRequest.Create(new Uri(path));
            reqFTP = (FtpWebRequest)req;
            // 指定数据传输类型  
            reqFTP.UseBinary = true;
            //reqFTP.Method= WebRequestMethods.Ftp.UploadFile;//将FtpWebRequest属性设置为上传文件
            // ftp用户名和密码  
            reqFTP.Credentials = new NetworkCredential(FtpUser, FtpPassword);
            reqFTP.Proxy = null;
        }

        /// <summary>
        /// 使用FtpWebRequest类上传文件
        /// </summary>
        /// <param name="ftpurl">"ftp://115.28.80.19/Album/"</param>
        /// <param name="file">HttpPostedFileBase</param>
        /// <param name="filename">filename文件名</param>
        public static void UploadFTPWebRequest(string ftpurl,string ftpfolder, Stream filestream)
        {
            MakeDir(ftpfolder);
            Connect(ftpurl);
            // 默认为true，连接不会被关闭
            // 在一个命令之后被执行
            reqFTP.KeepAlive = false;
            // 指定执行什么命令
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            reqFTP.UsePassive = UsePassive;  //选择主动还是被动模式,这句要加上的。
            // 指定数据传输类型
            reqFTP.UseBinary = true;
            // 上传文件时通知服务器文件的大小
            reqFTP.ContentLength = filestream.Length;
            // 缓冲大小设置为2kb
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
            Stream strm = reqFTP.GetRequestStream();
            filestream.Position = 0;
            // 每次读文件流的2kb
            contentLen = filestream.Read(buff, 0, buffLength);
            // 流内容没有结束
            while (contentLen != 0)
            {
                // 把内容从file stream 写入 upload stream
                strm.Write(buff, 0, contentLen);
                contentLen = filestream.Read(buff, 0, buffLength);
            }
            // 关闭两个流
            strm.Close();
            filestream.Close();
        }

        //创建目录
        public static void MakeDir(string uri)
        {
            try
            {
                Connect(uri);//连接      
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                
            }
        }


        public static bool Download(string filePath) //上面的代码实现了从ftp服务器下载文件的功能
        {
            try
            {
                //String onlyFileName = Path.GetFileName(fileName);
                //string newFileName = filePath + "\\" + onlyFileName;
                //if (File.Exists(newFileName))
                //{
                //    //errorinfo = string.Format("本地文件{0}已存在,无法下载", newFileName);
                //    return false;
                //}
                //string url = "ftp://" + ftpServerIP + "/" + fileName;
                Connect(filePath);//连接  
                reqFTP.Credentials = new NetworkCredential(FtpUser, FtpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                FileStream outputStream = new FileStream(filePath, FileMode.Create);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();
                //errorinfo = "";
                return true;
            }
            catch (Exception ex)
            {
                //errorinfo = string.Format("因{0},无法下载", ex.Message);
                return false;
            }
        }

        /// 将 Stream 转成 byte[]
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// 将 byte[] 转成 Stream
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
    }
}