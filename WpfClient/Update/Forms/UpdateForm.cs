using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Update.Common;
using Update.Model;

namespace Update.Forms
{
    public partial class UpdateForm : Form
    {
        private string appDirPath, tempDirName;
        private bool isUpdateXML = true;

        public UpdateForm(string _appDirPath, string _tempDirName)
        {
            appDirPath = _appDirPath;
            tempDirName = _tempDirName;

            InitializeComponent();
        }

        private void UpdateForm_Load(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(DownLoadFile));
                }
                else
                {
                    DownLoadFile();
                }
            });
        }

        private void DownLoadFile()
        {
            ZipHelper zipHelper = new ZipHelper();

            lblState.Text = @"正在下载...";

            var url = string.Format(@"{0}/{1}/v{2}/{3}", RootLocal.ServerUrl, RootLocal.Workshop, RootServer.LatestVersion, RootLocal.ZipName);
            var client = new WebClient();
            client.DownloadProgressChanged += (sender, e) =>
            {
                float proportion = (float)e.BytesReceived / e.TotalBytesToReceive;
                lblProportion.Text = proportion.ToString("P2");
                prgProportion.Value = (int)(proportion * 100);
            };
            client.DownloadDataCompleted += (sender, e) =>
            {
                lblProportion.Text = "100%";
                prgProportion.Value = 100;
                lblState.Text = @"下载完成...";

                string zipFilePath = Path.Combine(appDirPath, "Update Pack", tempDirName, RootLocal.ZipName);
                byte[] data = e.Result;
                BinaryWriter writer = new BinaryWriter(new FileStream(zipFilePath, FileMode.OpenOrCreate));
                writer.Write(data);
                writer.Flush();
                writer.Close();

                lblState.Text = @"正在执行更新，请稍候...";

                string tempDirPath = Path.Combine(appDirPath, "Update Pack", tempDirName, "temp");
                zipHelper.UnZip(zipFilePath, tempDirPath);

                Process[] processes = Process.GetProcessesByName(RootLocal.AppName);
                if (processes.Length > 0)
                {
                    foreach (var p in processes)
                    {
                        p.Kill();
                    }
                }

                if (UpdateInfo.UpdateType.ToLower().Equals("all"))
                {
                    DelDirectory(appDirPath);
                }
                CopyDirectory(tempDirPath, appDirPath);

                if (isUpdateXML)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load("LocalVersion.xml");
                    doc.SelectSingleNode("RootLocal/Version").InnerText = UpdateInfo.AppVersion.ToString();
                    doc.Save(Path.Combine(appDirPath, "LocalVersion.xml"));
                }

                lblState.Text = @"更新完成...";

                DialogResult dr = MessageBox.Show("更新成功！\n需要立即启动应用程序吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.OK)
                {
                    var info = new ProcessStartInfo(appDirPath + "/" + RootLocal.AppName);
                    info.UseShellExecute = true;
                    info.Arguments = "true";
                    Process.Start(info);
                }

                Application.Exit();
            };
            client.DownloadDataAsync(new Uri(url));
        }


        private void CopyDirectory(string sourceDirName, string destDirName)
        {
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
                File.SetAttributes(destDirName, File.GetAttributes(sourceDirName));
            }

            if (destDirName[destDirName.Length - 1] != Path.DirectorySeparatorChar)
            {
                destDirName = destDirName + Path.DirectorySeparatorChar;
            }

            string[] files = Directory.GetFiles(sourceDirName);
            foreach (string file in files)
            {
                if (Path.GetFileName(file).ToLower().Equals("localversion.xml"))
                {
                    isUpdateXML = false;
                }
                File.Copy(file, destDirName + Path.GetFileName(file), true);
                File.SetAttributes(destDirName + Path.GetFileName(file), FileAttributes.Normal);
            }
            string[] dirs = Directory.GetDirectories(sourceDirName);
            foreach (string dir in dirs)
            {
                CopyDirectory(dir, destDirName + Path.GetFileName(dir));
            }
        }

        public void DelDirectory(string sourceDirName)
        {
            string[] files = Directory.GetFiles(sourceDirName);
            foreach (string file in files)
            {
                File.Delete(file);
            }

            string[] dirs = Directory.GetDirectories(sourceDirName);
            foreach (string dir in dirs)
            {
                DirectoryInfo info = new DirectoryInfo(dir);
                if (!info.Name.Equals("Update Pack"))
                {
                    Directory.Delete(dir, true);
                }
            }
        }

    }
}
