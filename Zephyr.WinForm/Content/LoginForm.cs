using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zephyr.WinForm.Content
{
    public partial class LoginForm : Form
    {
        public SkinSharp.SkinH_Net skinh;

        private ActivateTypeEnum ActivateType { get; set; }

        /// <summary>
        /// 启动类型/方式
        /// </summary>
        public enum ActivateTypeEnum
        {
            /// <summary>
            /// 程序正常启动
            /// </summary>
            Normal = 0,
            /// <summary>
            /// 以新身份启动
            /// </summary>
            NewIdentity = 1,
            /// <summary>
            /// 验证身份模态窗口
            /// </summary>
            VerifyIdentity = 2
        }

        public LoginForm(ActivateTypeEnum _activateType)
        {
            skinh = new SkinSharp.SkinH_Net();
            skinh.AttachEx(@"grape.she", string.Empty);

            ActivateType = _activateType;

            InitializeComponent();

            Icon = Properties.Resources.favicon_32;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            if (ActivateType == ActivateTypeEnum.NewIdentity)
            {
                txtWorkGroupID.Text = Program.WorkGroupIDTemp;
                txtWorkGroupID_KeyPress(new object(), new KeyPressEventArgs((char)13));
            }
        }

        private void txtWorkGroupID_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strWorkGroupID = txtWorkGroupID.Text.Trim();
            if (string.IsNullOrEmpty(strWorkGroupID))
            {
                txtWorkGroupID.Focus();
                return;
            }

            if (e.KeyChar.Equals((char)13))
            {
                switch (ActivateType)
                {
                    case ActivateTypeEnum.Normal:
                    case ActivateTypeEnum.NewIdentity:
                        string json = Helpers.HttpHelper.GetJSON("http://" + Program.IP + "/api/Mms/WinFormClient/GetWorkGroupInfoByTCode?teamCode=" + strWorkGroupID);
                        Models.WorkGroupInfoModel result = JsonConvert.DeserializeObject<Models.WorkGroupInfoModel>(json);
                        if (result.result)
                        {
                            Program.WorkGroupInfo = result;
                            DialogResult = DialogResult.OK;
                        }
                        break;
                    case ActivateTypeEnum.VerifyIdentity:
                        if (strWorkGroupID.Equals(Program.WorkGroupInfo.data.GetWorkGroupInfo.TeamCode))
                        {
                            DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            Program.WorkGroupIDTemp = strWorkGroupID;
                            DialogResult = DialogResult.No;
                        }
                        break;
                    default: break;
                }

            }
        }
    }
}
