namespace Zephyr.WinForm.Content
{
    partial class LoginForm
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
            this.txtWorkGroupID = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtWorkGroupID
            // 
            this.txtWorkGroupID.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtWorkGroupID.Location = new System.Drawing.Point(116, 86);
            this.txtWorkGroupID.MaxLength = 32;
            this.txtWorkGroupID.Name = "txtWorkGroupID";
            this.txtWorkGroupID.Size = new System.Drawing.Size(350, 31);
            this.txtWorkGroupID.TabIndex = 0;
            this.txtWorkGroupID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtWorkGroupID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtWorkGroupID_KeyPress);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 203);
            this.Controls.Add(this.txtWorkGroupID);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "扫码登录";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtWorkGroupID;
    }
}