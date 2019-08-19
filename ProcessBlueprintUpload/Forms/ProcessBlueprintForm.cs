using log4net;
using Microsoft.AspNet.SignalR.Client;
using ProcessBlueprintUpload.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProcessBlueprintUpload.Forms
{
    public partial class ProcessBlueprintForm : Form
    {
        //public static Dictionary<string, string> fields = new Dictionary<string, string>();
        public static string UserCode = ConfigInfo.LocalKey;
        public int ID { get; set; }

        HubConnection hubConnection;
        IHubProxy hubProxy;
        private delegate void ReceiveProcessBomID(int BomID, string FigureNumber);

        public ProcessBlueprintForm()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();

            var servercon = "http://" + ConfigInfo.API;
            hubConnection = new HubConnection(servercon + "/messageHub/hubs");
            hubProxy = hubConnection.CreateHubProxy("MessageService");
            hubProxy.On<int, string>("Touch", (BomID, FigureNumber) => this.Invoke(new ReceiveProcessBomID(Show), BomID, FigureNumber));
            hubConnection.Start().Wait();
            hubProxy.Invoke("sendLogin", UserCode).Wait();
        }

        public void MessageBoxShow(Form form, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            if (form.InvokeRequired)
            {
                form.Invoke(new MethodInvoker(delegate
                {
                    MessageBox.Show(text, caption, buttons, icon);
                    prs_bar.Value = 0;
                }));
            }
            else
            {
                MessageBox.Show(text, caption, buttons, icon);
            }
        }

        private void Show(int BomID, string FigureNumber)
        {
            this.ID = BomID;
            lblFigureNumber.Text = "图号：" + FigureNumber;
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void BtnChooseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                dialog.FileNames.ToList().ForEach(item =>
                {
                    lst_filelist.Items.Add(item);
                });
            }
        }

        private void Btn_UploadFile_Click(object sender, EventArgs e)
        {
            ILog log = LogManager.GetLogger("ErrorName");
            try
            {
                string FtpServer = ConfigInfo.FtpSite;
                string FtpUser = ConfigInfo.FtpUid;
                string FtpPassword = ConfigInfo.FtpPwd;
                FtpWeb ftp = new FtpWeb(FtpServer, "PRS_ProcessFigure", FtpUser, FtpPassword);
                var FileList = lst_filelist.Items.Count;
                if (FileList == 0)
                {
                    MessageBox.Show("请选择需要上传的工艺图纸！", "提示");
                    return;
                }
                if (lblFigureNumber.Text == "图号：")
                {
                    MessageBox.Show("图号为空！", "提示");
                    return;
                }

                new Thread(new ThreadStart(delegate
                {
                    long TotalBytes = 0; var list = new List<string>();
                    foreach (string item in lst_filelist.Items)
                    {
                        FileInfo fileInfo = new FileInfo(item);
                        TotalBytes += fileInfo.Length;
                        list.Add(item);
                    }
                    prs_bar.Value = 0;
                    FtpWeb.file_jd = 0;

                    int id = ID;
                    log.Info("web端传入的PRS_ProcessBom的ID:" + id.ToString());
                    string url = string.Format(@"http://{0}/api/Mms/MES_BN_ProductProcessRoute/GetUpdateProcessFigureIsEnableByProcessBomID?processBomID=" + id, ConfigInfo.API);
                    log.Info("web端改变IsEnable的webapi地址:" + url);
                    string result = Helpers.HttpHelper.GetJSON(url);

                    foreach (string item in list)
                    {
                        string ftp_url = "", file_name = "";
                        string num = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        ftp.Upload(num, prs_bar, TotalBytes, item, out ftp_url, out file_name);
                        string url1 = string.Format(@"http://{0}/api/Mms/Home/PostUpdate3", ConfigInfo.API);
                        log.Info("web端插入上传数据的webapi地址:" + url1);
                        string result1 = Helpers.HttpHelper.PostJSON(url1, new { id = id, docName = file_name, fileName = num, filePath = ftp_url });
                        log.Info("web端插入上传数据的webapi返回内容:" + result1);
                        this.lst_filelist.Items.Remove(item);

                        if (FtpWeb.file_jd == TotalBytes)
                        {
                            hubProxy.Invoke("finishUpload", UserCode).Wait();
                            lblFigureNumber.Text = "图号：";
                            MessageBoxShow(this, "图纸已上传完成!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                }))
                { IsBackground = true }.Start();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

        }

    }
}
