namespace ClientFilesUpLoad.Forms
{
    partial class DrawRequestForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DrawRequestForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnKillTask = new System.Windows.Forms.Button();
            this.btnUpLoad = new System.Windows.Forms.Button();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.prgUpLoad = new System.Windows.Forms.ProgressBar();
            this.label4 = new System.Windows.Forms.Label();
            this.lblPercent = new System.Windows.Forms.Label();
            this.lblFileSize = new System.Windows.Forms.Label();
            this.lblFileName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.btnKillTask);
            this.panel1.Controls.Add(this.btnUpLoad);
            this.panel1.Controls.Add(this.btnOpenFolder);
            this.panel1.Location = new System.Drawing.Point(181, 9);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(310, 40);
            this.panel1.TabIndex = 1;
            // 
            // btnKillTask
            // 
            this.btnKillTask.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnKillTask.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnKillTask.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnKillTask.FlatAppearance.BorderSize = 0;
            this.btnKillTask.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(120)))), ((int)(((byte)(0)))));
            this.btnKillTask.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(125)))), ((int)(((byte)(0)))));
            this.btnKillTask.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKillTask.ForeColor = System.Drawing.Color.White;
            this.btnKillTask.Location = new System.Drawing.Point(209, 5);
            this.btnKillTask.Margin = new System.Windows.Forms.Padding(0);
            this.btnKillTask.Name = "btnKillTask";
            this.btnKillTask.Size = new System.Drawing.Size(84, 31);
            this.btnKillTask.TabIndex = 2;
            this.btnKillTask.Text = "结束任务";
            this.btnKillTask.UseVisualStyleBackColor = false;
            this.btnKillTask.Visible = false;
            this.btnKillTask.Click += new System.EventHandler(this.BtnKillTask_Click);
            // 
            // btnUpLoad
            // 
            this.btnUpLoad.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnUpLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(170)))), ((int)(((byte)(248)))));
            this.btnUpLoad.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpLoad.FlatAppearance.BorderSize = 0;
            this.btnUpLoad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(162)))), ((int)(((byte)(248)))));
            this.btnUpLoad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(167)))), ((int)(((byte)(248)))));
            this.btnUpLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpLoad.ForeColor = System.Drawing.Color.White;
            this.btnUpLoad.Location = new System.Drawing.Point(113, 5);
            this.btnUpLoad.Margin = new System.Windows.Forms.Padding(0);
            this.btnUpLoad.Name = "btnUpLoad";
            this.btnUpLoad.Size = new System.Drawing.Size(84, 31);
            this.btnUpLoad.TabIndex = 2;
            this.btnUpLoad.Text = "开始上传";
            this.btnUpLoad.UseVisualStyleBackColor = false;
            this.btnUpLoad.Click += new System.EventHandler(this.BtnUpLoad_Click);
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnOpenFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(231)))), ((int)(((byte)(90)))));
            this.btnOpenFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenFolder.FlatAppearance.BorderSize = 0;
            this.btnOpenFolder.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(226)))), ((int)(((byte)(21)))));
            this.btnOpenFolder.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(230)))), ((int)(((byte)(79)))));
            this.btnOpenFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenFolder.ForeColor = System.Drawing.Color.White;
            this.btnOpenFolder.Location = new System.Drawing.Point(17, 5);
            this.btnOpenFolder.Margin = new System.Windows.Forms.Padding(0);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(84, 31);
            this.btnOpenFolder.TabIndex = 1;
            this.btnOpenFolder.Text = "选择文件";
            this.btnOpenFolder.UseVisualStyleBackColor = false;
            this.btnOpenFolder.Click += new System.EventHandler(this.BtnOpenFolder_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtFilePath);
            this.panel2.Controls.Add(this.prgUpLoad);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.lblPercent);
            this.panel2.Controls.Add(this.lblFileSize);
            this.panel2.Controls.Add(this.lblFileName);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel2.Location = new System.Drawing.Point(102, 74);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(495, 170);
            this.panel2.TabIndex = 2;
            // 
            // txtFilePath
            // 
            this.txtFilePath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.txtFilePath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFilePath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.txtFilePath.Location = new System.Drawing.Point(127, 18);
            this.txtFilePath.Margin = new System.Windows.Forms.Padding(0);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(342, 20);
            this.txtFilePath.TabIndex = 14;
            // 
            // prgUpLoad
            // 
            this.prgUpLoad.Location = new System.Drawing.Point(127, 126);
            this.prgUpLoad.Margin = new System.Windows.Forms.Padding(0);
            this.prgUpLoad.Name = "prgUpLoad";
            this.prgUpLoad.Size = new System.Drawing.Size(273, 20);
            this.prgUpLoad.Step = 1;
            this.prgUpLoad.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 91);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 20);
            this.label4.TabIndex = 12;
            this.label4.Text = "文件大小：";
            // 
            // lblPercent
            // 
            this.lblPercent.AutoSize = true;
            this.lblPercent.Location = new System.Drawing.Point(416, 127);
            this.lblPercent.Margin = new System.Windows.Forms.Padding(0);
            this.lblPercent.Name = "lblPercent";
            this.lblPercent.Size = new System.Drawing.Size(53, 20);
            this.lblPercent.TabIndex = 9;
            this.lblPercent.Text = "0.00%";
            // 
            // lblFileSize
            // 
            this.lblFileSize.AutoSize = true;
            this.lblFileSize.Location = new System.Drawing.Point(123, 91);
            this.lblFileSize.Margin = new System.Windows.Forms.Padding(0);
            this.lblFileSize.Name = "lblFileSize";
            this.lblFileSize.Size = new System.Drawing.Size(65, 20);
            this.lblFileSize.TabIndex = 10;
            this.lblFileSize.Text = "0.00Mb";
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point(123, 56);
            this.lblFileName.Margin = new System.Windows.Forms.Padding(0);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(0, 20);
            this.lblFileName.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.label3.Location = new System.Drawing.Point(25, 21);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "文件路径：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.label2.Location = new System.Drawing.Point(25, 126);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "上传进度：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.label1.Location = new System.Drawing.Point(25, 56);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "文件名：";
            // 
            // DrawRequestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(698, 255);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "DrawRequestForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "图纸申请-文件上传";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnKillTask;
        private System.Windows.Forms.Button btnUpLoad;
        private System.Windows.Forms.Button btnOpenFolder;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ProgressBar prgUpLoad;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblPercent;
        private System.Windows.Forms.Label lblFileSize;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFilePath;
    }
}