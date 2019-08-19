namespace ProcessBlueprintUpload.Forms
{
    partial class ProcessRouteBlueprintForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProcessRouteBlueprintForm));
            this.lvFileInfo = new System.Windows.Forms.ListView();
            this.lvColumnRowNum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvColumnFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvColumnUploadState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.btnStartUpload = new System.Windows.Forms.Button();
            this.prgPercent = new System.Windows.Forms.ProgressBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvFileInfo
            // 
            this.lvFileInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvFileInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvColumnRowNum,
            this.lvColumnFileName,
            this.lvColumnUploadState});
            this.lvFileInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.lvFileInfo.FullRowSelect = true;
            this.lvFileInfo.GridLines = true;
            this.lvFileInfo.HideSelection = false;
            this.lvFileInfo.Location = new System.Drawing.Point(9, 67);
            this.lvFileInfo.Margin = new System.Windows.Forms.Padding(0);
            this.lvFileInfo.MultiSelect = false;
            this.lvFileInfo.Name = "lvFileInfo";
            this.lvFileInfo.Size = new System.Drawing.Size(564, 237);
            this.lvFileInfo.TabIndex = 0;
            this.lvFileInfo.UseCompatibleStateImageBehavior = false;
            this.lvFileInfo.View = System.Windows.Forms.View.Details;
            // 
            // lvColumnRowNum
            // 
            this.lvColumnRowNum.Text = "序号";
            // 
            // lvColumnFileName
            // 
            this.lvColumnFileName.Text = "文件名";
            this.lvColumnFileName.Width = 380;
            // 
            // lvColumnUploadState
            // 
            this.lvColumnUploadState.Text = "上传状态";
            this.lvColumnUploadState.Width = 100;
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(196)))), ((int)(((byte)(255)))));
            this.btnSelectFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectFile.FlatAppearance.BorderSize = 0;
            this.btnSelectFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectFile.ForeColor = System.Drawing.Color.White;
            this.btnSelectFile.Location = new System.Drawing.Point(298, 0);
            this.btnSelectFile.Margin = new System.Windows.Forms.Padding(0);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(100, 36);
            this.btnSelectFile.TabIndex = 1;
            this.btnSelectFile.Text = "选择文件";
            this.btnSelectFile.UseVisualStyleBackColor = false;
            this.btnSelectFile.Click += new System.EventHandler(this.BtnSelectFile_Click);
            // 
            // btnStartUpload
            // 
            this.btnStartUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartUpload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(154)))), ((int)(((byte)(255)))));
            this.btnStartUpload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStartUpload.FlatAppearance.BorderSize = 0;
            this.btnStartUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartUpload.ForeColor = System.Drawing.Color.White;
            this.btnStartUpload.Location = new System.Drawing.Point(415, 0);
            this.btnStartUpload.Margin = new System.Windows.Forms.Padding(0);
            this.btnStartUpload.Name = "btnStartUpload";
            this.btnStartUpload.Size = new System.Drawing.Size(100, 36);
            this.btnStartUpload.TabIndex = 2;
            this.btnStartUpload.Text = "开始上传";
            this.btnStartUpload.UseVisualStyleBackColor = false;
            this.btnStartUpload.Click += new System.EventHandler(this.BtnStartUpload_Click);
            // 
            // prgPercent
            // 
            this.prgPercent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prgPercent.Location = new System.Drawing.Point(0, 311);
            this.prgPercent.Margin = new System.Windows.Forms.Padding(0);
            this.prgPercent.Name = "prgPercent";
            this.prgPercent.Size = new System.Drawing.Size(582, 2);
            this.prgPercent.Step = 1;
            this.prgPercent.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.btnSelectFile);
            this.panel1.Controls.Add(this.btnStartUpload);
            this.panel1.Location = new System.Drawing.Point(9, 9);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(564, 46);
            this.panel1.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(150)))), ((int)(((byte)(228)))));
            this.panel2.Location = new System.Drawing.Point(143, 39);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(400, 3);
            this.panel2.TabIndex = 7;
            // 
            // ProcessRouteBlueprintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(582, 313);
            this.Controls.Add(this.prgPercent);
            this.Controls.Add(this.lvFileInfo);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProcessRouteBlueprintForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "工序工步编辑_图纸上传";
            this.Load += new System.EventHandler(this.ProcessRouteBlueprintForm_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvFileInfo;
        private System.Windows.Forms.ColumnHeader lvColumnRowNum;
        private System.Windows.Forms.ColumnHeader lvColumnFileName;
        private System.Windows.Forms.ColumnHeader lvColumnUploadState;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.Button btnStartUpload;
        private System.Windows.Forms.ProgressBar prgPercent;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}