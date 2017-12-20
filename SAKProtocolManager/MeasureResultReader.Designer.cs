﻿namespace SAKProtocolManager
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
            this.MeasuredParametersLbl = new System.Windows.Forms.Label();
            this.cableStructuresList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabControlTestResult = new System.Windows.Forms.TabControl();
            this.GeneratePDFProtocolButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.testedLengthInput = new System.Windows.Forms.NumericUpDown();
            this.updateCableLength = new System.Windows.Forms.Button();
            this.lengthEditor = new System.Windows.Forms.Panel();
            this.lengthUpdProgressBarField = new System.Windows.Forms.Panel();
            this.procNameLbl = new System.Windows.Forms.Label();
            this.lengthUpdProgressBarLbl = new System.Windows.Forms.Label();
            this.LengthUpdProgressBar = new System.Windows.Forms.ProgressBar();
            this.OutOfNormaRsltPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.measureResultReaderDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cableTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.measured_parameters_table_1)).BeginInit();
            this.TestInfoPanel.SuspendLayout();
            this.tabControlTestResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.testedLengthInput)).BeginInit();
            this.lengthEditor.SuspendLayout();
            this.lengthUpdProgressBarField.SuspendLayout();
            this.OutOfNormaRsltPanel.SuspendLayout();
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
            this.operatorLbl.Location = new System.Drawing.Point(382, 79);
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
            // TestInfoPanel
            // 
            this.TestInfoPanel.Controls.Add(this.MeasuredParametersLbl);
            this.TestInfoPanel.Controls.Add(this.cableTypeLbl);
            this.TestInfoPanel.Controls.Add(this.operatorLbl);
            this.TestInfoPanel.Controls.Add(this.testedAtLbl);
            this.TestInfoPanel.Controls.Add(this.barabanLbl);
            this.TestInfoPanel.Location = new System.Drawing.Point(12, 12);
            this.TestInfoPanel.Name = "TestInfoPanel";
            this.TestInfoPanel.Size = new System.Drawing.Size(591, 136);
            this.TestInfoPanel.TabIndex = 4;
            this.TestInfoPanel.TabStop = false;
            this.TestInfoPanel.Text = "Информация о испытании";
            // 
            // MeasuredParametersLbl
            // 
            this.MeasuredParametersLbl.AutoSize = true;
            this.MeasuredParametersLbl.Location = new System.Drawing.Point(23, 106);
            this.MeasuredParametersLbl.Name = "MeasuredParametersLbl";
            this.MeasuredParametersLbl.Size = new System.Drawing.Size(136, 13);
            this.MeasuredParametersLbl.TabIndex = 4;
            this.MeasuredParametersLbl.Text = "Измеренные параметры:";
            // 
            // cableStructuresList
            // 
            this.cableStructuresList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cableStructuresList.FormattingEnabled = true;
            this.cableStructuresList.Location = new System.Drawing.Point(3, 30);
            this.cableStructuresList.Name = "cableStructuresList";
            this.cableStructuresList.Size = new System.Drawing.Size(291, 21);
            this.cableStructuresList.TabIndex = 6;
            this.cableStructuresList.SelectedIndexChanged += new System.EventHandler(this.cableStructuresList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Структуры с выходом за норму";
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
            this.tabControlTestResult.Location = new System.Drawing.Point(3, 66);
            this.tabControlTestResult.Name = "tabControlTestResult";
            this.tabControlTestResult.SelectedIndex = 0;
            this.tabControlTestResult.Size = new System.Drawing.Size(794, 617);
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
            this.GeneratePDFProtocolButton.Location = new System.Drawing.Point(616, 17);
            this.GeneratePDFProtocolButton.Name = "GeneratePDFProtocolButton";
            this.GeneratePDFProtocolButton.Size = new System.Drawing.Size(199, 48);
            this.GeneratePDFProtocolButton.TabIndex = 9;
            this.GeneratePDFProtocolButton.Text = "экспорт в PDF";
            this.GeneratePDFProtocolButton.UseVisualStyleBackColor = false;
            this.GeneratePDFProtocolButton.Click += new System.EventHandler(this.GeneratePDFProtocolButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Длина, м";
            // 
            // testedLengthInput
            // 
            this.testedLengthInput.Location = new System.Drawing.Point(68, 7);
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
            this.testedLengthInput.Size = new System.Drawing.Size(138, 20);
            this.testedLengthInput.TabIndex = 12;
            this.testedLengthInput.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.testedLengthInput.ValueChanged += new System.EventHandler(this.testedLengthInput_ValueChanged);
            this.testedLengthInput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.testedLengthInput_KeyUp);
            // 
            // updateCableLength
            // 
            this.updateCableLength.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.updateCableLength.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.updateCableLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.updateCableLength.Image = ((System.Drawing.Image)(resources.GetObject("updateCableLength.Image")));
            this.updateCableLength.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.updateCableLength.Location = new System.Drawing.Point(7, 36);
            this.updateCableLength.Name = "updateCableLength";
            this.updateCableLength.Size = new System.Drawing.Size(199, 40);
            this.updateCableLength.TabIndex = 13;
            this.updateCableLength.Text = "пересчитать";
            this.updateCableLength.UseVisualStyleBackColor = true;
            this.updateCableLength.Click += new System.EventHandler(this.updateCableLength_Click);
            // 
            // lengthEditor
            // 
            this.lengthEditor.Controls.Add(this.testedLengthInput);
            this.lengthEditor.Controls.Add(this.updateCableLength);
            this.lengthEditor.Controls.Add(this.label2);
            this.lengthEditor.Location = new System.Drawing.Point(609, 71);
            this.lengthEditor.Name = "lengthEditor";
            this.lengthEditor.Size = new System.Drawing.Size(209, 81);
            this.lengthEditor.TabIndex = 14;
            // 
            // lengthUpdProgressBarField
            // 
            this.lengthUpdProgressBarField.Controls.Add(this.procNameLbl);
            this.lengthUpdProgressBarField.Controls.Add(this.lengthUpdProgressBarLbl);
            this.lengthUpdProgressBarField.Controls.Add(this.LengthUpdProgressBar);
            this.lengthUpdProgressBarField.Location = new System.Drawing.Point(609, 67);
            this.lengthUpdProgressBarField.Name = "lengthUpdProgressBarField";
            this.lengthUpdProgressBarField.Size = new System.Drawing.Size(209, 81);
            this.lengthUpdProgressBarField.TabIndex = 15;
            // 
            // procNameLbl
            // 
            this.procNameLbl.AutoSize = true;
            this.procNameLbl.Location = new System.Drawing.Point(4, 5);
            this.procNameLbl.Name = "procNameLbl";
            this.procNameLbl.Size = new System.Drawing.Size(125, 13);
            this.procNameLbl.TabIndex = 17;
            this.procNameLbl.Text = "Идёт пересчёт длины...";
            // 
            // lengthUpdProgressBarLbl
            // 
            this.lengthUpdProgressBarLbl.AutoSize = true;
            this.lengthUpdProgressBarLbl.Location = new System.Drawing.Point(4, 23);
            this.lengthUpdProgressBarLbl.Name = "lengthUpdProgressBarLbl";
            this.lengthUpdProgressBarLbl.Size = new System.Drawing.Size(166, 13);
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
            this.OutOfNormaRsltPanel.Controls.Add(this.label1);
            this.OutOfNormaRsltPanel.Controls.Add(this.cableStructuresList);
            this.OutOfNormaRsltPanel.Controls.Add(this.tabControlTestResult);
            this.OutOfNormaRsltPanel.Location = new System.Drawing.Point(12, 154);
            this.OutOfNormaRsltPanel.Name = "OutOfNormaRsltPanel";
            this.OutOfNormaRsltPanel.Size = new System.Drawing.Size(806, 689);
            this.OutOfNormaRsltPanel.TabIndex = 16;
            // 
            // MeasureResultReader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 820);
            this.Controls.Add(this.lengthUpdProgressBarField);
            this.Controls.Add(this.OutOfNormaRsltPanel);
            this.Controls.Add(this.lengthEditor);
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
            this.tabControlTestResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.testedLengthInput)).EndInit();
            this.lengthEditor.ResumeLayout(false);
            this.lengthEditor.PerformLayout();
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl tabControlTestResult;
        private System.Windows.Forms.Button GeneratePDFProtocolButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown testedLengthInput;
        private System.Windows.Forms.Button updateCableLength;
        private System.Windows.Forms.Panel lengthEditor;
        private System.Windows.Forms.Panel lengthUpdProgressBarField;
        private System.Windows.Forms.Label lengthUpdProgressBarLbl;
        private System.Windows.Forms.ProgressBar LengthUpdProgressBar;
        private System.Windows.Forms.Label procNameLbl;
        private System.Windows.Forms.Panel OutOfNormaRsltPanel;
        private System.Windows.Forms.Label MeasuredParametersLbl;
    }
}