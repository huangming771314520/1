using BlueprintUploadWinformUI.Forms;
using BlueprintUploadWinformUI.Models;
using BlueprintUploadWinformUI.Properties;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Windows.Forms;

namespace BlueprintUploadWinformUI
{
    public partial class MainForm : Form
    {
        private delegate void NoticeDealWithDelegate(string json);

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                StorageInfo.hubConnection = new HubConnection(ConfigInfo.SignalRURI);
                StorageInfo.hubProxy = StorageInfo.hubConnection.CreateHubProxy("NoticeService");
                StorageInfo.hubConnection.Start().Wait();

                StorageInfo.hubProxy.On<string>("OpenClient", (json) => Invoke(new NoticeDealWithDelegate(NoticeDealWith), json));
            }
            catch
            {
                MessageBox.Show(@"未连接到服务，即将退出...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
            }

            lblUserCode.Text = ConfigInfo.LocalKey.Trim();

            picLoading.Image = Resources.loading;
            picLoading.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void BtnUpdateUser_Click(object sender, EventArgs e)
        {
            if (btnUpdateUser.Text.Equals("修改"))
            {
                lblUserCode.Visible = false;
                txtUserCode.Text = ConfigInfo.LocalKey.Trim();
                txtUserCode.Visible = true;
                txtUserCode.Focus();
                btnUpdateUser.Text = @"确定";
            }
            else
            {
                string uCode = txtUserCode.Text.Trim();
                if (!string.IsNullOrEmpty(uCode))
                {
                    ConfigInfo.LocalKey = uCode;
                }
                txtUserCode.Visible = false;
                lblUserCode.Text = ConfigInfo.LocalKey.Trim();
                lblUserCode.Visible = true;
                btnUpdateUser.Text = @"修改";
            }
        }

        public void NoticeDealWith(string json)
        {
            DataModel<object> model = JsonConvert.DeserializeObject<DataModel<object>>(json);

            Form form = new Form();

            switch (model.UploadType)
            {
                case UploadTypeEnum.ProcessBlueprint:
                    ProcessBlueprintModel PBModel = JsonConvert.DeserializeObject<ProcessBlueprintModel>(model.ParamsModel.ToString());
                    if (PBModel.UserCode.Equals(ConfigInfo.LocalKey))
                    {
                        form = new ProcessBlueprintForm(PBModel);
                    }
                    else
                    {
                        return;
                    }
                    break;
                case UploadTypeEnum.PRouteBlueprint:
                    PRouteBlueprintModel PRBModel = JsonConvert.DeserializeObject<PRouteBlueprintModel>(model.ParamsModel.ToString());
                    if (PRBModel.UserCode.Equals(ConfigInfo.LocalKey))
                    {
                        form = new ProcessRouteBlueprintForm(PRBModel);
                    }
                    else
                    {
                        return;
                    }
                    break;
                default:
                    MessageBox.Show(@"参数异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
            }

            Hide();
            form.TopMost = true;
            DialogResult dr = form.ShowDialog();

            switch (dr)
            {
                case DialogResult.OK:
                    MessageBox.Show(@"上传成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    break;
                case DialogResult.No:
                    MessageBox.Show(@"上传失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    break;
                default:

                    break;
            }

            WindowState = FormWindowState.Minimized;
            Show();
        }

    }
}
