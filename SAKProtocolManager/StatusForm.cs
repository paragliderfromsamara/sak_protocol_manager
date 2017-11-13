using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace SAKProtocolManager
{

    public partial class StatusForm : Form
    {
        public delegate void Waiting();
        public Waiting waitingDelegate;
        public Thread waitingThread;
        public StatusForm()
        {
            try
            {
                InitializeComponent();
                waitingDelegate = new Waiting(WaitingMethod);
            }catch(ThreadAbortException)
            {

            }

        }

        private void WaitingStart()
        {
            try
            {
                WaitingThreadClass wtc = new WaitingThreadClass(this);
                wtc.Run();
            }catch(ThreadAbortException)
            {

            }
               


        }

        private void WaitingMethod()
        {
            try
            {
                string point = "• ";
                string txt = String.Empty;
                int pointNumbers = 4;
                this.Refresh();

                while(true)
                {
                    txt = String.Empty;
                    for (int i = 0; i < pointNumbers; i++)
                    {
                        this.label2.Text = txt;
                        this.label2.Update();
                        Thread.Sleep(300);
                        txt += point;
                    }
                }
            }
            catch (ThreadAbortException)
            {
             
            }
        }

        private void StatusForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            { 
               // waitingThread.Abort();
            }catch(ThreadAbortException)
            {

            }
        }

        private void StatusForm_Shown(object sender, EventArgs e)
        {
            try
            {
                waitingThread = new Thread(new ThreadStart(WaitingStart));
                waitingThread.Start();
             }
              catch (ThreadAbortException) { }
        }
    }

    public class WaitingStatus
    {
        public Thread thread;
        public WaitingStatus()
        {
            try
            {
                thread = new Thread(new ThreadStart(ThreadStartFunc));
                thread.Start();
            }
            catch (ThreadAbortException) { }
        } 
        private void ThreadStartFunc()
        {
            try
            {
                StatusForm stat = new StatusForm();
                stat.ShowDialog();
            }
            catch (ThreadAbortException) { }

        }

        public void StopStatus()
        {
            try
            {
                thread.Abort();
            }
            catch (ThreadAbortException) { }
            
        }
    }

    public class WaitingThreadClass
    {
        StatusForm stsForm;
        public WaitingThreadClass(StatusForm sts)
        {
            try
            {
                this.stsForm = sts;
            }
            catch (ThreadAbortException) { }
            
        }

        public void Run()
        {
            try
            {
                stsForm.label2.Invoke(stsForm.waitingDelegate);
            }
            catch (ThreadAbortException) { }
            finally { stsForm.waitingThread.Abort(); }
            
        }
    }

}
