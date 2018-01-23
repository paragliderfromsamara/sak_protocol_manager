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
        private DataGridView ValuesTable;
        private MeasureResultReader mReader;
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
            drawLabel("min_val", String.Format("Мин. значение: {0}{1};\n\nСреднее значение: {2}{1};\n\nМакс. значение: {3}{1};\n\nИзмерено {4}% из {5}%;", ParameterData.MinVal, ParameterData.ResultMeasure(), ParameterData.AverageVal, ParameterData.MaxVal, ParameterData.MeasuredPercent, ParameterData.NormalPercent));
            xPos = leftOffset;

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
                //case "Rж":
                //case "dR":
               // case "Cр":
               // case "Rиз1":
              //  case "Rиз2":
              //  case "Co":
                    
                    //return DrawPrimaryParameterTable(pData);
             //       return DrawByLeadsParameterTable();
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
                    return DrawByLeadsParameterTable();
                    //return DrawDefaultTable();
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
            ValuesTable.Size = new System.Drawing.Size(730, 280);
            ValuesTable.ScrollBars = ScrollBars.Vertical;
            ValuesTable.BackColor = System.Drawing.Color.MintCream;
            ValuesTable.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            ValuesTable.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            ValuesTable.RowsDefaultCellStyle = dataGridViewCellStyle2;
            ValuesTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ValuesTable.MultiSelect = false;
            ValuesTable.EditMode = DataGridViewEditMode.EditOnEnter;
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

        private DataGridView DrawByLeadsParameterTable()
        {
            DataGridView dgv = new DataGridView();
            bool isQuatro = ParameterData.ParameterType.Structure.BendingTypeLeadsNumber == 4;
            string[] subElHideAlways = isQuatro ? new string[] { "K1", "K2", "K3", "K9", "K10", "K11", "K12" } : new string[] { "dR", "K1", "K2", "K3", "K9", "K10", "K11", "K12", "Cр", "Ea" };
            bool isPairParam = ParameterData.ParameterType.Name == "Cр" || ParameterData.ParameterType.Name == "Ea" || ParameterData.ParameterType.Name == "dR";
            dgv.Columns.Add("element_number", String.Format("{0} №", ParameterData.ParameterType.Structure.BendingTypeName));
            dgv.Columns.Add("lead", isQuatro && isPairParam ? "Пара" : "Жила");
            dgv.Columns.Add("value", String.Format("Результат {0}", ParameterData.ResultMeasure()));
            dgv.Columns.Add("min_val", String.Format("Мин. {0}", ParameterData.ResultMeasure()));
            dgv.Columns.Add("max_val", String.Format("Макс. {0}", ParameterData.ResultMeasure()));
            dgv.Columns["min_val"].Visible = ParameterData.MinValue != Decimal.MinValue;
            dgv.Columns["max_val"].Visible = ParameterData.MaxValue != Decimal.MaxValue;
 
            dgv.Columns["lead"].Visible = !subElHideAlways.Contains(ParameterData.ParameterType.Name);// ParameterData.ParameterType.Name != "dR" && ParameterData.ParameterType.Name != "K1";
            dgv.MultiSelect = false;
            dgv.Columns["max_val"].ReadOnly = dgv.Columns["min_val"].ReadOnly = dgv.Columns["element_number"].ReadOnly = dgv.Columns["lead"].ReadOnly = true;

            TestResult[] results = ParameterData.TestResults;
            if (results.Length > 0)
            {
                foreach(TestResult result in results)
                {
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
            if (dgv.Rows[e.RowIndex].GetType().Name != "TestResultDataGridViewRow")
            {
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                return;
            }
            TestResultDataGridViewRow tdgr = dgv.Rows[e.RowIndex] as TestResultDataGridViewRow;
            try
            {
                decimal val = Convert.ToDecimal(tdgr.Cells[e.ColumnIndex].Value);
                if (val == tdgr.Result.BringingValue) return;
                tdgr.Result.UpdateResult(val);
                tdgr.DefaultCellStyle = GetRowStyle(tdgr.Result);
                MessageBox.Show("Значение успешно обновлено.", "Обновлено", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            catch (System.NullReferenceException)
            {

            }
            catch (System.FormatException ex)
            {
                MessageBox.Show(String.Format("Значение должно быть числовым с десятичным разделителем \"{0}\"", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator), ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                tdgr.Cells[e.ColumnIndex].Value = tdgr.Result.GetStringTableValue();
            }
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
            //string recElNumTxt, genElName
            DataGridView dgv = new DataGridView();
            bool isPair = ParameterData.ParameterType.Structure.BendingTypeLeadsNumber == 2;
            bool isAl = ParameterData.ParameterType.Name == "al";
            dgv.Columns.Add("receiver_number", String.Format("{0} приёмника №", ParameterData.ParameterType.Structure.BendingTypeName));    //0
            dgv.Columns.Add("sub_receiver_number", "Пара приёмника");                                                                       //1
            dgv.Columns.Add("sub_transponder_number", "Пара генератора");                                                                   //2
            dgv.Columns.Add("transponder_number", String.Format("{0} генератора №", ParameterData.ParameterType.Structure.BendingTypeName));//3
            dgv.Columns.Add("freq_range", "Диапазон, кГц");                                                                                 //4
            dgv.Columns.Add("value", "результат, " + ParameterData.ParameterType.ParameterDataList[0].ResultMeasure());                     //5
            dgv.Columns.Add("norma", isAl ? "Макс" : "Мин");                                                                                //6

            dgv.Columns["sub_receiver_number"].Visible = !isPair;
            dgv.Columns["sub_transponder_number"].Visible = !isPair && !isAl;
            dgv.Columns["transponder_number"].Visible = !isAl;

            foreach (DataGridViewColumn c in dgv.Columns) c.ReadOnly = c.Name != "value";

            TestResult[] results = ParameterData.TestResults;
            if (results.Length > 0)
            {
                foreach (TestResult result in results)
                {
                    TestResultDataGridViewRow r = new TestResultDataGridViewRow(result);
                    r.CreateCells(dgv);
                    r.Cells[0].Value = result.ElementNumber; //receiver_number
                    r.Cells[1].Value = result.SubElementTitle(); //sub_receiver_number
                    r.Cells[2].Value = result.GeneratorSubElementTitle(); //sub_transponder_number
                    r.Cells[3].Value = result.GeneratorElementNumber; //transponder_number
                    r.Cells[4].Value = result.ParameterData.GetFreqRangeTitle(); //freq_range
                    r.Cells[5].Value = result.GetStringTableValue(); //value
                    r.Cells[6].Value = isAl ? ParameterData.MaxValue : ParameterData.MinValue; //norma

                    r.Cells[5].ReadOnly = result.Affected; //лочим редактирование плохих структур

                    r.DefaultCellStyle = GetRowStyle(result);
                    dgv.Rows.Add(r);
                }
            }
            dgv.CellValueChanged += Dgv_CellValueChanged;
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
