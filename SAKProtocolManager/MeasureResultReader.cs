using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SAKProtocolManager.DBEntities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using SAKProtocolManager.MyFormElements;
using SAKProtocolManager.DBEntities.TestResultEntities;
using SAKProtocolManager.PDFProtocolEntities;
using SAKProtocolManager.MSWordProtocolBuilder;

namespace SAKProtocolManager
{
    public partial class MeasureResultReader : Form
    {
        private int selectedStructureIdx = -1;
        private int selectedParameterTypeIdx = -1;
        private CableTest CableTest;
      
        public MainForm MainForm;

        public MeasureResultReader(CableTest test, MainForm mForm)
        {
            this.MainForm = mForm;
            this.CableTest = test;
            InitializeComponent();
            lengthUpdProgressBarField.Visible = false;
            this.Text = String.Format("#{2} Испытание кабеля {0} от {1}", CableTest.TestedCable.Name, ServiceFunctions.MyDateTime(CableTest.TestDate), CableTest.Id);
            fillTestData();
            //cableTypeLbl.Text = TestResult.GetBigRound(0, CableTest.TestedCable.Structures[0].MeasuredParameters[0].TestResults).Length.ToString();//CableId.ToString();//CableId.ToString();//this.TestId.ToString();
        }

        private bool fillStructuresComboBox()
        {
            ///CableStructure[] FailedStructures = this.CableTest.TestedCable.GetFailedStructures();
            cableStructuresList.Items.Clear();
            int sLength = this.CableTest.TestedCable.Structures.Length;
            bool val = sLength > 0;
            if (val)
            {
                for(int i = 0; i< sLength; i++)
                {
                    cableStructuresList.Items.Insert(i, this.CableTest.TestedCable.Structures[i].Name);
                }
                cableStructuresList.SelectedIndex = 0;
                StructuresLbl.Text = "Структура кабеля";
                cableStructuresList.Visible = true;
                tabControlTestResult.Visible = true;
                OutOfNormaRsltPanel.Size = new Size(810, 450);
                //OutOfNormaRsltPanel.Visible = true;
            }
            else
            {
                cableStructuresList.Items.Insert(0, "Список пуст");
                cableStructuresList.SelectedIndex = 0;
                cableStructuresList.Visible= false;
                tabControlTestResult.Visible = false;
                OutOfNormaRsltPanel.Size = new Size(810, 100);
                //OutOfNormaRsltPanel.Visible = false;
                this.Height -= tabControlTestResult.Height - 10;
            }
            return val;
        }

        private void fillTestData()
        {
            cableTypeLbl.Text = String.Format("Марка кабеля: {0}", CableTest.TestedCable.Name);
            barabanLbl.Text = String.Format("Барабан: {0} № {1}", CableTest.Baraban.Name, CableTest.Baraban.Number);
            operatorLbl.Text = String.Format("Оператор: {0} {1}.{2}.", CableTest.Operator.LastName, CableTest.Operator.FirstName[0], CableTest.Operator.ThirdName[0]);
            testedAtLbl.Text = String.Format("Испытан {0}", ServiceFunctions.MyDateTime(CableTest.TestDate));
            TemperatureLbl.Text = String.Format("Температура: {0}°С", CableTest.Temperature);
            testedLengthInput.Value = CableTest.TestedLength;
            BruttoWeightTextField.Value = CableTest.BruttoWeight;
            if (!fillStructuresComboBox())
            {
                
            }
            //test.Text = CableTest.TestedCable.Structures[0].GetProszvonResult().Length.ToString();
            //DrawTable();
            this.Refresh();
        }


        private void drawLabel(string name, string text, int x, int y, Control parent)
        {
            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.Location = new System.Drawing.Point(x, y);
            lbl.Name = name;
            lbl.Size = new System.Drawing.Size(91, 13);
            lbl.TabIndex = 2;
            lbl.Text = text;
            lbl.Parent = parent;
        }


        private MeasureParameterType[] ParameterTypesForTabs(int structure_index)
        {
            List<MeasureParameterType> pTypes = new List<MeasureParameterType>();
            foreach (MeasureParameterType mp in CableTest.TestedCable.Structures[structure_index].MeasuredParameters)
            {
                if (mp.Name != "Prozvon") pTypes.Add(mp);
            }
            return pTypes.ToArray();
        }

        private void cableStructuresList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedStructureIdx != cableStructuresList.SelectedIndex)
            {
               selectedStructureIdx = cableStructuresList.SelectedIndex;
               fillParameterTypesComboBox(selectedStructureIdx);
               drawParameterTypeTabs();
               //DrawMeasureParametersTabs(selectedStructureIdx, "");
            }
            
        }


        private void fillParameterTypesComboBox(int structIdx)
        {
            MeasureParameterType[] pTypes = CableTest.TestedCable.Structures[structIdx].MeasuredParameters;
            parameterTypeCB.Items.Clear();
            for(int i=0; i<pTypes.Length; i++)
            {
                parameterTypeCB.Items.Insert(i, pTypes[i].NameWithMeasure());
            }
            if (parameterTypeCB.Items.Count > 0)
            {
                //parameterTypeCB.Items.Insert(parameterTypeCB.Items.Count, "Прозвонка");
                parameterTypeCB.SelectedIndex = 0;
            }
        }

        private void GeneratePDFProtocolButton_Click(object sender, EventArgs e)
        {
            //PDFProtocol protocol = new PDFProtocol(this.CableTest);
            DialogResult NeedGenerate = DialogResult.Yes;
            if(CheckCableLengthIsUpdated())
            {
                NeedGenerate = MessageBox.Show(String.Format("Протокол не был пересчитан на длину кабеля {0} м. Сформированный протокол будет соответствовать длине {1} м., которая указана в Базе Данных на текущий момент \n\nВы согласны?", testedLengthInput.Value, CableTest.TestedLength), "Вопрос.", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            if (NeedGenerate == DialogResult.Yes)
            {
                PDFProtocol.MakeOldStylePDFProtocol(this.CableTest.Id);
            }
        }

        private void updateCableLength_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(String.Format("Вы уверены, что хотите пересчитать результаты испытания под длину кабеля {0} м.", testedLengthInput.Value), "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                this.Cursor = Cursors.WaitCursor;
                this.Enabled = false;
                lengthEditor.Visible = false;
                lengthUpdProgressBarField.Visible = true;
                UpdateLength();
                lengthEditor.Visible = true;
                lengthUpdProgressBarField.Visible = false;
                this.Enabled = true;
                this.Cursor = Cursors.Default;
                this.MainForm.UpdateSelectedTest(CableTest);
                drawParameterTypeTabs();
                MessageBox.Show(String.Format("Результат успешно пересчитан на длину кабеля {0} м.", testedLengthInput.Value), "Длина успешно пересчитана", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void UpdateLength()
        {
            decimal newLength = this.testedLengthInput.Value;
            decimal curLength = CableTest.TestedLength;
            long status = CableTest.UpdateTestedLength(newLength);
            if (status == 0)
            {
                LengthUpdProgressBar.Maximum = CableTest.TestResultsCount();
                LengthUpdProgressBar.Value = 0;
                LengthUpdProgressBar.Step = 50;
                lengthUpdProgressBarLbl.Text = String.Format("Пересчитано {0} из {1}", LengthUpdProgressBar.Value, LengthUpdProgressBar.Maximum);
                lengthUpdProgressBarField.Refresh();
                foreach (CableStructure structure in CableTest.TestedCable.Structures)
                {
                    foreach (MeasureParameterType pType in structure.MeasuredParameters)
                    {
                        List<string> queries = new List<string>();
                        foreach (TestResult tr in pType.TestResults)
                        {
                            queries.Add(tr.BuildUpdLengthQuery(curLength, newLength));
                            if (queries.Count == LengthUpdProgressBar.Step || (LengthUpdProgressBar.Value + queries.Count) == LengthUpdProgressBar.Maximum)
                            {
                                DBBase.SendQueriesList(queries.ToArray());
                                queries.Clear();
                                LengthUpdProgressBar.PerformStep();
                                lengthUpdProgressBarLbl.Text = String.Format("Пересчитано {0} из {1}", LengthUpdProgressBar.Value, LengthUpdProgressBar.Maximum);
                                lengthUpdProgressBarField.Refresh();
                            }
                        }
                        if (queries.Count > 0)
                        {
                            DBBase.SendQueriesList(queries.ToArray());
                            queries.Clear();
                            LengthUpdProgressBar.PerformStep();
                            lengthUpdProgressBarLbl.Text = String.Format("Пересчитано {0} из {1}", LengthUpdProgressBar.Value, LengthUpdProgressBar.Maximum);
                            lengthUpdProgressBarField.Refresh();
                        }
                        pType.RefreshTestResultsOnParameterData(false);
                        /*
                        foreach (MeasuredParameterData pData in pType.ParameterDataList)
                        {
                            
                            foreach (TestResult tr in pData.TestResults)
                            {
                                queries.Add(tr.BuildUpdLengthQuery(curLength, newLength));
                                if (queries.Count == LengthUpdProgressBar.Step || (LengthUpdProgressBar.Value + queries.Count) == LengthUpdProgressBar.Maximum)
                                {
                                    DBBase.SendQueriesList(queries.ToArray());
                                    queries.Clear();
                                    LengthUpdProgressBar.PerformStep();
                                    lengthUpdProgressBarLbl.Text = String.Format("Пересчитано {0} из {1}", LengthUpdProgressBar.Value, LengthUpdProgressBar.Maximum);
                                    lengthUpdProgressBarField.Refresh();
                                }

                            }
                            if (queries.Count > 0)
                            {
                                DBBase.SendQueriesList(queries.ToArray());
                                queries.Clear();
                                LengthUpdProgressBar.PerformStep();
                                lengthUpdProgressBarLbl.Text = String.Format("Пересчитано {0} из {1}", LengthUpdProgressBar.Value, LengthUpdProgressBar.Maximum);
                                lengthUpdProgressBarField.Refresh();
                            }                            
                        }
                        */
                    }
                }
            }
        }

        private void testedLengthInput_ValueChanged(object sender, EventArgs e)
        {
            CheckCableLengthIsUpdated();
        }

        private bool CheckCableLengthIsUpdated()
        {
            return updateCableLength.Enabled = CableTest.TestedLength != testedLengthInput.Value;
        }

        private void testedLengthInput_KeyUp(object sender, KeyEventArgs e)
        {
            CheckCableLengthIsUpdated();
        }

        private void parameterTypeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedParameterTypeIdx != parameterTypeCB.SelectedIndex)
            {
                selectedParameterTypeIdx = parameterTypeCB.SelectedIndex;
                drawParameterTypeTabs();
            }
        }

        private void drawParameterTypeTabs()
        {
            try
            {
                MeasureParameterType pType = CableTest.TestedCable.Structures[selectedStructureIdx].MeasuredParameters[selectedParameterTypeIdx];
                int sIndex = tabControlTestResult.TabPages.Count > 0 ? tabControlTestResult.SelectedIndex : 0;
                tabControlTestResult.TabPages.Clear();

                CableStructure curStructure = this.CableTest.TestedCable.Structures[cableStructuresList.SelectedIndex];
                MeasureParameterType[] mParams = ParameterTypesForTabs(cableStructuresList.SelectedIndex);
                List<TabPage> pages = new List<TabPage>();
                //if (curStructure.AffectedElementNumbers.Length>1)test.Text = curStructure.AffectedElementNumbers[1].ToString();
                //if (curStructure.AffectedElements.Length > 0) pages.Add(new ParameterTypeTabPage(curStructure, this));
                foreach (MeasuredParameterData pData in pType.ParameterDataList)
                {
                    if (pData.TestResults.Length == 0) continue;
                    ParameterTypeTabPage ptp = new ParameterTypeTabPage(pData, this);
                    ptp.Height = this.Height - 10;
                    pages.Add(ptp);
                }
                if (pages.Count > 0)
                {
                    tabControlTestResult.TabPages.AddRange(pages.ToArray());
                    tabControlTestResult.SelectedIndex = sIndex;
                    tabControlTestResult.Refresh();
                }
            }
            catch (IndexOutOfRangeException) { OutOfNormaRsltPanel.Visible = false; this.Height = this.Height - OutOfNormaRsltPanel.Height; this.Refresh(); }

        }

        public void RefreshTabs()
        {
            drawParameterTypeTabs();
        }

        private void BruttoWeightTextField_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown ud = sender as NumericUpDown;
            EditSaveBruttoButton.Enabled = ud.Value != CableTest.BruttoWeight;
        }

        private void EditSaveBruttoButton_Click(object sender, EventArgs e)
        {
            decimal w = BruttoWeightTextField.Value;
            long sts = CableTest.UpdateBruttoWeight(w);
            if (sts == 0)
            {
                this.Cursor = Cursors.WaitCursor;
                this.MainForm.UpdateSelectedTest(CableTest);
                MessageBox.Show("Вес БРУТТО успешно обновлён", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                EditSaveBruttoButton.Enabled = false;
                this.Cursor = Cursors.Default;
            }
        }

        private void MSWordImport_Click(object sender, EventArgs e)
        {
            //MSWordProtocol protocol = new MSWordProtocol();
            //protocol.Init();
            //protocol.CutCreatedTableFromTmpFile();
            //OpenXMLTableCreator.GetTableFromFile();
            //OpenXMLTableCreator.WDAddTable("test.docx", new string[,] { { "4", "5", "6" } });
            MSWordProtocolBuilder.MSWordProtocolBuilder.BuildProtocolForTest(CableTest);
        }
    }
}
