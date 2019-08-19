using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlueprintUploadWinformUI.Helpers
{
    public class FtpWeb
    {
        string ftpServerIP;
        string ftpRemotePath;
        string ftpUserID;
        string ftpPassword;
        string ftpURI;

        public static long file_jd = 0;


        /// <summary>

        /// 连接FTP

        /// </summary>

        /// <param name="FtpServerIP">FTP连接地址</param>

        /// <param name="FtpRemotePath">指定FTP连接成功后的当前目录, 如果不指定即默认为根目录</param>

        /// <param name="FtpUserID">用户名</param>

        /// <param name="FtpPassword">密码</param>

        public FtpWeb(string FtpServerIP, string FtpRemotePath, string FtpUserID, string FtpPassword)
        {
            ftpServerIP = FtpServerIP;
            ftpRemotePath = FtpRemotePath;
            ftpUserID = FtpUserID;
            ftpPassword = FtpPassword;
            ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";

        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="filename"></param>
        public void Upload(string NewFileName, System.Windows.Forms.ProgressBar prs_bar, long total_bytes, string filename, out string ftp_url, out string file_name)
        {
            //if (window.InvokeRequired)
            //{
            //    window.Invoke(new MethodInvoker(()=> {
            //    }));
            //}


            FileInfo fileInf = new FileInfo(filename);
            string ext = Path.GetExtension(fileInf.Name);
            string uri = ftpURI + NewFileName + ext;
            ftp_url = uri;
            file_name = fileInf.Name;
            FtpWebRequest reqFTP;

            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));//在ftp服务器上创建一个实例
            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            reqFTP.KeepAlive = false;
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            reqFTP.UsePassive = false;  //选择主动还是被动模式,这句要加上的。
            reqFTP.UseBinary = true;
            reqFTP.ContentLength = fileInf.Length;
            int buffLength = 2048;//声明一个2kb的缓冲变量
            byte[] buff = new byte[buffLength];//声明一个2kb字节长度的字节数组：用于存放本地文件流
            int contentLen;
            FileStream fs = fileInf.OpenRead();//将本地文件转换成文件流
            try
            {
                Stream strm = reqFTP.GetRequestStream();//检索出：在ftp服务器上创建的流
                contentLen = fs.Read(buff, 0, buffLength);//将本地的文件流写入 已声明的2kb字节长度的字节数组变量中
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);//ftp服务器上创建的流写入字节数组
                    file_jd = file_jd + contentLen;
                    int jd = Convert.ToInt32((file_jd * 100) / total_bytes);
                    prs_bar.Value = jd;
                    contentLen = fs.Read(buff, 0, buffLength);
                    //if (file_jd == total_bytes)
                    //{
                    //    MessageBox.Show("图纸已上传完成！");
                    //}
                }
                strm.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
            }
        }

        /// <summary>

        /// 下载

        /// </summary>

        /// <param name="filePath"></param>

        /// <param name="fileName"></param>

        public void Download(string filePath, string fileName)
        {
            FtpWebRequest reqFTP;
            try
            {
                FileStream outputStream = new FileStream(filePath + "\\" + fileName, FileMode.Create);

                //reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + fileName));
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + fileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>

        /// 删除文件

        /// </summary>

        /// <param name="fileName"></param>

        public void Delete(string fileName)
        {
            try
            {
                string uri = ftpURI + fileName;
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;

                string result = String.Empty;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();

            }
            catch
            {

            }
        }
    }
}

