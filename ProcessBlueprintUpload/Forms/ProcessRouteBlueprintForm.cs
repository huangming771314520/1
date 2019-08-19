using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using ProcessBlueprintUpload.Helpers;
using ProcessBlueprintUpload.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace ProcessBlueprintUpload.Forms
{
    public partial class ProcessRouteBlueprintForm : Form
    {
        private delegate void NoticeDealWithDelegate(string json);
        private static HubConnection hubConnection = new HubConnection(string.Format(@"http://{0}/NoticeHub/hubs", ConfigInfo.API));
        private IHubProxy hubProxy = hubConnection.CreateHubProxy("NoticeService");

        private PRouteBlueprintParamsModel parModel;
        private List<string> filePaths = new List<string>();

        FtpHelper ftpHelper = new FtpHelper(ConfigInfo.FtpSite, ConfigInfo.FtpUid, ConfigInfo.FtpPwd)
        {
            RemotePath = ConfigInfo.FtpSiteDicPath
        };

        public ProcessRouteBlueprintForm()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();

            hubConnection.Start().Wait();
            hubProxy.On<string>("Touch", (json) => Invoke(new NoticeDealWithDelegate(NoticeDealWith), json));
        }

        private void ProcessRouteBlueprintForm_Load(object sender, EventArgs e)
        {

        }

        private void NoticeDealWith(string json)
        {
            var tempModel = JsonConvert.DeserializeObject<PRouteBlueprintParamsModel>(json);

            if (tempModel.UserCode.Equals(ConfigInfo.LocalKey))
            {
                parModel = tempModel;
                WindowState = FormWindowState.Normal;
                Activate();
            }
        }

        private void BtnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Multiselect = true
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                filePaths.Clear();
                lvFileInfo.Items.Clear();

                for (int i = 0; i < dlg.FileNames.Length; i++)
                {
                    ListViewItem lvi = new ListViewItem((i + 1).ToString());
                    lvi.SubItems.Add(dlg.SafeFileNames[i]);
                    lvi.SubItems.Add(UpLoadStateEnum.未开始.ToString());

                    lvi.BackColor = i % 2 == 0 ? Color.FromArgb(252, 252, 252) : Color.FromArgb(245, 245, 245);
                    lvi.Tag = dlg.FileNames[i];

                    lvFileInfo.Items.Add(lvi);

                    filePaths.Add(dlg.FileNames[i]);
                }
            }
        }

        private void BtnStartUpload_Click(object sender, EventArgs e)
        {
            prgPercent.Value = 0;

            if (lvFileInfo.Items.Count <= 0)
            {
                return;
            }

            if (parModel == null)
            {
                MessageBox.Show(@"请在浏览器相关页面点击上传按钮！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnSelectFile.Enabled = false;
            btnStartUpload.Enabled = false;

            new Thread(new ThreadStart(delegate
            {
                bool isOK = true;

                for (int i = 0; i < lvFileInfo.Items.Count; i++)
                {
                    lvFileInfo.Items[i].SubItems[2].Text = UpLoadStateEnum.上传中.ToString();

                    string filePath = lvFileInfo.Items[i].Tag.ToString();
                    string fileName = Path.GetFileName(filePath);

                    FileInfo fileInfo = new FileInfo(filePath);
                    bool result = ftpHelper.Upload(fileInfo, fileName, prgPercent, (double)(i + 1) / filePaths.Count);

                    if (result)
                    {
                        lvFileInfo.Items[i].SubItems[2].Text = UpLoadStateEnum.上传成功.ToString();
                        lvFileInfo.Items[i].SubItems[2].BackColor = Color.FromArgb(240, 255, 240);
                    }
                    else
                    {
                        isOK = false;
                        lvFileInfo.Items[i].SubItems[2].Text = UpLoadStateEnum.上传失败.ToString();
                        lvFileInfo.Items[i].SubItems[2].BackColor = Color.FromArgb(255, 240, 240);
                    }
                }

                prgPercent.Value = 100;

                SetUploadInfo();

                Invoke(new MethodInvoker(delegate
                {
                    if (isOK)
                    {
                        MessageBox.Show(@"上传成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    else
                    {
                        DialogResult dr = MessageBox.Show("上传完成，但是有错误！\n需要重试吗？", "提示", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question);
                        if (dr == DialogResult.Retry)
                        {
                            BtnStartUpload_Click(new object(), new EventArgs());
                        }
                    }

                    btnSelectFile.Enabled = true;
                    btnStartUpload.Enabled = true;
                }));


            }))
            { IsBackground = true }.Start();
        }

        public ResultModel SetUploadInfo()
        {
            List<object> fileInfos = new List<object>();
            string strDT = DateTime.Now.ToString("yyyyMMddHHmmss");
            Random random = new Random(Guid.NewGuid().GetHashCode());
            string tempID = ((long)(DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1))).TotalSeconds).ToString();

            foreach (var item in filePaths)
            {
                string fileName = Path.GetFileName(item);
                string fileSitePath = string.Format(@"ftp://{0}/{1}{2}", ConfigInfo.FtpSite, ConfigInfo.FtpSiteDicPath, fileName);

                fileInfos.Add(new
                {
                    FileName = strDT + random.Next(10000, 100000).ToString() + Path.GetExtension(item),
                    FileAddress = fileSitePath,
                    DocName = fileName
                });
            }

            string url = string.Format(@"http://{0}/api/Mms/MES_BN_ProductProcessRoute/PostUpLoadPRouteBlueprint", ConfigInfo.API);

            ResultModel result = HttpHelper.PostTByUrl<ResultModel>(url, new
            {
                Files = fileInfos,
                User = new
                {
                    UserCode = parModel.UserCode,
                    UserName = parModel.UserName
                },
                SourceID = parModel.ID,
                TempID = tempID,
                Type = (int)parModel.Type
            });

            result.Data = fileInfos;
            hubProxy.Invoke("finishUpload", JsonConvert.SerializeObject(new
            {
                UserCode = parModel.UserCode,
                Result = result.Result,
                Data = new
                {
                    Files = result.Data,
                    SourceID = tempID
                },
                Msg = result.Msg
            })).Wait();

            return result;
        }


    }
}
