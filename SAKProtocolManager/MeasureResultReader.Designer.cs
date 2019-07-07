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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MeasureResultReader));
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
            this.TestInfoPanel = new System.Windows.Forms.GroupBox();
            this.BruttoWeightTextField = new System.Windows.Forms.NumericUpDown();
            this.EditSaveBruttoButton = new System.Windows.Forms.Button();
            this.TemperatureLbl = new System.Windows.Forms.Label();
            this.BruttoWeight = new System.Windows.Forms.Label();
            this.cableStructuresList = new System.Windows.Forms.ComboBox();
            this.StructuresLbl = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabControlTestResult = new System.Windows.Forms.TabControl();
            this.GeneratePDFProtocolButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.testedLengthInput = new System.Windows.Forms.NumericUpDown();
            this.lengthUpdProgressBarField = new System.Windows.Forms.Panel();
            this.procNameLbl = new System.Windows.Forms.Label();
            this.lengthUpdProgressBarLbl = new System.Windows.Forms.Label();
            this.LengthUpdProgressBar = new System.Windows.Forms.ProgressBar();
            this.OutOfNormaRsltPanel = new System.Windows.Forms.GroupBox();
            this.parameterTypeLbl = new System.Windows.Forms.Label();
            this.parameterTypeCB = new System.Windows.Forms.ComboBox();
            this.MSWordImport = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.measureResultReaderDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cableTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.measured_parameters_table_1)).BeginInit();
            this.TestInfoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BruttoWeightTextField)).BeginInit();
            this.tabControlTestResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.testedLengthInput)).BeginInit();
            this.lengthUpdProgressBarField.SuspendLayout();
            this.OutOfNormaRsltPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // cableTypeLbl
            // 
            this.cableTypeLbl.AutoSize = true;
            this.cableTypeLbl.Location = new System.Drawing.Point(23, 29);
            this.cableTypeLbl.Name = "cableTypeLbl";
            this.cableTypeLbl.Size = new System.Drawing.Size(75, 16);
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
            this.operatorLbl.Location = new System.Drawing.Point(382, 79);
            this.operatorLbl.Name = "operatorLbl";
            this.operatorLbl.Size = new System.Drawing.Size(90, 16);
            this.operatorLbl.TabIndex = 1;
            this.operatorLbl.Text = "Фамилия И.О.";
            // 
            // testedAtLbl
            // 
            this.testedAtLbl.AutoSize = true;
            this.testedAtLbl.Location = new System.Drawing.Point(23, 79);
            this.testedAtLbl.Name = "testedAtLbl";
            this.testedAtLbl.Size = new System.Drawing.Size(104, 16);
            this.testedAtLbl.TabIndex = 2;
            this.testedAtLbl.Text = "Дата испытания";
            // 
            // barabanLbl
            // 
            this.barabanLbl.AutoSize = true;
            this.barabanLbl.Location = new System.Drawing.Point(23, 53);
            this.barabanLbl.Name = "barabanLbl";
            this.barabanLbl.Size = new System.Drawing.Size(58, 16);
            this.barabanLbl.TabIndex = 3;
            this.barabanLbl.Text = "Барабан";
            // 
            // TestInfoPanel
            // 
            this.TestInfoPanel.Controls.Add(this.label2);
            this.TestInfoPanel.Controls.Add(this.testedLengthInput);
            this.TestInfoPanel.Controls.Add(this.BruttoWeightTextField);
            this.TestInfoPanel.Controls.Add(this.EditSaveBruttoButton);
            this.TestInfoPanel.Controls.Add(this.TemperatureLbl);
            this.TestInfoPanel.Controls.Add(this.BruttoWeight);
            this.TestInfoPanel.Controls.Add(this.cableTypeLbl);
            this.TestInfoPanel.Controls.Add(this.operatorLbl);
            this.TestInfoPanel.Controls.Add(this.testedAtLbl);
            this.TestInfoPanel.Controls.Add(this.barabanLbl);
            this.TestInfoPanel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TestInfoPanel.Location = new System.Drawing.Point(12, 12);
            this.TestInfoPanel.Name = "TestInfoPanel";
            this.TestInfoPanel.Size = new System.Drawing.Size(580, 136);
            this.TestInfoPanel.TabIndex = 4;
            this.TestInfoPanel.TabStop = false;
            this.TestInfoPanel.Text = "Информация о испытании";
            // 
            // BruttoWeightTextField
            // 
            this.BruttoWeightTextField.Location = new System.Drawing.Point(96, 102);
            this.BruttoWeightTextField.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.BruttoWeightTextField.Name = "BruttoWeightTextField";
            this.BruttoWeightTextField.Size = new System.Drawing.Size(111, 23);
            this.BruttoWeightTextField.TabIndex = 7;
            this.BruttoWeightTextField.ValueChanged += new System.EventHandler(this.BruttoWeightTextField_ValueChanged);
            this.BruttoWeightTextField.KeyUp += new System.Windows.Forms.KeyEventHandler(this.BruttoWeightTextField_ValueChanged);
            // 
            // EditSaveBruttoButton
            // 
            this.EditSaveBruttoButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.EditSaveBruttoButton.Location = new System.Drawing.Point(213, 102);
            this.EditSaveBruttoButton.Name = "EditSaveBruttoButton";
            this.EditSaveBruttoButton.Size = new System.Drawing.Size(94, 23);
            this.EditSaveBruttoButton.TabIndex = 6;
            this.EditSaveBruttoButton.Text = "Сохранить";
            this.EditSaveBruttoButton.UseVisualStyleBackColor = true;
            this.EditSaveBruttoButton.Click += new System.EventHandler(this.EditSaveBruttoButton_Click);
            // 
            // TemperatureLbl
            // 
            this.TemperatureLbl.AutoSize = true;
            this.TemperatureLbl.Location = new System.Drawing.Point(382, 29);
            this.TemperatureLbl.Name = "TemperatureLbl";
            this.TemperatureLbl.Size = new System.Drawing.Size(85, 16);
            this.TemperatureLbl.TabIndex = 5;
            this.TemperatureLbl.Text = "Температура";
            // 
            // BruttoWeight
            // 
            this.BruttoWeight.AutoSize = true;
            this.BruttoWeight.Location = new System.Drawing.Point(23, 106);
            this.BruttoWeight.Name = "BruttoWeight";
            this.BruttoWeight.Size = new System.Drawing.Size(67, 16);
            this.BruttoWeight.TabIndex = 4;
            this.BruttoWeight.Text = "Брутто, кг";
            // 
            // cableStructuresList
            // 
            this.cableStructuresList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cableStructuresList.FormattingEnabled = true;
            this.cableStructuresList.Location = new System.Drawing.Point(16, 47);
            this.cableStructuresList.Name = "cableStructuresList";
            this.cableStructuresList.Size = new System.Drawing.Size(291, 22);
            this.cableStructuresList.TabIndex = 6;
            this.cableStructuresList.SelectedIndexChanged += new System.EventHandler(this.cableStructuresList_SelectedIndexChanged);
            // 
            // StructuresLbl
            // 
            this.StructuresLbl.AutoSize = true;
            this.StructuresLbl.Location = new System.Drawing.Point(13, 31);
            this.StructuresLbl.Name = "StructuresLbl";
            this.StructuresLbl.Size = new System.Drawing.Size(64, 14);
            this.StructuresLbl.TabIndex = 7;
            this.StructuresLbl.Text = "Структура";
            // 
            // tabPage1
            // 
            this.tabPage1.AutoScroll = true;
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(771, 319);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.ToolTipText = "fgfgh";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabControlTestResult
            // 
            this.tabControlTestResult.Controls.Add(this.tabPage1);
            this.tabControlTestResult.Location = new System.Drawing.Point(16, 87);
            this.tabControlTestResult.Name = "tabControlTestResult";
            this.tabControlTestResult.SelectedIndex = 0;
            this.tabControlTestResult.Size = new System.Drawing.Size(779, 346);
            this.tabControlTestResult.TabIndex = 5;
            // 
            // GeneratePDFProtocolButton
            // 
            this.GeneratePDFProtocolButton.BackColor = System.Drawing.Color.LightYellow;
            this.GeneratePDFProtocolButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.GeneratePDFProtocolButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Ivory;
            this.GeneratePDFProtocolButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GeneratePDFProtocolButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GeneratePDFProtocolButton.Image = ((System.Drawing.Image)(resources.GetObject("GeneratePDFProtocolButton.Image")));
            this.GeneratePDFProtocolButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.GeneratePDFProtocolButton.Location = new System.Drawing.Point(620, 15);
            this.GeneratePDFProtocolButton.Name = "GeneratePDFProtocolButton";
            this.GeneratePDFProtocolButton.Size = new System.Drawing.Size(96, 48);
            this.GeneratePDFProtocolButton.TabIndex = 9;
            this.GeneratePDFProtocolButton.Text = " PDF";
            this.GeneratePDFProtocolButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.GeneratePDFProtocolButton.UseVisualStyleBackColor = false;
            this.GeneratePDFProtocolButton.Click += new System.EventHandler(this.GeneratePDFProtocolButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(321, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 16);
            this.label2.TabIndex = 11;
            this.label2.Text = "Длина, м";
            // 
            // testedLengthInput
            // 
            this.testedLengthInput.Location = new System.Drawing.Point(385, 102);
            this.testedLengthInput.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.testedLengthInput.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.testedLengthInput.Name = "testedLengthInput";
            this.testedLengthInput.Size = new System.Drawing.Size(138, 23);
            this.testedLengthInput.TabIndex = 12;
            this.testedLengthInput.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.testedLengthInput.ValueChanged += new System.EventHandler(this.testedLengthInput_ValueChanged);
            this.testedLengthInput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.testedLengthInput_KeyUp);
            // 
            // lengthUpdProgressBarField
            // 
            this.lengthUpdProgressBarField.Controls.Add(this.procNameLbl);
            this.lengthUpdProgressBarField.Controls.Add(this.lengthUpdProgressBarLbl);
            this.lengthUpdProgressBarField.Controls.Add(this.LengthUpdProgressBar);
            this.lengthUpdProgressBarField.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lengthUpdProgressBarField.Location = new System.Drawing.Point(615, 65);
            this.lengthUpdProgressBarField.Name = "lengthUpdProgressBarField";
            this.lengthUpdProgressBarField.Size = new System.Drawing.Size(209, 81);
            this.lengthUpdProgressBarField.TabIndex = 15;
            // 
            // procNameLbl
            // 
            this.procNameLbl.AutoSize = true;
            this.procNameLbl.Location = new System.Drawing.Point(4, 5);
            this.procNameLbl.Name = "procNameLbl";
            this.procNameLbl.Size = new System.Drawing.Size(131, 13);
            this.procNameLbl.TabIndex = 17;
            this.procNameLbl.Text = "Идёт пересчёт длины...";
            // 
            // lengthUpdProgressBarLbl
            // 
            this.lengthUpdProgressBarLbl.AutoSize = true;
            this.lengthUpdProgressBarLbl.Location = new System.Drawing.Point(4, 23);
            this.lengthUpdProgressBarLbl.Name = "lengthUpdProgressBarLbl";
            this.lengthUpdProgressBarLbl.Size = new System.Drawing.Size(165, 13);
            this.lengthUpdProgressBarLbl.TabIndex = 16;
            this.lengthUpdProgressBarLbl.Text = "Пересчитано 100000 из 100000";
            // 
            // LengthUpdProgressBar
            // 
            this.LengthUpdProgressBar.Location = new System.Drawing.Point(7, 41);
            this.LengthUpdProgressBar.Name = "LengthUpdProgressBar";
            this.LengthUpdProgressBar.Size = new System.Drawing.Size(199, 23);
            this.LengthUpdProgressBar.Step = 1;
            this.LengthUpdProgressBar.TabIndex = 16;
            // 
            // OutOfNormaRsltPanel
            // 
            this.OutOfNormaRsltPanel.Controls.Add(this.parameterTypeLbl);
            this.OutOfNormaRsltPanel.Controls.Add(this.parameterTypeCB);
            this.OutOfNormaRsltPanel.Controls.Add(this.cableStructuresList);
            this.OutOfNormaRsltPanel.Controls.Add(this.tabControlTestResult);
            this.OutOfNormaRsltPanel.Controls.Add(this.StructuresLbl);
            this.OutOfNormaRsltPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.OutOfNormaRsltPanel.Location = new System.Drawing.Point(12, 158);
            this.OutOfNormaRsltPanel.Name = "OutOfNormaRsltPanel";
            this.OutOfNormaRsltPanel.Size = new System.Drawing.Size(810, 453);
            this.OutOfNormaRsltPanel.TabIndex = 16;
            this.OutOfNormaRsltPanel.TabStop = false;
            this.OutOfNormaRsltPanel.Text = "Панель просмотра результата";
            // 
            // parameterTypeLbl
            // 
            this.parameterTypeLbl.AutoSize = true;
            this.parameterTypeLbl.Location = new System.Drawing.Point(326, 31);
            this.parameterTypeLbl.Name = "parameterTypeLbl";
            this.parameterTypeLbl.Size = new System.Drawing.Size(62, 14);
            this.parameterTypeLbl.TabIndex = 9;
            this.parameterTypeLbl.Text = "Параметр";
            // 
            // parameterTypeCB
            // 
            this.parameterTypeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameterTypeCB.FormattingEnabled = true;
            this.parameterTypeCB.Location = new System.Drawing.Point(329, 47);
            this.parameterTypeCB.Name = "parameterTypeCB";
            this.parameterTypeCB.Size = new System.Drawing.Size(143, 22);
            this.parameterTypeCB.TabIndex = 8;
            this.parameterTypeCB.SelectedIndexChanged += new System.EventHandler(this.parameterTypeCB_SelectedIndexChanged);
            // 
            // MSWordImport
            // 
            this.MSWordImport.Location = new System.Drawing.Point(722, 15);
            this.MSWordImport.Name = "MSWordImport";
            this.MSWordImport.Size = new System.Drawing.Size(97, 48);
            this.MSWordImport.TabIndex = 17;
            this.MSWordImport.Text = "MSWord";
            this.MSWordImport.UseVisualStyleBackColor = true;
            this.MSWordImport.Click += new System.EventHandler(this.MSWordImport_Click);
            // 
            // MeasureResultReader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(835, 615);
            this.Controls.Add(this.lengthUpdProgressBarField);
            this.Controls.Add(this.MSWordImport);
            this.Controls.Add(this.OutOfNormaRsltPanel);
            this.Controls.Add(this.GeneratePDFProtocolButton);
            this.Controls.Add(this.TestInfoPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MeasureResultReader";
            this.Text = "MeasureResultReader";
            ((System.ComponentModel.ISupportInitialize)(this.measureResultReaderDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cableTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.measured_parameters_table_1)).EndInit();
            this.TestInfoPanel.ResumeLayout(false);
            this.TestInfoPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BruttoWeightTextField)).EndInit();
            this.tabControlTestResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.testedLengthInput)).EndInit();
            this.lengthUpdProgressBarField.ResumeLayout(false);
            this.lengthUpdProgressBarField.PerformLayout();
            this.OutOfNormaRsltPanel.ResumeLayout(false);
            this.OutOfNormaRsltPanel.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.GroupBox TestInfoPanel;
        private System.Windows.Forms.ComboBox cableStructuresList;
        private System.Windows.Forms.Label StructuresLbl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl tabControlTestResult;
        private System.Windows.Forms.Button GeneratePDFProtocolButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown testedLengthInput;
        private System.Windows.Forms.Panel lengthUpdProgressBarField;
        private System.Windows.Forms.Label lengthUpdProgressBarLbl;
        private System.Windows.Forms.ProgressBar LengthUpdProgressBar;
        private System.Windows.Forms.Label procNameLbl;
        private System.Windows.Forms.Label BruttoWeight;
        private System.Windows.Forms.GroupBox OutOfNormaRsltPanel;
        private System.Windows.Forms.Label TemperatureLbl;
        private System.Windows.Forms.Label parameterTypeLbl;
        private System.Windows.Forms.ComboBox parameterTypeCB;
        private System.Windows.Forms.Button EditSaveBruttoButton;
        private System.Windows.Forms.NumericUpDown BruttoWeightTextField;
        private System.Windows.Forms.Button MSWordImport;
    }
}