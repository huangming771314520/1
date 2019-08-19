namespace Zephyr.WinForm.Content
{
    partial class PrintBarcodeForm
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblCorrespondingBarcode = new System.Windows.Forms.Label();
            this.lblTypeName = new System.Windows.Forms.Label();
            this.lblPartName = new System.Windows.Forms.Label();
            this.lblModel = new System.Windows.Forms.Label();
            this.lblPartCode = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.nudPrintNum = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.lvwPrintInfo = new System.Windows.Forms.ListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.txtPartCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPartType = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dgvPartDataShow = new System.Windows.Forms.DataGridView();
            this.chkSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Model = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Spec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Weight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SafetyStock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaxStock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MinStock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QuantityUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FigureCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QualityCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CorrespondingBarcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox3.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPrintNum)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPartDataShow)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.panel3);
            this.groupBox3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox3.Location = new System.Drawing.Point(9, 520);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox3.Size = new System.Drawing.Size(545, 224);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "详情";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblCorrespondingBarcode);
            this.panel3.Controls.Add(this.lblTypeName);
            this.panel3.Controls.Add(this.lblPartName);
            this.panel3.Controls.Add(this.lblModel);
            this.panel3.Controls.Add(this.lblPartCode);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.nudPrintNum);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel3.Location = new System.Drawing.Point(7, 38);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(530, 121);
            this.panel3.TabIndex = 1;
            // 
            // lblCorrespondingBarcode
            // 
            this.lblCorrespondingBarcode.AutoSize = true;
            this.lblCorrespondingBarcode.Location = new System.Drawing.Point(108, 84);
            this.lblCorrespondingBarcode.Name = "lblCorrespondingBarcode";
            this.lblCorrespondingBarcode.Size = new System.Drawing.Size(76, 20);
            this.lblCorrespondingBarcode.TabIndex = 7;
            this.lblCorrespondingBarcode.Text = "tempInfo";
            // 
            // lblTypeName
            // 
            this.lblTypeName.AutoSize = true;
            this.lblTypeName.Location = new System.Drawing.Point(352, 49);
            this.lblTypeName.Name = "lblTypeName";
            this.lblTypeName.Size = new System.Drawing.Size(76, 20);
            this.lblTypeName.TabIndex = 7;
            this.lblTypeName.Text = "tempInfo";
            // 
            // lblPartName
            // 
            this.lblPartName.AutoSize = true;
            this.lblPartName.Location = new System.Drawing.Point(108, 49);
            this.lblPartName.Name = "lblPartName";
            this.lblPartName.Size = new System.Drawing.Size(76, 20);
            this.lblPartName.TabIndex = 7;
            this.lblPartName.Text = "tempInfo";
            // 
            // lblModel
            // 
            this.lblModel.AutoSize = true;
            this.lblModel.Location = new System.Drawing.Point(349, 14);
            this.lblModel.Name = "lblModel";
            this.lblModel.Size = new System.Drawing.Size(76, 20);
            this.lblModel.TabIndex = 7;
            this.lblModel.Text = "tempInfo";
            // 
            // lblPartCode
            // 
            this.lblPartCode.AutoSize = true;
            this.lblPartCode.Location = new System.Drawing.Point(108, 14);
            this.lblPartCode.Name = "lblPartCode";
            this.lblPartCode.Size = new System.Drawing.Size(76, 20);
            this.lblPartCode.TabIndex = 7;
            this.lblPartCode.Text = "tempInfo";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 84);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 20);
            this.label8.TabIndex = 6;
            this.label8.Text = "对应条码:";
            // 
            // nudPrintNum
            // 
            this.nudPrintNum.Location = new System.Drawing.Point(352, 82);
            this.nudPrintNum.Name = "nudPrintNum";
            this.nudPrintNum.Size = new System.Drawing.Size(120, 27);
            this.nudPrintNum.TabIndex = 3;
            this.nudPrintNum.ValueChanged += new System.EventHandler(this.nudPrintNum_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(271, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 20);
            this.label7.TabIndex = 5;
            this.label7.Text = "零件类型:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(271, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 20);
            this.label5.TabIndex = 2;
            this.label5.Text = "打印数量:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(271, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 20);
            this.label6.TabIndex = 4;
            this.label6.Text = "型号:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 20);
            this.label4.TabIndex = 1;
            this.label4.Text = "零件名称:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "零件编码:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnPrint);
            this.groupBox2.Controls.Add(this.lvwPrintInfo);
            this.groupBox2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(566, 520);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox2.Size = new System.Drawing.Size(607, 224);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "打印信息";
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrint.Location = new System.Drawing.Point(478, 185);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(87, 28);
            this.btnPrint.TabIndex = 1;
            this.btnPrint.Text = "打印";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // lvwPrintInfo
            // 
            this.lvwPrintInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwPrintInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lvwPrintInfo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lvwPrintInfo.FullRowSelect = true;
            this.lvwPrintInfo.GridLines = true;
            this.lvwPrintInfo.HideSelection = false;
            this.lvwPrintInfo.Location = new System.Drawing.Point(3, 34);
            this.lvwPrintInfo.Margin = new System.Windows.Forms.Padding(0);
            this.lvwPrintInfo.MultiSelect = false;
            this.lvwPrintInfo.Name = "lvwPrintInfo";
            this.lvwPrintInfo.Size = new System.Drawing.Size(600, 141);
            this.lvwPrintInfo.TabIndex = 0;
            this.lvwPrintInfo.UseCompatibleStateImageBehavior = false;
            this.lvwPrintInfo.View = System.Windows.Forms.View.Details;
            this.lvwPrintInfo.Click += new System.EventHandler(this.lvwPrintInfo_Click);
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "序号";
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "编码";
            this.columnHeader1.Width = 170;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "名称";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "对应条码";
            this.columnHeader3.Width = 160;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "打印数量";
            this.columnHeader4.Width = 100;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(9, 9);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox1.Size = new System.Drawing.Size(1164, 60);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "检索条件";
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.btnReset);
            this.panel1.Controls.Add(this.txtPartCode);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtPartType);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.Location = new System.Drawing.Point(143, 14);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(879, 40);
            this.panel1.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(770, 6);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 28);
            this.btnSearch.TabIndex = 13;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(675, 6);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 28);
            this.btnReset.TabIndex = 12;
            this.btnReset.Text = "重置";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // txtPartCode
            // 
            this.txtPartCode.Location = new System.Drawing.Point(128, 8);
            this.txtPartCode.Name = "txtPartCode";
            this.txtPartCode.Size = new System.Drawing.Size(206, 27);
            this.txtPartCode.TabIndex = 9;
            this.txtPartCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "零件编码:";
            // 
            // txtPartType
            // 
            this.txtPartType.Location = new System.Drawing.Point(449, 8);
            this.txtPartType.Name = "txtPartType";
            this.txtPartType.Size = new System.Drawing.Size(206, 27);
            this.txtPartType.TabIndex = 11;
            this.txtPartType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(354, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 20);
            this.label3.TabIndex = 10;
            this.label3.Text = "零件类型:";
            // 
            // dgvPartDataShow
            // 
            this.dgvPartDataShow.AllowUserToAddRows = false;
            this.dgvPartDataShow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPartDataShow.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPartDataShow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPartDataShow.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chkSelect,
            this.ID,
            this.PartCode,
            this.PartName,
            this.TypeName,
            this.Model,
            this.Spec,
            this.Weight,
            this.SafetyStock,
            this.MaxStock,
            this.MinStock,
            this.QuantityUnit,
            this.FigureCode,
            this.QualityCode,
            this.CorrespondingBarcode});
            this.dgvPartDataShow.Location = new System.Drawing.Point(9, 80);
            this.dgvPartDataShow.Margin = new System.Windows.Forms.Padding(0);
            this.dgvPartDataShow.Name = "dgvPartDataShow";
            this.dgvPartDataShow.RowTemplate.Height = 27;
            this.dgvPartDataShow.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPartDataShow.Size = new System.Drawing.Size(1164, 426);
            this.dgvPartDataShow.TabIndex = 10;
            this.dgvPartDataShow.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPartDataShow_CellClick);
            // 
            // chkSelect
            // 
            this.chkSelect.HeaderText = "选择";
            this.chkSelect.Name = "chkSelect";
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ID";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.Visible = false;
            // 
            // PartCode
            // 
            this.PartCode.DataPropertyName = "PartCode";
            this.PartCode.HeaderText = "零件编码";
            this.PartCode.Name = "PartCode";
            // 
            // PartName
            // 
            this.PartName.DataPropertyName = "PartName";
            this.PartName.HeaderText = "零件名称";
            this.PartName.Name = "PartName";
            // 
            // TypeName
            // 
            this.TypeName.DataPropertyName = "TypeName";
            this.TypeName.HeaderText = "零件类型";
            this.TypeName.Name = "TypeName";
            // 
            // Model
            // 
            this.Model.DataPropertyName = "Model";
            this.Model.HeaderText = "型号";
            this.Model.Name = "Model";
            // 
            // Spec
            // 
            this.Spec.DataPropertyName = "Spec";
            this.Spec.HeaderText = "规格";
            this.Spec.Name = "Spec";
            this.Spec.Visible = false;
            // 
            // Weight
            // 
            this.Weight.DataPropertyName = "Weight";
            this.Weight.HeaderText = "重量";
            this.Weight.Name = "Weight";
            // 
            // SafetyStock
            // 
            this.SafetyStock.DataPropertyName = "SafetyStock";
            this.SafetyStock.HeaderText = "安全库存";
            this.SafetyStock.Name = "SafetyStock";
            this.SafetyStock.Visible = false;
            // 
            // MaxStock
            // 
            this.MaxStock.DataPropertyName = "MaxStock";
            this.MaxStock.HeaderText = "最高库存";
            this.MaxStock.Name = "MaxStock";
            this.MaxStock.Visible = false;
            // 
            // MinStock
            // 
            this.MinStock.DataPropertyName = "MinStock";
            this.MinStock.HeaderText = "最低库存";
            this.MinStock.Name = "MinStock";
            this.MinStock.Visible = false;
            // 
            // QuantityUnit
            // 
            this.QuantityUnit.DataPropertyName = "QuantityUnit";
            this.QuantityUnit.HeaderText = "计量单位";
            this.QuantityUnit.Name = "QuantityUnit";
            // 
            // FigureCode
            // 
            this.FigureCode.DataPropertyName = "FigureCode";
            this.FigureCode.HeaderText = "图号";
            this.FigureCode.Name = "FigureCode";
            // 
            // QualityCode
            // 
            this.QualityCode.DataPropertyName = "QualityCode";
            this.QualityCode.HeaderText = "质检批号";
            this.QualityCode.Name = "QualityCode";
            // 
            // CorrespondingBarcode
            // 
            this.CorrespondingBarcode.DataPropertyName = "CorrespondingBarcode";
            this.CorrespondingBarcode.HeaderText = "对应条码";
            this.CorrespondingBarcode.Name = "CorrespondingBarcode";
            // 
            // PrintBarcodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1182, 753);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dgvPartDataShow);
            this.Name = "PrintBarcodeForm";
            this.Text = "打印条码";
            this.Load += new System.EventHandler(this.PrintBarcodeForm_Load);
            this.groupBox3.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPrintNum)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPartDataShow)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblCorrespondingBarcode;
        private System.Windows.Forms.Label lblTypeName;
        private System.Windows.Forms.Label lblPartName;
        private System.Windows.Forms.Label lblModel;
        private System.Windows.Forms.Label lblPartCode;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown nudPrintNum;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.ListView lvwPrintInfo;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.TextBox txtPartCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPartType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgvPartDataShow;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chkSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TypeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Model;
        private System.Windows.Forms.DataGridViewTextBoxColumn Spec;
        private System.Windows.Forms.DataGridViewTextBoxColumn Weight;
        private System.Windows.Forms.DataGridViewTextBoxColumn SafetyStock;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaxStock;
        private System.Windows.Forms.DataGridViewTextBoxColumn MinStock;
        private System.Windows.Forms.DataGridViewTextBoxColumn QuantityUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn FigureCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn QualityCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn CorrespondingBarcode;
    }
}