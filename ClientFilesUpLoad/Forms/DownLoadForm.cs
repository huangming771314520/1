using System;
using System.IO;
using System.Windows.Forms;

namespace ClientFilesUpLoad.Forms
{
    public partial class DownLoadForm : Form
    {
        public string strWebAddress = string.Empty;
        public string strFileName = string.Empty;
        public string strFilePath = string.Empty;

        public DownLoadForm(DataGridViewRow _dgr)
        {
            strWebAddress = _dgr.Cells["FileAddress"].Value.ToString();
            strFileName = _dgr.Cells["FileName"].Value.ToString();

            InitializeComponent();
        }

        public DownLoadForm(string _fileAddress, string _fileName)
        {
            strWebAddress = _fileAddress;
            strFileName = _fileName;

            InitializeComponent();
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

        private void btnDownloadAndOpen_Click(object sender, EventArgs e)
        {
            DownloadFile();

            System.Diagnostics.Process.Start(strFilePath);

            DialogResult = DialogResult.OK;
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            DownloadFile();
            MessageBox.Show("下载完成!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
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
                Stream stream = System.Net.WebRequest.Create(strWebAddress).GetResponse().GetResponseStream();
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
