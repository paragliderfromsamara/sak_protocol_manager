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
using SAKProtocolManager.DBEntities.TestResultEntities;

namespace SAKProtocolManager.MSWordProtocolBuilder
{
    public partial class MSWordProtoco_StatusForm : Form
    {
        CableTest test;
        public MSWordProtoco_StatusForm(CableTest testForProtocol)
        {
            InitializeComponent();
            test = testForProtocol;
            //MSWordProtocolBuilder.ProtocolExist();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
