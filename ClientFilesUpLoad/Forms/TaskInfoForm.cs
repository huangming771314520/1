using ClientFilesUpLoad.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace ClientFilesUpLoad.Forms
{
    public partial class TaskInfoForm : Form
    {
        public TaskInfoModel TaskInfoData = null;//所有生产任务
        public DrawTaskInfoModel DrawTaskInfoData = null;//所有图纸申请任务
        public List<DesignTaskResultModel.DataModel> DesignTaskResultData = new List<DesignTaskResultModel.DataModel>();//设计任务结果类型
        private bool isTaskBindEvent = false;
        private bool isDrawTaskBindEvent = false;

        /// <summary>
        /// 构造函数 初始化窗体
        /// </summary>
        public TaskInfoForm()
        {
            InitializeComponent();

            TaskTableColWidthAuto();
            DrawTaskTableColWidthAuto();
            DoubleBufferDataGridView.DoubleBufferedDataGirdView(dgvTaskTableShow, true);
            DoubleBufferDataGridView.DoubleBufferedDataGirdView(dgvDrawTaskTableShow, true);
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TaskInfoForm_Load(object sender, EventArgs e)
        {
            isTaskBindEvent = false;
            isDrawTaskBindEvent = false;

            TaskInfoData = GetTaskInfoData();
            DesignTaskResultData = GetDesignTaskResultData();
            DrawTaskInfoData = GetDrawTaskInfoData();

            //绑定-是否完成-下拉框选项
            List<object> AchieveTypeData = new List<object>()
            {
                new {Value=0,Text="全部" },
                new {Value=1,Text="未完成" },
                new {Value=2,Text="已完成" }
            };
            cmbAchieveType.DataSource = AchieveTypeData;
            cmbAchieveType.DisplayMember = "Text";
            cmbAchieveType.ValueMember = "Value";

            //下拉框、单选按钮 设置默认值
            cmbAchieveType.SelectedIndex = 1;
            rdbDesignDeptA.Checked = true;
            rdbTaskTypeA.Checked = true;

            //绑定-是否完成-下拉框选项 选择修改事件
            cmbAchieveType.SelectedIndexChanged += new EventHandler(this.CmbAchieveType_SelectedIndexChanged);

            //绑定-任务类型-单选按钮 选择修改事件
            for (int i = 0; i < pnlTaskType.Controls.Count; i++)
            {
                if (pnlTaskType.Controls[i] is RadioButton)
                {
                    ((RadioButton)pnlTaskType.Controls[i]).CheckedChanged += new EventHandler(this.Rdb_CheckedChanged);
                }
            }

            //绑定-设计部门-单选按钮 选择修改事件
            for (int i = 0; i < pnlDesignDept.Controls.Count; i++)
            {
                if (pnlDesignDept.Controls[i] is RadioButton)
                {
                    ((RadioButton)pnlDesignDept.Controls[i]).CheckedChanged += new EventHandler(this.Rdb_CheckedChanged);
                }
            }

            //默认加载数据
            LoadTaskData(string.Empty, string.Empty);
        }

        /// <summary>
        /// 按条件查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string strCCode = txtContractCode.Text.Trim();
            string strPName = txtProductName.Text.Trim();

            if (dgvTaskTableShow.Visible)
            {
                LoadTaskData(strCCode, strPName);
            }

            if (dgvDrawTaskTableShow.Visible)
            {
                LoadDrawTaskData(strCCode, strPName);
            }
        }

        /// <summary>
        /// 单击重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            //txtContractCode.Text = string.Empty;
            //txtProductName.Text = string.Empty;
            //cmbAchieveType.SelectedIndex = 1;
            //rdbTaskTypeA.Checked = true;
            //rdbDesignDeptA.Checked = true;

            //LoadTaskData(string.Empty, string.Empty);

            TaskInfoForm_Load(new object(), new EventArgs());
        }

        /// <summary>
        /// 标记颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSign_Click(object sender, EventArgs e)
        {
            if (dgvTaskTableShow.Visible)
            {
                dgvTaskTableShow.ClearSelection();

                Dictionary<string, Color> setColor = new Dictionary<string, Color>();

                foreach (DataGridViewRow dr in dgvTaskTableShow.Rows)
                {
                    string contractCode = dr.Cells["ContractCode"].Value.ToString();
                    if (setColor.Keys.Contains(contractCode))
                    {
                        dr.DefaultCellStyle.BackColor = setColor[contractCode];
                    }
                    else
                    {
                        Color color = GetColor();
                        setColor.Add(dr.Cells["ContractCode"].Value.ToString(), color);
                        dr.DefaultCellStyle.BackColor = color;
                    }
                }

                if (isTaskBindEvent)
                {
                    dgvTaskTableShow.CellMouseEnter -= new DataGridViewCellEventHandler(dgvTaskTableShow_CellMouseEnter);
                    dgvTaskTableShow.CellMouseLeave -= new DataGridViewCellEventHandler(dgvTaskTableShow_CellMouseLeave);
                    isTaskBindEvent = false;
                }
            }

            if (dgvDrawTaskTableShow.Visible)
            {
                dgvDrawTaskTableShow.ClearSelection();

                Dictionary<string, Color> setColor = new Dictionary<string, Color>();

                foreach (DataGridViewRow dr in dgvDrawTaskTableShow.Rows)
                {
                    string contractCode = dr.Cells["dContractCode"].Value.ToString();
                    if (setColor.Keys.Contains(contractCode))
                    {
                        dr.DefaultCellStyle.BackColor = setColor[contractCode];
                    }
                    else
                    {
                        Color color = GetColor();
                        setColor.Add(dr.Cells["dContractCode"].Value.ToString(), color);
                        dr.DefaultCellStyle.BackColor = color;
                    }
                }

                if (isDrawTaskBindEvent)
                {
                    dgvDrawTaskTableShow.CellMouseEnter -= new DataGridViewCellEventHandler(dgvDrawTaskTableShow_CellMouseEnter);
                    dgvDrawTaskTableShow.CellMouseLeave -= new DataGridViewCellEventHandler(dgvDrawTaskTableShow_CellMouseLeave);
                    isDrawTaskBindEvent = false;
                }
            }
        }


        /// <summary>
        /// 下拉选择框-是否完成-选择修改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbAchieveType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strCCode = txtContractCode.Text.Trim();
            string strPName = txtProductName.Text.Trim();

            if (rdbTaskTypeD.Checked)
            {
                LoadDrawTaskData(strCCode, strPName);
            }
            else
            {
                LoadTaskData(strCCode, strPName);
            }
        }

        /// <summary>
        /// 单选按钮 选择修改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rdb_CheckedChanged(object sender, EventArgs e)
        {
            string strCCode = txtContractCode.Text.Trim();
            string strPName = txtProductName.Text.Trim();

            RadioButton rdb = sender as RadioButton;
            if (rdb.Checked)
            {
                if (rdb.Name.Equals("rdbTaskTypeD"))
                {
                    dgvTaskTableShow.Visible = false;
                    dgvDrawTaskTableShow.Visible = true;

                    LoadDrawTaskData(strCCode, strPName);
                }
                else
                {
                    if (rdbTaskTypeD.Checked)
                    {

                    }
                    else
                    {
                        dgvTaskTableShow.Visible = true;
                        dgvDrawTaskTableShow.Visible = false;

                        LoadTaskData(strCCode, strPName);
                    }
                }
            }
        }

        /// <summary>
        /// 获取随机颜色
        /// </summary>
        /// <returns></returns>
        private Color GetColor()
        {
            Random rd = new Random(Guid.NewGuid().GetHashCode());

            int R = rd.Next(220, 255);
            int G = rd.Next(220, 255);
            int B = rd.Next(220, 255);

            return Color.FromArgb(R, G, B);
        }


        /// <summary>
        /// DataGrigView加载数据
        /// </summary>
        /// <param name="cCode"></param>
        /// <param name="pName"></param>
        public void LoadTaskData(string cCode, string pName)
        {
            if (TaskInfoData.Result)
            {
                dgvTaskTableShow.Rows.Clear();
                int GetAchieveType = Convert.ToInt32(cmbAchieveType.SelectedValue);

                try
                {
                    var data = TaskInfoData.Data.Where(item =>
                    {
                        bool a = string.IsNullOrEmpty(cCode) ? true : item.ContractCode.Equals(cCode);
                        bool b = string.IsNullOrEmpty(pName) ? true : item.ProductName.Contains(pName);
                        bool c = GetAchieveType.Equals(0) ? true : GetAchieveType.Equals(1) ? (item.ActualFinishTime == null ? true : false) : (item.ActualFinishTime != null ? true : false);
                        bool d = rdbTaskTypeA.Checked ? true : rdbTaskTypeB.Checked ? (item.DesignType == null ? false : item.DesignType.Equals(1)) : (item.DesignType == null ? false : item.DesignType.Equals(2));
                        bool e = rdbDesignDeptA.Checked ? true : rdbDesignDeptB.Checked ? (item.DesignDepartmentType == null ? false : item.DesignDepartmentType.Equals("1")) : (item.DesignDepartmentType == null ? false : item.DesignDepartmentType.Equals("2"));
                        return a && b && c && d && e;
                    }).ToList();

                    for (int i = 0; i < data.Count; i++)
                    {
                        DataGridViewRow dgr = new DataGridViewRow();

                        DataGridViewTextBoxCell textboxcellA = new DataGridViewTextBoxCell();
                        textboxcellA.Value = i + 1;
                        DataGridViewTextBoxCell textboxcellB = new DataGridViewTextBoxCell();
                        textboxcellB.Value = data[i].ContractCode;
                        DataGridViewTextBoxCell textboxcellL = new DataGridViewTextBoxCell();
                        textboxcellL.Value = data[i].ProjectName;
                        DataGridViewTextBoxCell textboxcellC = new DataGridViewTextBoxCell();
                        textboxcellC.Value = data[i].ProductName;
                        DataGridViewTextBoxCell textboxcellD = new DataGridViewTextBoxCell();
                        textboxcellD.Value = data[i].Model;
                        DataGridViewTextBoxCell textboxcellE = new DataGridViewTextBoxCell();
                        textboxcellE.Value = data[i].Specifications;
                        DataGridViewTextBoxCell textboxcellF = new DataGridViewTextBoxCell();
                        textboxcellF.Value = data[i].DesignTypeName;
                        DataGridViewTextBoxCell textboxcellG = new DataGridViewTextBoxCell();
                        textboxcellG.Value = data[i].TaskTypeName;
                        DataGridViewTextBoxCell textboxcellH = new DataGridViewTextBoxCell();
                        textboxcellH.Value = data[i].TaskFinishDate == null ? "" : Convert.ToDateTime(data[i].TaskFinishDate.ToString()).ToString("yyyy-MM-dd");
                        DataGridViewTextBoxCell textboxcellI = new DataGridViewTextBoxCell();
                        textboxcellI.Value = data[i].ActualFinishTime;

                        DataGridViewLinkCell linkcell = new DataGridViewLinkCell();
                        linkcell.ActiveLinkColor = Color.FromArgb(201, 223, 77);
                        linkcell.LinkColor = Color.FromArgb(77, 147, 223);
                        linkcell.VisitedLinkColor = Color.FromArgb(132, 222, 78);
                        if (data[i].DesignType != null && data[i].DesignType.Equals(2))
                        {
                            linkcell.Value = "下载文件";
                        }

                        DataGridViewTextBoxCell textboxcellJ = new DataGridViewTextBoxCell();
                        textboxcellJ.Value = data[i].TaskID;
                        DataGridViewTextBoxCell textboxcellK = new DataGridViewTextBoxCell();
                        textboxcellK.Value = data[i].ProjectID;
                        DataGridViewTextBoxCell textboxcellM = new DataGridViewTextBoxCell();
                        textboxcellM.Value = data[i].ProjectDetailID;
                        DataGridViewTextBoxCell textboxcellN = new DataGridViewTextBoxCell();
                        textboxcellN.Value = data[i].DesignType;
                        DataGridViewTextBoxCell textboxcellO = new DataGridViewTextBoxCell();
                        textboxcellO.Value = data[i].BillCode;
                        DataGridViewTextBoxCell textboxcellP = new DataGridViewTextBoxCell();
                        textboxcellP.Value = data[i].FileName;
                        DataGridViewTextBoxCell textboxcellQ = new DataGridViewTextBoxCell();
                        textboxcellQ.Value = data[i].FileAddress;
                        DataGridViewTextBoxCell textboxcellR = new DataGridViewTextBoxCell();
                        textboxcellR.Value = data[i].DocName;
                        DataGridViewTextBoxCell textboxcellS = new DataGridViewTextBoxCell();
                        textboxcellS.Value = data[i].DesignDepartmentType;
                        DataGridViewTextBoxCell textboxcellT = new DataGridViewTextBoxCell();
                        textboxcellT.Value = data[i].DesignDepartmentName;

                        dgr.Cells.Add(textboxcellA);
                        dgr.Cells.Add(textboxcellB);
                        dgr.Cells.Add(textboxcellL);
                        dgr.Cells.Add(textboxcellC);
                        dgr.Cells.Add(textboxcellD);
                        dgr.Cells.Add(textboxcellE);
                        dgr.Cells.Add(textboxcellF);
                        dgr.Cells.Add(textboxcellG);
                        dgr.Cells.Add(textboxcellH);
                        dgr.Cells.Add(textboxcellI);
                        dgr.Cells.Add(linkcell);
                        dgr.Cells.Add(textboxcellJ);
                        dgr.Cells.Add(textboxcellK);
                        dgr.Cells.Add(textboxcellM);
                        dgr.Cells.Add(textboxcellN);
                        dgr.Cells.Add(textboxcellO);
                        dgr.Cells.Add(textboxcellP);
                        dgr.Cells.Add(textboxcellQ);
                        dgr.Cells.Add(textboxcellR);
                        dgr.Cells.Add(textboxcellS);
                        dgr.Cells.Add(textboxcellT);

                        dgr.Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        //dgr.Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        //dgr.Cells[2].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgr.Cells[3].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        //dgr.Cells[4].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgr.Cells[5].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgr.Cells[6].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgr.Cells[7].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgr.Cells[8].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgr.Cells[9].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                        dgr.Height = 36;
                        dgvTaskTableShow.Rows.Add(dgr);
                    }
                }
                catch (Exception ex)
                {
                    var errMsg = ex.Message;
                    MessageBox.Show(errMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(TaskInfoData.Msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            dgvTaskTableShow.ClearSelection();

            if (!isTaskBindEvent)
            {
                dgvTaskTableShow.CellMouseEnter += new DataGridViewCellEventHandler(dgvTaskTableShow_CellMouseEnter);
                dgvTaskTableShow.CellMouseLeave += new DataGridViewCellEventHandler(dgvTaskTableShow_CellMouseLeave);
                isTaskBindEvent = true;
            }
        }

        public void LoadDrawTaskData(string cCode, string pName)
        {
            if (DrawTaskInfoData.Result)
            {
                dgvDrawTaskTableShow.Rows.Clear();
                int GetAchieveType = Convert.ToInt32(cmbAchieveType.SelectedValue);

                try
                {
                    var data = DrawTaskInfoData.Data.Where(item =>
                    {
                        bool a = string.IsNullOrEmpty(cCode) ? true : item.ContractCode.Equals(cCode);
                        bool b = string.IsNullOrEmpty(pName) ? true : item.ProductName.Contains(pName);
                        bool c = GetAchieveType.Equals(0) ? true : (item.RequestStatus == null ? true : GetAchieveType.Equals(item.RequestStatus));
                        return a && b && c;
                    }).ToList();

                    for (int i = 0; i < data.Count; i++)
                    {
                        DataGridViewRow dgr = new DataGridViewRow();

                        DataGridViewTextBoxCell textboxcellA = new DataGridViewTextBoxCell();
                        textboxcellA.Value = i + 1;
                        DataGridViewTextBoxCell textboxcellB = new DataGridViewTextBoxCell();
                        textboxcellB.Value = data[i].RequestCode;
                        DataGridViewTextBoxCell textboxcellL = new DataGridViewTextBoxCell();
                        textboxcellL.Value = data[i].ContractCode;
                        DataGridViewTextBoxCell textboxcellC = new DataGridViewTextBoxCell();
                        textboxcellC.Value = data[i].ProductName;
                        DataGridViewTextBoxCell textboxcellD = new DataGridViewTextBoxCell();
                        textboxcellD.Value = data[i].FigureCode;
                        DataGridViewTextBoxCell textboxcellE = new DataGridViewTextBoxCell();
                        textboxcellE.Value = data[i].PartName;
                        DataGridViewTextBoxCell textboxcellF = new DataGridViewTextBoxCell();
                        textboxcellF.Value = data[i].ApplicationReason;
                        DataGridViewTextBoxCell textboxcellG = new DataGridViewTextBoxCell();
                        textboxcellG.Value = data[i].RequestStatusName;
                        DataGridViewTextBoxCell textboxcellH = new DataGridViewTextBoxCell();
                        textboxcellH.Value = data[i].FileName ?? "";

                        DataGridViewLinkCell linkcell = new DataGridViewLinkCell();
                        linkcell.ActiveLinkColor = Color.FromArgb(201, 223, 77);
                        linkcell.LinkColor = Color.FromArgb(77, 147, 223);
                        linkcell.VisitedLinkColor = Color.FromArgb(132, 222, 78);
                        if (!string.IsNullOrEmpty(data[i].FileAddress))
                        {
                            linkcell.Value = "下载文件";
                        }

                        DataGridViewTextBoxCell textboxcellJ = new DataGridViewTextBoxCell();
                        textboxcellJ.Value = data[i].FileAddress ?? "";
                        DataGridViewTextBoxCell textboxcellK = new DataGridViewTextBoxCell();
                        textboxcellK.Value = data[i].RequestStatus;

                        dgr.Cells.Add(textboxcellA);
                        dgr.Cells.Add(textboxcellB);
                        dgr.Cells.Add(textboxcellL);
                        dgr.Cells.Add(textboxcellC);
                        dgr.Cells.Add(textboxcellD);
                        dgr.Cells.Add(textboxcellE);
                        dgr.Cells.Add(textboxcellF);
                        dgr.Cells.Add(textboxcellG);
                        dgr.Cells.Add(textboxcellH);
                        dgr.Cells.Add(linkcell);
                        dgr.Cells.Add(textboxcellJ);
                        dgr.Cells.Add(textboxcellK);

                        dgr.Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgr.Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgr.Cells[2].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgr.Cells[3].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgr.Cells[4].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgr.Cells[5].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgr.Cells[6].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgr.Cells[7].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgr.Cells[8].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgr.Cells[9].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                        dgr.Height = 36;
                        dgvDrawTaskTableShow.Rows.Add(dgr);
                    }
                }
                catch (Exception ex)
                {
                    var errMsg = ex.Message;
                    MessageBox.Show(errMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(DrawTaskInfoData.Msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            dgvDrawTaskTableShow.ClearSelection();

            if (!isDrawTaskBindEvent)
            {
                dgvDrawTaskTableShow.CellMouseEnter += new DataGridViewCellEventHandler(dgvDrawTaskTableShow_CellMouseEnter);
                dgvDrawTaskTableShow.CellMouseLeave += new DataGridViewCellEventHandler(dgvDrawTaskTableShow_CellMouseLeave);
                isDrawTaskBindEvent = true;
            }
        }


        /// <summary>
        /// 双击表格 - 进入上传文件界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvTaskTableShow_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvTaskTableShow.SelectedRows.Count > 0)
            {
                FilesInfoForm form = new FilesInfoForm(this, dgvTaskTableShow.SelectedRows[0], DesignTaskResultData);
                form.ShowDialog();
            }
        }

        private void dgvDrawTaskTableShow_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDrawTaskTableShow.SelectedRows.Count > 0)
            {
                DrawRequestForm form = new DrawRequestForm(this, dgvDrawTaskTableShow.SelectedRows[0]);
                form.ShowDialog();
            }
        }


        /// <summary>
        /// 单击表格 - 进入文件下载界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvTaskTableShow_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex > 0 && e.RowIndex >= 0)
            {
                if (dgvTaskTableShow.Columns[e.ColumnIndex].HeaderText.Equals("下载"))
                {
                    DataGridViewRow dgr = dgvTaskTableShow.SelectedRows[0];

                    if (dgr.Cells[e.ColumnIndex].Value == null || string.IsNullOrEmpty(dgr.Cells[e.ColumnIndex].Value.ToString()))
                    {
                        return;
                    }

                    if (dgr.Cells[e.ColumnIndex].Value.ToString() == "已下载")
                    {
                        DialogResult dialogResult = MessageBox.Show("文件已下载！\n是否需要重新下载？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                        if (dialogResult == DialogResult.OK)
                        {
                            if (!string.IsNullOrEmpty(dgr.Cells["FileAddress"].Value.ToString()))
                            {
                                //DownLoadForm form = new DownLoadForm(dgr);
                                DownLoadForm form = new DownLoadForm(dgr.Cells["FileAddress"].Value.ToString(), dgr.Cells["FileName"].Value.ToString());
                                DialogResult dr = form.ShowDialog();
                                if (dr == DialogResult.OK)
                                {
                                    dgr.Cells[e.ColumnIndex].Value = "已下载";
                                    (dgr.Cells["DownLoad"] as DataGridViewLinkCell).VisitedLinkColor = Color.FromArgb(132, 222, 78);
                                }
                            }
                            else
                            {
                                MessageBox.Show("找不到文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                (dgr.Cells["DownLoad"] as DataGridViewLinkCell).VisitedLinkColor = Color.FromArgb(255, 128, 0);
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(dgr.Cells["FileAddress"].Value.ToString()))
                        {
                            //DownLoadForm form = new DownLoadForm(dgr);
                            DownLoadForm form = new DownLoadForm(dgr.Cells["FileAddress"].Value.ToString(), dgr.Cells["FileName"].Value.ToString());
                            DialogResult dr = form.ShowDialog();
                            if (dr == DialogResult.OK)
                            {
                                dgr.Cells[e.ColumnIndex].Value = "已下载";
                                (dgr.Cells["DownLoad"] as DataGridViewLinkCell).VisitedLinkColor = Color.FromArgb(132, 222, 78);
                            }
                        }
                        else
                        {
                            MessageBox.Show("找不到文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            (dgr.Cells["DownLoad"] as DataGridViewLinkCell).VisitedLinkColor = Color.FromArgb(255, 128, 0);
                        }
                    }
                    //string str = dgr.Cells["TaskID"].Value.ToString();
                    //string address = dgr.Cells["FileAddress"].Value.ToString();       
                }
            }
        }

        private void dgvDrawTaskTableShow_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex > 0 && e.RowIndex >= 0)
            {
                if (dgvDrawTaskTableShow.Columns[e.ColumnIndex].HeaderText.Equals("操作"))
                {
                    DataGridViewRow dgr = dgvDrawTaskTableShow.SelectedRows[0];

                    if (dgr.Cells[e.ColumnIndex].Value == null || string.IsNullOrEmpty(dgr.Cells[e.ColumnIndex].Value.ToString()))
                    {
                        return;
                    }

                    if (dgr.Cells[e.ColumnIndex].Value.ToString() == "已下载")
                    {
                        DialogResult dialogResult = MessageBox.Show("文件已下载！\n是否需要重新下载？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                        if (dialogResult == DialogResult.OK)
                        {
                            if (!string.IsNullOrEmpty(dgr.Cells["dFileAddress"].Value.ToString()))
                            {
                                //DownLoadForm form = new DownLoadForm(dgr);
                                DownLoadForm form = new DownLoadForm(dgr.Cells["dFileAddress"].Value.ToString(), dgr.Cells["dFileName"].Value.ToString());
                                DialogResult dr = form.ShowDialog();
                                if (dr == DialogResult.OK)
                                {
                                    dgr.Cells[e.ColumnIndex].Value = "已下载";
                                    (dgr.Cells["dDownLoad"] as DataGridViewLinkCell).VisitedLinkColor = Color.FromArgb(132, 222, 78);
                                }
                            }
                            else
                            {
                                MessageBox.Show("找不到文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                (dgr.Cells["dDownLoad"] as DataGridViewLinkCell).VisitedLinkColor = Color.FromArgb(255, 128, 0);
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(dgr.Cells["dFileAddress"].Value.ToString()))
                        {
                            //DownLoadForm form = new DownLoadForm(dgr);
                            DownLoadForm form = new DownLoadForm(dgr.Cells["dFileAddress"].Value.ToString(), dgr.Cells["dFileName"].Value.ToString());
                            DialogResult dr = form.ShowDialog();
                            if (dr == DialogResult.OK)
                            {
                                dgr.Cells[e.ColumnIndex].Value = "已下载";
                                (dgr.Cells["dDownLoad"] as DataGridViewLinkCell).VisitedLinkColor = Color.FromArgb(132, 222, 78);
                            }
                        }
                        else
                        {
                            MessageBox.Show("找不到文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            (dgr.Cells["dDownLoad"] as DataGridViewLinkCell).VisitedLinkColor = Color.FromArgb(255, 128, 0);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 光标移入事件 改变样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvTaskTableShow_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dgvTaskTableShow.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(236, 237, 238);
            }
        }

        private void dgvDrawTaskTableShow_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dgvDrawTaskTableShow.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(236, 237, 238);
            }
        }


        /// <summary>
        /// 光标移出事件 改变样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvTaskTableShow_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.RowIndex % 2 == 0)
                {
                    dgvTaskTableShow.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(245, 245, 247);
                }
                else
                {
                    dgvTaskTableShow.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
                }
            }
        }

        private void dgvDrawTaskTableShow_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.RowIndex % 2 == 0)
                {
                    dgvDrawTaskTableShow.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(245, 245, 247);
                }
                else
                {
                    dgvDrawTaskTableShow.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
                }
            }
        }


        /// <summary>
        /// 窗体大小改变事件 - 触发dgv表格列宽自适应改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvTaskTableShow_SizeChanged(object sender, EventArgs e)
        {
            TaskTableColWidthAuto();
        }

        private void dgvDrawTaskTableShow_SizeChanged(object sender, EventArgs e)
        {
            DrawTaskTableColWidthAuto();
        }


        /// <summary>
        /// 表格 自适应列宽
        /// </summary>
        private void TaskTableColWidthAuto()
        {
            double unit = (dgvTaskTableShow.Width - 24 - 60) / 156.0;

            dgvTaskTableShow.Columns[0].Width = Convert.ToInt32(60);
            dgvTaskTableShow.Columns[1].Width = Convert.ToInt32(unit * 12);
            dgvTaskTableShow.Columns[2].Width = Convert.ToInt32(unit * 20);
            dgvTaskTableShow.Columns[3].Width = Convert.ToInt32(unit * 18);
            dgvTaskTableShow.Columns[4].Width = Convert.ToInt32(unit * 18);
            dgvTaskTableShow.Columns[5].Width = Convert.ToInt32(unit * 10);
            dgvTaskTableShow.Columns[6].Width = Convert.ToInt32(unit * 20);
            dgvTaskTableShow.Columns[7].Width = Convert.ToInt32(unit * 12);
            dgvTaskTableShow.Columns[8].Width = Convert.ToInt32(unit * 18);
            dgvTaskTableShow.Columns[9].Width = Convert.ToInt32(unit * 18);
            dgvTaskTableShow.Columns[10].Width = Convert.ToInt32(unit * 10);
        }

        private void DrawTaskTableColWidthAuto()
        {
            double unit = (dgvDrawTaskTableShow.Width - 24 - 60) / 137.0;

            dgvDrawTaskTableShow.Columns[0].Width = Convert.ToInt32(60);
            dgvDrawTaskTableShow.Columns[1].Width = Convert.ToInt32(unit * 15);
            dgvDrawTaskTableShow.Columns[2].Width = Convert.ToInt32(unit * 14);
            dgvDrawTaskTableShow.Columns[3].Width = Convert.ToInt32(unit * 14);
            dgvDrawTaskTableShow.Columns[4].Width = Convert.ToInt32(unit * 14);
            dgvDrawTaskTableShow.Columns[5].Width = Convert.ToInt32(unit * 15);
            dgvDrawTaskTableShow.Columns[6].Width = Convert.ToInt32(unit * 15);
            dgvDrawTaskTableShow.Columns[7].Width = Convert.ToInt32(unit * 13);
            dgvDrawTaskTableShow.Columns[8].Width = Convert.ToInt32(unit * 24);
            dgvDrawTaskTableShow.Columns[9].Width = Convert.ToInt32(unit * 13);
        }


        /// <summary>
        /// 查所有设计任务
        /// </summary>
        /// <returns></returns>
        public TaskInfoModel GetTaskInfoData()
        {
            try
            {
                string url = string.Format(@"http://{0}/api/Mms/WinFormClient/GetTaskInfo", Program.API);
                return Helpers.HttpHelper.GetTByUrl<TaskInfoModel>(url);
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
                return new TaskInfoModel()
                {
                    Result = false,
                    Msg = errMsg
                };
            }
        }

        /// <summary>
        /// 查所有图纸申请任务
        /// </summary>
        /// <returns></returns>
        public DrawTaskInfoModel GetDrawTaskInfoData()
        {
            try
            {
                string url = string.Format(@"http://{0}/api/Mms/WinFormClient/GetDrawTaskInfo", Program.API);
                return Helpers.HttpHelper.GetTByUrl<DrawTaskInfoModel>(url);
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
                return new DrawTaskInfoModel()
                {
                    Result = false,
                    Msg = errMsg
                };
            }
        }

        /// <summary>
        /// 获取所有设计任务结果
        /// </summary>
        /// <returns></returns>
        public List<DesignTaskResultModel.DataModel> GetDesignTaskResultData()
        {
            try
            {
                string url = string.Format(@"http://{0}/api/Mms/WinFormClient/GetDesignTaskResult", Program.API);
                var result = Helpers.HttpHelper.GetTByUrl<DesignTaskResultModel>(url);

                if (result.Result)
                {
                    result.Data.Insert(0, new DesignTaskResultModel.DataModel() { Text = "请选择类型", Value = "-1" });
                    return result.Data;
                }
                else
                {
                    return new List<DesignTaskResultModel.DataModel>();
                }
            }
            catch (Exception ex)
            {
                return new List<DesignTaskResultModel.DataModel>();
            }
        }

    }

    public static class DoubleBufferDataGridView
    {
        /// <summary>
        /// 双缓冲，解决闪烁问题
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="flag"></param>
        public static void DoubleBufferedDataGirdView(this DataGridView dgv, bool flag)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, flag, null);
        }
    }
}
