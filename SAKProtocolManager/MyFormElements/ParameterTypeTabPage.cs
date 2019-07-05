using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SAKProtocolManager.DBEntities;
using SAKProtocolManager.DBEntities.TestResultEntities;
using System.Data;
//using System.Diagnostics;

using NormaMeasure.DBControl;
using Tables = NormaMeasure.DBControl.Tables;

namespace SAKProtocolManager.MyFormElements
{
    partial class ParameterTypeTabPage : TabPage
    {
        private MeasuredParameterData ParameterDataOld;
        private Tables.MeasuredParameterData ParameterData;

        private DataGridView ValuesTable;
        private MeasureResultReader mReader;
        private int xPos, yPos;
        private int leftOffset = 20;
        private int topOffset = 20;
        private string[] LeadTitles = new string[] { "Жила 1", "Жила 2", "Жила 3", "Жила 4" };
        private Label HeadLbl;

        public ParameterTypeTabPage(Tables.MeasuredParameterData paramerter_data, MeasureResultReader r)
        {
            this.ParameterData = paramerter_data;
            this.mReader = r;
            InitializeOnParameterData();
        }


        public ParameterTypeTabPage(MeasuredParameterData paramerter_data, MeasureResultReader r)
        {
            this.ParameterDataOld = paramerter_data;
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
            drawLabel("min_val", String.Format("Мин. значение: {0}{1};\n\nСреднее значение: {2}{1};\n\nМакс. значение: {3}{1};\n\nИзмерено {4}% из {5}%;", ParameterDataOld.MinVal, ParameterDataOld.ResultMeasure(), ParameterDataOld.AverageVal, ParameterDataOld.MaxVal, ParameterDataOld.MeasuredPercent, ParameterDataOld.NormalPercent));
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
            
            switch (ParameterData.ParameterTypeId)
            {
                case Tables.MeasuredParameterType.al:// "al":
                case Tables.MeasuredParameterType.Ao:
                case Tables.MeasuredParameterType.Az:
                    return DrawPVTable();
                case Tables.MeasuredParameterType.Risol3: //"Rиз3":
                case Tables.MeasuredParameterType.Risol4:
                    return DrawIzolationCombinationTable();
                default:
                    return DrawByLeadsParameterTable();
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

            foreach (DataGridViewRow r in ValuesTable.Rows)
            {
                SetRowStyle(r);
            }
           // ValuesTable.Update();
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



            if (ValuesTable.Columns.Contains(Tables.CableTestResult.ResultForView_ColumnName)) ValuesTable.Columns[Tables.CableTestResult.ResultForView_ColumnName].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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
            ValuesTable.AllowUserToAddRows = false;
            ValuesTable.AllowUserToDeleteRows = false;
            


        }


        private DataGridView DrawIzolationCombinationTable()
        {
            DataGridView dgv = Build_DataGridView();

            dgv.Columns[Tables.CableTestResult.StructElementNumber_ColumnName].HeaderText = "Пучок №";
            dgv.Columns[Tables.CableTestResult.MeasureOnElementNumber_ColumnName].HeaderText = "Комбинация №";
            dgv.Columns[Tables.CableTestResult.ResultForView_ColumnName].HeaderText = String.Format("Результат, {0}", ParameterData.ResultMeasure_WithLength);


            dgv.MultiSelect = false;
            dgv.DataSource = ParameterData.TestResults;
            if (ParameterData.TestResults.Rows.Count > 0)
            {
                dgv.Columns[Tables.CableTestResult.StructElementNumber_ColumnName].Visible = ((Tables.CableTestResult)ParameterData.TestResults.Rows[0]).ElementNumber > 0;
            }
            return dgv;
        }

        private DataGridView Build_DataGridView()
        {
            DBEntityTable t = new DBEntityTable(typeof(Tables.CableTestResult));
            DataGridView dgv = new DataGridView();
            string[] HiddenCols = new string[] 
            {
                Tables.MeasuredParameterType.ParameterTypeId_ColumnName,
                "Temperatur",
                Tables.CableTest.CableTestId_ColumnName,
                Tables.CableTestResult.ElementNumberOnGenerator_ColumnName,
                Tables.CableTestResult.PairNumberOnGenerator_ColumnName,
                Tables.CableStructure.StructureId_ColumnName,
                "FreqDiap",
                Tables.CableTestResult.BringingResult_ColumnName,
                Tables.CableTestResult.IsAffected_ColumnName,
                Tables.CableTestResult.IsOutOfNorma_ColumnName,
                Tables.CableTestResult.MeasureResult_ColumnName
            };
            dgv.ReadOnly = true;
            foreach (DataColumn c in t.Columns)
            {
                dgv.Columns.Add(c.ColumnName, c.ColumnName);
                //dgv.Columns[c.ColumnName].HeaderText = c.ColumnName;
                dgv.Columns[c.ColumnName].DataPropertyName = c.ColumnName;
                dgv.Columns[c.ColumnName].Visible = !HiddenCols.Contains(c.ColumnName);
                //Debug.WriteLine($"Build_DataGridView: column {c.ColumnName}; visible {!HiddenCols.Contains(c.ColumnName)}");  
            }
            dgv.Columns[Tables.CableTestResult.MinAllowedValue_ColumnName].Visible = ParameterData.HasMinLimit;
            dgv.Columns[Tables.CableTestResult.MaxAllowedValue_ColumnName].Visible = ParameterData.HasMaxLimit;



            dgv.DataBindingComplete += Dgv_DataBindingComplete;
            dgv.Refresh();
            return dgv;
        }

        private void Dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridView v = sender as DataGridView;
            foreach (DataGridViewRow r in v.Rows)
            {
                SetRowStyle(r);
            }
        }

        private DataGridView DrawByLeadsParameterTable()
        {
            DataGridView dgv = Build_DataGridView();
            bool isQuatro = ParameterData.TestedStructure.StructureType.StructureLeadsAmount == 4;
            string[] subElHideAlways = isQuatro ? new string[] { "K1", "K2", "K3", "K9", "K10", "K11", "K12" } : new string[] { "dR", "K1", "K2", "K3", "K9", "K10", "K11", "K12", "Cр", "Ea" };
            bool isPairParam = ParameterData.ParameterName == "Cр" || ParameterData.ParameterName == "Ea" || ParameterData.ParameterName == "dR";
            string measure = ParameterData.ResultMeasure_WithLength;

            dgv.Columns[Tables.CableTestResult.StructElementNumber_ColumnName].HeaderText = $"{ParameterData.TestedStructure.StructureType.StructureTypeName} №";
            dgv.Columns[Tables.CableTestResult.MeasureOnElementNumber_ColumnName].HeaderText = isQuatro && isPairParam ? "Пара" : "Жила";
            dgv.Columns[Tables.CableTestResult.MeasureOnElementNumber_ColumnName].Visible = !subElHideAlways.Contains(ParameterData.ParameterName);

            dgv.Columns[Tables.CableTestResult.ResultForView_ColumnName].HeaderText = String.Format("Результат {0}", measure);

            dgv.MultiSelect = false;
            dgv.DataSource = ParameterData.TestResults;
            dgv.Refresh();


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
            dgv.Columns.Add("element_number", String.Format("{0} №", ParameterDataOld.ParameterType.Structure.BendingTypeName));
            dgv.Columns.Add("value", String.Format("Результат, {0}", ParameterDataOld.ResultMeasure()));
            dgv.Columns.Add("min_val", String.Format("Мин., {0}", ParameterDataOld.ResultMeasure()));
            dgv.Columns.Add("max_val", String.Format("Макс., {0}", ParameterDataOld.ResultMeasure()));
            dgv.Columns.Add("percent_out", String.Format("Отклонение {0}", ParameterDataOld.ParameterType.DeviationMeasure()));
            dgv.Columns["min_val"].Visible = ParameterDataOld.MinValue > Decimal.MinValue;
            dgv.Columns["max_val"].Visible = ParameterDataOld.MaxValue < Decimal.MaxValue;
           
            TestResult[] results = ParameterDataOld.TestResults;
            if (results.Length > 0)
            {
                dgv.Rows.Add(results.Length);
                for (int i = 0; i < results.Length; i++)
                {
                    dgv.Rows[i].Cells["element_number"].Value = results[i].ElementNumber;
                    dgv.Rows[i].Cells["value"].Value = results[i].GetStringTableValue();
                    dgv.Rows[i].Cells["min_val"].Value = ParameterDataOld.MinValue.ToString();
                    dgv.Rows[i].Cells["max_val"].Value = ParameterDataOld.MaxValue.ToString();
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
            DataGridView dgv = Build_DataGridView();

            bool isPair = ParameterData.TestedStructure.StructureType.StructureLeadsAmount == 2;
            bool isAl = ParameterData.ParameterTypeId == Tables.MeasuredParameterType.al;

            dgv.Columns[Tables.CableTestResult.StructElementNumber_ColumnName].HeaderText = String.Format("{0} приёмника №", ParameterData.TestedStructure.StructureType.StructureTypeName);

            dgv.Columns[Tables.CableTestResult.MeasureOnElementNumber_ColumnName].HeaderText = "Пара приёмника";
            dgv.Columns[Tables.CableTestResult.MeasureOnElementNumber_ColumnName].Visible = ParameterData.TestedStructure.StructureType.StructureLeadsAmount > 2;

            dgv.Columns[Tables.CableTestResult.PairNumberOnGenerator_ColumnName].HeaderText = "Пара генератора";
            dgv.Columns[Tables.CableTestResult.PairNumberOnGenerator_ColumnName].Visible = ParameterData.TestedStructure.StructureType.StructureLeadsAmount > 2;

            dgv.Columns[Tables.CableTestResult.ElementNumberOnGenerator_ColumnName].HeaderText = String.Format("{0} генератора №", ParameterData.TestedStructure.StructureType.StructureTypeName);
            dgv.Columns[Tables.CableTestResult.ResultForView_ColumnName].HeaderText = String.Format("Результат {0}", ParameterData.ResultMeasure_WithLength);


            dgv.Columns[Tables.CableTestResult.PairNumberOnGenerator_ColumnName].Visible = !isAl && !isPair;
            dgv.Columns[Tables.CableTestResult.ElementNumberOnGenerator_ColumnName].Visible = !isAl;

            dgv.MultiSelect = false;
            dgv.DataSource = ParameterData.TestResults;
            dgv.Refresh();
            return dgv;
        }



        private DataGridView MakeAffectedElementsTable(CableStructure structure)
        {
            DataGridView dgv = new DataGridView();
            dgv.Columns.Add("element_number", String.Format("{0} №", structure.BendingTypeName));
            if (structure.AffectedElements.Count > 0)
            {
                for (int i = 0; i < structure.AffectedElements[0].Values.Length; i++)
                {
                    string title = String.Format("result_{0}", i);
                    dgv.Columns.Add(title, LeadTitles[i]);
                    dgv.Columns[title].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                dgv.Rows.Add(structure.AffectedElements.Count);
                int idx = 0;
                foreach (ProzvonTestResult tr in structure.AffectedElements.Values)
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

        private void SetRowStyle(DataGridViewRow row)
        {
            DataGridViewCellStyle s = new DataGridViewCellStyle();
            bool IsAffected = (bool)row.Cells[Tables.CableTestResult.IsAffected_ColumnName].Value;
            bool IsOutOfNorm = (bool)row.Cells[Tables.CableTestResult.IsOutOfNorma_ColumnName].Value;
            //Debug.WriteLine($"{row.Cells[Tables.CableTestResult.StructElementNumber_ColumnName].Value} {IsAffected}");
            if (IsAffected)
            {
                s.BackColor = System.Drawing.Color.DarkRed;
                s.ForeColor = System.Drawing.Color.MintCream;
            }
            else if (IsOutOfNorm)
            {
                s.BackColor = System.Drawing.Color.DarkOrange;
                s.ForeColor = System.Drawing.Color.MintCream;
            }
            else
            {
                s.BackColor = System.Drawing.Color.MintCream;
            }
            s.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            s.SelectionBackColor = System.Drawing.Color.Teal;
            s.SelectionForeColor = System.Drawing.Color.OldLace;
            row.DefaultCellStyle = s;
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

            }
            else
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
