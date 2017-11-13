namespace SAKProtocolManager
{
    partial class MeasureResultReader
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
            this.cableTypeLbl = new System.Windows.Forms.Label();
            this.measureResultReaderDataSet = new System.Data.DataSet();
            this.cableTable = new System.Data.DataTable();
            this.dataColumn1 = new System.Data.DataColumn();
            this.dataColumn2 = new System.Data.DataColumn();
            this.dataColumn4 = new System.Data.DataColumn();
            this.measured_parameters_table_1 = new System.Data.DataTable();
            this.mpId = new System.Data.DataColumn();
            this.mpName = new System.Data.DataColumn();
            this.mpMeasureName = new System.Data.DataColumn();
            this.mpDescription = new System.Data.DataColumn();
            this.operatorLbl = new System.Windows.Forms.Label();
            this.testedAtLbl = new System.Windows.Forms.Label();
            this.barabanLbl = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cableStructuresList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabControlTestResult = new System.Windows.Forms.TabControl();
            this.GeneratePDFProtocolButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.measureResultReaderDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cableTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.measured_parameters_table_1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabControlTestResult.SuspendLayout();
            this.SuspendLayout();
            // 
            // cableTypeLbl
            // 
            this.cableTypeLbl.AutoSize = true;
            this.cableTypeLbl.Location = new System.Drawing.Point(23, 29);
            this.cableTypeLbl.Name = "cableTypeLbl";
            this.cableTypeLbl.Size = new System.Drawing.Size(65, 13);
            this.cableTypeLbl.TabIndex = 0;
            this.cableTypeLbl.Text = "Тип кабеля";
            // 
            // measureResultReaderDataSet
            // 
            this.measureResultReaderDataSet.DataSetName = "NewDataSet";
            this.measureResultReaderDataSet.Tables.AddRange(new System.Data.DataTable[] {
            this.cableTable,
            this.measured_parameters_table_1});
            // 
            // cableTable
            // 
            this.cableTable.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn1,
            this.dataColumn2,
            this.dataColumn4});
            this.cableTable.TableName = "cable";
            // 
            // dataColumn1
            // 
            this.dataColumn1.ColumnName = "id";
            // 
            // dataColumn2
            // 
            this.dataColumn2.ColumnName = "name";
            // 
            // dataColumn4
            // 
            this.dataColumn4.ColumnName = "build_length";
            // 
            // measured_parameters_table_1
            // 
            this.measured_parameters_table_1.Columns.AddRange(new System.Data.DataColumn[] {
            this.mpId,
            this.mpName,
            this.mpMeasureName,
            this.mpDescription});
            this.measured_parameters_table_1.TableName = "measured_parameters";
            // 
            // mpId
            // 
            this.mpId.ColumnName = "id";
            // 
            // mpName
            // 
            this.mpName.ColumnName = "name";
            // 
            // mpMeasureName
            // 
            this.mpMeasureName.ColumnName = "measure";
            // 
            // mpDescription
            // 
            this.mpDescription.ColumnName = "description";
            // 
            // operatorLbl
            // 
            this.operatorLbl.AutoSize = true;
            this.operatorLbl.Location = new System.Drawing.Point(212, 79);
            this.operatorLbl.Name = "operatorLbl";
            this.operatorLbl.Size = new System.Drawing.Size(81, 13);
            this.operatorLbl.TabIndex = 1;
            this.operatorLbl.Text = "Фамилия И.О.";
            // 
            // testedAtLbl
            // 
            this.testedAtLbl.AutoSize = true;
            this.testedAtLbl.Location = new System.Drawing.Point(23, 79);
            this.testedAtLbl.Name = "testedAtLbl";
            this.testedAtLbl.Size = new System.Drawing.Size(91, 13);
            this.testedAtLbl.TabIndex = 2;
            this.testedAtLbl.Text = "Дата испытания";
            // 
            // barabanLbl
            // 
            this.barabanLbl.AutoSize = true;
            this.barabanLbl.Location = new System.Drawing.Point(23, 53);
            this.barabanLbl.Name = "barabanLbl";
            this.barabanLbl.Size = new System.Drawing.Size(50, 13);
            this.barabanLbl.TabIndex = 3;
            this.barabanLbl.Text = "Барабан";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cableTypeLbl);
            this.groupBox1.Controls.Add(this.operatorLbl);
            this.groupBox1.Controls.Add(this.testedAtLbl);
            this.groupBox1.Controls.Add(this.barabanLbl);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(382, 108);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Информация о испытании";
            // 
            // cableStructuresList
            // 
            this.cableStructuresList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cableStructuresList.FormattingEnabled = true;
            this.cableStructuresList.Location = new System.Drawing.Point(103, 164);
            this.cableStructuresList.Name = "cableStructuresList";
            this.cableStructuresList.Size = new System.Drawing.Size(291, 21);
            this.cableStructuresList.TabIndex = 6;
            this.cableStructuresList.SelectedIndexChanged += new System.EventHandler(this.cableStructuresList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 168);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Тип структуры";
            // 
            // tabPage1
            // 
            this.tabPage1.AutoScroll = true;
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(786, 591);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.ToolTipText = "fgfgh";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabControlTestResult
            // 
            this.tabControlTestResult.Controls.Add(this.tabPage1);
            this.tabControlTestResult.Location = new System.Drawing.Point(12, 210);
            this.tabControlTestResult.Name = "tabControlTestResult";
            this.tabControlTestResult.SelectedIndex = 0;
            this.tabControlTestResult.Size = new System.Drawing.Size(794, 617);
            this.tabControlTestResult.TabIndex = 5;
            // 
            // GeneratePDFProtocolButton
            // 
            this.GeneratePDFProtocolButton.Location = new System.Drawing.Point(417, 12);
            this.GeneratePDFProtocolButton.Name = "GeneratePDFProtocolButton";
            this.GeneratePDFProtocolButton.Size = new System.Drawing.Size(103, 33);
            this.GeneratePDFProtocolButton.TabIndex = 9;
            this.GeneratePDFProtocolButton.Text = "Импорт в PDF";
            this.GeneratePDFProtocolButton.UseVisualStyleBackColor = true;
            this.GeneratePDFProtocolButton.Click += new System.EventHandler(this.GeneratePDFProtocolButton_Click);
            // 
            // MeasureResultReader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 820);
            this.Controls.Add(this.GeneratePDFProtocolButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cableStructuresList);
            this.Controls.Add(this.tabControlTestResult);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MeasureResultReader";
            this.Text = "MeasureResultReader";
            ((System.ComponentModel.ISupportInitialize)(this.measureResultReaderDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cableTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.measured_parameters_table_1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControlTestResult.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label cableTypeLbl;
        private System.Data.DataSet measureResultReaderDataSet;
        private System.Data.DataTable cableTable;
        private System.Data.DataColumn dataColumn1;
        private System.Data.DataColumn dataColumn2;
        private System.Data.DataColumn dataColumn4;
        private System.Data.DataTable measured_parameters_table_1;
        private System.Data.DataColumn mpId;
        private System.Data.DataColumn mpName;
        private System.Data.DataColumn mpMeasureName;
        private System.Data.DataColumn mpDescription;
        private System.Windows.Forms.Label operatorLbl;
        private System.Windows.Forms.Label testedAtLbl;
        private System.Windows.Forms.Label barabanLbl;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cableStructuresList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl tabControlTestResult;
        private System.Windows.Forms.Button GeneratePDFProtocolButton;
    }
}