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
            inProcessLabel.Visible = false;
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

            button1.Enabled = false;
            string comDateRange = DBQueries.Default.MinMaxDateQuery;
            mySql.MyConn.Open();
            MySqlDataAdapter dateRange = new MySqlDataAdapter(comDateRange, mySql.MyConn);
            dataSetTest.Tables["date_range"].Rows.Clear();
            dateRange.Fill(dataSetTest.Tables["date_range"]);
            mySql.MyConn.Close();

            if (dataSetTest.Tables["date_range"].Rows.Count > 0)
            {
                string dateMin, dateMax, dateToMin, dateToMax, dateFromMin, dateFromMax;
                dateMin = dataSetTest.Tables["date_range"].Rows[0][1].ToString();
                dateMax = dataSetTest.Tables["date_range"].Rows[0][0].ToString();
                dateFromMin = dateMin.Replace(dateMin.Substring(10), " 00:00:00");
                dateFromMax = dateMax.Replace(dateMax.Substring(10), " 00:00:00");
                dateToMin = dateMin.Replace(dateMin.Substring(10), " 23:59:59");
                dateToMax = dateMax.Replace(dateMax.Substring(10), " 23:59:59");
                //label3.Text = dbDateFormat(DateTime.Parse(dateFromMax));
                dateTimeFrom.MinDate = dateTimeFrom.Value = DateTime.Parse(dateFromMin);
                dateTimeFrom.MaxDate = DateTime.Parse(dateFromMax);
                dateTimeTo.MinDate = DateTime.Parse(dateToMin);
                dateTimeTo.MaxDate = dateTimeTo.Value = DateTime.Parse(dateToMax);
                fillTestList(dbDateFormat(dateTimeFrom.Value), dbDateFormat(dateTimeTo.Value));
                button1.Enabled = true;
            }


        }

        private void fillTestList(string dateMin, string dateMax)
        {
            string com = String.Format(DBQueries.Default.SelectTestsList, dateMin, dateMax);
            com += " limit 10000";
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
            this.Cursor = Cursors.Arrow;
            button1.Enabled = true;
            this.Refresh();
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
            this.Enabled = true;
        }


        private void OpenButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                WaitingStatus wts = new WaitingStatus();
            //if (testsListView.SelectedRows.Count == 0) return;
            string test_id = testsListView.SelectedRows[0].Cells[0].Value.ToString();
            //if (String.IsNullOrWhiteSpace(test_id)) return;
            
            MeasureResultReader form = new MeasureResultReader(test_id, this);
            form.FormClosed += new FormClosedEventHandler(this.MeasureResultReaderClosed);
            form.Show();
            this.Enabled = false;
            wts.StopStatus();
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
                string test_id = testsListView.SelectedRows[0].Cells[0].Value.ToString();
                openMeasureResultReaderToolStripMenuItem.Enabled = !String.IsNullOrWhiteSpace(test_id);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //PDFProtocol p = new PDFProtocol();
            string[] arr = new string[100];
            arr[0] = "rrrf";
            arr.SetValue("444", 101);
            label3.Text = arr.Length.ToString();
            
        }


    }

   
}
