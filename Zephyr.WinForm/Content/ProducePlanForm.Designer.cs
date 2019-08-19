namespace Zephyr.WinForm.Content
{
    partial class ProducePlanForm
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
            this.dgvProducePlanInfoShow = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContractCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProcessName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProcessDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WorkshopName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EquipmentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WorkGroupName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ManHour = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlanedStartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlanedFinishTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Model = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Specifications = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EquipmentID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducePlanInfoShow)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvProducePlanInfoShow
            // 
            this.dgvProducePlanInfoShow.AllowUserToAddRows = false;
            this.dgvProducePlanInfoShow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProducePlanInfoShow.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.ContractCode,
            this.ProjectName,
            this.PartCode,
            this.ProcessName,
            this.ProcessDesc,
            this.WorkshopName,
            this.EquipmentName,
            this.WorkGroupName,
            this.Quantity,
            this.ManHour,
            this.PlanedStartTime,
            this.PlanedFinishTime,
            this.ProductName,
            this.ProductType,
            this.Model,
            this.Specifications,
            this.EquipmentID});
            this.dgvProducePlanInfoShow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProducePlanInfoShow.Location = new System.Drawing.Point(0, 0);
            this.dgvProducePlanInfoShow.Name = "dgvProducePlanInfoShow";
            this.dgvProducePlanInfoShow.RowTemplate.Height = 27;
            this.dgvProducePlanInfoShow.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducePlanInfoShow.Size = new System.Drawing.Size(1182, 753);
            this.dgvProducePlanInfoShow.TabIndex = 0;
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ID";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            // 
            // ContractCode
            // 
            this.ContractCode.DataPropertyName = "ContractCode";
            this.ContractCode.HeaderText = "合同编号";
            this.ContractCode.Name = "ContractCode";
            // 
            // ProjectName
            // 
            this.ProjectName.DataPropertyName = "ProjectName";
            this.ProjectName.HeaderText = "项目名称";
            this.ProjectName.Name = "ProjectName";
            // 
            // PartCode
            // 
            this.PartCode.DataPropertyName = "PartCode";
            this.PartCode.HeaderText = "零件编码";
            this.PartCode.Name = "PartCode";
            // 
            // ProcessName
            // 
            this.ProcessName.DataPropertyName = "ProcessName";
            this.ProcessName.HeaderText = "工序名称";
            this.ProcessName.Name = "ProcessName";
            // 
            // ProcessDesc
            // 
            this.ProcessDesc.DataPropertyName = "ProcessDesc";
            this.ProcessDesc.HeaderText = "工序说明";
            this.ProcessDesc.Name = "ProcessDesc";
            // 
            // WorkshopName
            // 
            this.WorkshopName.DataPropertyName = "WorkshopName";
            this.WorkshopName.HeaderText = "车间名称";
            this.WorkshopName.Name = "WorkshopName";
            // 
            // EquipmentName
            // 
            this.EquipmentName.DataPropertyName = "EquipmentName";
            this.EquipmentName.HeaderText = "设备名称";
            this.EquipmentName.Name = "EquipmentName";
            // 
            // WorkGroupName
            // 
            this.WorkGroupName.DataPropertyName = "WorkGroupName";
            this.WorkGroupName.HeaderText = "班组名称";
            this.WorkGroupName.Name = "WorkGroupName";
            // 
            // Quantity
            // 
            this.Quantity.DataPropertyName = "Quantity";
            this.Quantity.HeaderText = "生产数量";
            this.Quantity.Name = "Quantity";
            // 
            // ManHour
            // 
            this.ManHour.DataPropertyName = "ManHour";
            this.ManHour.HeaderText = "工时定额";
            this.ManHour.Name = "ManHour";
            // 
            // PlanedStartTime
            // 
            this.PlanedStartTime.DataPropertyName = "PlanedStartTime";
            this.PlanedStartTime.HeaderText = "计划开始时间";
            this.PlanedStartTime.Name = "PlanedStartTime";
            // 
            // PlanedFinishTime
            // 
            this.PlanedFinishTime.DataPropertyName = "PlanedFinishTime";
            this.PlanedFinishTime.HeaderText = "计划结束时间";
            this.PlanedFinishTime.Name = "PlanedFinishTime";
            // 
            // ProductName
            // 
            this.ProductName.DataPropertyName = "ProductName";
            this.ProductName.HeaderText = "产品名称";
            this.ProductName.Name = "ProductName";
            // 
            // ProductType
            // 
            this.ProductType.DataPropertyName = "ProductType";
            this.ProductType.HeaderText = "产品类型";
            this.ProductType.Name = "ProductType";
            // 
            // Model
            // 
            this.Model.DataPropertyName = "Model";
            this.Model.HeaderText = "型号";
            this.Model.Name = "Model";
            // 
            // Specifications
            // 
            this.Specifications.DataPropertyName = "Specifications";
            this.Specifications.HeaderText = "规格";
            this.Specifications.Name = "Specifications";
            // 
            // EquipmentID
            // 
            this.EquipmentID.DataPropertyName = "EquipmentID";
            this.EquipmentID.HeaderText = "设备ID/编码";
            this.EquipmentID.Name = "EquipmentID";
            this.EquipmentID.Visible = false;
            // 
            // ProducePlanForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1182, 753);
            this.Controls.Add(this.dgvProducePlanInfoShow);
            this.Name = "ProducePlanForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "生产计划";
            this.Load += new System.EventHandler(this.ProducePlanForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducePlanInfoShow)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvProducePlanInfoShow;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContractCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProcessName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProcessDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn WorkshopName;
        private System.Windows.Forms.DataGridViewTextBoxColumn EquipmentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn WorkGroupName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn ManHour;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlanedStartTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlanedFinishTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Model;
        private System.Windows.Forms.DataGridViewTextBoxColumn Specifications;
        private System.Windows.Forms.DataGridViewTextBoxColumn EquipmentID;
    }
}