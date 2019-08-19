using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Update.Lib;
using Update.Model;

namespace Update.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            lblNewVersionCode.Text = UpdateInfo.AppVersion.ToString();
            lblUpdateTime.Text = UpdateInfo.UpdateTime.ToString("yyyy-MM-dd");
            rtxUpdateContent.Text = Regex.Unescape(UpdateInfo.Describe);
        }

        private void BtnStartUpdate_Click(object sender, EventArgs e)
        {
            Updater.StartUpdate();

            Application.Exit();
        }
    }
}
