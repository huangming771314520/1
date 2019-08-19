using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FtpFileUpload
{
    public partial class DownLoadForm : Form
    {
        public string strWebAddress = string.Empty;
        public string strFileName = string.Empty;
        public string strFilePath = string.Empty;
        public string strDownType = string.Empty;

        public DownLoadForm()
        {
            InitializeComponent();
        }

        public DownLoadForm(string _fileAddress, string _fileName, string _downType)
        {
            strWebAddress = _fileAddress;
            strFileName = _fileName;
            strDownType = _downType;



            InitializeComponent();

            if (_downType == "http")
            {
                _fileAddress = _fileAddress.Replace('\\', '/');
                string API = ConfigurationManager.AppSettings["API"];
                _fileAddress = "http://" + API + _fileAddress.Substring(_fileAddress.IndexOf("/Upload"));
            }
            txtWebAddress.Text = _fileAddress;
            txtFileName.Text = _fileName;

            if (txtFilePath.Text == "")
            {
                var a1 = Environment.CurrentDirectory;
                txtFilePath.Text = a1;
            }
        }

        private void DownLoadForm_Load(object sender, EventArgs e)
        {
            string appPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            txtWebAddress.Text = strWebAddress;
            txtFileName.Text = strFileName;
            txtFilePath.Text = appPath.Substring(0, appPath.IndexOf(@"\")) + "\\";
        }

        private void btnBrowsePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.SelectedPath = txtFilePath.Text;

            if (folder.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = folder.SelectedPath;
            }
        }

        //private void btnDownloadAndOpen_Click(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtFilePath.Text))
        //    {
        //        MessageBox.Show("请选择保存路径!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        //        return;
        //    }
        //    if (strDownType == "ftp")
        //    {
        //        DownloadFile_ftp(txtFilePath.Text, txtWebAddress.Text);
        //    }
        //    else if (strDownType == "http")
        //    {
        //        DownloadFile();
        //    }

        //    //DownloadFile();

        //    System.Diagnostics.Process.Start(strFilePath);

        //    DialogResult = DialogResult.OK;
        //}

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFilePath.Text))
            {
                MessageBox.Show("请选择保存路径!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            if (strDownType == "ftp")
            {
                DownloadFile_ftp(txtFilePath.Text, txtWebAddress.Text);
            }
            else if (strDownType == "http")
            {
                DownloadFile();
            }
            MessageBox.Show("下载完成!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void DownloadFile_ftp(string LocalUrl, string FileAddress_ftp)
        {
            string FtpServer = ConfigurationManager.AppSettings["FtpServer"];
            string FtpPort = ConfigurationManager.AppSettings["FtpPort"];
            string FtpUser = ConfigurationManager.AppSettings["FtpUser"];
            string FtpPassword = ConfigurationManager.AppSettings["FtpPassword"];
            FtpWeb ftp = new FtpWeb(FtpServer + ":" + FtpPort, "PRS_ProcessFigure", FtpUser, FtpPassword);
            var filename = FileAddress_ftp.Substring(FileAddress_ftp.LastIndexOf('/') + 1);
            ftp.Download(LocalUrl, filename);
        }

        private void DownloadFile()
        {
            strFilePath = txtFilePath.Text.Trim();
            strFilePath.Replace('/', '\\');

            if (string.IsNullOrEmpty(strFilePath))
            {
                MessageBox.Show("请选择保存路径!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (string.IsNullOrEmpty(txtFileName.Text.Trim()))
            {
                MessageBox.Show("请填写文件名!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (!Directory.Exists(strFilePath))
            {
                Directory.CreateDirectory(strFilePath);
            }

            if (!strFilePath[strFilePath.Length - 1].Equals('\\'))
            {
                strFilePath += "\\";
            }

            strFilePath += txtFileName.Text.Trim();

            try
            {
                Stream stream = System.Net.WebRequest.Create(txtWebAddress.Text).GetResponse().GetResponseStream();
                Stream fileStream = new FileStream(strFilePath, FileMode.Create, FileAccess.Write);

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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
    }
}
