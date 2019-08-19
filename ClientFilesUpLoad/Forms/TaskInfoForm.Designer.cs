namespace ClientFilesUpLoad.Forms
{
    partial class TaskInfoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskInfoForm));
            this.dgvTaskTableShow = new System.Windows.Forms.DataGridView();
            this.Num = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContractCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Model = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Specifications = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DesignTypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskTypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskFinishDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActualFinishTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DownLoad = new System.Windows.Forms.DataGridViewLinkColumn();
            this.TaskID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectDetailID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DesignType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BillCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DesignDepartmentType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DesignDepartmentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpSearchContent = new System.Windows.Forms.GroupBox();
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.pnlDesignDept = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.rdbDesignDeptA = new System.Windows.Forms.RadioButton();
            this.rdbDesignDeptB = new System.Windows.Forms.RadioButton();
            this.rdbDesignDeptC = new System.Windows.Forms.RadioButton();
            this.pnlTaskType = new System.Windows.Forms.Panel();
            this.rdbTaskTypeD = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.rdbTaskTypeB = new System.Windows.Forms.RadioButton();
            this.rdbTaskTypeA = new System.Windows.Forms.RadioButton();
            this.rdbTaskTypeC = new System.Windows.Forms.RadioButton();
            this.cmbAchieveType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSign = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtProductName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtContractCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlTaskTableContainer = new System.Windows.Forms.Panel();
            this.dgvDrawTaskTableShow = new System.Windows.Forms.DataGridView();
            this.dNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dRequestCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dContractCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dFigureCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dPartName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dApplicationReason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dRequestStatusName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dDownLoad = new System.Windows.Forms.DataGridViewLinkColumn();
            this.dFileAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dRequestStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTaskTableShow)).BeginInit();
            this.grpSearchContent.SuspendLayout();
            this.pnlContainer.SuspendLayout();
            this.pnlDesignDept.SuspendLayout();
            this.pnlTaskType.SuspendLayout();
            this.pnlTaskTableContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDrawTaskTableShow)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvTaskTableShow
            // 
            this.dgvTaskTableShow.AllowUserToAddRows = false;
            this.dgvTaskTableShow.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.dgvTaskTableShow.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTaskTableShow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTaskTableShow.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.dgvTaskTableShow.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTaskTableShow.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(115)))), ((int)(((byte)(115)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(237)))), ((int)(((byte)(238)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTaskTableShow.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvTaskTableShow.ColumnHeadersHeight = 32;
            this.dgvTaskTableShow.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Num,
            this.ContractCode,
            this.ProjectName,
            this.ProductName,
            this.Model,
            this.Specifications,
            this.DesignTypeName,
            this.TaskTypeName,
            this.TaskFinishDate,
            this.ActualFinishTime,
            this.DownLoad,
            this.TaskID,
            this.ProjectID,
            this.ProjectDetailID,
            this.DesignType,
            this.BillCode,
            this.FileName,
            this.FileAddress,
            this.DocName,
            this.DesignDepartmentType,
            this.DesignDepartmentName});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(115)))), ((int)(((byte)(115)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(237)))), ((int)(((byte)(238)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTaskTableShow.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvTaskTableShow.EnableHeadersVisualStyles = false;
            this.dgvTaskTableShow.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.dgvTaskTableShow.Location = new System.Drawing.Point(0, 0);
            this.dgvTaskTableShow.Margin = new System.Windows.Forms.Padding(0);
            this.dgvTaskTableShow.MultiSelect = false;
            this.dgvTaskTableShow.Name = "dgvTaskTableShow";
            this.dgvTaskTableShow.ReadOnly = true;
            this.dgvTaskTableShow.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(115)))), ((int)(((byte)(115)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(237)))), ((int)(((byte)(238)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTaskTableShow.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvTaskTableShow.RowHeadersVisible = false;
            this.dgvTaskTableShow.RowHeadersWidth = 51;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(237)))), ((int)(((byte)(238)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.dgvTaskTableShow.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvTaskTableShow.RowTemplate.Height = 36;
            this.dgvTaskTableShow.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTaskTableShow.Size = new System.Drawing.Size(1222, 417);
            this.dgvTaskTableShow.TabIndex = 0;
            this.dgvTaskTableShow.TabStop = false;
            this.dgvTaskTableShow.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTaskTableShow_CellDoubleClick);
            this.dgvTaskTableShow.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvTaskTableShow_CellMouseClick);
            this.dgvTaskTableShow.SizeChanged += new System.EventHandler(this.dgvTaskTableShow_SizeChanged);
            // 
            // Num
            // 
            this.Num.DataPropertyName = "Num";
            this.Num.HeaderText = "序号";
            this.Num.MinimumWidth = 6;
            this.Num.Name = "Num";
            this.Num.ReadOnly = true;
            this.Num.Width = 121;
            // 
            // ContractCode
            // 
            this.ContractCode.DataPropertyName = "ContractCode";
            this.ContractCode.HeaderText = "合同编号";
            this.ContractCode.MinimumWidth = 6;
            this.ContractCode.Name = "ContractCode";
            this.ContractCode.ReadOnly = true;
            this.ContractCode.Width = 121;
            // 
            // ProjectName
            // 
            this.ProjectName.DataPropertyName = "ProjectName";
            this.ProjectName.HeaderText = "工程项目";
            this.ProjectName.MinimumWidth = 6;
            this.ProjectName.Name = "ProjectName";
            this.ProjectName.ReadOnly = true;
            this.ProjectName.Width = 125;
            // 
            // ProductName
            // 
            this.ProductName.DataPropertyName = "ProductName";
            this.ProductName.HeaderText = "产品名称";
            this.ProductName.MinimumWidth = 6;
            this.ProductName.Name = "ProductName";
            this.ProductName.ReadOnly = true;
            this.ProductName.Width = 121;
            // 
            // Model
            // 
            this.Model.DataPropertyName = "Model";
            this.Model.HeaderText = "型号";
            this.Model.MinimumWidth = 6;
            this.Model.Name = "Model";
            this.Model.ReadOnly = true;
            this.Model.Width = 121;
            // 
            // Specifications
            // 
            this.Specifications.DataPropertyName = "Specifications";
            this.Specifications.HeaderText = "规格";
            this.Specifications.MinimumWidth = 6;
            this.Specifications.Name = "Specifications";
            this.Specifications.ReadOnly = true;
            this.Specifications.Width = 120;
            // 
            // DesignTypeName
            // 
            this.DesignTypeName.DataPropertyName = "DesignTypeName";
            this.DesignTypeName.HeaderText = "类型";
            this.DesignTypeName.MinimumWidth = 6;
            this.DesignTypeName.Name = "DesignTypeName";
            this.DesignTypeName.ReadOnly = true;
            this.DesignTypeName.Width = 125;
            // 
            // TaskTypeName
            // 
            this.TaskTypeName.DataPropertyName = "TaskTypeName";
            this.TaskTypeName.HeaderText = "任务类型";
            this.TaskTypeName.MinimumWidth = 6;
            this.TaskTypeName.Name = "TaskTypeName";
            this.TaskTypeName.ReadOnly = true;
            this.TaskTypeName.Width = 121;
            // 
            // TaskFinishDate
            // 
            this.TaskFinishDate.DataPropertyName = "TaskFinishDate";
            this.TaskFinishDate.HeaderText = "结束日期";
            this.TaskFinishDate.MinimumWidth = 6;
            this.TaskFinishDate.Name = "TaskFinishDate";
            this.TaskFinishDate.ReadOnly = true;
            this.TaskFinishDate.Width = 121;
            // 
            // ActualFinishTime
            // 
            this.ActualFinishTime.DataPropertyName = "ActualFinishTime";
            this.ActualFinishTime.HeaderText = "实际结束日期";
            this.ActualFinishTime.MinimumWidth = 6;
            this.ActualFinishTime.Name = "ActualFinishTime";
            this.ActualFinishTime.ReadOnly = true;
            this.ActualFinishTime.Width = 121;
            // 
            // DownLoad
            // 
            this.DownLoad.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(223)))), ((int)(((byte)(77)))));
            this.DownLoad.HeaderText = "下载";
            this.DownLoad.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(147)))), ((int)(((byte)(223)))));
            this.DownLoad.MinimumWidth = 6;
            this.DownLoad.Name = "DownLoad";
            this.DownLoad.ReadOnly = true;
            this.DownLoad.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.DownLoad.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.DownLoad.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(222)))), ((int)(((byte)(78)))));
            this.DownLoad.Width = 125;
            // 
            // TaskID
            // 
            this.TaskID.DataPropertyName = "TaskID";
            this.TaskID.HeaderText = "TaskID";
            this.TaskID.MinimumWidth = 6;
            this.TaskID.Name = "TaskID";
            this.TaskID.ReadOnly = true;
            this.TaskID.Visible = false;
            this.TaskID.Width = 125;
            // 
            // ProjectID
            // 
            this.ProjectID.DataPropertyName = "ProjectID";
            this.ProjectID.HeaderText = "ProjectID";
            this.ProjectID.MinimumWidth = 6;
            this.ProjectID.Name = "ProjectID";
            this.ProjectID.ReadOnly = true;
            this.ProjectID.Visible = false;
            this.ProjectID.Width = 125;
            // 
            // ProjectDetailID
            // 
            this.ProjectDetailID.DataPropertyName = "ProjectDetailID";
            this.ProjectDetailID.HeaderText = "ProjectDetailID";
            this.ProjectDetailID.MinimumWidth = 6;
            this.ProjectDetailID.Name = "ProjectDetailID";
            this.ProjectDetailID.ReadOnly = true;
            this.ProjectDetailID.Visible = false;
            this.ProjectDetailID.Width = 125;
            // 
            // DesignType
            // 
            this.DesignType.DataPropertyName = "DesignType";
            this.DesignType.HeaderText = "DesignType";
            this.DesignType.MinimumWidth = 6;
            this.DesignType.Name = "DesignType";
            this.DesignType.ReadOnly = true;
            this.DesignType.Visible = false;
            this.DesignType.Width = 125;
            // 
            // BillCode
            // 
            this.BillCode.DataPropertyName = "BillCode";
            this.BillCode.HeaderText = "BillCode";
            this.BillCode.MinimumWidth = 6;
            this.BillCode.Name = "BillCode";
            this.BillCode.ReadOnly = true;
            this.BillCode.Visible = false;
            this.BillCode.Width = 125;
            // 
            // FileName
            // 
            this.FileName.DataPropertyName = "FileName";
            this.FileName.HeaderText = "FileName";
            this.FileName.MinimumWidth = 6;
            this.FileName.Name = "FileName";
            this.FileName.ReadOnly = true;
            this.FileName.Visible = false;
            this.FileName.Width = 125;
            // 
            // FileAddress
            // 
            this.FileAddress.DataPropertyName = "FileAddress";
            this.FileAddress.HeaderText = "FileAddress";
            this.FileAddress.MinimumWidth = 6;
            this.FileAddress.Name = "FileAddress";
            this.FileAddress.ReadOnly = true;
            this.FileAddress.Visible = false;
            this.FileAddress.Width = 125;
            // 
            // DocName
            // 
            this.DocName.DataPropertyName = "DocName";
            this.DocName.HeaderText = "DocName";
            this.DocName.MinimumWidth = 6;
            this.DocName.Name = "DocName";
            this.DocName.ReadOnly = true;
            this.DocName.Visible = false;
            this.DocName.Width = 125;
            // 
            // DesignDepartmentType
            // 
            this.DesignDepartmentType.DataPropertyName = "DesignDepartmentType";
            this.DesignDepartmentType.HeaderText = "DesignDepartmentType";
            this.DesignDepartmentType.MinimumWidth = 6;
            this.DesignDepartmentType.Name = "DesignDepartmentType";
            this.DesignDepartmentType.ReadOnly = true;
            this.DesignDepartmentType.Visible = false;
            this.DesignDepartmentType.Width = 125;
            // 
            // DesignDepartmentName
            // 
            this.DesignDepartmentName.DataPropertyName = "DesignDepartmentName";
            this.DesignDepartmentName.HeaderText = "DesignDepartmentName";
            this.DesignDepartmentName.MinimumWidth = 6;
            this.DesignDepartmentName.Name = "DesignDepartmentName";
            this.DesignDepartmentName.ReadOnly = true;
            this.DesignDepartmentName.Visible = false;
            this.DesignDepartmentName.Width = 125;
            // 
            // grpSearchContent
            // 
            this.grpSearchContent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSearchContent.Controls.Add(this.pnlContainer);
            this.grpSearchContent.Location = new System.Drawing.Point(9, 9);
            this.grpSearchContent.Margin = new System.Windows.Forms.Padding(0);
            this.grpSearchContent.Name = "grpSearchContent";
            this.grpSearchContent.Padding = new System.Windows.Forms.Padding(0);
            this.grpSearchContent.Size = new System.Drawing.Size(1222, 221);
            this.grpSearchContent.TabIndex = 1;
            this.grpSearchContent.TabStop = false;
            // 
            // pnlContainer
            // 
            this.pnlContainer.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pnlContainer.Controls.Add(this.pnlDesignDept);
            this.pnlContainer.Controls.Add(this.pnlTaskType);
            this.pnlContainer.Controls.Add(this.cmbAchieveType);
            this.pnlContainer.Controls.Add(this.label3);
            this.pnlContainer.Controls.Add(this.btnSign);
            this.pnlContainer.Controls.Add(this.btnReset);
            this.pnlContainer.Controls.Add(this.btnSearch);
            this.pnlContainer.Controls.Add(this.txtProductName);
            this.pnlContainer.Controls.Add(this.label2);
            this.pnlContainer.Controls.Add(this.txtContractCode);
            this.pnlContainer.Controls.Add(this.label1);
            this.pnlContainer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.pnlContainer.Location = new System.Drawing.Point(81, 20);
            this.pnlContainer.Margin = new System.Windows.Forms.Padding(0);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Size = new System.Drawing.Size(1067, 189);
            this.pnlContainer.TabIndex = 0;
            // 
            // pnlDesignDept
            // 
            this.pnlDesignDept.Controls.Add(this.label5);
            this.pnlDesignDept.Controls.Add(this.rdbDesignDeptA);
            this.pnlDesignDept.Controls.Add(this.rdbDesignDeptB);
            this.pnlDesignDept.Controls.Add(this.rdbDesignDeptC);
            this.pnlDesignDept.Location = new System.Drawing.Point(683, 50);
            this.pnlDesignDept.Margin = new System.Windows.Forms.Padding(0);
            this.pnlDesignDept.Name = "pnlDesignDept";
            this.pnlDesignDept.Size = new System.Drawing.Size(356, 40);
            this.pnlDesignDept.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 20);
            this.label5.TabIndex = 10;
            this.label5.Text = "设计部门：";
            // 
            // rdbDesignDeptA
            // 
            this.rdbDesignDeptA.AutoSize = true;
            this.rdbDesignDeptA.Location = new System.Drawing.Point(112, 8);
            this.rdbDesignDeptA.Margin = new System.Windows.Forms.Padding(0);
            this.rdbDesignDeptA.Name = "rdbDesignDeptA";
            this.rdbDesignDeptA.Size = new System.Drawing.Size(60, 24);
            this.rdbDesignDeptA.TabIndex = 8;
            this.rdbDesignDeptA.TabStop = true;
            this.rdbDesignDeptA.Text = "全部";
            this.rdbDesignDeptA.UseVisualStyleBackColor = true;
            // 
            // rdbDesignDeptB
            // 
            this.rdbDesignDeptB.AutoSize = true;
            this.rdbDesignDeptB.Location = new System.Drawing.Point(179, 8);
            this.rdbDesignDeptB.Margin = new System.Windows.Forms.Padding(0);
            this.rdbDesignDeptB.Name = "rdbDesignDeptB";
            this.rdbDesignDeptB.Size = new System.Drawing.Size(75, 24);
            this.rdbDesignDeptB.TabIndex = 8;
            this.rdbDesignDeptB.TabStop = true;
            this.rdbDesignDeptB.Text = "设计院";
            this.rdbDesignDeptB.UseVisualStyleBackColor = true;
            // 
            // rdbDesignDeptC
            // 
            this.rdbDesignDeptC.AutoSize = true;
            this.rdbDesignDeptC.Location = new System.Drawing.Point(261, 8);
            this.rdbDesignDeptC.Margin = new System.Windows.Forms.Padding(0);
            this.rdbDesignDeptC.Name = "rdbDesignDeptC";
            this.rdbDesignDeptC.Size = new System.Drawing.Size(75, 24);
            this.rdbDesignDeptC.TabIndex = 9;
            this.rdbDesignDeptC.TabStop = true;
            this.rdbDesignDeptC.Text = "新品部";
            this.rdbDesignDeptC.UseVisualStyleBackColor = true;
            // 
            // pnlTaskType
            // 
            this.pnlTaskType.Controls.Add(this.rdbTaskTypeD);
            this.pnlTaskType.Controls.Add(this.label4);
            this.pnlTaskType.Controls.Add(this.rdbTaskTypeB);
            this.pnlTaskType.Controls.Add(this.rdbTaskTypeA);
            this.pnlTaskType.Controls.Add(this.rdbTaskTypeC);
            this.pnlTaskType.Location = new System.Drawing.Point(388, 12);
            this.pnlTaskType.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTaskType.Name = "pnlTaskType";
            this.pnlTaskType.Size = new System.Drawing.Size(271, 130);
            this.pnlTaskType.TabIndex = 12;
            // 
            // rdbTaskTypeD
            // 
            this.rdbTaskTypeD.AutoSize = true;
            this.rdbTaskTypeD.Location = new System.Drawing.Point(99, 97);
            this.rdbTaskTypeD.Name = "rdbTaskTypeD";
            this.rdbTaskTypeD.Size = new System.Drawing.Size(90, 24);
            this.rdbTaskTypeD.TabIndex = 11;
            this.rdbTaskTypeD.TabStop = true;
            this.rdbTaskTypeD.Text = "图纸申请";
            this.rdbTaskTypeD.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 20);
            this.label4.TabIndex = 10;
            this.label4.Text = "任务类型：";
            // 
            // rdbTaskTypeB
            // 
            this.rdbTaskTypeB.AutoSize = true;
            this.rdbTaskTypeB.Location = new System.Drawing.Point(99, 40);
            this.rdbTaskTypeB.Margin = new System.Windows.Forms.Padding(0);
            this.rdbTaskTypeB.Name = "rdbTaskTypeB";
            this.rdbTaskTypeB.Size = new System.Drawing.Size(150, 24);
            this.rdbTaskTypeB.TabIndex = 8;
            this.rdbTaskTypeB.TabStop = true;
            this.rdbTaskTypeB.Text = "新建项目设计任务";
            this.rdbTaskTypeB.UseVisualStyleBackColor = true;
            // 
            // rdbTaskTypeA
            // 
            this.rdbTaskTypeA.AutoSize = true;
            this.rdbTaskTypeA.Location = new System.Drawing.Point(99, 11);
            this.rdbTaskTypeA.Margin = new System.Windows.Forms.Padding(0);
            this.rdbTaskTypeA.Name = "rdbTaskTypeA";
            this.rdbTaskTypeA.Size = new System.Drawing.Size(60, 24);
            this.rdbTaskTypeA.TabIndex = 8;
            this.rdbTaskTypeA.TabStop = true;
            this.rdbTaskTypeA.Text = "全部";
            this.rdbTaskTypeA.UseVisualStyleBackColor = true;
            // 
            // rdbTaskTypeC
            // 
            this.rdbTaskTypeC.AutoSize = true;
            this.rdbTaskTypeC.Location = new System.Drawing.Point(99, 69);
            this.rdbTaskTypeC.Margin = new System.Windows.Forms.Padding(0);
            this.rdbTaskTypeC.Name = "rdbTaskTypeC";
            this.rdbTaskTypeC.Size = new System.Drawing.Size(150, 24);
            this.rdbTaskTypeC.TabIndex = 9;
            this.rdbTaskTypeC.TabStop = true;
            this.rdbTaskTypeC.Text = "设计更改申请任务";
            this.rdbTaskTypeC.UseVisualStyleBackColor = true;
            // 
            // cmbAchieveType
            // 
            this.cmbAchieveType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.cmbAchieveType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAchieveType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.cmbAchieveType.FormattingEnabled = true;
            this.cmbAchieveType.Items.AddRange(new object[] {
            "全部",
            "已完成",
            "未完成"});
            this.cmbAchieveType.Location = new System.Drawing.Point(162, 105);
            this.cmbAchieveType.Margin = new System.Windows.Forms.Padding(0);
            this.cmbAchieveType.Name = "cmbAchieveType";
            this.cmbAchieveType.Size = new System.Drawing.Size(200, 28);
            this.cmbAchieveType.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(52, 108);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "是否完成：";
            // 
            // btnSign
            // 
            this.btnSign.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(234)))), ((int)(((byte)(130)))));
            this.btnSign.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSign.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSign.FlatAppearance.BorderSize = 0;
            this.btnSign.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(225)))), ((int)(((byte)(79)))));
            this.btnSign.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(230)))), ((int)(((byte)(100)))));
            this.btnSign.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSign.ForeColor = System.Drawing.Color.White;
            this.btnSign.Location = new System.Drawing.Point(589, 159);
            this.btnSign.Margin = new System.Windows.Forms.Padding(0);
            this.btnSign.Name = "btnSign";
            this.btnSign.Size = new System.Drawing.Size(100, 30);
            this.btnSign.TabIndex = 4;
            this.btnSign.Text = "标记";
            this.btnSign.UseVisualStyleBackColor = false;
            this.btnSign.Click += new System.EventHandler(this.btnSign_Click);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(170)))), ((int)(((byte)(85)))));
            this.btnReset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReset.FlatAppearance.BorderSize = 0;
            this.btnReset.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(150)))), ((int)(((byte)(45)))));
            this.btnReset.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(164)))), ((int)(((byte)(72)))));
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Location = new System.Drawing.Point(483, 159);
            this.btnReset.Margin = new System.Windows.Forms.Padding(0);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(100, 30);
            this.btnReset.TabIndex = 4;
            this.btnReset.Text = "重置";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(170)))), ((int)(((byte)(248)))));
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(158)))), ((int)(((byte)(236)))));
            this.btnSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(167)))), ((int)(((byte)(248)))));
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(377, 159);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 30);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtProductName
            // 
            this.txtProductName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.txtProductName.Location = new System.Drawing.Point(162, 67);
            this.txtProductName.Margin = new System.Windows.Forms.Padding(0);
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.Size = new System.Drawing.Size(200, 27);
            this.txtProductName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(52, 70);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "产品名称：";
            // 
            // txtContractCode
            // 
            this.txtContractCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.txtContractCode.Location = new System.Drawing.Point(162, 29);
            this.txtContractCode.Margin = new System.Windows.Forms.Padding(0);
            this.txtContractCode.Name = "txtContractCode";
            this.txtContractCode.Size = new System.Drawing.Size(200, 27);
            this.txtContractCode.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 32);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "合同编号：";
            // 
            // pnlTaskTableContainer
            // 
            this.pnlTaskTableContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTaskTableContainer.Controls.Add(this.dgvDrawTaskTableShow);
            this.pnlTaskTableContainer.Controls.Add(this.dgvTaskTableShow);
            this.pnlTaskTableContainer.Location = new System.Drawing.Point(9, 252);
            this.pnlTaskTableContainer.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTaskTableContainer.Name = "pnlTaskTableContainer";
            this.pnlTaskTableContainer.Size = new System.Drawing.Size(1222, 417);
            this.pnlTaskTableContainer.TabIndex = 2;
            // 
            // dgvDrawTaskTableShow
            // 
            this.dgvDrawTaskTableShow.AllowUserToAddRows = false;
            this.dgvDrawTaskTableShow.AllowUserToResizeRows = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.dgvDrawTaskTableShow.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvDrawTaskTableShow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDrawTaskTableShow.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.dgvDrawTaskTableShow.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDrawTaskTableShow.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(115)))), ((int)(((byte)(115)))));
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(237)))), ((int)(((byte)(238)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDrawTaskTableShow.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvDrawTaskTableShow.ColumnHeadersHeight = 32;
            this.dgvDrawTaskTableShow.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dNum,
            this.dRequestCode,
            this.dContractCode,
            this.dProductName,
            this.dFigureCode,
            this.dPartName,
            this.dApplicationReason,
            this.dRequestStatusName,
            this.dFileName,
            this.dDownLoad,
            this.dFileAddress,
            this.dRequestStatus});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle8.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(115)))), ((int)(((byte)(115)))));
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(237)))), ((int)(((byte)(238)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDrawTaskTableShow.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvDrawTaskTableShow.EnableHeadersVisualStyles = false;
            this.dgvDrawTaskTableShow.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.dgvDrawTaskTableShow.Location = new System.Drawing.Point(0, 0);
            this.dgvDrawTaskTableShow.Margin = new System.Windows.Forms.Padding(0);
            this.dgvDrawTaskTableShow.MultiSelect = false;
            this.dgvDrawTaskTableShow.Name = "dgvDrawTaskTableShow";
            this.dgvDrawTaskTableShow.ReadOnly = true;
            this.dgvDrawTaskTableShow.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(115)))), ((int)(((byte)(115)))));
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(237)))), ((int)(((byte)(238)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDrawTaskTableShow.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvDrawTaskTableShow.RowHeadersVisible = false;
            this.dgvDrawTaskTableShow.RowHeadersWidth = 51;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(237)))), ((int)(((byte)(238)))));
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.dgvDrawTaskTableShow.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvDrawTaskTableShow.RowTemplate.Height = 36;
            this.dgvDrawTaskTableShow.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDrawTaskTableShow.Size = new System.Drawing.Size(1222, 417);
            this.dgvDrawTaskTableShow.TabIndex = 0;
            this.dgvDrawTaskTableShow.TabStop = false;
            this.dgvDrawTaskTableShow.Visible = false;
            this.dgvDrawTaskTableShow.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDrawTaskTableShow_CellDoubleClick);
            this.dgvDrawTaskTableShow.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvDrawTaskTableShow_CellMouseClick);
            this.dgvDrawTaskTableShow.SizeChanged += new System.EventHandler(this.dgvDrawTaskTableShow_SizeChanged);
            // 
            // dNum
            // 
            this.dNum.DataPropertyName = "Num";
            this.dNum.HeaderText = "序号";
            this.dNum.MinimumWidth = 6;
            this.dNum.Name = "dNum";
            this.dNum.ReadOnly = true;
            this.dNum.Width = 121;
            // 
            // dRequestCode
            // 
            this.dRequestCode.DataPropertyName = "RequestCode";
            this.dRequestCode.HeaderText = "申请单号";
            this.dRequestCode.MinimumWidth = 6;
            this.dRequestCode.Name = "dRequestCode";
            this.dRequestCode.ReadOnly = true;
            this.dRequestCode.Width = 121;
            // 
            // dContractCode
            // 
            this.dContractCode.DataPropertyName = "ContractCode";
            this.dContractCode.HeaderText = "合同编号";
            this.dContractCode.MinimumWidth = 6;
            this.dContractCode.Name = "dContractCode";
            this.dContractCode.ReadOnly = true;
            this.dContractCode.Width = 125;
            // 
            // dProductName
            // 
            this.dProductName.DataPropertyName = "ProductName";
            this.dProductName.HeaderText = "产品名称";
            this.dProductName.MinimumWidth = 6;
            this.dProductName.Name = "dProductName";
            this.dProductName.ReadOnly = true;
            this.dProductName.Width = 121;
            // 
            // dFigureCode
            // 
            this.dFigureCode.DataPropertyName = "FigureCode";
            this.dFigureCode.HeaderText = "零件图号";
            this.dFigureCode.MinimumWidth = 6;
            this.dFigureCode.Name = "dFigureCode";
            this.dFigureCode.ReadOnly = true;
            this.dFigureCode.Width = 121;
            // 
            // dPartName
            // 
            this.dPartName.DataPropertyName = "PartName";
            this.dPartName.HeaderText = "零件名称";
            this.dPartName.MinimumWidth = 6;
            this.dPartName.Name = "dPartName";
            this.dPartName.ReadOnly = true;
            this.dPartName.Width = 120;
            // 
            // dApplicationReason
            // 
            this.dApplicationReason.DataPropertyName = "ApplicationReason";
            this.dApplicationReason.HeaderText = "申请原因";
            this.dApplicationReason.MinimumWidth = 6;
            this.dApplicationReason.Name = "dApplicationReason";
            this.dApplicationReason.ReadOnly = true;
            this.dApplicationReason.Width = 125;
            // 
            // dRequestStatusName
            // 
            this.dRequestStatusName.DataPropertyName = "RequestStatusName";
            this.dRequestStatusName.HeaderText = "上传状态";
            this.dRequestStatusName.MinimumWidth = 6;
            this.dRequestStatusName.Name = "dRequestStatusName";
            this.dRequestStatusName.ReadOnly = true;
            this.dRequestStatusName.Width = 125;
            // 
            // dFileName
            // 
            this.dFileName.DataPropertyName = "FileName";
            this.dFileName.HeaderText = "文件名";
            this.dFileName.MinimumWidth = 6;
            this.dFileName.Name = "dFileName";
            this.dFileName.ReadOnly = true;
            this.dFileName.Width = 121;
            // 
            // dDownLoad
            // 
            this.dDownLoad.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(223)))), ((int)(((byte)(77)))));
            this.dDownLoad.HeaderText = "操作";
            this.dDownLoad.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(147)))), ((int)(((byte)(223)))));
            this.dDownLoad.MinimumWidth = 6;
            this.dDownLoad.Name = "dDownLoad";
            this.dDownLoad.ReadOnly = true;
            this.dDownLoad.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dDownLoad.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dDownLoad.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(222)))), ((int)(((byte)(78)))));
            this.dDownLoad.Width = 125;
            // 
            // dFileAddress
            // 
            this.dFileAddress.DataPropertyName = "FileAddress";
            this.dFileAddress.HeaderText = "FileAddress";
            this.dFileAddress.MinimumWidth = 6;
            this.dFileAddress.Name = "dFileAddress";
            this.dFileAddress.ReadOnly = true;
            this.dFileAddress.Visible = false;
            this.dFileAddress.Width = 125;
            // 
            // dRequestStatus
            // 
            this.dRequestStatus.DataPropertyName = "RequestStatus";
            this.dRequestStatus.HeaderText = "RequestStatus";
            this.dRequestStatus.MinimumWidth = 6;
            this.dRequestStatus.Name = "dRequestStatus";
            this.dRequestStatus.ReadOnly = true;
            this.dRequestStatus.Visible = false;
            this.dRequestStatus.Width = 121;
            // 
            // TaskInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1240, 678);
            this.Controls.Add(this.pnlTaskTableContainer);
            this.Controls.Add(this.grpSearchContent);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(898, 651);
            this.Name = "TaskInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "任务列表";
            this.Load += new System.EventHandler(this.TaskInfoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTaskTableShow)).EndInit();
            this.grpSearchContent.ResumeLayout(false);
            this.pnlContainer.ResumeLayout(false);
            this.pnlContainer.PerformLayout();
            this.pnlDesignDept.ResumeLayout(false);
            this.pnlDesignDept.PerformLayout();
            this.pnlTaskType.ResumeLayout(false);
            this.pnlTaskType.PerformLayout();
            this.pnlTaskTableContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDrawTaskTableShow)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTaskTableShow;
        private System.Windows.Forms.GroupBox grpSearchContent;
        private System.Windows.Forms.Panel pnlContainer;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtProductName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtContractCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbAchieveType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rdbTaskTypeC;
        private System.Windows.Forms.RadioButton rdbTaskTypeB;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.RadioButton rdbTaskTypeA;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton rdbDesignDeptB;
        private System.Windows.Forms.RadioButton rdbDesignDeptC;
        private System.Windows.Forms.RadioButton rdbDesignDeptA;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pnlDesignDept;
        private System.Windows.Forms.Panel pnlTaskType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Num;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContractCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Model;
        private System.Windows.Forms.DataGridViewTextBoxColumn Specifications;
        private System.Windows.Forms.DataGridViewTextBoxColumn DesignTypeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskTypeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskFinishDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ActualFinishTime;
        private System.Windows.Forms.DataGridViewLinkColumn DownLoad;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectDetailID;
        private System.Windows.Forms.DataGridViewTextBoxColumn DesignType;
        private System.Windows.Forms.DataGridViewTextBoxColumn BillCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DesignDepartmentType;
        private System.Windows.Forms.DataGridViewTextBoxColumn DesignDepartmentName;
        private System.Windows.Forms.Button btnSign;
        private System.Windows.Forms.RadioButton rdbTaskTypeD;
        private System.Windows.Forms.Panel pnlTaskTableContainer;
        private System.Windows.Forms.DataGridView dgvDrawTaskTableShow;
        private System.Windows.Forms.DataGridViewTextBoxColumn dNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn dRequestCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn dContractCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn dProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dFigureCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn dPartName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dApplicationReason;
        private System.Windows.Forms.DataGridViewTextBoxColumn dRequestStatusName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dFileName;
        private System.Windows.Forms.DataGridViewLinkColumn dDownLoad;
        private System.Windows.Forms.DataGridViewTextBoxColumn dFileAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn dRequestStatus;
    }
}