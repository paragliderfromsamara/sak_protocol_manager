using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using SAKProtocolManager.DBEntities;
using MySql.Data.MySqlClient;
using System.Threading;
using System.IO;
using System.Globalization;
using System.Diagnostics;
using NormaMeasure.DBControl;
using Tables = NormaMeasure.DBControl.Tables;

namespace SAKProtocolManager
{
    public partial class MainForm : Form
    {
        private int CurrentWidth = 0;
        private int CurrentHeight = 0;
        private long TestCount = 0;
        private MeasureResultReader readerForm = null;
        private DBEntityTable tests = new DBEntityTable(typeof(Tables.CableTest));
        //public MeasureParameterType[] MeasureParameterTypes = new MeasureParameterType[] { }; //Типы измеряемых параметров
        //public MeasureParameterType MPT;
        // public FrequencyRange[] FreqRanges = new FrequencyRange[] { }; //Диапазоны частот
        //public BendingType[] BendingTypes = new BendingType[] { }; //Типы повива
        //public DRFormula[] DRFormuls = new DRFormula[] { };   //Формулы вычисления омической ассиметрии
        //public DRAdductionFormula[] DRAdductionFormuls = new DRAdductionFormula[] { };  //Формулы приведения оммической ассиметрии


        public MainForm()
        {
            bool isActive = !Properties.Settings.Default.NeedKey || hasValidKey();
            TestHistoryItem[] historyItems = TestHistoryItem.GetFromIniFile();
            setSeparator();
            InitializeComponent();
            setSearchType();
            progressBarPanel.Visible = false;
            initTestsList();
            //SetDBConstants();
            this.Text =Application.ProductName + " v." + Application.ProductVersion;
            if (!isActive) this.Text += String.Format(" (ознакомительный период до {0})", Properties.Settings.Default.FreePeriodEndDate.ToShortDateString());
            CurrentWidth = this.Width;
            CurrentHeight = this.Height;
            OpenRegForm.Visible = !isActive;
            руководствоПользователяToolStripMenuItem.Enabled = File.Exists("Руководство пользователя.pdf");
            FillOpenedTestHistory(historyItems);
        /// Thread.Sleep(6000);
        // sts.Close();
    }
        private void setSeparator()
        {
            string CultureName = Thread.CurrentThread.CurrentCulture.Name;
            CultureInfo ci = new CultureInfo(CultureName);
            if (ci.NumberFormat.NumberDecimalSeparator != ".")
            {
                // Forcing use of decimal separator for numerical values
                ci.NumberFormat.NumberDecimalSeparator = ".";
                Thread.CurrentThread.CurrentCulture = ci;
            }
        }
        private void SetDBConstants()
        {
            //MeasureParameterType mpt = new MeasureParameterType();
            //FrequencyRange fr = new FrequencyRange();
            //BendingType bt = new BendingType();
            //DRFormula drf = new DRFormula();
            //DRAdductionFormula draf = new DRAdductionFormula();
            //Cable cable = new Cable("22");
            //this.Text = cable.Name;
            //this.MeasureParameterTypes = mpt.GetAll();
            //this.FreqRanges = fr.GetAll();
            //this.BendingTypes = bt.GetAll();
            //this.DRFormuls = drf.GetAll();
            //this.DRAdductionFormuls = draf.GetAll();
         //   this.FreqRanges = FrequencyRange.GetAll();
          //  this.BendingTypes = BendingType.GetAll();
           // this.DRFormuls = DRFormula.GetAll();
          //  this.DRAdductionFormuls = DRAdductionFormula.GetAll();
        }

        private void intiTestsList()
        {

        }

        private void initTestsList()
        {
            try
            {
                //TestCount = tests.CountEntities();
                ClearList.Enabled = SearchButton.Enabled = false;
                //statusPanel.Text = "База данных испытаний пуста";
                fillTestList();
                ClearList.Enabled = SearchButton.Enabled = true;
                /*
                DBControl mySql = new DBControl(DBQueries.Default.DBName);
                ClearList.Enabled = SearchButton.Enabled = false;
                string comDateRange = DBQueries.Default.MinMaxDateQuery;
                mySql.MyConn.Open();
                TestCount = mySql.RunNoQuery(DBQueries.Default.TestCount);
                MySqlDataAdapter dateRange = new MySqlDataAdapter(comDateRange, mySql.MyConn);
                dataSetTest.Tables["date_range"].Rows.Clear();
                dateRange.Fill(dataSetTest.Tables["date_range"]);
                mySql.MyConn.Close();
                statusPanel.Text = "База данных испытаний пуста";
                if (dataSetTest.Tables["date_range"].Rows.Count > 0)
                {
                    string dateMin, dateMax, dateToMin, dateToMax, dateFromMin, dateFromMax;
                    dateMin = dataSetTest.Tables["date_range"].Rows[0][1].ToString();
                    dateMax = dataSetTest.Tables["date_range"].Rows[0][0].ToString();
                    if (String.IsNullOrWhiteSpace(dateMin) || String.IsNullOrWhiteSpace(dateMin)) return;
                    dateFromMin = dateMin.Replace(dateMin.Substring(10), " 00:00:00");
                    dateFromMax = dateMax.Replace(dateMax.Substring(10), " 00:00:00");
                    dateToMin = dateMin.Replace(dateMin.Substring(10), " 23:59:59");
                    dateToMax = dateMax.Replace(dateMax.Substring(10), " 23:59:59");
                    //label3.Text = dbDateFormat(DateTime.Parse(dateFromMax));
                    dateTimeFrom.MinDate = DateTime.Parse(dateFromMin);
                    dateTimeFrom.MaxDate = dateTimeFrom.Value = DateTime.Parse(dateFromMax);
                    dateTimeTo.MinDate = DateTime.Parse(dateToMin);
                    dateTimeTo.MaxDate = dateTimeTo.Value = DateTime.Parse(dateToMax);
                    fillTestList();
                }
                */
            }
            catch(MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TestHistoryItemsToolStrip.Enabled = searchPanel.Enabled = false;
            }



        }


        private void fillTestList()
        {
            try
            {
                string defaultText = SearchButton.Text;
                //com += " limit 10000";
                ClearList.Visible = false;
                SearchButton.Enabled = false;
                SearchButton.Text = "ИДЁТ ПОИСК...";
                this.Refresh();
                this.Cursor = Cursors.WaitCursor;


                //string sType = Properties.Settings.Default.SearchType;
                //return (sType == "byTestId") ? String.Format(DBQueries.Default.SelectTestById, testIdField.Value) : String.Format(DBQueries.Default.SelectTestsList, dbDateFormat(dateTimeFrom.Value), dbDateFormat(dateTimeTo.Value));
                if (Properties.Settings.Default.SearchType == "byTestId")
                {
                    tests = Tables.CableTest.find_by_id_for_test_list((uint)testIdField.Value);
                }else
                {
                    tests = Tables.CableTest.find_by_date_for_test_list(dbDateFormat(dateTimeFrom.Value), dbDateFormat(dateTimeTo.Value));
                }


                testsListView.DataSource = tests;
                testsListView.Refresh();
                ClearList.Visible = true;
                SearchButton.Text = defaultText;
                this.Cursor = Cursors.Arrow;
                SearchButton.Enabled = true;
                this.Refresh();
                ClearList.Enabled = tests.Rows.Count > 0;
                if (TestCount == 0)
                {
                    selectedCountLbl.Text = "База данных испытаний пуста";
                }
                else
                {
                    if (tests.Rows.Count == 0) selectedCountLbl.Text = String.Format("Показано 0 испытаний из {0}", TestCount);
                    else if (tests.Rows.Count == 1) selectedCountLbl.Text = String.Format("Показано 1 испытание из {0}", TestCount);
                    else if (tests.Rows.Count > 1 && tests.Rows.Count < 5) selectedCountLbl.Text = String.Format("Показано {0} испытания из {1}", tests.Rows.Count, TestCount);
                    else selectedCountLbl.Text = String.Format("Показано {0} испытаний из {1}", tests.Rows.Count, TestCount);
                }
                /*
                string com = buildQueryBySearchType();
                DBControl mySql = new DBControl(DBQueries.Default.DBName);
                int rowsCount;
                string defaultText = SearchButton.Text;
                //com += " limit 10000";
                ClearList.Visible = false;
                SearchButton.Enabled = false;
                SearchButton.Text = "ИДЁТ ПОИСК...";
                this.Refresh();
                this.Cursor = Cursors.WaitCursor;
                mySql.MyConn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(com, mySql.MyConn);
                dataSetTest.Tables["ispytan"].Rows.Clear();
                da.Fill(dataSetTest.Tables["ispytan"]);
                mySql.MyConn.Close();
                testsListView.DataSource = dataSetTest.Tables["ispytan"];
                testsListView.Refresh();
                ClearList.Visible = true;
                SearchButton.Text = defaultText;
                this.Cursor = Cursors.Arrow;
                SearchButton.Enabled = true;
                this.Refresh();
                rowsCount = testsListView.Rows.Count - 1;
                ClearList.Enabled = rowsCount > 0;
                if (TestCount == 0)
                {
                    selectedCountLbl.Text = "База данных испытаний пуста";
                }
                else
                {
                    if (rowsCount == 0) selectedCountLbl.Text = String.Format("Показано 0 испытаний из {0}", TestCount);
                    else if (rowsCount == 1) selectedCountLbl.Text = String.Format("Показано 1 испытание из {0}", TestCount);
                    else if (rowsCount > 1 && rowsCount < 5) selectedCountLbl.Text = String.Format("Показано {0} испытания из {1}", rowsCount, TestCount);
                    else selectedCountLbl.Text = String.Format("Показано {0} испытаний из {1}", rowsCount, TestCount);

                }
                */
            }
            catch(MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка связи с базой данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TestHistoryItemsToolStrip.Enabled = searchPanel.Enabled = false;
            }

        }

        private string dbDateFormat(DateTime dt)
        {
            //return dt.ToString("dd-MM-yyyy hh:mm:ss");
            return String.Format("{0}-{1}-{2} {3}:{4}:{5}", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            fillTestList();
            //label3.Text = String.Format("{0} - {1}", dateTimeFrom.Value.ToString(), dateTimeTo.Value.ToString());
        }
        private void MeasureResultReaderClosed(object sender, EventArgs e)
        {
            TestListtPanel.Enabled = topMenu.Enabled = true;
            this.readerForm = null;
        }


        private void OpenButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenTestFromTable();
        }
        private void OpenTestFromTable()
        {
            try
            {
                string test_id = testsListView.SelectedRows[0].Cells[0].Value.ToString();
                OpenTest(test_id);
            }
            catch (NullReferenceException) { }
        }

        private Tables.CableTest GetTestById(string testId)
        {
            uint tId = 0;
            uint.TryParse(testId, out tId);
            Tables.CableTest test = null;
            DataRow[] r = tests.Select($"{Tables.CableTest.CableTestId_ColumnName} = {tId}");
            if (r.Length > 0) test = (Tables.CableTest)r[0];
            else
            {
                DBEntityTable t = Tables.CableTest.find_by_id_for_test_list(tId);
                if (t.Rows.Count > 0) test = (Tables.CableTest)t.Rows[0];
            }
            return test;
        }

        private void OpenTest(string testId)
        {
            try
            {
                Tables.CableTest test = GetTestById(testId);
                topMenu.Enabled = TestListtPanel.Enabled = false;
                this.Cursor = Cursors.WaitCursor;
                if (test != null)
                {
                    MeasureResultReader form = new MeasureResultReader(test, this);
                    form.FormClosed += new FormClosedEventHandler(this.MeasureResultReaderClosed);
                    form.Show();
                    this.readerForm = form;
                    FillOpenedTestHistory(TestHistoryItem.PushToHistory(test.TestId.ToString(), test.CableName));
                }
                else
                {
                    topMenu.Enabled = TestListtPanel.Enabled = true;
                    MessageBox.Show("Испытание с номером " + testId + " не найдено!!!", "Испытание не найдено", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                this.Cursor = Cursors.Default;
            }
            catch (NullReferenceException) { }
        }

        

        private void testsListView_SelectionChanged(object sender, EventArgs e)
        {
            if (testsListView.SelectedRows.Count == 0)
            {
                testListContextMenu.Enabled = false;
            }
            else
            {
                if (testsListView.SelectedRows[0].Cells[0].Value == null) return;
                string test_id = testsListView.SelectedRows[0].Cells[0].Value.ToString();
                testListContextMenu.Enabled = !String.IsNullOrWhiteSpace(test_id);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<string> ids = new List<string>();
            DialogResult dr = MessageBox.Show("Вы уверены, что хотите удалить выбранные испытания навсегда?", "Подтверждение операции", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                foreach (DataGridViewRow r in testsListView.Rows)
                {
                    if (r.Cells[0].Value == null) continue;
                    string id = r.Cells[0].Value.ToString();
                    if (!String.IsNullOrWhiteSpace(id))
                    {
                        ids.Add(id);
                    }
                }
                if (ids.Count > 0)
                {
                    progressBarTest.Value = 0;
                    progressBarTest.Maximum = ids.Count;
                    progressBarLbl.Text = "";
                    controlPanel.Visible = false;
                    progressBarPanel.Visible = true;
                    int i = 0;
                    int j = 0;
                    string strIds = String.Empty;
                    foreach (string id in ids)
                    {
                        throw new NotImplementedException();
                        /*
                        if (i > 0) strIds += ",";
                        strIds += id;
                        if (i == 100 || j == (ids.Count - 1))
                        {
                            CableTest.DeleteTest(strIds);
                            progressBarTest.Value += i;
                            progressBarLbl.Text = String.Format("Удалено {0} из {1} испытаний", j, ids.Count);
                            progressBarLbl.Refresh();
                            strIds = String.Empty;
                            i = -1;
                        }
                        */
                        i++;
                        j++;
                    }
                    progressBarPanel.Visible = false;
                    controlPanel.Visible = true;
                    initTestsList();
                    //fillTestList(dbDateFormat(dateTimeFrom.Value), dbDateFormat(dateTimeTo.Value));
                }
            }


        }

        private void MainForm_ResizeBegin(object sender, EventArgs e)
        {
            CurrentWidth = this.Width;
            CurrentHeight = this.Height;
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            int heightDelta = this.Height - CurrentHeight;
            int widthDelta = this.Width - CurrentWidth;
            TestListtPanel.Height += heightDelta;
            TestListtPanel.Width += widthDelta;
            testsListView.Width += widthDelta;
            testsListView.Height += heightDelta;
            CurrentWidth = this.Width;
            CurrentHeight = this.Height;
            //searchPanel.Location = new System.Drawing.Point(searchPanel.Location.X + widthDelta, searchPanel.Location.Y);

        }

        /*
        public void UpdateSelectedTest(CableTest test)
        {
            //MessageBox.Show(String.Format("{0} {1} {2} {3}", test.Id, test.BruttoWeight, test.TestedLength, testsListView.SelectedRows[0].Cells["id"]));
            int rowIndex = -1;
            try
            {
                DataGridViewRow row = testsListView.Rows
                .Cast<DataGridViewRow>()
                .Where(r => r.Cells["id"].Value.ToString().Equals(test.Id))
                .First();
                rowIndex = row.Index;
            }
            catch (NullReferenceException) { }

            if (rowIndex != -1)
            {
                testsListView.Rows[rowIndex].Cells["length"].Value = test.TestedLength;
                testsListView.Rows[rowIndex].Cells["BruttoWeight"].Value = test.BruttoWeight;
            }
            
        }
        */

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы уверены, что хотите удалить выбранное испытание навсегда?", "Подтверждение операции", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                throw new NotImplementedException();
                string test_id = testsListView.SelectedRows[0].Cells[0].Value.ToString();
               // CableTest.DeleteTest(test_id);
                TestHistoryItem.RemoveFromHistory(test_id);
                testsListView.Rows.Remove(testsListView.SelectedRows[0]);
                if (testsListView.SelectedRows.Count > 0) testsListView.SelectedRows[0].Selected = false;
                MessageBox.Show("Испытание успешно удалено из Базы Данных", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void FillOpenedTestHistory(TestHistoryItem[] items)
        {
            TestHistoryItemsToolStrip.DropDownItems.Clear();
            foreach(TestHistoryItem item in items)
            {
                TestHistoryItemsToolStrip.DropDownItems.Add(String.Format("№{0} Марка: {1}", item.Id, item.CableMark));
                TestHistoryItemsToolStrip.DropDownItems[TestHistoryItemsToolStrip.DropDownItems.Count - 1].Name = "toolStripItem_" + item.Id;
                TestHistoryItemsToolStrip.DropDownItems[TestHistoryItemsToolStrip.DropDownItems.Count - 1].Click += new EventHandler(openTestFromHistory);
            }
        }


        private void conditionsChanged_ValueChanged(object sender, EventArgs e)
        {
            //SearchButton.Enabled = true;
            ClearList.Enabled = false;
        }

        private void dateTimeTo_ValueChanged(object sender, EventArgs e)
        {
            SearchButton.Enabled = true;
            ClearList.Enabled = false;
        }

        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (!this.Enabled && this.readerForm != null) readerForm.Focus(); 
        }

        private void openTestFromHistory(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            string test_id = item.Name.Replace("toolStripItem_", "");
            OpenTest(test_id);
        }
        private void exportToPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string test_id = testsListView.SelectedRows[0].Cells[0].Value.ToString();
            PDFProtocolEntities.PDFProtocol.MakeOldStylePDFProtocol(test_id);
        }

        private void setSearchType()
        {
            getFromSettins:
            string curSearch = Properties.Settings.Default.SearchType;
            switch(curSearch)
            {
                case "byDate":
                    byDate.Checked = true;
                    return;
                case "byTestId":
                    byTestId.Checked = true;
                    return;
                default:
                    Properties.Settings.Default.SearchType = "byDate";
                    goto getFromSettins;
            }
        }

        private void searchTypeRadioBut_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (!rb.Checked) return;
            dateTimeFrom.Visible = dateTimeTo.Visible = label2.Visible = rb.Name == "byDate";
            testIdField.Visible = rb.Name == "byTestId";
            label1.Text = rb.Name == "byDate" ? "Начальная дата" : "Номер испытания";
            Properties.Settings.Default.SearchType = rb.Name;
            Properties.Settings.Default.Save();
        }

        private string buildQueryBySearchType()
        {
            string sType = Properties.Settings.Default.SearchType;
            return (sType == "byTestId") ? String.Format(DBQueries.Default.SelectTestById, testIdField.Value) : String.Format(DBQueries.Default.SelectTestsList, dbDateFormat(dateTimeFrom.Value), dbDateFormat(dateTimeTo.Value));

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            bool needReg = Properties.Settings.Default.NeedKey && !hasValidKey();
            DateTime lastOpenDate = Properties.Settings.Default.FreePeriodEndDate;
            DateTime dt = DateTime.Now;
            if (needReg)
            {
                if (dt > lastOpenDate)
                {
                    RegistrationForm rf = new RegistrationForm();
                    DialogResult dr = rf.ShowDialog();
                    if (dr == DialogResult.Abort) this.Close();
                    else if(dr == DialogResult.OK)
                    {
                        this.OpenRegForm.Visible = dr != DialogResult.OK;
                        if (dr == DialogResult.OK) this.Text = Application.ProductName + " v." + Application.ProductVersion;
                    }
                }
            }
        }

        private void OpenRegForm_Click(object sender, EventArgs e)
        {
            RegistrationForm rf = new RegistrationForm();
            DialogResult dr = rf.ShowDialog();
            this.OpenRegForm.Visible = dr != DialogResult.OK;
            if (dr == DialogResult.OK) this.Text = Application.ProductName + " v." + Application.ProductVersion; 
        }

        private bool hasValidKey()
        {
            IniFile ini = new IniFile(Properties.Settings.Default.IniSettingsFileName);
            string exKey = Properties.Settings.Default.ExpectedKey;
            string iniVal = ini.Read("ProductKey");
            return exKey == iniVal;
        }

        private void руководствоПользователяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("Руководство пользователя.pdf")) Process.Start("Руководство пользователя.pdf");
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }

   
}
