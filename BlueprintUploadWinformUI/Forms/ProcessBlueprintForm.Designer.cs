namespace BlueprintUploadWinformUI.Forms
{
    partial class ProcessBlueprintForm
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProcessBlueprintForm));
            this.lst_filelist = new System.Windows.Forms.ListBox();
            this.prs_bar = new System.Windows.Forms.ProgressBar();
            this.BtnChooseFile = new System.Windows.Forms.Button();
            this.Btn_UploadFile = new System.Windows.Forms.Button();
            this.lblFigureNumber = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lst_filelist
            // 
            this.lst_filelist.FormattingEnabled = true;
            this.lst_filelist.ItemHeight = 20;
            this.lst_filelist.Location = new System.Drawing.Point(9, 83);
            this.lst_filelist.Margin = new System.Windows.Forms.Padding(0);
            this.lst_filelist.Name = "lst_filelist";
            this.lst_filelist.Size = new System.Drawing.Size(764, 284);
            this.lst_filelist.TabIndex = 3;
            // 
            // prs_bar
            // 
            this.prs_bar.Location = new System.Drawing.Point(9, 376);
            this.prs_bar.Margin = new System.Windows.Forms.Padding(0);
            this.prs_bar.Name = "prs_bar";
            this.prs_bar.Size = new System.Drawing.Size(764, 28);
            this.prs_bar.Step = 1;
            this.prs_bar.TabIndex = 5;
            // 
            // BtnChooseFile
            // 
            this.BtnChooseFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(184)))), ((int)(((byte)(255)))));
            this.BtnChooseFile.FlatAppearance.BorderSize = 0;
            this.BtnChooseFile.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(166)))), ((int)(((byte)(255)))));
            this.BtnChooseFile.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(184)))), ((int)(((byte)(255)))));
            this.BtnChooseFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnChooseFile.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnChooseFile.ForeColor = System.Drawing.Color.White;
            this.BtnChooseFile.Location = new System.Drawing.Point(518, 10);
            this.BtnChooseFile.Margin = new System.Windows.Forms.Padding(0);
            this.BtnChooseFile.Name = "BtnChooseFile";
            this.BtnChooseFile.Size = new System.Drawing.Size(100, 40);
            this.BtnChooseFile.TabIndex = 12;
            this.BtnChooseFile.Text = "选择图纸";
            this.BtnChooseFile.UseVisualStyleBackColor = false;
            this.BtnChooseFile.Click += new System.EventHandler(this.BtnChooseFile_Click);
            // 
            // Btn_UploadFile
            // 
            this.Btn_UploadFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(184)))), ((int)(((byte)(255)))));
            this.Btn_UploadFile.FlatAppearance.BorderSize = 0;
            this.Btn_UploadFile.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(166)))), ((int)(((byte)(255)))));
            this.Btn_UploadFile.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(184)))), ((int)(((byte)(255)))));
            this.Btn_UploadFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_UploadFile.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_UploadFile.ForeColor = System.Drawing.Color.White;
            this.Btn_UploadFile.Location = new System.Drawing.Point(631, 10);
            this.Btn_UploadFile.Margin = new System.Windows.Forms.Padding(0);
            this.Btn_UploadFile.Name = "Btn_UploadFile";
            this.Btn_UploadFile.Size = new System.Drawing.Size(100, 40);
            this.Btn_UploadFile.TabIndex = 12;
            this.Btn_UploadFile.Text = "上传";
            this.Btn_UploadFile.UseVisualStyleBackColor = false;
            this.Btn_UploadFile.Click += new System.EventHandler(this.Btn_UploadFile_Click);
            // 
            // lblFigureNumber
            // 
            this.lblFigureNumber.AutoSize = true;
            this.lblFigureNumber.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblFigureNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblFigureNumber.Location = new System.Drawing.Point(9, 14);
            this.lblFigureNumber.Margin = new System.Windows.Forms.Padding(0);
            this.lblFigureNumber.Name = "lblFigureNumber";
            this.lblFigureNumber.Size = new System.Drawing.Size(0, 32);
            this.lblFigureNumber.TabIndex = 13;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.lblFigureNumber);
            this.panel1.Controls.Add(this.Btn_UploadFile);
            this.panel1.Controls.Add(this.BtnChooseFile);
            this.panel1.Location = new System.Drawing.Point(9, 9);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(764, 60);
            this.panel1.TabIndex = 15;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(129)))), ((int)(((byte)(216)))), ((int)(((byte)(65)))));
            this.panel2.Location = new System.Drawing.Point(382, 57);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(382, 3);
            this.panel2.TabIndex = 16;
            // 
            // ProcessBlueprintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(782, 413);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.prs_bar);
            this.Controls.Add(this.lst_filelist);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProcessBlueprintForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "工艺图纸上传";
            this.Load += new System.EventHandler(this.ProcessBlueprintForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListBox lst_filelist;
        private System.Windows.Forms.ProgressBar prs_bar;
        private System.Windows.Forms.Button BtnChooseFile;
        private System.Windows.Forms.Button Btn_UploadFile;
        private System.Windows.Forms.Label lblFigureNumber;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}