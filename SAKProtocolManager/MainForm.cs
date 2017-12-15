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
using MySql.Data.MySqlClient;
using System.Threading;

namespace SAKProtocolManager
{
    public partial class MainForm : Form
    {
        private int CurrentWidth = 0;
        private int CurrentHeight = 0;
        private long TestCount = 0;
        private MeasureResultReader readerForm = null;
        private DBControl mySql = new DBControl(DBQueries.Default.DBName);
        public MeasureParameterType[] MeasureParameterTypes = new MeasureParameterType[] { }; //Типы измеряемых параметров
        public MeasureParameterType MPT;
        public FrequencyRange[] FreqRanges = new FrequencyRange[] { }; //Диапазоны частот
        public BendingType[] BendingTypes = new BendingType[] { }; //Типы повива
        public DRFormula[] DRFormuls = new DRFormula[] { };   //Формулы вычисления омической ассиметрии
        public DRAdductionFormula[] DRAdductionFormuls = new DRAdductionFormula[] { };  //Формулы приведения оммической ассиметрии
     
        public MainForm()
        {
            InitializeComponent();
            inProcessLabel.Visible = progressBarPanel.Visible = false;
            initTestsList();
            SetDBConstants();

            /// Thread.Sleep(6000);
            // sts.Close();
        }
        private void SetDBConstants()
        {
            MeasureParameterType mpt = new MeasureParameterType();
            FrequencyRange fr = new FrequencyRange();
            BendingType bt = new BendingType();
            DRFormula drf = new DRFormula();
            DRAdductionFormula draf = new DRAdductionFormula();
            //Cable cable = new Cable("22");
            //this.Text = cable.Name;
            this.MeasureParameterTypes = mpt.GetAll();
            this.FreqRanges = fr.GetAll();
            this.BendingTypes = bt.GetAll();
            this.DRFormuls = drf.GetAll();
            this.DRAdductionFormuls = draf.GetAll();
         //   this.FreqRanges = FrequencyRange.GetAll();
          //  this.BendingTypes = BendingType.GetAll();
           // this.DRFormuls = DRFormula.GetAll();
          //  this.DRAdductionFormuls = DRAdductionFormula.GetAll();
        }
        private void initTestsList()
        {

            ClearList.Enabled = button1.Enabled = false;
            string comDateRange = DBQueries.Default.MinMaxDateQuery;
            mySql.MyConn.Open();
            TestCount = mySql.RunNoQuery(DBQueries.Default.TestCount);
            MySqlDataAdapter dateRange = new MySqlDataAdapter(comDateRange, mySql.MyConn);
            dataSetTest.Tables["date_range"].Rows.Clear();
            dateRange.Fill(dataSetTest.Tables["date_range"]);
            mySql.MyConn.Close();
            selectedCountLbl.Text = "База данных испытаний пуста";
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
                fillTestList(dbDateFormat(dateTimeFrom.Value), dbDateFormat(dateTimeTo.Value));
            }


        }


        private void fillTestList(string dateMin, string dateMax)
        {
            string com = String.Format(DBQueries.Default.SelectTestsList, dateMin, dateMax);
            int rowsCount;
            //com += " limit 10000";
            ClearList.Visible = false;
            inProcessLabel.Visible = true;
            button1.Enabled = false;
            this.Refresh();
            this.Cursor = Cursors.WaitCursor;
            mySql.MyConn.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(com, mySql.MyConn);
            dataSetTest.Tables["ispytan"].Rows.Clear();
            da.Fill(dataSetTest.Tables["ispytan"]);
            mySql.MyConn.Close();
            testsListView.DataSource = dataSetTest.Tables["ispytan"];
            testsListView.Refresh();
            inProcessLabel.Visible = false;
            ClearList.Visible = true;
            this.Cursor = Cursors.Arrow;
            //button1.Enabled = true;
            this.Refresh();
            rowsCount = testsListView.Rows.Count - 1;
            ClearList.Enabled = rowsCount > 0;
            if (TestCount == 0)
            {
                selectedCountLbl.Text = "База данных испытаний пуста";
            }
            else
            {
                if (rowsCount == 0) selectedCountLbl.Text = "В заданном промежутке времени испытаний не найдено";
                else if (rowsCount == 1) selectedCountLbl.Text = String.Format("Выбрано 1 испытание из {0}", TestCount);
                else if (rowsCount > 1 && rowsCount < 5) selectedCountLbl.Text = String.Format("Выбрано {0} испытания из {1}", rowsCount, TestCount);
                else selectedCountLbl.Text = String.Format("Выбрано {0} испытаний из {1}", rowsCount, TestCount);

            }
        }

        private string dbDateFormat(DateTime dt)
        {
            return String.Format("{0}-{1}-{2} {3}:{4}:{5}", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            fillTestList(dbDateFormat(dateTimeFrom.Value), dbDateFormat(dateTimeTo.Value));
            //label3.Text = String.Format("{0} - {1}", dateTimeFrom.Value.ToString(), dateTimeTo.Value.ToString());
        }
        private void MeasureResultReaderClosed(object sender, EventArgs e)
        {
            TestListtPanel.Enabled = true;
            this.readerForm = null;
        }


        private void OpenButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
             //   WaitingStatus wts = new WaitingStatus();
            //if (testsListView.SelectedRows.Count == 0) return;
            string test_id = testsListView.SelectedRows[0].Cells[0].Value.ToString();
            //if (String.IsNullOrWhiteSpace(test_id)) return;
            TestListtPanel.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            MeasureResultReader form = new MeasureResultReader(test_id, this);
            form.FormClosed += new FormClosedEventHandler(this.MeasureResultReaderClosed);
            form.Show();
            this.readerForm = form;
            this.Cursor = Cursors.Default;
                //wts.StopStatus();
            }
            catch (ThreadAbortException) { }
            
        }

        private void testsListView_SelectionChanged(object sender, EventArgs e)
        {
            if (testsListView.SelectedRows.Count == 0)
            {
                openMeasureResultReaderToolStripMenuItem.Enabled = false;
            }
            else
            {
                if (testsListView.SelectedRows[0].Cells[0].Value == null) return;
                string test_id = testsListView.SelectedRows[0].Cells[0].Value.ToString();
                openMeasureResultReaderToolStripMenuItem.Enabled = !String.IsNullOrWhiteSpace(test_id);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<string> ids = new List<string>();
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
                progressBarPanel.Visible = true;
                int i = 0;
                int j = 0;
                string strIds = String.Empty;
                foreach (string id in ids)
                {
                    if (i > 0) strIds += ",";
                    strIds += id;
                    if (i == 100 || j == (ids.Count -1))
                    {
                        CableTest.DeleteTest(strIds);
                        progressBarTest.Value += i;
                        progressBarLbl.Text = String.Format("Удалено {0} из {1} испытаний", j, ids.Count);
                        progressBarLbl.Refresh();
                        strIds = String.Empty;
                        i = -1;
                    }
                    i++;
                    j++;
                }
                progressBarPanel.Visible = false;
                initTestsList();
                //fillTestList(dbDateFormat(dateTimeFrom.Value), dbDateFormat(dateTimeTo.Value));
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
            testsListView.Width += widthDelta;
            testsListView.Height += heightDelta;
            CurrentWidth = this.Width;
            CurrentHeight = this.Height;
        }

        public void UpdateSelectedCableLength(int length)
        {
            testsListView.SelectedRows[0].Cells["length"].Value = length; 
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string test_id = testsListView.SelectedRows[0].Cells[0].Value.ToString();
            CableTest.DeleteTest(test_id);
            testsListView.Rows.Remove(testsListView.SelectedRows[0]);
            if (testsListView.SelectedRows.Count > 0) testsListView.SelectedRows[0].Selected = false;
        }

        private void dateTimeFrom_ValueChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            ClearList.Enabled = false;
        }

        private void dateTimeTo_ValueChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            ClearList.Enabled = false;
        }

        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (!this.Enabled && this.readerForm != null) readerForm.Focus(); 
        }
    }

   
}
