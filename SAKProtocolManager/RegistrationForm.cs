using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAKProtocolManager
{
    public partial class RegistrationForm : Form
    {
        private DialogResult CancelDialogResult;
        private string enteredValue = String.Empty;
        private string key = Properties.Settings.Default.ExpectedKey;
        public RegistrationForm()
        {
            InitializeComponent();
            DateTime lastOpenDate = Properties.Settings.Default.FreePeriodEndDate;
            DateTime dt = DateTime.Now;
            this.freePeriodLbl.Text = (lastOpenDate > dt) ? String.Format("Ознакомительный период истекает {0}", lastOpenDate.ToShortDateString()) : "Ознакомительный период окончен,\nвведите ключ продукта";
            submitButton.Enabled = false;
            CancelDialogResult = (lastOpenDate > dt) ? DialogResult.Cancel : DialogResult.Abort;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = CancelDialogResult;
        }


        private void regKeyField_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void regKeyField_TextChanged(object sender, EventArgs e)
        {
            string v = regKeyField.Text.Replace("-", "");
            if (v.Length > key.Length)
            {
                regKeyField.Text = enteredValue.ToUpper();
            }
            else
            {
                enteredValue = v;
                string newString = "";
                int t = 5;
                int j = 0;
                for (int i = 0; i < enteredValue.Length; i++)
                {
                    if (j == t)
                    {
                        newString += "-";
                        j = 0;
                    }
                    newString += enteredValue[i];
                    j++;
                }
                regKeyField.Text = newString.ToUpper();
                regKeyField.SelectionStart = newString.Length;
                submitButton.Enabled = enteredValue.Length == key.Length;
            }

        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            if (enteredValue == key)
            {
                IniFile ini = new IniFile(Properties.Settings.Default.IniSettingsFileName);
                ini.Write("ProductKey", enteredValue);
                this.DialogResult = DialogResult.OK;
            }else
            {
                MessageBox.Show("Ключ продукта введён неверно", "Ошибка при активации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
