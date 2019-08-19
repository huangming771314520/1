using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FtpFileUpload
{
    public partial class ProcessBomList : Form
    {
        public static string PartCode { get; set; }

        //HubConnection hubConnection;
        //IHubProxy hubProxy;
        //private delegate void AddTxt(string color);

        public ProcessBomList()
        {
            //CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();


            //var servercon = ConfigurationManager.AppSettings["SignalRURI"].ToString();
            //hubConnection = new HubConnection(servercon + "/messageHub/hubs");
            //hubProxy = hubConnection.CreateHubProxy("MessageService");
            //hubProxy.On<string>("Touch", (message) => this.Invoke(new AddTxt(Show), message));
            //hubConnection.Start().Wait();
            //hubProxy.Invoke<string>("getUserList");
        }

        private void Show(string color)
        {
            MessageBox.Show("测试成功!" + color);
        }

        private void ProcessBomList_Load(object sender, EventArgs e)
        {
            LoadDataGridView("NothingContent");
        }

        public void LoadDataGridView(string PartCode)
        {
            dgv_bom_list.ColumnCount = 0;
            if (dgv_bom_list.RowCount > 0)
            {
                dgv_bom_list.Rows.Clear();
            }
            string url = string.Format(@"http://{0}/api/Mms/MES_BN_ProductProcessRoute/GetBlueprintPageBomTreeGrid?PartCode=" + PartCode + "&VersionCode=1", Program.API);
            List<dynamic> result = Helpers.HttpHelper.GetTByUrl<List<dynamic>>(url);

            var ColumnList = new Dictionary<string, dynamic>();
            ColumnList.Add("Seq", new { ControlName = "DataGridViewTextBoxColumn", Name = "序号" });
            ColumnList.Add("TuHao", new { ControlName = "DataGridViewTextBoxColumn", Name = "图号" });
            ColumnList.Add("PartName", new { ControlName = "DataGridViewTextBoxColumn", Name = "零件名称" });
            ColumnList.Add("CZ1", new { ControlName = "DataGridViewButtonColumn", Name = "设计图纸下载" });
            ColumnList.Add("CZ2", new { ControlName = "DataGridViewButtonColumn", Name = "工艺图纸上传" });
            ColumnList.Add("ID", new { ControlName = "DataGridViewHiddenColumn", Name = "工艺BOM的主键ID" });
            ColumnList.Add("CZ3", new { ControlName = "DataGridViewButtonColumn", Name = "工艺图纸下载" });
            ColumnList.Add("PFigure_FileAddress", new { ControlName = "DataGridViewHiddenColumn", Name = "工艺图纸地址" });
            ColumnList.Add("PFigure_DocNames", new { ControlName = "DataGridViewHiddenColumn", Name = "工艺图纸名称" });
            ColumnList.Add("PFile_FileAddress", new { ControlName = "DataGridViewHiddenColumn", Name = "设计图纸地址" });
            ColumnList.Add("PFile_DocNames", new { ControlName = "DataGridViewHiddenColumn", Name = "设计图纸名称" });

            //PFigure_DocNames
            ColumnList.ToList().ForEach
                (item =>
                {
                    string Key = item.Key;
                    string ControlName = item.Value.ControlName;
                    string Name = item.Value.Name;
                    DataGridViewColumn column = new DataGridViewColumn();
                    if (ControlName == "DataGridViewTextBoxColumn")
                    {
                        column = new DataGridViewTextBoxColumn();
                    }
                    else if (ControlName == "DataGridViewButtonColumn")
                    {
                        column = new DataGridViewButtonColumn();
                        column.Width = 100;
                    }
                    else if (ControlName == "DataGridViewHiddenColumn")
                    {
                        column = new DataGridViewTextBoxColumn();
                        column.Visible = false;
                    }
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    column.Name = Key;
                    column.DataPropertyName = Key;
                    column.HeaderText = Name;
                    dgv_bom_list.Columns.Add(column);
                });

            int Index = 0;
            foreach (dynamic item in result)
            {
                int ID = item.ID;
                string PFigure_FileAddress = item.PFigure_FileAddress;
                string PFigure_DocNames = item.PFigure_DocNames;
                string PFile_FileAddress = item.PFile_FileAddress;
                string PFile_DocNames = item.PFile_DocNames;
                string PartFigureCode = item.PartFigureCode;
                string PartName = item.PartName;
                DataGridViewRow dgr = new DataGridViewRow();

                DataGridViewTextBoxCell C1 = new DataGridViewTextBoxCell();
                C1.Value = Index + 1;

                DataGridViewTextBoxCell C2 = new DataGridViewTextBoxCell();
                C2.Value = PartFigureCode;

                DataGridViewTextBoxCell C3 = new DataGridViewTextBoxCell();
                C3.Value = PartName;

                DataGridViewButtonCell C4 = new DataGridViewButtonCell();

                string count = PFile_DocNames == null ? "0" : PFile_DocNames.Split('|').Count().ToString();
                C4.Value = "设计图纸下载" + "(" + count + ")";
                C4.Style.BackColor = Color.FromArgb(255, 170, 85);
                C4.Style.Padding = new Padding(1);
                C4.FlatStyle = FlatStyle.Flat;

                DataGridViewButtonCell C5 = new DataGridViewButtonCell();
                C5.Value = "工艺图纸上传";
                C5.Style.BackColor = Color.FromArgb(232, 234, 130);
                C5.Style.Padding = new Padding(1);
                C5.FlatStyle = FlatStyle.Flat;

                DataGridViewButtonCell C6 = new DataGridViewButtonCell();
                C6.Value = ID;

                string count1 = PFigure_DocNames == null ? "0" : PFigure_DocNames.Split('|').Count().ToString();
                DataGridViewButtonCell C7 = new DataGridViewButtonCell();
                C7.Value = "工艺图纸下载" + "(" + count1 + ")";
                C7.Style.BackColor = Color.FromArgb(68, 170, 248);
                C7.Style.Padding = new Padding(1);
                C7.FlatStyle = FlatStyle.Flat;

                DataGridViewButtonCell C8 = new DataGridViewButtonCell();
                C8.Value = PFigure_FileAddress;

                DataGridViewButtonCell C9 = new DataGridViewButtonCell();
                C9.Value = PFigure_DocNames;

                DataGridViewButtonCell C10 = new DataGridViewButtonCell();
                C10.Value = PFile_FileAddress;

                DataGridViewButtonCell C11 = new DataGridViewButtonCell();
                C11.Value = PFile_DocNames;

                Index++;

                dgr.Cells.Add(C1);
                dgr.Cells.Add(C2);
                dgr.Cells.Add(C3);
                dgr.Cells.Add(C4);
                dgr.Cells.Add(C5);
                dgr.Cells.Add(C6);
                dgr.Cells.Add(C7);
                dgr.Cells.Add(C8);
                dgr.Cells.Add(C9);
                dgr.Cells.Add(C10);
                dgr.Cells.Add(C11);

                dgv_bom_list.Rows.Add(dgr);
            }
        }

        private void dgv_bom_list_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex > 0 && e.RowIndex >= 0)
            {
                string header_text = dgv_bom_list.Columns[e.ColumnIndex].HeaderText;
                if (header_text.Equals("设计图纸下载"))
                {
                    var PFile_FileAddress = dgv_bom_list.Rows[e.RowIndex].Cells["PFile_FileAddress"].Value;
                    var PFile_DocNames = dgv_bom_list.Rows[e.RowIndex].Cells["PFile_DocNames"].Value;
                    if (PFile_FileAddress == null)
                    {
                        MessageBox.Show("没有设计图纸可下载！");
                    }
                    else
                    {
                        Form upload = new FileDownLoad(PFile_FileAddress.ToString(), PFile_DocNames.ToString(), "http");
                        upload.StartPosition = FormStartPosition.CenterScreen;
                        upload.ShowDialog();
                    }
                }
                else if (header_text.Equals("工艺图纸上传"))
                {
                    var ID = Convert.ToInt32(dgv_bom_list.Rows[e.RowIndex].Cells["ID"].Value);
                    //Form upload = new FileUpload(ID);
                    Form upload = new FileUpload();
                    upload.StartPosition = FormStartPosition.CenterScreen;
                    upload.ShowDialog();
                    LoadDataGridView(PartCode);
                }
                else if (header_text.Equals("工艺图纸下载"))
                {
                    var PFigure_FileAddress = dgv_bom_list.Rows[e.RowIndex].Cells["PFigure_FileAddress"].Value;
                    var PFigure_DocNames = dgv_bom_list.Rows[e.RowIndex].Cells["PFigure_DocNames"].Value;
                    if (PFigure_FileAddress == null || PFigure_DocNames == null)
                    {
                        MessageBox.Show("尚未上传工艺图纸！");
                    }
                    else
                    {
                        Form upload = new FileDownLoad(PFigure_FileAddress.ToString(), PFigure_DocNames.ToString(), "ftp");
                        upload.StartPosition = FormStartPosition.CenterScreen;
                        upload.ShowDialog();
                        //MessageBox.Show(PFigure_FileAddress.ToString() + "---------" + PFigure_DocNames.ToString());
                    }
                    //MessageBox.Show(PFigure_FileAddress);
                }
            }
        }

        private void Btn_Refresh_Click(object sender, EventArgs e)
        {
            //dgv_bom_list.ColumnCount = 0;
            //dgv_bom_list.Rows.Clear();
            LoadDataGridView(PartCode);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ChooseRootPartCodeList partcode = new ChooseRootPartCodeList();
            partcode.StartPosition = FormStartPosition.CenterScreen;
            partcode.ShowDialog();
            LoadDataGridView(PartCode);
        }
    }
}
