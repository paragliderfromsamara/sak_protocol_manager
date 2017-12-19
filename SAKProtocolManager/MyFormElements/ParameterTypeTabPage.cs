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
        private MeasureParameterType ParameterType;
        private ComboBox FrequencyComboBox;
        private ComboBox CorrectionLimitComboBox;
        private Button CorrectResultsButton;
        private DataGridView ValuesTable;
        private MeasureResultReader mReader;
        private ProgressBar CorrectionResultPB;
        private Label CorrectionLimitLbl;
        private int xPos, yPos;
        private int leftOffset = 20;
        private int topOffset = 30;
        private string[] LeadTitles = new string[] { "Жила a", "Жила b", "Жила c", "Жила d" };
        private Label HeadLbl;
        public ParameterTypeTabPage(MeasureParameterType parameter_type, MeasureResultReader r)
        {
            this.ParameterType = parameter_type;
            this.mReader = r;
            Initialize();
        }


        private void Initialize()
        {
            this.Text = this.Name = ParameterType.Name;
            xPos = leftOffset;
            yPos = topOffset;
            DrawParameterTypeHeadPart();
        }

        private void DrawParameterTypeHeadPart()
        {
            int counter = 0;
            foreach (MeasuredParameterData pd in ParameterType.ParameterData) counter += pd.NotNormalResults.Count;
            drawLabel(String.Format("tab_header_{0}", ParameterType.Id), String.Format("Количество результатов с выходом за норму: {0}", counter)); //заголовок
            yPos += 30;
            DrawCorrectionLimitControl(); 
            yPos += 30;
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

        private DataGridView MakeTable(MeasuredParameterData pData)
        {
            
            switch (pData.ParameterType.Name)
            {
                case "Rж":
                case "dR":
                case "Cр":
                case "Rиз1":
                case "Rиз2":
                case "Co":
                case "al":
                    //return DrawPrimaryParameterTable(pData);

                    return DrawByLeadsParameterTable(pData);
                //case "Cр":
                
                //return DrawByElementsParameterTable(pData);
                case "Ao":
                case "Az":
                    return DrawPVTable();
                default:
                    return DrawDefaultTable(pData);
            }
        }

        private void DrawValuesTable()
        {
            MeasuredParameterData parameterData = FrequencyComboBox == null ? ParameterType.ParameterData.First() : ParameterType.ParameterData[FrequencyComboBox.SelectedIndex];
            if (ValuesTable != null) { ValuesTable.Dispose(); }
            ValuesTable = MakeTable(parameterData);
            if (ValuesTable.Rows.Count > 1)
            {
                if (ValuesTable.Columns.Contains("value")) ValuesTable.Columns["value"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                ValuesTable.Name = String.Format("dataGridViewOfResult_{0}", 1);
                ValuesTable.Parent = this;
                ValuesTable.Location = new System.Drawing.Point(xPos, yPos);
                ValuesTable.Size = new System.Drawing.Size(700, 450);
                ValuesTable.ScrollBars = ScrollBars.Vertical;
            }
            //ValuesTable.Update();
        }

        private DataGridView DrawByElementsParameterTable(MeasuredParameterData pData)
        {
            DataGridView dgv = new DataGridView();
            dgv.Columns.Add("element_number", String.Format("{0} №", pData.ParameterType.Structure.BendingTypeName));
            dgv.Columns.Add("value", "Значение");
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
        private DataGridView DrawByLeadsParameterTable(MeasuredParameterData pData)
        {
            DataGridView dgv = new DataGridView();
            dgv.Columns.Add("element_number", String.Format("{0} №", pData.ParameterType.Structure.BendingTypeName));
            dgv.Columns.Add("lead", "Жила");
            dgv.Columns.Add("value", String.Format("Результат {0}", pData.ResultMeasure()));
            dgv.Columns.Add("min_val", String.Format("Мин. {0}", pData.ResultMeasure()));
            dgv.Columns.Add("max_val", String.Format("Макс. {0}", pData.ResultMeasure()));
            dgv.Columns.Add("percent_out", String.Format("Отклонение {0}", pData.ParameterType.DeviationMeasure()));
            dgv.Columns["min_val"].Visible = pData.MinValue != Decimal.MinValue;
            dgv.Columns["max_val"].Visible = pData.MaxValue != Decimal.MaxValue;
            dgv.Columns["lead"].Visible = pData.ParameterType.Name != "dR";
            dgv.MultiSelect = false;
            dgv.Columns["max_val"].ReadOnly = dgv.Columns["min_val"].ReadOnly = dgv.Columns["element_number"].ReadOnly = dgv.Columns["lead"].ReadOnly = true;

            TestResult[] results = pData.NotNormalResults.ToArray();
            if (results.Length > 0)
            {
                int rowsNumber = -1;
                for (int i = 0; i < results.Length; i++)
                {
                    rowsNumber++;
                    dgv.Rows.Add(1);
                    dgv.Rows[rowsNumber].Cells["element_number"].Value = results[i].ElementNumber;
                    //dgv.Rows[rowsNumber].Cells["measure"].Value = pData.ResultMeasure();
                    dgv.Rows[rowsNumber].Cells["lead"].Value = LeadTitles[results[i].SubElementNumber - 1];
                    dgv.Rows[rowsNumber].Cells["value"].Value = results[i].BringingValue;
                    dgv.Rows[rowsNumber].Cells["min_val"].Value = pData.MinValue.ToString();
                    dgv.Rows[rowsNumber].Cells["max_val"].Value = pData.MaxValue.ToString();
                    dgv.Rows[rowsNumber].Cells["percent_out"].Value = results[i].DeviationPercent;
                }
            }
            return dgv;
        }

        private DataGridView DrawDefaultTable(MeasuredParameterData pData)
        {
            DataGridView dgv = new DataGridView();
            dgv.Columns.Add("element_number", String.Format("{0} №", pData.ParameterType.Structure.BendingTypeName));
            dgv.Columns.Add("measure_number", "№ измерения");
            dgv.Columns.Add("value", "результат");
            dgv.Columns.Add("measure", "мера");
            TestResult[] results = pData.TestResults;
            if (results.Length > 0)
            {
                dgv.Rows.Add(results.Length);
                for (int i = 0; i < results.Length; i++)
                {
                    dgv.Rows[i].Cells["element_number"].Value = results[i].ElementNumber;
                    dgv.Rows[i].Cells["measure_number"].Value = results[i].SubElementNumber;
                    dgv.Rows[i].Cells["value"].Value = results[i].GetStringTableValue();
                    dgv.Rows[i].Cells["measure"].Value = pData.ResultMeasure();
                }
            }
            dgv.Refresh();
            return dgv;
        }

        private DataGridView DrawPVTable()
        {
            DataGridView dgv = new DataGridView();
            if (ParameterType.Structure.BendingTypeLeadsNumber > 2)
            {
                dgv.Columns.Add("receiver_number", String.Format("{0} приёмника №", ParameterType.Structure.BendingTypeName));
                dgv.Columns.Add("sub_receiver_number", String.Format("пара приёмника", ParameterType.Structure.BendingTypeName));
                dgv.Columns.Add("sub_transponder_number", String.Format("пара генератора", ParameterType.Structure.BendingTypeName));
                dgv.Columns.Add("transponder_number", String.Format("{0} генератора №", ParameterType.Structure.BendingTypeName));
            }
            else
            {
                dgv.Columns.Add("receiver_number", String.Format("{0} приёмника №", ParameterType.Structure.BendingTypeName));
                dgv.Columns.Add("transponder_number", String.Format("{0} генератора №", ParameterType.Structure.BendingTypeName));
            }
            dgv.Columns.Add("freq_range", "Диапазон частот, кГц");
            dgv.Columns.Add("value", "результат, " + ParameterType.ParameterData[0].ResultMeasure());
            dgv.Columns.Add("min", "Мин");
            dgv.Columns.Add("percent_out", "Отклонение, дБ");
            foreach (MeasuredParameterData pData in ParameterType.ParameterData)
            {
                TestResult[] results = pData.NotNormalResults.ToArray();
                if (results.Length > 0)
                {
                    int lastCount = dgv.Rows.Count-1;
                    dgv.Rows.Add(results.Length);
                    for (int i = lastCount; i < dgv.Rows.Count-1; i++)
                    {
                        dgv.Rows[i].Cells["receiver_number"].Value = results[i-lastCount].ElementNumber;
                        if (ParameterType.Structure.BendingTypeLeadsNumber > 2)
                        {
                            dgv.Rows[i].Cells["sub_receiver_number"].Value = results[i- lastCount].SubElementTitle();
                            dgv.Rows[i].Cells["sub_transponder_number"].Value = results[i- lastCount].GeneratorSubElementTitle();
                        }
                        dgv.Rows[i].Cells["freq_range"].Value = results[i - lastCount].ParameterData.GetFreqRange();
                        dgv.Rows[i].Cells["transponder_number"].Value = results[i- lastCount].GeneratorElementNumber;
                        dgv.Rows[i].Cells["value"].Value = results[i- lastCount].GetStringTableValue();
                        dgv.Rows[i].Cells["min"].Value = pData.MinValue;
                        dgv.Rows[i].Cells["percent_out"].Value = results[i- lastCount].DeviationPercent;
                    }
                }
            }

            dgv.Refresh();
            return dgv;
        }


        private void DrawFrequencyComboBox()
        {
            if (ParameterType.Name != "Ao" && ParameterType.Name != "Az") return;
            FrequencyComboBox = new ComboBox();
            FrequencyComboBox.Location = new System.Drawing.Point(xPos, yPos);
            FrequencyComboBox.Name = String.Format("freq_combobox_{0}", ParameterType.Name);
            foreach (MeasuredParameterData pd in ParameterType.ParameterData)
            {
                string itemName = pd.MinFrequency.ToString();
                if (pd.MaxFrequency > 0) itemName += pd.MaxFrequency.ToString();
                FrequencyComboBox.Items.Add(itemName);
            }
            FrequencyComboBox.Parent = this;
            FrequencyComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            FrequencyComboBox.SelectedIndexChanged += new EventHandler(ChangeFreqComboBoxSelectedIndex);
            FrequencyComboBox.SelectedIndex = 0;
        }

        private void DrawCorrectionLimitControl()
        {

            CorrectionLimitLbl = new Label();
            CorrectionLimitLbl.AutoSize = true;
            CorrectionLimitLbl.Location = new System.Drawing.Point(xPos-3, yPos);
            CorrectionLimitLbl.Name = "CorrectionLimitLbl";
            CorrectionLimitLbl.Size = new System.Drawing.Size(91, 13);
            CorrectionLimitLbl.TabIndex = 2;
            CorrectionLimitLbl.Text = "Предел допустимой коррекции, " + ParameterType.DeviationMeasure();
            CorrectionLimitLbl.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            CorrectionLimitLbl.Parent = this;
            yPos += 20;
            //if (ParameterType.Name != "Ao" && ParameterType.Name != "Az") return;
            CorrectionLimitComboBox = new ComboBox();
            CorrectionLimitComboBox.Location = new System.Drawing.Point(xPos, yPos);
            CorrectionLimitComboBox.Name = String.Format("CorrectionLimitComboBox_{0}", ParameterType.Name);
            CorrectionLimitComboBox.Parent = this;
            CorrectionLimitComboBox.Width = 200;
            FillCorrectionLimitsCBItems();
            CorrectionLimitComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ///CorrectionLimitComboBox.SelectedIndexChanged += new EventHandler(ChangeFreqComboBoxSelectedIndex);
            CorrectResultsButton = new Button();
            CorrectResultsButton.Location = new System.Drawing.Point(xPos + 20 + CorrectionLimitComboBox.Width, yPos-1);
            CorrectResultsButton.Name = String.Format("CorrectResultsButton_{0}", ParameterType.Name);
            CorrectResultsButton.Text = "Произвести коррекцию";
            CorrectResultsButton.Parent = this;
            CorrectResultsButton.Width = 180;
            CorrectResultsButton.Click += new EventHandler(CorrectResults);

            CorrectionResultPB = new ProgressBar();
            CorrectionResultPB.Name = String.Format("CorrectionResultPB_{0}", ParameterType.Name);
            CorrectionResultPB.Parent = this;
            CorrectionResultPB.Location = new System.Drawing.Point(xPos + 15 + CorrectionLimitComboBox.Width + 20 + CorrectResultsButton.Width + 20, yPos);
            CorrectionResultPB.Width = 150;
            CorrectionResultPB.Visible = false;

        }
        private void FillCorrectionLimitsCBItems()
        {
            decimal[] corrLimList = ParameterType.GetCorrectionLimitsList();
            CorrectionLimitComboBox.Items.Clear();
            foreach (decimal l in corrLimList)
            {
                CorrectionLimitComboBox.Items.Add(l.ToString());
            }
            if (CorrectionLimitComboBox.Items.Count > 0 ) CorrectionLimitComboBox.SelectedIndex = 0;
        }
        private void CorrectResults(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Будет произведена коррекция результатов вышедших за норму с отклонением <= " + CorrectionLimitComboBox.Text + ParameterType.DeviationMeasure() + "\n\nВы согласны?", "Коррекция параметра " + ParameterType.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                //RefreshAfterCorrection();
                //FillCorrectionLimitsCBItems();
                SendCorrectionQueriesToDataBase(ParameterType.CorrectNotNormalResults(ServiceFunctions.convertToDecimal(CorrectionLimitComboBox.Text)));
                mReader.RefreshOutOfNormaPanel(ParameterType.Name);
                


            }
        }

        private void SendCorrectionQueriesToDataBase(List<string> queries)
        {
            if (queries.Count == 0) return;
            List<string> tmp = new List<string>();
            int sendLimit = 50;
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
    }
}
