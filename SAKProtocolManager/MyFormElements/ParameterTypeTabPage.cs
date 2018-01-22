using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SAKProtocolManager.DBEntities;
using SAKProtocolManager.DBEntities.TestResultEntities;

namespace SAKProtocolManager.MyFormElements
{
    partial class ParameterTypeTabPage : TabPage
    {
        private MeasuredParameterData ParameterData;
        private ComboBox CorrectionLimitComboBox;
        private Button CorrectResultsButton;
        private DataGridView ValuesTable;
        private MeasureResultReader mReader;
        private ProgressBar CorrectionResultPB;
        private Label CorrectionLimitLbl;
        private GroupBox CorrectionLimitGB;
        private int xPos, yPos;
        private int leftOffset = 20;
        private int topOffset = 20;
        private string[] LeadTitles = new string[] { "Жила 1", "Жила 2", "Жила 3", "Жила 4" };
        private Label HeadLbl;


        public ParameterTypeTabPage(MeasuredParameterData paramerter_data, MeasureResultReader r)
        {
            this.ParameterData = paramerter_data;
            this.mReader = r;
            InitializeOnParameterData();
        }


        public ParameterTypeTabPage(CableStructure structure, MeasureResultReader r)
        {
            this.mReader = r;
            this.Text = this.Name = "Прозвонка";
            xPos = leftOffset;
            yPos = topOffset;
            ValuesTable = MakeAffectedElementsTable(structure);
            SetValuesTableStyle();
        }


        private void InitializeOnParameterData()
        {
            this.Text = this.Name = ParameterData.GetTitle();
            xPos = leftOffset;
            yPos = topOffset;
            //yPos += 5;
            //DrawCorrectionLimitControl();
            //drawMeasureStatLabel();
            //yPos += 115;
            DrawValuesTable();
            SetValuesTableStyle();
        }

        private void drawMeasureStatLabel()
        {
            yPos = topOffset;
            xPos = CorrectionLimitGB.Width + 40;
            drawLabel("min_val", String.Format("Мин. значение: {0}{1};\n\nСреднее значение: {2}{1};\n\nМакс. значение: {3}{1};\n\nИзмерено {4}% из {5}%;", ParameterData.MinVal, ParameterData.ResultMeasure(), ParameterData.AverageVal, ParameterData.MaxVal, ParameterData.MeasuredPercent, ParameterData.NormalPercent));
            xPos = leftOffset;

        }

        private void DrawParameterTypeHeadPart()
        {
            int counter = 0;
            //foreach (MeasuredParameterData pd in ParameterType.ParameterDataList) counter += pd.NotNormalResults.Length;
            //drawLabel(String.Format("tab_header_{0}", ParameterType.Id), String.Format("Количество результатов с выходом за норму: {0}", counter)); //заголовок
            yPos += 30;
            //DrawCorrectionLimitControl(); 
            yPos += 100;
            DrawValuesTable();
        }

        private void drawLabel(string name, string text)
        {
            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.Location = new System.Drawing.Point(xPos-3, yPos);
            lbl.Name = name;
            lbl.Size = new System.Drawing.Size(91, 13);
            lbl.TabIndex = 2;
            lbl.Text = text;
            lbl.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            lbl.Parent = this;
            HeadLbl = lbl;
        }


        private DataGridView MakeTable()
        {
            
            switch (ParameterData.ParameterType.Name)
            {
                case "Rж":
                case "dR":
                case "Cр":
                case "Rиз1":
                case "Rиз2":
                case "Co":
                    //return DrawPrimaryParameterTable(pData);
                    return DrawByLeadsParameterTable();
                //case "Cр":

                //return DrawByElementsParameterTable(pData);
                case "al":
                case "Ao":
                case "Az":
                    return DrawPVTable();
                case "Rиз3":
                case "Rиз4":
                    return DrawIzolationCombinationTable();
                default:
                    return DrawDefaultTable();
            }
        }



        private void DrawValuesTable()
        {
            if (ValuesTable != null) { ValuesTable.Dispose(); }
            ValuesTable = MakeTable();
        }

        private void SetValuesTableStyle()
        {
            if (ValuesTable == null) return;
            //if (ValuesTable.RowCount < 2) return;
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.MintCream;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Teal;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.OldLace;

            if (ValuesTable.Columns.Contains("value")) ValuesTable.Columns["value"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ValuesTable.Name = String.Format("dataGridViewOfResult_{0}", 1);
            ValuesTable.Parent = this;
            ValuesTable.Location = new System.Drawing.Point(xPos, yPos);
            ValuesTable.Size = new System.Drawing.Size(730, 360);
            ValuesTable.ScrollBars = ScrollBars.Vertical;
            ValuesTable.BackColor = System.Drawing.Color.MintCream;
            ValuesTable.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            ValuesTable.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            ValuesTable.RowsDefaultCellStyle = dataGridViewCellStyle2;
            ValuesTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }


        private DataGridView DrawIzolationCombinationTable()
        {
            DataGridView dgv = new DataGridView();
            dgv.Columns.Add("elements_group", "Пучок №");
            dgv.Columns.Add("combination", "Комбинация №");
            dgv.Columns.Add("value", String.Format("Результат, {0}", ParameterData.ResultMeasure()));
            dgv.Columns.Add("min_norma", String.Format("Макс., {0}", ParameterData.ResultMeasure()));
            dgv.Columns.Add("max_norma", String.Format("Мин., {0}", ParameterData.ResultMeasure()));
            dgv.Columns["min_norma"].Visible = ParameterData.MinValue > Decimal.MinValue;
            dgv.Columns["max_norma"].Visible = ParameterData.MaxValue < Decimal.MaxValue;
            TestResult[] results = ParameterData.TestResults;
            if (results.Length > 0)
            {
                dgv.Columns["elements_group"].Visible = results[0].ElementNumber > 0;
                dgv.Rows.Add(results.Length);
                for (int i = 0; i < results.Length; i++)
                {
                    dgv.Rows[i].Cells["elements_group"].Value = results[i].ElementNumber;
                    dgv.Rows[i].Cells["combination"].Value = results[i].SubElementNumber;
                    dgv.Rows[i].Cells["value"].Value = results[i].GetStringTableValue();
                    dgv.Rows[i].Cells["min_norma"].Value = ParameterData.MinValue;
                    dgv.Rows[i].Cells["max_norma"].Value = ParameterData.MaxValue;
                }
            }

            return dgv;
        }

        private DataGridView DrawByElementsParameterTable(MeasuredParameterData pData)
        {
            DataGridView dgv = new DataGridView();
            dgv.Columns.Add("element_number", String.Format("{0} №", pData.ParameterType.Structure.BendingTypeName));
            dgv.Columns.Add("value", "Результат");
            dgv.Columns.Add("measure", "мера");
            TestResult[] results = pData.TestResults;
            if (results.Length > 0)
            {
                dgv.Rows.Add(results.Length);
                for (int i = 0; i < results.Length; i++)
                {
                    dgv.Rows[i].Cells["element_number"].Value = results[i].ElementNumber;
                    dgv.Rows[i].Cells["value"].Value = results[i].GetStringTableValue();
                    dgv.Rows[i].Cells["measure"].Value = pData.ResultMeasure();
                }
            }

            return dgv;

        }
        private DataGridView DrawPrimaryParameterTable(MeasuredParameterData pData)
        {
            DataGridView dgv = new DataGridView();
            dgv.Columns.Add("element_number", String.Format("{0} №", pData.ParameterType.Structure.BendingTypeName));
            if (pData.TestResults.Length > 0)
            {
                for (int i = 0; i < pData.TestResults[0].Values.Length; i++)
                {
                    string title = String.Format("result_{0}", i);
                    dgv.Columns.Add(title, LeadTitles[i]);
                    dgv.Columns[title].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                dgv.Columns.Add("measure", "мера");
                dgv.Columns.Add("vals_count", "кол-во");
                dgv.Columns.Add("max", "Макс");
                dgv.Columns.Add("min", "Мин");
                dgv.Rows.Add(pData.TestResults.Length);
                int idx = 0;
                foreach (TestResult tr in pData.TestResults)
                {
                    dgv.Rows[idx].Cells["element_number"].Value = tr.ElementNumber;
                    dgv.Rows[idx].Cells["measure"].Value = pData.ResultMeasure();
                    dgv.Rows[idx].Cells["vals_count"].Value = pData.ParameterType.Name;
                    for (int i=0; i<tr.Values.Length; i++)
                    {
                        dgv.Rows[idx].Cells[String.Format("result_{0}", i)].Value = tr.Values[i];
                    }
                    idx++;
                }
            }
            return dgv;
        }
        private DataGridView DrawByLeadsParameterTable()
        {
            DataGridView dgv = new DataGridView();
            dgv.Columns.Add("element_number", String.Format("{0} №", ParameterData.ParameterType.Structure.BendingTypeName));
            dgv.Columns.Add("lead", "Жила");
            dgv.Columns.Add("value", String.Format("Результат {0}", ParameterData.ResultMeasure()));
            dgv.Columns.Add("min_val", String.Format("Мин. {0}", ParameterData.ResultMeasure()));
            dgv.Columns.Add("max_val", String.Format("Макс. {0}", ParameterData.ResultMeasure()));
            dgv.Columns["min_val"].Visible = ParameterData.MinValue != Decimal.MinValue;
            dgv.Columns["max_val"].Visible = ParameterData.MaxValue != Decimal.MaxValue;
            dgv.Columns["lead"].Visible = ParameterData.ParameterType.Name != "dR";
            dgv.MultiSelect = false;
            dgv.Columns["max_val"].ReadOnly = dgv.Columns["min_val"].ReadOnly = dgv.Columns["element_number"].ReadOnly = dgv.Columns["lead"].ReadOnly = true;

            TestResult[] results = ParameterData.TestResults;
            if (results.Length > 0)
            {
                int rowsNumber = -1;
                foreach(TestResult result in results)
                {
                    rowsNumber++;
                    TestResultDataGridViewRow r = new TestResultDataGridViewRow(result);
                    r.CreateCells(dgv);
                    r.Cells[0].Value = result.ElementNumber;
                    r.Cells[1].Value = result.SubElementTitle();
                    r.Cells[2].ReadOnly = result.Affected;
                    r.Cells[2].Value = result.GetStringTableValue();
                    r.Cells[3].Value = ParameterData.MinValue.ToString();
                    r.Cells[4].Value = ParameterData.MaxValue.ToString();
                    r.DefaultCellStyle = GetRowStyle(result);
                    dgv.Rows.Add(r);
                }
            }
            dgv.CellValueChanged += Dgv_CellValueChanged;
            return dgv;
        }

        private void Dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            TestResultDataGridViewRow tdgr = dgv.Rows[e.RowIndex] as TestResultDataGridViewRow;
            decimal was = tdgr.Result.RawValue;
            decimal val = ServiceFunctions.convertToDecimal(tdgr.Cells[e.ColumnIndex].Value);
            tdgr.Result.UpdateResult(val);
            tdgr.DefaultCellStyle = GetRowStyle(tdgr.Result);
            MessageBox.Show(was.ToString() + " - " + tdgr.Result.RawValue.ToString());
        }



        private DataGridView DrawDefaultTable()
        {
            DataGridView dgv = new DataGridView();
            dgv.Columns.Add("element_number", String.Format("{0} №", ParameterData.ParameterType.Structure.BendingTypeName));
            dgv.Columns.Add("value", String.Format("Результат, {0}", ParameterData.ResultMeasure()));
            dgv.Columns.Add("min_val", String.Format("Мин., {0}", ParameterData.ResultMeasure()));
            dgv.Columns.Add("max_val", String.Format("Макс., {0}", ParameterData.ResultMeasure()));
            dgv.Columns.Add("percent_out", String.Format("Отклонение {0}", ParameterData.ParameterType.DeviationMeasure()));
            dgv.Columns["min_val"].Visible = ParameterData.MinValue > Decimal.MinValue;
            dgv.Columns["max_val"].Visible = ParameterData.MaxValue < Decimal.MaxValue;
            
            TestResult[] results = ParameterData.TestResults;
            if (results.Length > 0)
            {
                dgv.Rows.Add(results.Length);
                for (int i = 0; i < results.Length; i++)
                {
                    dgv.Rows[i].Cells["element_number"].Value = results[i].ElementNumber;
                    dgv.Rows[i].Cells["value"].Value = results[i].GetStringTableValue();
                    dgv.Rows[i].Cells["min_val"].Value = ParameterData.MinValue.ToString();
                    dgv.Rows[i].Cells["max_val"].Value = ParameterData.MaxValue.ToString();
                    dgv.Rows[i].Cells["percent_out"].Value = results[i].DeviationPercent;
                    dgv.Rows[i].DefaultCellStyle = GetRowStyle(results[i]);
                }
            }
            dgv.Refresh();
            return dgv;
        }

        private DataGridView DrawPVTable()
        {
            DataGridView dgv = new DataGridView();
            if (ParameterData.ParameterType.Structure.BendingTypeLeadsNumber > 2)
            {
                dgv.Columns.Add("receiver_number", String.Format("{0} приёмника №", ParameterData.ParameterType.Structure.BendingTypeName));
                dgv.Columns.Add("sub_receiver_number", String.Format("пара приёмника", ParameterData.ParameterType.Structure.BendingTypeName));
                dgv.Columns.Add("sub_transponder_number", String.Format("пара генератора", ParameterData.ParameterType.Structure.BendingTypeName));
                dgv.Columns.Add("transponder_number", String.Format("{0} генератора №", ParameterData.ParameterType.Structure.BendingTypeName));
                dgv.Columns["sub_receiver_number"].Visible = dgv.Columns["sub_transponder_number"].Visible = dgv.Columns["transponder_number"].Visible = ParameterData.ParameterType.Name != "al";
            }
            else
            {
                dgv.Columns.Add("receiver_number", String.Format("{0} приёмника №", ParameterData.ParameterType.Structure.BendingTypeName));
                dgv.Columns.Add("transponder_number", String.Format("{0} генератора №", ParameterData.ParameterType.Structure.BendingTypeName));
                dgv.Columns["transponder_number"].Visible = ParameterData.ParameterType.Name != "al";
            }
            dgv.Columns.Add("freq_range", "Диапазон частот, кГц");
            dgv.Columns.Add("value", "результат, " + ParameterData.ParameterType.ParameterDataList[0].ResultMeasure());
            dgv.Columns.Add("norma", ParameterData.ParameterType.Name == "al" ? "Макс" : "Мин" );

            TestResult[] results = ParameterData.TestResults;
            if (results.Length > 0)
            {
                int lastCount = dgv.Rows.Count-1;
                dgv.Rows.Add(results.Length);
                for (int i = lastCount; i < dgv.Rows.Count-1; i++)
                {
                    dgv.Rows[i].Cells["receiver_number"].Value = results[i-lastCount].ElementNumber;
                    if (ParameterData.ParameterType.Structure.BendingTypeLeadsNumber > 2)
                    {
                        dgv.Rows[i].Cells["sub_receiver_number"].Value = results[i- lastCount].SubElementTitle();
                        dgv.Rows[i].Cells["sub_transponder_number"].Value = results[i- lastCount].GeneratorSubElementTitle();
                    }
                    dgv.Rows[i].Cells["freq_range"].Value = results[i - lastCount].ParameterData.GetFreqRangeTitle();
                    dgv.Rows[i].Cells["transponder_number"].Value = results[i- lastCount].GeneratorElementNumber;
                    dgv.Rows[i].Cells["value"].Value = results[i - lastCount].GetStringTableValue();
                    dgv.Rows[i].Cells["norma"].Value = ParameterData.ParameterType.Name == "al" ? ParameterData.MaxValue : ParameterData.MinValue;
                    dgv.Rows[i].DefaultCellStyle = GetRowStyle(results[i]);
                }
            }
            dgv.Refresh();

            return dgv;
        }



        private DataGridView MakeAffectedElementsTable(CableStructure structure)
        {
            DataGridView dgv = new DataGridView();
            dgv.Columns.Add("element_number", String.Format("{0} №", structure.BendingTypeName));
            if (structure.AffectedElements.Length > 0)
            {
                for (int i = 0; i < structure.AffectedElements[0].Values.Length; i++)
                {
                    string title = String.Format("result_{0}", i);
                    dgv.Columns.Add(title, LeadTitles[i]);
                    dgv.Columns[title].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                dgv.Rows.Add(structure.AffectedElements.Length);
                int idx = 0;
                foreach (ProzvonTestResult tr in structure.AffectedElements)
                {
                    dgv.Rows[idx].Cells["element_number"].Value = tr.ElementNumber;
                    for (int i = 0; i < tr.Statuses.Length; i++)
                    {
                        dgv.Rows[idx].Cells[String.Format("result_{0}", i)].Value = tr.Statuses[i];
                    }
                    idx++;
                }
            }
            return dgv;
        }


        private void DrawCorrectionLimitControl()
        {
            int y = 30;
            int x = 10;
            CorrectionLimitGB = new GroupBox();
            CorrectionLimitGB.Name = "CorrectionLimitGB";
            CorrectionLimitGB.Location = new System.Drawing.Point(xPos, yPos);
            CorrectionLimitGB.Text = "Панель автокоррекции результата";
            CorrectionLimitGB.Parent = this;
            CorrectionLimitGB.Enabled = ParameterData.NotNormalResults.Length > 0;

            CorrectionLimitLbl = new Label();
            CorrectionLimitLbl.AutoSize = true;
            CorrectionLimitLbl.Location = new System.Drawing.Point(x, y);
            CorrectionLimitLbl.Name = "CorrectionLimitLbl";
            CorrectionLimitLbl.Size = new System.Drawing.Size(91, 13);
            CorrectionLimitLbl.TabIndex = 2;
            CorrectionLimitLbl.Text = "Предел допустимой коррекции, " + ParameterData.ParameterType.DeviationMeasure();
            CorrectionLimitLbl.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            CorrectionLimitLbl.Parent = CorrectionLimitGB;
            y += 20;
            //if (ParameterType.Name != "Ao" && ParameterType.Name != "Az") return;
            CorrectionLimitComboBox = new ComboBox();
            CorrectionLimitComboBox.Location = new System.Drawing.Point(x, y);
            CorrectionLimitComboBox.Name = String.Format("CorrectionLimitComboBox_{0}", ParameterData.ParameterType.Name);
            CorrectionLimitComboBox.Parent = CorrectionLimitGB;
            CorrectionLimitComboBox.Width = 200;
            FillCorrectionLimitsCBItems();
            CorrectionLimitComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ///CorrectionLimitComboBox.SelectedIndexChanged += new EventHandler(ChangeFreqComboBoxSelectedIndex);
            CorrectResultsButton = new Button();
            CorrectResultsButton.Location = new System.Drawing.Point(x + 20 + CorrectionLimitComboBox.Width, y-1);
            CorrectResultsButton.Name = String.Format("CorrectResultsButton_{0}", ParameterData.ParameterType.Name);
            CorrectResultsButton.Text = "Произвести коррекцию";
            CorrectResultsButton.Parent = CorrectionLimitGB;
            CorrectResultsButton.Width = 180;


            CorrectionResultPB = new ProgressBar();
            CorrectionResultPB.Name = String.Format("CorrectionResultPB_{0}", ParameterData.ParameterType.Name);
            CorrectionResultPB.Parent = CorrectionLimitGB;
            CorrectionResultPB.Location = new System.Drawing.Point(x + 20 + CorrectionLimitComboBox.Width, y);
            CorrectionResultPB.Width = 180;
            CorrectionResultPB.Visible = false;

            CorrectionLimitGB.Size = new System.Drawing.Size(x + 20 + CorrectionLimitComboBox.Width + CorrectResultsButton.Width + 10, 90);

        }
        private void FillCorrectionLimitsCBItems()
        {
            decimal[] corrLimList = ParameterData.GetCorrectionLimitsList();
            CorrectionLimitComboBox.Items.Clear();
            foreach (decimal l in corrLimList)
            {
                CorrectionLimitComboBox.Items.Add(l.ToString());
            }
            if (CorrectionLimitComboBox.Items.Count > 0 ) CorrectionLimitComboBox.SelectedIndex = 0;
        }
        /*
        private void CorrectResults(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Будет произведена коррекция результатов вышедших за норму с отклонением до " + CorrectionLimitComboBox.Text + ParameterData.ParameterType.DeviationMeasure() + " включительно\n\nВы согласны?", "Коррекция параметра " + ParameterData.ParameterType.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                //RefreshAfterCorrection();
                //FillCorrectionLimitsCBItems();
                //SendCorrectionQueriesToDataBase(ParameterData.ParameterType.CorrectNotNormalResults(ServiceFunctions.convertToDecimal(CorrectionLimitComboBox.Text)));
                //mReader.RefreshOutOfNormaPanel(ParameterData.ParameterType.Name);
                //List<string> queries = ParameterData.CorrectNotNormalResults(ServiceFunctions.convertToDecimal(CorrectionLimitComboBox.Text));
                //if (queries.Count == 0) return;
                List<string> tmp = new List<string>();
                int sendLimit = 50;
                CorrectResultsButton.Visible = false;
                CorrectionResultPB.Visible = true;
                CorrectionResultPB.Maximum = queries.Count;
                CorrectionResultPB.Value = 0;
                foreach (string q in queries)
                {
                    tmp.Add(q);
                    if (tmp.Count == sendLimit || q == queries.Last())
                    {
                        CorrectionResultPB.Value += tmp.Count;
                        DBBase.SendQueriesList(tmp.ToArray());
                        tmp.Clear();
                    }
                }
                CorrectionResultPB.Visible = false;
                CorrectResultsButton.Visible = true;
                ParameterData.ParameterType.RefreshTestResultsOnParameterData(true);
                this.mReader.RefreshTabs();
                MessageBox.Show("Коррекция успешно произведена.", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);


            }
        }
        */
        private void SendCorrectionQueriesToDataBase(List<string> queries)
        {
            if (queries.Count == 0) return;
            List<string> tmp = new List<string>();
            int sendLimit = 50;
            CorrectResultsButton.Visible = false;
            CorrectionResultPB.Visible = true;
            CorrectionResultPB.Maximum = queries.Count;
            CorrectionResultPB.Value = 0;
            foreach (string q in queries)
            {
                tmp.Add(q);
                if (tmp.Count == sendLimit || q == queries.Last())
                {
                    CorrectionResultPB.Value += tmp.Count;
                    DBBase.SendQueriesList(tmp.ToArray());
                    tmp.Clear();
                }
            }
            CorrectionResultPB.Visible = false;
            CorrectResultsButton.Visible = true;
            MessageBox.Show("Коррекция успешно произведена.", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void RefreshAfterCorrection()
        {
            DrawValuesTable();
        }

        private void SetPos(int x, int y)
        {
            xPos = x;
            yPos = y;
        }

        private void ChangeFreqComboBoxSelectedIndex(object sender, EventArgs e)
        {
            DrawValuesTable();
        }

        private DataGridViewCellStyle GetRowStyle(TestResult tr)
        {
            DataGridViewCellStyle s = new DataGridViewCellStyle();
            if (tr.Affected)
            {
                s.BackColor = System.Drawing.Color.DarkRed;
            }
            else if (tr.DeviationPercent > 0)
            {
                s.BackColor = System.Drawing.Color.DarkOrange;
            }else
            {
                s.BackColor = System.Drawing.Color.MintCream;
            }
            s.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            s.SelectionBackColor = System.Drawing.Color.Teal;
            s.SelectionForeColor = System.Drawing.Color.OldLace;
            return s;
        }
    }

    public partial class TestResultDataGridViewRow : DataGridViewRow
    {
        public TestResult Result;
        private bool hasChangeValueHandler = false;

        public TestResultDataGridViewRow()
        {

        }

        public TestResultDataGridViewRow(TestResult result)
        {
            this.Result = result;
            //this.CreateCells(dgv);
        }


    }
}
