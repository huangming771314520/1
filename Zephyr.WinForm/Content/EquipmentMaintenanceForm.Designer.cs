namespace Zephyr.WinForm.Content
{
    partial class EquipmentMaintenanceForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblEquipmentName = new System.Windows.Forms.Label();
            this.lblMaintenancePlanCode = new System.Windows.Forms.Label();
            this.lblMaintenanceName = new System.Windows.Forms.Label();
            this.lblPlanedStartTime = new System.Windows.Forms.Label();
            this.lblPlanedFinishTime = new System.Windows.Forms.Label();
            this.lblActualStartTime = new System.Windows.Forms.Label();
            this.lblActualFinishTime = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "设备名称：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 27);
            this.label2.TabIndex = 1;
            this.label2.Text = "设备编码：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 162);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(152, 27);
            this.label5.TabIndex = 4;
            this.label5.Text = "计划开始时间：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 216);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(152, 27);
            this.label6.TabIndex = 5;
            this.label6.Text = "计划结束时间：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 270);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(152, 27);
            this.label7.TabIndex = 6;
            this.label7.Text = "实际开始时间：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 324);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(152, 27);
            this.label8.TabIndex = 6;
            this.label8.Text = "实际结束时间：";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblEquipmentName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblMaintenancePlanCode, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblMaintenanceName, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblPlanedStartTime, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblPlanedFinishTime, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblActualStartTime, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblActualFinishTime, 1, 6);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(189, 129);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(610, 379);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // lblEquipmentName
            // 
            this.lblEquipmentName.AutoSize = true;
            this.lblEquipmentName.Location = new System.Drawing.Point(203, 0);
            this.lblEquipmentName.Name = "lblEquipmentName";
            this.lblEquipmentName.Size = new System.Drawing.Size(100, 27);
            this.lblEquipmentName.TabIndex = 7;
            this.lblEquipmentName.Text = "tempInfo";
            // 
            // lblMaintenancePlanCode
            // 
            this.lblMaintenancePlanCode.AutoSize = true;
            this.lblMaintenancePlanCode.Location = new System.Drawing.Point(203, 54);
            this.lblMaintenancePlanCode.Name = "lblMaintenancePlanCode";
            this.lblMaintenancePlanCode.Size = new System.Drawing.Size(100, 27);
            this.lblMaintenancePlanCode.TabIndex = 8;
            this.lblMaintenancePlanCode.Text = "tempInfo";
            // 
            // lblMaintenanceName
            // 
            this.lblMaintenanceName.AutoSize = true;
            this.lblMaintenanceName.Location = new System.Drawing.Point(203, 108);
            this.lblMaintenanceName.Name = "lblMaintenanceName";
            this.lblMaintenanceName.Size = new System.Drawing.Size(100, 27);
            this.lblMaintenanceName.TabIndex = 9;
            this.lblMaintenanceName.Text = "tempInfo";
            // 
            // lblPlanedStartTime
            // 
            this.lblPlanedStartTime.AutoSize = true;
            this.lblPlanedStartTime.Location = new System.Drawing.Point(203, 162);
            this.lblPlanedStartTime.Name = "lblPlanedStartTime";
            this.lblPlanedStartTime.Size = new System.Drawing.Size(100, 27);
            this.lblPlanedStartTime.TabIndex = 10;
            this.lblPlanedStartTime.Text = "tempInfo";
            // 
            // lblPlanedFinishTime
            // 
            this.lblPlanedFinishTime.AutoSize = true;
            this.lblPlanedFinishTime.Location = new System.Drawing.Point(203, 216);
            this.lblPlanedFinishTime.Name = "lblPlanedFinishTime";
            this.lblPlanedFinishTime.Size = new System.Drawing.Size(100, 27);
            this.lblPlanedFinishTime.TabIndex = 11;
            this.lblPlanedFinishTime.Text = "tempInfo";
            // 
            // lblActualStartTime
            // 
            this.lblActualStartTime.AutoSize = true;
            this.lblActualStartTime.Location = new System.Drawing.Point(203, 270);
            this.lblActualStartTime.Name = "lblActualStartTime";
            this.lblActualStartTime.Size = new System.Drawing.Size(100, 27);
            this.lblActualStartTime.TabIndex = 12;
            this.lblActualStartTime.Text = "tempInfo";
            // 
            // lblActualFinishTime
            // 
            this.lblActualFinishTime.AutoSize = true;
            this.lblActualFinishTime.Location = new System.Drawing.Point(203, 324);
            this.lblActualFinishTime.Name = "lblActualFinishTime";
            this.lblActualFinishTime.Size = new System.Drawing.Size(100, 27);
            this.lblActualFinishTime.TabIndex = 13;
            this.lblActualFinishTime.Text = "tempInfo";
            // 
            // EquipmentMaintenanceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 636);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "EquipmentMaintenanceForm";
            this.Text = "设备保养/维修/周检";
            this.Load += new System.EventHandler(this.EquipmentMaintenanceForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblEquipmentName;
        private System.Windows.Forms.Label lblMaintenancePlanCode;
        private System.Windows.Forms.Label lblMaintenanceName;
        private System.Windows.Forms.Label lblPlanedStartTime;
        private System.Windows.Forms.Label lblPlanedFinishTime;
        private System.Windows.Forms.Label lblActualStartTime;
        private System.Windows.Forms.Label lblActualFinishTime;
    }
}