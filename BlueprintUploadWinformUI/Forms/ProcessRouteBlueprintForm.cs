using BlueprintUploadWinformUI;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using BlueprintUploadWinformUI.Helpers;
using BlueprintUploadWinformUI.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace BlueprintUploadWinformUI.Forms
{
    public partial class ProcessRouteBlueprintForm : Form
    {
        private PRouteBlueprintModel parModel;
        private List<string> filePaths = new List<string>();

        FtpHelper ftpHelper = new FtpHelper(ConfigInfo.FtpSite, ConfigInfo.FtpUid, ConfigInfo.FtpPwd)
        {
            RemotePath = ConfigInfo.FtpSiteDicPath
        };

        public ProcessRouteBlueprintForm(PRouteBlueprintModel _parModel)
        {
            parModel = _parModel;

            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void ProcessRouteBlueprintForm_Load(object sender, EventArgs e)
        {

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
                bool result = ftpHelper.UploadMultiFile(filePaths, prgPercent, lvFileInfo);

                //SetUploadInfo();

                if (result)
                {
                    if (SetUploadInfo().Result)
                    {
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        DialogResult = DialogResult.No;
                    }
                }
                else
                {
                    DialogResult = DialogResult.No;
                }
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
            StorageInfo.hubProxy.Invoke("finishUpload", JsonConvert.SerializeObject(new
            {
                UserCode = parModel.UserCode,
                UploadType = 2,
                Result = result.Result,
                Data = new
                {
                    Files = result.Data,
                    SourceID = tempID
                },
                Msg = result.Msg
            }));

            return result;
        }


    }
}
