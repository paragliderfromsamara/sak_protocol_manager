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
        private DataGridView ValuesTable;
        private int xPos, yPos;
        private int leftOffset = 20;
        private int topOffset = 30;
        private string[] LeadTitles = new string[] { "Жила a", "Жила b", "Жила c", "Жила d" };
        private Label HeadLbl;
        public ParameterTypeTabPage(MeasureParameterType parameter_type)
        {
            this.ParameterType = parameter_type;
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
            drawLabel(String.Format("tab_header_{0}", ParameterType.Id), String.Format("Результаты измерения параметра {0}", ParameterType.Name)); //заголовок
            yPos += 20;
            DrawFrequencyComboBox();
            if (FrequencyComboBox != null) yPos += 30;
            DrawValuesTable();
        }

        private void drawLabel(string name, string text)
        {
            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.Location = new System.Drawing.Point(xPos, yPos);
            lbl.Name = name;
            lbl.Size = new System.Drawing.Size(91, 13);
            lbl.TabIndex = 2;
            lbl.Text = text;
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
                    return DrawPrimaryParameterTable(pData);

                //    return DrawByLeadsParameterTable(pData);
                //case "Cр":
                
                //return DrawByElementsParameterTable(pData);
                case "Ao":
                case "Az":
                    return DrawPVTable(pData);
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
                ValuesTable.Size = new System.Drawing.Size(700, 500);
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
            int leadsNumber = pData.ParameterType.Structure.BendingTypeLeadsNumber;
            for (uint i = 0; i < leadsNumber; i++)
            {
                string title = String.Format("lead_{0}", i + 1);
                dgv.Columns.Add(title, LeadTitles[i]);
                dgv.Columns[title].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            dgv.Columns.Add("measure", "мера");
            TestResult[] results = pData.TestResults;
            if (results.Length > 0)
            {
                int leadNumber = 1;
                int rowsNumber = -1;
                for (int i = 0; i < results.Length; i++)
                {
                    if (leadNumber > results[i].SubElementNumber || rowsNumber < 0)
                    {
                        rowsNumber++;
                        dgv.Rows.Add(1);
                        dgv.Rows[rowsNumber].Cells["element_number"].Value = results[i].ElementNumber;
                        dgv.Rows[rowsNumber].Cells["measure"].Value = pData.ResultMeasure();
                    }
                    leadNumber = results[i].SubElementNumber;
                    dgv.Rows[rowsNumber].Cells[String.Format("lead_{0}", leadNumber)].Value = results[i].GetStringTableValue();
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

        private DataGridView DrawPVTable(MeasuredParameterData pData)
        {
            DataGridView dgv = new DataGridView();
            if (pData.ParameterType.Structure.BendingTypeLeadsNumber > 2)
            {
                dgv.Columns.Add("receiver_number", String.Format("{0} приёмника №", pData.ParameterType.Structure.BendingTypeName));
                dgv.Columns.Add("sub_receiver_number", String.Format("пара приёмника", pData.ParameterType.Structure.BendingTypeName));
                dgv.Columns.Add("sub_transponder_number", String.Format("пара генератора", pData.ParameterType.Structure.BendingTypeName));
                dgv.Columns.Add("transponder_number", String.Format("{0} генератора №", pData.ParameterType.Structure.BendingTypeName));
            }
            else
            {
                dgv.Columns.Add("receiver_number", String.Format("{0} приёмника №", pData.ParameterType.Structure.BendingTypeName));
                dgv.Columns.Add("transponder_number", String.Format("{0} генератора №", pData.ParameterType.Structure.BendingTypeName));
            }
            dgv.Columns.Add("value", "результат");
            dgv.Columns.Add("measure", "мера");
            TestResult[] results = pData.TestResults;
            if (results.Length > 0)
            {
                dgv.Rows.Add(results.Length);
                for (int i = 0; i < results.Length; i++)
                {
                    dgv.Rows[i].Cells["receiver_number"].Value = results[i].ElementNumber;
                    if(pData.ParameterType.Structure.BendingTypeLeadsNumber > 2)
                    {
                        dgv.Rows[i].Cells["sub_receiver_number"].Value = results[i].SubElementTitle();
                        dgv.Rows[i].Cells["sub_transponder_number"].Value = results[i].GeneratorSubElementTitle();
                    }
                    dgv.Rows[i].Cells["transponder_number"].Value = results[i].GeneratorElementNumber;
                    dgv.Rows[i].Cells["value"].Value = results[i].GetStringTableValue();
                    dgv.Rows[i].Cells["measure"].Value = pData.ResultMeasure();
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
