using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace MesClient
{
    /// <summary>
    /// 日志帮助类
    /// </summary>
    public class LogHelper
    {
        private static Queue<string> listQueue = new Queue<string>();
        private static readonly string filePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, $"Logs/log{DateTime.Now.ToString("yyyyMMdd")}.txt");

        static LogHelper()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            new Thread(new ThreadStart(delegate
            {
                while (true)
                {
                    if (listQueue.Count > 0)
                    {
                        while (listQueue.Count > 0)
                        {
                            try
                            {
                                StreamWriter sw = File.AppendText(filePath);
                                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff : ") + listQueue.Dequeue());
                                sw.Close();
                            }
                            catch
                            {

                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
            }))
            { IsBackground = true }.Start();
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="content">内容</param>
        public static void WriteLog(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                listQueue.Enqueue(content);
            }
        }
    }
}
