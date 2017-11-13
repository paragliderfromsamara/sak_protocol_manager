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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimeFrom = new System.Windows.Forms.DateTimePicker();
            this.dateTimeTo = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.inProcessLabel = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetTest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ispytan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.date_range)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.testsListView)).BeginInit();
            this.testListContextMenu.SuspendLayout();
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
            this.testsListView.Location = new System.Drawing.Point(44, 108);
            this.testsListView.MultiSelect = false;
            this.testsListView.Name = "testsListView";
            this.testsListView.ReadOnly = true;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.testsListView.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.testsListView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.testsListView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.testsListView.Size = new System.Drawing.Size(950, 323);
            this.testsListView.TabIndex = 0;
            this.testsListView.SelectionChanged += new System.EventHandler(this.testsListView_SelectionChanged);
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "Номер испытания";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Width = 124;
            // 
            // tested_at
            // 
            this.tested_at.DataPropertyName = "tested_at";
            this.tested_at.HeaderText = "Дата и время";
            this.tested_at.Name = "tested_at";
            this.tested_at.ReadOnly = true;
            this.tested_at.Width = 102;
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
            this.length.Width = 104;
            // 
            // bNumber
            // 
            this.bNumber.DataPropertyName = "baraban_number";
            this.bNumber.HeaderText = "Номер барабана";
            this.bNumber.Name = "bNumber";
            this.bNumber.ReadOnly = true;
            this.bNumber.Width = 117;
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
            this.openMeasureResultReaderToolStripMenuItem});
            this.testListContextMenu.Name = "testListContextMenu";
            this.testListContextMenu.Size = new System.Drawing.Size(122, 26);
            // 
            // openMeasureResultReaderToolStripMenuItem
            // 
            this.openMeasureResultReaderToolStripMenuItem.Name = "openMeasureResultReaderToolStripMenuItem";
            this.openMeasureResultReaderToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.openMeasureResultReaderToolStripMenuItem.Text = "Открыть";
            this.openMeasureResultReaderToolStripMenuItem.Click += new System.EventHandler(this.OpenButtonToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Начальная дата";
            // 
            // dateTimeFrom
            // 
            this.dateTimeFrom.Location = new System.Drawing.Point(44, 71);
            this.dateTimeFrom.Name = "dateTimeFrom";
            this.dateTimeFrom.Size = new System.Drawing.Size(200, 20);
            this.dateTimeFrom.TabIndex = 2;
            // 
            // dateTimeTo
            // 
            this.dateTimeTo.Location = new System.Drawing.Point(267, 71);
            this.dateTimeTo.Name = "dateTimeTo";
            this.dateTimeTo.Size = new System.Drawing.Size(200, 20);
            this.dateTimeTo.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(264, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Конечная дата";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(488, 70);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(89, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Искать";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // inProcessLabel
            // 
            this.inProcessLabel.AutoSize = true;
            this.inProcessLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.inProcessLabel.Location = new System.Drawing.Point(583, 75);
            this.inProcessLabel.Name = "inProcessLabel";
            this.inProcessLabel.Size = new System.Drawing.Size(84, 14);
            this.inProcessLabel.TabIndex = 6;
            this.inProcessLabel.Text = "Идёт поиск...";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(44, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(191, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "label3";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 477);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.inProcessLabel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimeFrom);
            this.Controls.Add(this.dateTimeTo);
            this.Controls.Add(this.testsListView);
            this.Name = "MainForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataSetTest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ispytan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.date_range)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.testsListView)).EndInit();
            this.testListContextMenu.ResumeLayout(false);
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
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label inProcessLabel;
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
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
    }
}

