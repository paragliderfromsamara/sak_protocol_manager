using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAKProtocolManager.MSWordProtocolBuilder
{
    public partial class MSWordProtoco_StatusForm : Form
    {
        public MSWordProtoco_StatusForm()
        {
            InitializeComponent();
        }

        public void SetStatus(string stat_text)
        {
            statusLbl.Text = stat_text;
            progressBar1.PerformStep();
        }

        public void Reset()
        {
            statusLbl.Text = "";
            progressBar1.Value = 0;
        }
    }
}
