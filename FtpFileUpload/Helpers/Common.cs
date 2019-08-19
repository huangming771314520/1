using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace FtpFileUpload.Helpers
{
    public class Common
    {
        public static string GetStreamMd5(string filePath)
        {
            using (FileStream fStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] data = md5.ComputeHash(fStream);
                fStream.Close();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sb.Append(data[i].ToString("x4"));
                }
                return sb.ToString();
            }
        }

        public static void MessageBoxShow(Form form, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            if (form.InvokeRequired)
            {
                form.Invoke(new MethodInvoker(delegate
                {
                    MessageBox.Show(text, caption, buttons, icon);
                }));
            }
            else
            {
                MessageBox.Show(text, caption, buttons, icon);
            }
        }

        /// <summary>
        /// 记录bug，以便调试
        /// </summary>
        public static bool WriteTxt(string str)
        {
            try
            {
                str = DateTime.Now.ToString("yyyy_MM_dd HH:mm:ss    ") + str;
                string LogPath = Application.StartupPath + @"/Err_log/";
                if (!Directory.Exists(LogPath))
                {
                    Directory.CreateDirectory(LogPath);
                }
                FileStream FileStream = new FileStream(string.Format(@"{0}/Err_log/err_log_{1}.txt", LogPath, DateTime.Now.ToString("yyyy_MM_dd")), FileMode.Append);
                StreamWriter StreamWriter = new StreamWriter(FileStream);
                StreamWriter.WriteLine(str);
                StreamWriter.Flush();
                StreamWriter.Close();
                FileStream.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

    }
}
