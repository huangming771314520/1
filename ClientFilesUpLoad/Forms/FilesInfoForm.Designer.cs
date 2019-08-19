namespace ClientFilesUpLoad.Forms
{
    partial class FilesInfoForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilesInfoForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnKillTask = new System.Windows.Forms.Button();
            this.btnUpLoad = new System.Windows.Forms.Button();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.txtBomFilePath = new System.Windows.Forms.TextBox();
            this.lvFileInfo = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmbDesignTaskResult = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.cmbDesignTaskResult);
            this.panel1.Controls.Add(this.btnKillTask);
            this.panel1.Controls.Add(this.btnUpLoad);
            this.panel1.Controls.Add(this.btnOpenFolder);
            this.panel1.Controls.Add(this.txtBomFilePath);
            this.panel1.Location = new System.Drawing.Point(48, 9);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(801, 40);
            this.panel1.TabIndex = 0;
            // 
            // btnKillTask
            // 
            this.btnKillTask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKillTask.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnKillTask.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnKillTask.FlatAppearance.BorderSize = 0;
            this.btnKillTask.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(120)))), ((int)(((byte)(0)))));
            this.btnKillTask.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(125)))), ((int)(((byte)(0)))));
            this.btnKillTask.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKillTask.ForeColor = System.Drawing.Color.White;
            this.btnKillTask.Location = new System.Drawing.Point(705, 4);
            this.btnKillTask.Margin = new System.Windows.Forms.Padding(0);
            this.btnKillTask.Name = "btnKillTask";
            this.btnKillTask.Size = new System.Drawing.Size(84, 31);
            this.btnKillTask.TabIndex = 2;
            this.btnKillTask.Text = "结束任务";
            this.btnKillTask.UseVisualStyleBackColor = false;
            this.btnKillTask.Click += new System.EventHandler(this.BtnKillTask_Click);
            // 
            // btnUpLoad
            // 
            this.btnUpLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(170)))), ((int)(((byte)(248)))));
            this.btnUpLoad.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpLoad.FlatAppearance.BorderSize = 0;
            this.btnUpLoad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(162)))), ((int)(((byte)(248)))));
            this.btnUpLoad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(167)))), ((int)(((byte)(248)))));
            this.btnUpLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpLoad.ForeColor = System.Drawing.Color.White;
            this.btnUpLoad.Location = new System.Drawing.Point(610, 4);
            this.btnUpLoad.Margin = new System.Windows.Forms.Padding(0);
            this.btnUpLoad.Name = "btnUpLoad";
            this.btnUpLoad.Size = new System.Drawing.Size(84, 31);
            this.btnUpLoad.TabIndex = 2;
            this.btnUpLoad.Text = "开始上传";
            this.btnUpLoad.UseVisualStyleBackColor = false;
            this.btnUpLoad.Click += new System.EventHandler(this.btnUpLoad_Click);
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(231)))), ((int)(((byte)(90)))));
            this.btnOpenFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenFolder.FlatAppearance.BorderSize = 0;
            this.btnOpenFolder.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(226)))), ((int)(((byte)(21)))));
            this.btnOpenFolder.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(230)))), ((int)(((byte)(79)))));
            this.btnOpenFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenFolder.ForeColor = System.Drawing.Color.White;
            this.btnOpenFolder.Location = new System.Drawing.Point(378, 4);
            this.btnOpenFolder.Margin = new System.Windows.Forms.Padding(0);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(84, 31);
            this.btnOpenFolder.TabIndex = 1;
            this.btnOpenFolder.Text = "选择文件";
            this.btnOpenFolder.UseVisualStyleBackColor = false;
            this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
            // 
            // txtBomFilePath
            // 
            this.txtBomFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBomFilePath.BackColor = System.Drawing.SystemColors.Window;
            this.txtBomFilePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBomFilePath.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBomFilePath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.txtBomFilePath.Location = new System.Drawing.Point(12, 7);
            this.txtBomFilePath.Margin = new System.Windows.Forms.Padding(0);
            this.txtBomFilePath.Name = "txtBomFilePath";
            this.txtBomFilePath.ReadOnly = true;
            this.txtBomFilePath.Size = new System.Drawing.Size(353, 27);
            this.txtBomFilePath.TabIndex = 0;
            this.txtBomFilePath.TabStop = false;
            // 
            // lvFileInfo
            // 
            this.lvFileInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFileInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.lvFileInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvFileInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.lvFileInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.lvFileInfo.FullRowSelect = true;
            this.lvFileInfo.GridLines = true;
            this.lvFileInfo.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvFileInfo.HideSelection = false;
            this.lvFileInfo.Location = new System.Drawing.Point(9, 66);
            this.lvFileInfo.Margin = new System.Windows.Forms.Padding(0);
            this.lvFileInfo.Name = "lvFileInfo";
            this.lvFileInfo.Size = new System.Drawing.Size(864, 438);
            this.lvFileInfo.TabIndex = 0;
            this.lvFileInfo.UseCompatibleStateImageBehavior = false;
            this.lvFileInfo.View = System.Windows.Forms.View.Details;
            this.lvFileInfo.SizeChanged += new System.EventHandler(this.lvFileInfo_SizeChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "序号";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "名称";
            this.columnHeader2.Width = 434;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "大小";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 80;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "进度";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader4.Width = 86;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "状态";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader5.Width = 80;
            // 
            // cmbDesignTaskResult
            // 
            this.cmbDesignTaskResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbDesignTaskResult.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDesignTaskResult.FormattingEnabled = true;
            this.cmbDesignTaskResult.Location = new System.Drawing.Point(473, 5);
            this.cmbDesignTaskResult.Name = "cmbDesignTaskResult";
            this.cmbDesignTaskResult.Size = new System.Drawing.Size(126, 28);
            this.cmbDesignTaskResult.TabIndex = 3;
            // 
            // FilesInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(882, 513);
            this.Controls.Add(this.lvFileInfo);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "FilesInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "文件上传";
            this.Load += new System.EventHandler(this.FilesInfoForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnUpLoad;
        private System.Windows.Forms.Button btnOpenFolder;
        private System.Windows.Forms.TextBox txtBomFilePath;
        private System.Windows.Forms.ListView lvFileInfo;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Button btnKillTask;
        private System.Windows.Forms.ComboBox cmbDesignTaskResult;
    }
}

