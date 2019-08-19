namespace FtpFileUpload
{
    partial class ChooseRootPartCodeList
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvProjectPart = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnPrePage = new System.Windows.Forms.Button();
            this.btnAfterPage = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPartFigureCode = new System.Windows.Forms.TextBox();
            this.txtProjectName = new System.Windows.Forms.TextBox();
            this.txtModel = new System.Windows.Forms.TextBox();
            this.txtSpec = new System.Windows.Forms.TextBox();
            this.txtContractCode = new System.Windows.Forms.TextBox();
            this.PageInfo = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.PageTotal = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.ContractCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taskText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Model = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Specifications = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartFigureCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProjectPart)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dgvProjectPart, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1110, 450);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dgvProjectPart
            // 
            this.dgvProjectPart.AllowUserToAddRows = false;
            this.dgvProjectPart.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.dgvProjectPart.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvProjectPart.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProjectPart.BackgroundColor = System.Drawing.Color.White;
            this.dgvProjectPart.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvProjectPart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProjectPart.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ContractCode,
            this.ProjectName,
            this.ProductName,
            this.ProductType,
            this.taskText,
            this.Model,
            this.Specifications,
            this.Quantity,
            this.PartFigureCode,
            this.TaskDescription,
            this.PartName,
            this.PartCode});
            this.dgvProjectPart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dgvProjectPart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProjectPart.Location = new System.Drawing.Point(3, 93);
            this.dgvProjectPart.Name = "dgvProjectPart";
            this.dgvProjectPart.ReadOnly = true;
            this.dgvProjectPart.RowHeadersVisible = false;
            this.dgvProjectPart.RowTemplate.Height = 23;
            this.dgvProjectPart.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProjectPart.Size = new System.Drawing.Size(1104, 314);
            this.dgvProjectPart.TabIndex = 9;
            this.dgvProjectPart.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProjectPart_CellDoubleClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.PageTotal);
            this.panel1.Controls.Add(this.PageInfo);
            this.panel1.Controls.Add(this.btnPrePage);
            this.panel1.Controls.Add(this.btnAfterPage);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 413);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1104, 34);
            this.panel1.TabIndex = 7;
            // 
            // btnPrePage
            // 
            this.btnPrePage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(170)))), ((int)(((byte)(248)))));
            this.btnPrePage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnPrePage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrePage.FlatAppearance.BorderSize = 0;
            this.btnPrePage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(158)))), ((int)(((byte)(236)))));
            this.btnPrePage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(167)))), ((int)(((byte)(248)))));
            this.btnPrePage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrePage.ForeColor = System.Drawing.Color.White;
            this.btnPrePage.Location = new System.Drawing.Point(879, 5);
            this.btnPrePage.Margin = new System.Windows.Forms.Padding(0);
            this.btnPrePage.Name = "btnPrePage";
            this.btnPrePage.Size = new System.Drawing.Size(100, 25);
            this.btnPrePage.TabIndex = 6;
            this.btnPrePage.Text = "上一页";
            this.btnPrePage.UseVisualStyleBackColor = false;
            this.btnPrePage.Click += new System.EventHandler(this.btnPrePage_Click);
            // 
            // btnAfterPage
            // 
            this.btnAfterPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(170)))), ((int)(((byte)(248)))));
            this.btnAfterPage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnAfterPage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAfterPage.FlatAppearance.BorderSize = 0;
            this.btnAfterPage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(158)))), ((int)(((byte)(236)))));
            this.btnAfterPage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(167)))), ((int)(((byte)(248)))));
            this.btnAfterPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAfterPage.ForeColor = System.Drawing.Color.White;
            this.btnAfterPage.Location = new System.Drawing.Point(998, 5);
            this.btnAfterPage.Margin = new System.Windows.Forms.Padding(0);
            this.btnAfterPage.Name = "btnAfterPage";
            this.btnAfterPage.Size = new System.Drawing.Size(100, 25);
            this.btnAfterPage.TabIndex = 6;
            this.btnAfterPage.Text = "下一页";
            this.btnAfterPage.UseVisualStyleBackColor = false;
            this.btnAfterPage.Click += new System.EventHandler(this.btnAfterPage_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.btnClear);
            this.panel2.Controls.Add(this.btnSearch);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.txtPartFigureCode);
            this.panel2.Controls.Add(this.txtProjectName);
            this.panel2.Controls.Add(this.txtModel);
            this.panel2.Controls.Add(this.txtSpec);
            this.panel2.Controls.Add(this.txtContractCode);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1104, 84);
            this.panel2.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(651, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "零件图号：";
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(170)))), ((int)(((byte)(248)))));
            this.btnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(158)))), ((int)(((byte)(236)))));
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(167)))), ((int)(((byte)(248)))));
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(1017, 53);
            this.btnClear.Margin = new System.Windows.Forms.Padding(0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(80, 25);
            this.btnClear.TabIndex = 6;
            this.btnClear.Text = "清空";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
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
            this.btnSearch.Location = new System.Drawing.Point(921, 53);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 25);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(328, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "工程项目：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(328, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "型号：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "规格：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "合同编码：";
            // 
            // txtPartFigureCode
            // 
            this.txtPartFigureCode.Location = new System.Drawing.Point(717, 11);
            this.txtPartFigureCode.Name = "txtPartFigureCode";
            this.txtPartFigureCode.Size = new System.Drawing.Size(197, 21);
            this.txtPartFigureCode.TabIndex = 0;
            // 
            // txtProjectName
            // 
            this.txtProjectName.Location = new System.Drawing.Point(395, 11);
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.Size = new System.Drawing.Size(220, 21);
            this.txtProjectName.TabIndex = 0;
            // 
            // txtModel
            // 
            this.txtModel.Location = new System.Drawing.Point(395, 52);
            this.txtModel.Name = "txtModel";
            this.txtModel.Size = new System.Drawing.Size(220, 21);
            this.txtModel.TabIndex = 0;
            // 
            // txtSpec
            // 
            this.txtSpec.Location = new System.Drawing.Point(89, 52);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.Size = new System.Drawing.Size(202, 21);
            this.txtSpec.TabIndex = 0;
            // 
            // txtContractCode
            // 
            this.txtContractCode.Location = new System.Drawing.Point(89, 10);
            this.txtContractCode.Name = "txtContractCode";
            this.txtContractCode.Size = new System.Drawing.Size(202, 21);
            this.txtContractCode.TabIndex = 0;
            // 
            // PageInfo
            // 
            this.PageInfo.AutoSize = true;
            this.PageInfo.Location = new System.Drawing.Point(769, 12);
            this.PageInfo.Name = "PageInfo";
            this.PageInfo.Size = new System.Drawing.Size(11, 12);
            this.PageInfo.TabIndex = 7;
            this.PageInfo.Text = "1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(748, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 7;
            this.label6.Text = "第";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(784, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 12);
            this.label7.TabIndex = 7;
            this.label7.Text = "页";
            // 
            // PageTotal
            // 
            this.PageTotal.AutoSize = true;
            this.PageTotal.Location = new System.Drawing.Point(822, 13);
            this.PageTotal.Name = "PageTotal";
            this.PageTotal.Size = new System.Drawing.Size(11, 12);
            this.PageTotal.TabIndex = 7;
            this.PageTotal.Text = "1";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(799, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(23, 12);
            this.label9.TabIndex = 7;
            this.label9.Text = "/共";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(835, 13);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 12);
            this.label10.TabIndex = 7;
            this.label10.Text = "页";
            // 
            // ContractCode
            // 
            this.ContractCode.DataPropertyName = "ContractCode";
            this.ContractCode.HeaderText = "合同编号";
            this.ContractCode.Name = "ContractCode";
            this.ContractCode.ReadOnly = true;
            // 
            // ProjectName
            // 
            this.ProjectName.DataPropertyName = "ProjectName";
            this.ProjectName.HeaderText = "工程项目";
            this.ProjectName.Name = "ProjectName";
            this.ProjectName.ReadOnly = true;
            // 
            // ProductName
            // 
            this.ProductName.DataPropertyName = "ProductName";
            this.ProductName.HeaderText = "产品名称";
            this.ProductName.Name = "ProductName";
            this.ProductName.ReadOnly = true;
            // 
            // ProductType
            // 
            this.ProductType.DataPropertyName = "ProductType";
            this.ProductType.HeaderText = "产品类型";
            this.ProductType.Name = "ProductType";
            this.ProductType.ReadOnly = true;
            // 
            // taskText
            // 
            this.taskText.DataPropertyName = "taskText";
            this.taskText.HeaderText = "设计任务描述";
            this.taskText.Name = "taskText";
            this.taskText.ReadOnly = true;
            // 
            // Model
            // 
            this.Model.DataPropertyName = "Model";
            this.Model.HeaderText = "型号";
            this.Model.Name = "Model";
            this.Model.ReadOnly = true;
            // 
            // Specifications
            // 
            this.Specifications.DataPropertyName = "Specifications";
            this.Specifications.HeaderText = "规格";
            this.Specifications.Name = "Specifications";
            this.Specifications.ReadOnly = true;
            // 
            // Quantity
            // 
            this.Quantity.DataPropertyName = "Quantity";
            this.Quantity.HeaderText = "产品数量";
            this.Quantity.Name = "Quantity";
            this.Quantity.ReadOnly = true;
            // 
            // PartFigureCode
            // 
            this.PartFigureCode.DataPropertyName = "PartFigureCode";
            this.PartFigureCode.HeaderText = "零件图号";
            this.PartFigureCode.Name = "PartFigureCode";
            this.PartFigureCode.ReadOnly = true;
            // 
            // TaskDescription
            // 
            this.TaskDescription.DataPropertyName = "TaskDescription";
            this.TaskDescription.HeaderText = "设计结果描述";
            this.TaskDescription.Name = "TaskDescription";
            this.TaskDescription.ReadOnly = true;
            // 
            // PartName
            // 
            this.PartName.DataPropertyName = "PartName";
            this.PartName.HeaderText = "零件名称";
            this.PartName.Name = "PartName";
            this.PartName.ReadOnly = true;
            // 
            // PartCode
            // 
            this.PartCode.DataPropertyName = "PartCode";
            this.PartCode.HeaderText = "零件编号";
            this.PartCode.Name = "PartCode";
            this.PartCode.ReadOnly = true;
            this.PartCode.Visible = false;
            // 
            // ChooseRootPartCodeList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1110, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChooseRootPartCodeList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择项目零件";
            this.Load += new System.EventHandler(this.ChooseRootPartCodeList_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProjectPart)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnPrePage;
        private System.Windows.Forms.Button btnAfterPage;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtContractCode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPartFigureCode;
        private System.Windows.Forms.TextBox txtProjectName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtModel;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.DataGridView dgvProjectPart;
        private System.Windows.Forms.Label PageInfo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label PageTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContractCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductType;
        private System.Windows.Forms.DataGridViewTextBoxColumn taskText;
        private System.Windows.Forms.DataGridViewTextBoxColumn Model;
        private System.Windows.Forms.DataGridViewTextBoxColumn Specifications;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartFigureCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartCode;
    }
}