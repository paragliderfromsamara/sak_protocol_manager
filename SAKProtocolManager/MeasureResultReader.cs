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
using System.Diagnostics;
using SAKProtocolManager.DBEntities.TestResultEntities;
using SAKProtocolManager.PDFProtocolEntities;
namespace SAKProtocolManager
{
    public partial class MeasureResultReader : Form
    {
        private int selectedStructureIdx = -1;
        private CableTest CableTest;
      
        public MainForm MainForm;

        public MeasureResultReader(string test_id, MainForm mForm)
        {
            this.MainForm = mForm;
            this.CableTest = new CableTest(test_id);
            InitializeComponent();
            this.Text = String.Format("Испытание кабеля {0} от {1}", CableTest.TestedCable.Name, ServiceFunctions.MyDateTime(CableTest.TestDate));
            fillTestData();
            
            //cableTypeLbl.Text = TestResult.GetBigRound(0, CableTest.TestedCable.Structures[0].MeasuredParameters[0].TestResults).Length.ToString();//CableId.ToString();//CableId.ToString();//this.TestId.ToString();
        }

        private bool fillStructuresComboBox()
        {
            int sLength = this.CableTest.TestedCable.Structures.Length;
            bool val = sLength > 0;
            if (val)
            {
                for(int i = 0; i< sLength; i++)
                {
                    cableStructuresList.Items.Insert(i, this.CableTest.TestedCable.Structures[i].Name);
                }
                cableStructuresList.SelectedIndex = 0;
            }
            else
            {
                cableStructuresList.Items.Insert(0, "Список структур кабеля пуст.");
                cableStructuresList.SelectedIndex = 0;
                cableStructuresList.Enabled = false;
                tabControlTestResult.Visible = false;
            }
            return val;
        }

        private void fillTestData()
        {

            cableTypeLbl.Text = String.Format("Марка кабеля: {0}", CableTest.TestedCable.Name);
            barabanLbl.Text = String.Format("Барабан: {0} № {1}", CableTest.Baraban.Name, CableTest.Baraban.Number);
            operatorLbl.Text = String.Format("Оператор: {0} {1}.{2}.", CableTest.Operator.LastName, CableTest.Operator.FirstName[0], CableTest.Operator.ThirdName[0]);
            testedAtLbl.Text = String.Format("Испытан {0}", ServiceFunctions.MyDateTime(CableTest.TestDate));
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


        /// <summary>
        /// Отрисовка таб в зависимости от выбранного 
        /// </summary>
        private void DrawMeasureParametersTabs(int structure_index)
        {
            if (this.CableTest.TestedCable.Structures.Length == 0 || structure_index < 0) return;
            tabControlTestResult.TabPages.Clear();
            CableStructure curStructure = this.CableTest.TestedCable.Structures[cableStructuresList.SelectedIndex];
            MeasureParameterType[] mParams = ParameterTypesForTabs(cableStructuresList.SelectedIndex);
            TabPage[] pages = new TabPage[mParams.Length];
            //if (curStructure.AffectedElementNumbers.Length>1)test.Text = curStructure.AffectedElementNumbers[1].ToString();
            int i = 0;
            foreach(MeasureParameterType mpt in mParams) 
            {
                pages[i] = new ParameterTypeTabPage(mpt);
                i++;
            }
            tabControlTestResult.TabPages.AddRange(pages);
            tabControlTestResult.Refresh();
        }




        private MeasureParameterType[] ParameterTypesForTabs(int structure_index)
        {
            int count = 0;
            foreach(MeasureParameterType mp in CableTest.TestedCable.Structures[structure_index].MeasuredParameters)
            {
                if (mp.Name != "Prozvon") count++;
            }
            MeasureParameterType[] mpts = new MeasureParameterType[count];
            count = 0;
            foreach (MeasureParameterType mp in CableTest.TestedCable.Structures[structure_index].MeasuredParameters)
            {
                if(mp.Name != "Prozvon")
                {
                    mpts[count] = mp;
                    count++;
                }
            }
            return mpts;
        }

        private void cableStructuresList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedStructureIdx != cableStructuresList.SelectedIndex)
            {
               selectedStructureIdx = cableStructuresList.SelectedIndex;
               DrawMeasureParametersTabs(selectedStructureIdx);
            }
            
        }

        private void GeneratePDFProtocolButton_Click(object sender, EventArgs e)
        {
            PDFProtocol protocol = new PDFProtocol(this.CableTest);
            //Process pr = Process.Start("C:\\CAK\\Client3.exe", this.CableTest.Id);

        }



       

    }
}
