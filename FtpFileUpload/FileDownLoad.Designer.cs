namespace FtpFileUpload
{
    partial class FileDownLoad
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
            this.dgv_file_list = new System.Windows.Forms.DataGridView();
            this.Seq = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileNames = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DownLoad = new System.Windows.Forms.DataGridViewLinkColumn();
            this.FileAddress_ftp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_file_list)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_file_list
            // 
            this.dgv_file_list.AllowUserToAddRows = false;
            this.dgv_file_list.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.dgv_file_list.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_file_list.BackgroundColor = System.Drawing.Color.White;
            this.dgv_file_list.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_file_list.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_file_list.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Seq,
            this.FileNames,
            this.DownLoad,
            this.FileAddress_ftp});
            this.dgv_file_list.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_file_list.Location = new System.Drawing.Point(0, 0);
            this.dgv_file_list.Name = "dgv_file_list";
            this.dgv_file_list.ReadOnly = true;
            this.dgv_file_list.RowHeadersVisible = false;
            this.dgv_file_list.RowTemplate.Height = 23;
            this.dgv_file_list.Size = new System.Drawing.Size(404, 484);
            this.dgv_file_list.TabIndex = 1;
            this.dgv_file_list.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_file_list_CellMouseClick);
            // 
            // Seq
            // 
            this.Seq.HeaderText = "序号";
            this.Seq.Name = "Seq";
            this.Seq.ReadOnly = true;
            this.Seq.Width = 55;
            // 
            // FileNames
            // 
            this.FileNames.HeaderText = "文件名称";
            this.FileNames.Name = "FileNames";
            this.FileNames.ReadOnly = true;
            this.FileNames.Width = 300;
            // 
            // DownLoad
            // 
            this.DownLoad.HeaderText = "下载";
            this.DownLoad.Name = "DownLoad";
            this.DownLoad.ReadOnly = true;
            this.DownLoad.Width = 50;
            // 
            // FileAddress_ftp
            // 
            this.FileAddress_ftp.HeaderText = "ftp文件地址";
            this.FileAddress_ftp.Name = "FileAddress_ftp";
            this.FileAddress_ftp.ReadOnly = true;
            this.FileAddress_ftp.Visible = false;
            // 
            // FileDownLoad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(404, 484);
            this.Controls.Add(this.dgv_file_list);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FileDownLoad";
            this.Text = "工艺图纸下载";
            this.Load += new System.EventHandler(this.FileDownLoad_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_file_list)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_file_list;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seq;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileNames;
        private System.Windows.Forms.DataGridViewLinkColumn DownLoad;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileAddress_ftp;
    }
}