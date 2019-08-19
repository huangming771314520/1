namespace Update.Forms
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.lblNewVersionCode = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rtxUpdateContent = new System.Windows.Forms.RichTextBox();
            this.lblUpdateTime = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnStartUpdate = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "更新时间：";
            // 
            // lblNewVersionCode
            // 
            this.lblNewVersionCode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblNewVersionCode.AutoSize = true;
            this.lblNewVersionCode.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblNewVersionCode.Location = new System.Drawing.Point(256, 19);
            this.lblNewVersionCode.Margin = new System.Windows.Forms.Padding(0);
            this.lblNewVersionCode.Name = "lblNewVersionCode";
            this.lblNewVersionCode.Size = new System.Drawing.Size(75, 27);
            this.lblNewVersionCode.TabIndex = 1;
            this.lblNewVersionCode.Text = "0.0.0.0";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.rtxUpdateContent);
            this.panel1.Controls.Add(this.lblUpdateTime);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(41, 57);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(400, 160);
            this.panel1.TabIndex = 2;
            // 
            // rtxUpdateContent
            // 
            this.rtxUpdateContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxUpdateContent.Location = new System.Drawing.Point(18, 59);
            this.rtxUpdateContent.Margin = new System.Windows.Forms.Padding(0);
            this.rtxUpdateContent.Name = "rtxUpdateContent";
            this.rtxUpdateContent.Size = new System.Drawing.Size(364, 91);
            this.rtxUpdateContent.TabIndex = 1;
            this.rtxUpdateContent.TabStop = false;
            this.rtxUpdateContent.Text = "";
            // 
            // lblUpdateTime
            // 
            this.lblUpdateTime.AutoSize = true;
            this.lblUpdateTime.Location = new System.Drawing.Point(98, 10);
            this.lblUpdateTime.Margin = new System.Windows.Forms.Padding(0);
            this.lblUpdateTime.Name = "lblUpdateTime";
            this.lblUpdateTime.Size = new System.Drawing.Size(93, 20);
            this.lblUpdateTime.TabIndex = 0;
            this.lblUpdateTime.Text = "2019-06-26";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 34);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 20);
            this.label4.TabIndex = 0;
            this.label4.Text = "更新内容：";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(151, 19);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 27);
            this.label5.TabIndex = 0;
            this.label5.Text = "最新版本：";
            // 
            // btnStartUpdate
            // 
            this.btnStartUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStartUpdate.Location = new System.Drawing.Point(168, 227);
            this.btnStartUpdate.Margin = new System.Windows.Forms.Padding(0);
            this.btnStartUpdate.Name = "btnStartUpdate";
            this.btnStartUpdate.Size = new System.Drawing.Size(147, 40);
            this.btnStartUpdate.TabIndex = 0;
            this.btnStartUpdate.Text = "立即更新";
            this.btnStartUpdate.UseVisualStyleBackColor = true;
            this.btnStartUpdate.Click += new System.EventHandler(this.BtnStartUpdate_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(482, 279);
            this.Controls.Add(this.btnStartUpdate);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblNewVersionCode);
            this.Controls.Add(this.label5);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "更新提示";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblNewVersionCode;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox rtxUpdateContent;
        private System.Windows.Forms.Label lblUpdateTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnStartUpdate;
    }
}