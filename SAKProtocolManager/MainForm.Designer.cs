namespace SAKProtocolManager
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.dataSetTest = new System.Data.DataSet();
            this.ispytan = new System.Data.DataTable();
            this.test_id = new System.Data.DataColumn();
            this.IspData = new System.Data.DataColumn();
            this.cable_name = new System.Data.DataColumn();
            this.cable_length = new System.Data.DataColumn();
            this.baraban_number = new System.Data.DataColumn();
            this.brutto = new System.Data.DataColumn();
            this.dataColumn3 = new System.Data.DataColumn();
            this.dataColumn4 = new System.Data.DataColumn();
            this.date_range = new System.Data.DataTable();
            this.dataColumn1 = new System.Data.DataColumn();
            this.dataColumn2 = new System.Data.DataColumn();
            this.testsListView = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tested_at = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cable_mark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BruttoWeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cable_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.build_length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.testListContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openMeasureResultReaderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToPDFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimeFrom = new System.Windows.Forms.DateTimePicker();
            this.dateTimeTo = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.SearchButton = new System.Windows.Forms.Button();
            this.ClearList = new System.Windows.Forms.Button();
            this.progressBarLbl = new System.Windows.Forms.Label();
            this.progressBarTest = new System.Windows.Forms.ProgressBar();
            this.progressBarPanel = new System.Windows.Forms.Panel();
            this.TestListtPanel = new System.Windows.Forms.Panel();
            this.searchPanel = new System.Windows.Forms.Panel();
            this.testIdField = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.byTestId = new System.Windows.Forms.RadioButton();
            this.byDate = new System.Windows.Forms.RadioButton();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.topMenu = new System.Windows.Forms.MenuStrip();
            this.TestHistoryItemsToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.statusPanel = new System.Windows.Forms.StatusStrip();
            this.selectedCountLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.OpenRegForm = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetTest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ispytan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.date_range)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.testsListView)).BeginInit();
            this.testListContextMenu.SuspendLayout();
            this.progressBarPanel.SuspendLayout();
            this.TestListtPanel.SuspendLayout();
            this.searchPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.testIdField)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.controlPanel.SuspendLayout();
            this.topMenu.SuspendLayout();
            this.statusPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataSetTest
            // 
            this.dataSetTest.DataSetName = "NewDataSet";
            this.dataSetTest.Tables.AddRange(new System.Data.DataTable[] {
            this.ispytan,
            this.date_range});
            // 
            // ispytan
            // 
            this.ispytan.Columns.AddRange(new System.Data.DataColumn[] {
            this.test_id,
            this.IspData,
            this.cable_name,
            this.cable_length,
            this.baraban_number,
            this.brutto,
            this.dataColumn3,
            this.dataColumn4});
            this.ispytan.Constraints.AddRange(new System.Data.Constraint[] {
            new System.Data.UniqueConstraint("Constraint1", new string[] {
                        "id"}, false)});
            this.ispytan.TableName = "ispytan";
            // 
            // test_id
            // 
            this.test_id.ColumnName = "id";
            // 
            // IspData
            // 
            this.IspData.ColumnName = "tested_at";
            // 
            // cable_name
            // 
            this.cable_name.ColumnName = "cable_name";
            // 
            // cable_length
            // 
            this.cable_length.Caption = "cable_length";
            this.cable_length.ColumnName = "cable_length";
            // 
            // baraban_number
            // 
            this.baraban_number.ColumnName = "baraban_number";
            // 
            // brutto
            // 
            this.brutto.ColumnName = "brutto";
            // 
            // dataColumn3
            // 
            this.dataColumn3.ColumnName = "build_length";
            // 
            // dataColumn4
            // 
            this.dataColumn4.ColumnName = "cable_id";
            // 
            // date_range
            // 
            this.date_range.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn1,
            this.dataColumn2});
            this.date_range.TableName = "date_range";
            // 
            // dataColumn1
            // 
            this.dataColumn1.ColumnName = "max_date";
            this.dataColumn1.DataType = typeof(System.DateTime);
            // 
            // dataColumn2
            // 
            this.dataColumn2.ColumnName = "min_date";
            this.dataColumn2.DataType = typeof(System.DateTime);
            // 
            // testsListView
            // 
            this.testsListView.AutoGenerateColumns = false;
            this.testsListView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.testsListView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.testsListView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.testsListView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.testsListView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.testsListView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.tested_at,
            this.cable_mark,
            this.length,
            this.bNumber,
            this.BruttoWeight,
            this.cable_id,
            this.build_length});
            this.testsListView.ContextMenuStrip = this.testListContextMenu;
            this.testsListView.DataSource = this.dataSetTest;
            this.testsListView.Location = new System.Drawing.Point(25, 89);
            this.testsListView.MultiSelect = false;
            this.testsListView.Name = "testsListView";
            this.testsListView.ReadOnly = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.testsListView.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.MintCream;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Teal;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.OldLace;
            this.testsListView.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.testsListView.RowTemplate.Height = 26;
            this.testsListView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.testsListView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.testsListView.Size = new System.Drawing.Size(970, 486);
            this.testsListView.TabIndex = 0;
            this.testsListView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.OpenButtonToolStripMenuItem_Click);
            this.testsListView.SelectionChanged += new System.EventHandler(this.testsListView_SelectionChanged);
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "Номер испытания";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Width = 114;
            // 
            // tested_at
            // 
            this.tested_at.DataPropertyName = "tested_at";
            this.tested_at.HeaderText = "Дата и время";
            this.tested_at.Name = "tested_at";
            this.tested_at.ReadOnly = true;
            this.tested_at.Width = 94;
            // 
            // cable_mark
            // 
            this.cable_mark.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cable_mark.DataPropertyName = "cable_name";
            this.cable_mark.HeaderText = "Марка кабеля";
            this.cable_mark.Name = "cable_mark";
            this.cable_mark.ReadOnly = true;
            // 
            // length
            // 
            this.length.DataPropertyName = "cable_length";
            this.length.HeaderText = "Длина кабеля";
            this.length.Name = "length";
            this.length.ReadOnly = true;
            this.length.Width = 96;
            // 
            // bNumber
            // 
            this.bNumber.DataPropertyName = "baraban_number";
            this.bNumber.HeaderText = "Номер барабана";
            this.bNumber.Name = "bNumber";
            this.bNumber.ReadOnly = true;
            this.bNumber.Width = 107;
            // 
            // BruttoWeight
            // 
            this.BruttoWeight.DataPropertyName = "brutto";
            this.BruttoWeight.HeaderText = "Брутто";
            this.BruttoWeight.Name = "BruttoWeight";
            this.BruttoWeight.ReadOnly = true;
            this.BruttoWeight.Width = 66;
            // 
            // cable_id
            // 
            this.cable_id.DataPropertyName = "cable_id";
            this.cable_id.HeaderText = "cable_id";
            this.cable_id.Name = "cable_id";
            this.cable_id.ReadOnly = true;
            this.cable_id.Visible = false;
            this.cable_id.Width = 72;
            // 
            // build_length
            // 
            this.build_length.DataPropertyName = "build_length";
            this.build_length.HeaderText = "build_length";
            this.build_length.Name = "build_length";
            this.build_length.ReadOnly = true;
            this.build_length.Visible = false;
            this.build_length.Width = 89;
            // 
            // testListContextMenu
            // 
            this.testListContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMeasureResultReaderToolStripMenuItem,
            this.exportToPDFToolStripMenuItem,
            this.удалитьToolStripMenuItem});
            this.testListContextMenu.Name = "testListContextMenu";
            this.testListContextMenu.Size = new System.Drawing.Size(153, 70);
            // 
            // openMeasureResultReaderToolStripMenuItem
            // 
            this.openMeasureResultReaderToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openMeasureResultReaderToolStripMenuItem.Image")));
            this.openMeasureResultReaderToolStripMenuItem.Name = "openMeasureResultReaderToolStripMenuItem";
            this.openMeasureResultReaderToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openMeasureResultReaderToolStripMenuItem.Text = "Открыть";
            this.openMeasureResultReaderToolStripMenuItem.Click += new System.EventHandler(this.OpenButtonToolStripMenuItem_Click);
            // 
            // exportToPDFToolStripMenuItem
            // 
            this.exportToPDFToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exportToPDFToolStripMenuItem.Image")));
            this.exportToPDFToolStripMenuItem.Name = "exportToPDFToolStripMenuItem";
            this.exportToPDFToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exportToPDFToolStripMenuItem.Text = "Экспорт в PDF";
            this.exportToPDFToolStripMenuItem.Click += new System.EventHandler(this.exportToPDFToolStripMenuItem_Click);
            // 
            // удалитьToolStripMenuItem
            // 
            this.удалитьToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("удалитьToolStripMenuItem.Image")));
            this.удалитьToolStripMenuItem.Name = "удалитьToolStripMenuItem";
            this.удалитьToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.удалитьToolStripMenuItem.Text = "Удалить";
            this.удалитьToolStripMenuItem.Click += new System.EventHandler(this.удалитьToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(-3, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "Начальная дата";
            // 
            // dateTimeFrom
            // 
            this.dateTimeFrom.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateTimeFrom.Location = new System.Drawing.Point(0, 34);
            this.dateTimeFrom.Name = "dateTimeFrom";
            this.dateTimeFrom.Size = new System.Drawing.Size(146, 22);
            this.dateTimeFrom.TabIndex = 2;
            this.dateTimeFrom.ValueChanged += new System.EventHandler(this.conditionsChanged_ValueChanged);
            // 
            // dateTimeTo
            // 
            this.dateTimeTo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateTimeTo.Location = new System.Drawing.Point(169, 34);
            this.dateTimeTo.Name = "dateTimeTo";
            this.dateTimeTo.Size = new System.Drawing.Size(146, 22);
            this.dateTimeTo.TabIndex = 3;
            this.dateTimeTo.ValueChanged += new System.EventHandler(this.dateTimeTo_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(166, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 14);
            this.label2.TabIndex = 4;
            this.label2.Text = "Конечная дата";
            // 
            // SearchButton
            // 
            this.SearchButton.BackColor = System.Drawing.Color.Honeydew;
            this.SearchButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.SearchButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.SearchButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Beige;
            this.SearchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SearchButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SearchButton.ForeColor = System.Drawing.Color.Black;
            this.SearchButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.SearchButton.Location = new System.Drawing.Point(5, 27);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(108, 30);
            this.SearchButton.TabIndex = 5;
            this.SearchButton.Text = "ПОИСК";
            this.SearchButton.UseVisualStyleBackColor = false;
            this.SearchButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // ClearList
            // 
            this.ClearList.BackColor = System.Drawing.Color.LavenderBlush;
            this.ClearList.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClearList.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.ClearList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ClearList.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ClearList.Location = new System.Drawing.Point(119, 27);
            this.ClearList.Name = "ClearList";
            this.ClearList.Size = new System.Drawing.Size(154, 30);
            this.ClearList.TabIndex = 7;
            this.ClearList.Text = "УДАЛИТЬ ВЫБРАННОЕ";
            this.ClearList.UseVisualStyleBackColor = false;
            this.ClearList.Click += new System.EventHandler(this.button2_Click);
            // 
            // progressBarLbl
            // 
            this.progressBarLbl.AutoSize = true;
            this.progressBarLbl.Location = new System.Drawing.Point(3, 11);
            this.progressBarLbl.Name = "progressBarLbl";
            this.progressBarLbl.Size = new System.Drawing.Size(35, 13);
            this.progressBarLbl.TabIndex = 8;
            this.progressBarLbl.Text = "label3";
            // 
            // progressBarTest
            // 
            this.progressBarTest.Location = new System.Drawing.Point(3, 28);
            this.progressBarTest.Name = "progressBarTest";
            this.progressBarTest.Size = new System.Drawing.Size(269, 23);
            this.progressBarTest.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarTest.TabIndex = 9;
            this.progressBarTest.Value = 50;
            // 
            // progressBarPanel
            // 
            this.progressBarPanel.Controls.Add(this.progressBarTest);
            this.progressBarPanel.Controls.Add(this.progressBarLbl);
            this.progressBarPanel.Location = new System.Drawing.Point(330, 3);
            this.progressBarPanel.Name = "progressBarPanel";
            this.progressBarPanel.Size = new System.Drawing.Size(280, 68);
            this.progressBarPanel.TabIndex = 10;
            // 
            // TestListtPanel
            // 
            this.TestListtPanel.Controls.Add(this.searchPanel);
            this.TestListtPanel.Controls.Add(this.testsListView);
            this.TestListtPanel.Location = new System.Drawing.Point(12, 39);
            this.TestListtPanel.Name = "TestListtPanel";
            this.TestListtPanel.Size = new System.Drawing.Size(1006, 679);
            this.TestListtPanel.TabIndex = 12;
            // 
            // searchPanel
            // 
            this.searchPanel.Controls.Add(this.testIdField);
            this.searchPanel.Controls.Add(this.groupBox1);
            this.searchPanel.Controls.Add(this.controlPanel);
            this.searchPanel.Controls.Add(this.progressBarPanel);
            this.searchPanel.Controls.Add(this.label2);
            this.searchPanel.Controls.Add(this.label1);
            this.searchPanel.Controls.Add(this.dateTimeFrom);
            this.searchPanel.Controls.Add(this.dateTimeTo);
            this.searchPanel.Location = new System.Drawing.Point(25, 3);
            this.searchPanel.Name = "searchPanel";
            this.searchPanel.Size = new System.Drawing.Size(970, 80);
            this.searchPanel.TabIndex = 13;
            // 
            // testIdField
            // 
            this.testIdField.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.testIdField.Location = new System.Drawing.Point(0, 34);
            this.testIdField.Maximum = new decimal(new int[] {
            -1981284353,
            -1966660860,
            0,
            0});
            this.testIdField.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.testIdField.Name = "testIdField";
            this.testIdField.Size = new System.Drawing.Size(315, 22);
            this.testIdField.TabIndex = 16;
            this.testIdField.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.testIdField.ValueChanged += new System.EventHandler(this.conditionsChanged_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.byTestId);
            this.groupBox1.Controls.Add(this.byDate);
            this.groupBox1.Location = new System.Drawing.Point(616, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(236, 65);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Тип поиска";
            // 
            // byTestId
            // 
            this.byTestId.AutoSize = true;
            this.byTestId.Location = new System.Drawing.Point(90, 31);
            this.byTestId.Name = "byTestId";
            this.byTestId.Size = new System.Drawing.Size(137, 17);
            this.byTestId.TabIndex = 14;
            this.byTestId.TabStop = true;
            this.byTestId.Text = "По номеру испытания";
            this.byTestId.UseVisualStyleBackColor = true;
            this.byTestId.CheckedChanged += new System.EventHandler(this.searchTypeRadioBut_CheckedChanged);
            // 
            // byDate
            // 
            this.byDate.AutoSize = true;
            this.byDate.Location = new System.Drawing.Point(19, 31);
            this.byDate.Name = "byDate";
            this.byDate.Size = new System.Drawing.Size(65, 17);
            this.byDate.TabIndex = 13;
            this.byDate.TabStop = true;
            this.byDate.Text = "По дате";
            this.byDate.UseVisualStyleBackColor = true;
            this.byDate.CheckedChanged += new System.EventHandler(this.searchTypeRadioBut_CheckedChanged);
            // 
            // controlPanel
            // 
            this.controlPanel.Controls.Add(this.ClearList);
            this.controlPanel.Controls.Add(this.SearchButton);
            this.controlPanel.Location = new System.Drawing.Point(321, 3);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(280, 68);
            this.controlPanel.TabIndex = 12;
            // 
            // topMenu
            // 
            this.topMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TestHistoryItemsToolStrip,
            this.OpenRegForm});
            this.topMenu.Location = new System.Drawing.Point(0, 0);
            this.topMenu.Name = "topMenu";
            this.topMenu.Size = new System.Drawing.Size(1044, 24);
            this.topMenu.TabIndex = 13;
            this.topMenu.Text = "menuStrip1";
            // 
            // TestHistoryItemsToolStrip
            // 
            this.TestHistoryItemsToolStrip.Name = "TestHistoryItemsToolStrip";
            this.TestHistoryItemsToolStrip.Size = new System.Drawing.Size(137, 20);
            this.TestHistoryItemsToolStrip.Text = "История просмотров";
            // 
            // statusPanel
            // 
            this.statusPanel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectedCountLbl});
            this.statusPanel.Location = new System.Drawing.Point(0, 639);
            this.statusPanel.Name = "statusPanel";
            this.statusPanel.Size = new System.Drawing.Size(1044, 22);
            this.statusPanel.TabIndex = 14;
            this.statusPanel.Text = "statusStrip1";
            // 
            // selectedCountLbl
            // 
            this.selectedCountLbl.Name = "selectedCountLbl";
            this.selectedCountLbl.Size = new System.Drawing.Size(123, 17);
            this.selectedCountLbl.Text = "Показано 0 из 104234";
            // 
            // OpenRegForm
            // 
            this.OpenRegForm.Name = "OpenRegForm";
            this.OpenRegForm.Size = new System.Drawing.Size(88, 20);
            this.OpenRegForm.Text = "Регистрация";
            this.OpenRegForm.Click += new System.EventHandler(this.OpenRegForm_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 661);
            this.Controls.Add(this.statusPanel);
            this.Controls.Add(this.topMenu);
            this.Controls.Add(this.TestListtPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.topMenu;
            this.MinimumSize = new System.Drawing.Size(1000, 700);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResizeBegin += new System.EventHandler(this.MainForm_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseClick);
            this.Resize += new System.EventHandler(this.MainForm_ResizeEnd);
            ((System.ComponentModel.ISupportInitialize)(this.dataSetTest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ispytan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.date_range)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.testsListView)).EndInit();
            this.testListContextMenu.ResumeLayout(false);
            this.progressBarPanel.ResumeLayout(false);
            this.progressBarPanel.PerformLayout();
            this.TestListtPanel.ResumeLayout(false);
            this.searchPanel.ResumeLayout(false);
            this.searchPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.testIdField)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.controlPanel.ResumeLayout(false);
            this.topMenu.ResumeLayout(false);
            this.topMenu.PerformLayout();
            this.statusPanel.ResumeLayout(false);
            this.statusPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Data.DataSet dataSetTest;
        private System.Data.DataTable ispytan;
        private System.Data.DataColumn test_id;
        private System.Data.DataColumn IspData;
        private System.Data.DataColumn cable_name;
        private System.Data.DataColumn cable_length;
        private System.Data.DataColumn baraban_number;
        private System.Data.DataColumn brutto;
        private System.Windows.Forms.DataGridView testsListView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimeFrom;
        private System.Windows.Forms.DateTimePicker dateTimeTo;
        private System.Data.DataTable date_range;
        private System.Data.DataColumn dataColumn1;
        private System.Data.DataColumn dataColumn2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.ContextMenuStrip testListContextMenu;
        private System.Windows.Forms.ToolStripMenuItem openMeasureResultReaderToolStripMenuItem;
        private System.Data.DataColumn dataColumn3;
        private System.Data.DataColumn dataColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn tested_at;
        private System.Windows.Forms.DataGridViewTextBoxColumn cable_mark;
        private System.Windows.Forms.DataGridViewTextBoxColumn length;
        private System.Windows.Forms.DataGridViewTextBoxColumn bNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn BruttoWeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn cable_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn build_length;
        private System.Windows.Forms.Button ClearList;
        private System.Windows.Forms.Label progressBarLbl;
        private System.Windows.Forms.ToolStripMenuItem удалитьToolStripMenuItem;
        private System.Windows.Forms.ProgressBar progressBarTest;
        private System.Windows.Forms.Panel progressBarPanel;
        private System.Windows.Forms.Panel TestListtPanel;
        private System.Windows.Forms.ToolStripMenuItem exportToPDFToolStripMenuItem;
        private System.Windows.Forms.Panel controlPanel;
        private System.Windows.Forms.Panel searchPanel;
        private System.Windows.Forms.MenuStrip topMenu;
        private System.Windows.Forms.ToolStripMenuItem TestHistoryItemsToolStrip;
        private System.Windows.Forms.StatusStrip statusPanel;
        private System.Windows.Forms.ToolStripStatusLabel selectedCountLbl;
        private System.Windows.Forms.RadioButton byTestId;
        private System.Windows.Forms.RadioButton byDate;
        private System.Windows.Forms.NumericUpDown testIdField;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStripMenuItem OpenRegForm;
    }
}

