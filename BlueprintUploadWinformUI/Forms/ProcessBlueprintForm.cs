using Microsoft.AspNet.SignalR.Client;
using BlueprintUploadWinformUI.Helpers;
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
using BlueprintUploadWinformUI.Models;
using Newtonsoft.Json;

namespace BlueprintUploadWinformUI.Forms
{
    public partial class ProcessBlueprintForm : Form
    {
        private ProcessBlueprintModel parModel;

        public ProcessBlueprintForm(ProcessBlueprintModel _parModel)
        {
            parModel = _parModel;

            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void ProcessBlueprintForm_Load(object sender, EventArgs e)
        {
            lblFigureNumber.Text = parModel.PartFigureCode;
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
            try
            {
                string FtpServer = ConfigInfo.FtpSite;
                string FtpUser = ConfigInfo.FtpUid;
                string FtpPassword = ConfigInfo.FtpPwd;
                FtpWeb ftp = new FtpWeb(FtpServer, ConfigInfo.FtpSiteDicPath.Trim('/'), FtpUser, FtpPassword);
                var FileList = lst_filelist.Items.Count;
                if (FileList == 0)
                {
                    MessageBox.Show("请选择需要上传的工艺图纸！", "提示");
                    return;
                }
                if (string.IsNullOrEmpty(lblFigureNumber.Text))
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

                    string url = string.Format(@"http://{0}/api/Mms/MES_BN_ProductProcessRoute/GetUpdateProcessFigureIsEnableByProcessBomID?processBomID=" + parModel.BomID, ConfigInfo.API);
                    string result = Helpers.HttpHelper.GetJSON(url);

                    foreach (string item in list)
                    {
                        string ftp_url = "", file_name = "";
                        string num = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        ftp.Upload(num, prs_bar, TotalBytes, item, out ftp_url, out file_name);
                        string url1 = string.Format(@"http://{0}/api/Mms/Home/PostUpdate3", ConfigInfo.API);
                        string result1 = Helpers.HttpHelper.PostJSON(url1, new { id = parModel.BomID, docName = file_name, fileName = num, filePath = ftp_url });
                        this.lst_filelist.Items.Remove(item);

                        if (FtpWeb.file_jd == TotalBytes)
                        {
                            StorageInfo.hubProxy.Invoke("finishUpload", JsonConvert.SerializeObject(new
                            {
                                UserCode = parModel.UserCode,
                                UploadType = 1,
                                Result = true,
                                Data = new
                                {

                                },
                                Msg = @"图纸已上传完成!"
                            }));

                            lblFigureNumber.Text = string.Empty;

                            DialogResult = DialogResult.OK;
                        }
                    }
                }))
                { IsBackground = true }.Start();

            }
            catch (Exception ex)
            {
                DialogResult = DialogResult.No;
            }

        }

    }
}
