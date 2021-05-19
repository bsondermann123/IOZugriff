using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace IOZugriff
{
    public partial class IOForm : Form
    {
        private IOWorker simulation = new IOWorker();
        
        

        public IOForm()
        {
            InitializeComponent();

        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {
            textBox1.Text = folderBrowserDialog1.SelectedPath;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath;
            textBox1.Show();

           
            backgroundWorker1.DoWork += new DoWorkEventHandler(BackgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.WorkerSupportsCancellation = true;           

            button1.Enabled = false;
            if(!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync(1); 
                textBox2.Text = ("Aufruf");
                button2.Show();
            }
            else
            {
                backgroundWorker1.Dispose();
            }
           



        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.backgroundWorker1.CancelAsync();
            textBox2.Text = "Abbruch";
            button1.Enabled = true;
            button2.Hide();

        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            

            // Extract the argument.
            
            int arg = (int)e.Argument;

            // Start the time-consuming operation.
            e.Result = TimeConsumingOperation(backgroundWorker1, arg);
            

            // If the operation was canceled by the user,
            // set the DoWorkEventArgs.Cancel property to true.
            if (backgroundWorker1.CancellationPending)
            {
                e.Cancel = true;
               
            }           
           
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            if(!e.Cancelled)
            {
                textBox2.Text = e.Result.ToString();
            }
               
            button1.Enabled = true;
            button2.Hide();
        }

        private string TimeConsumingOperation(BackgroundWorker bw, int sleepPeriod)
        {
            string result = "";
            bool exit = false;

            while (!bw.CancellationPending)
            {
                try
                {
                    simulation.DoWork(folderBrowserDialog1.SelectedPath);              // Wait                 
                    result = "Fertig";                    
                    exit = true;
                }
                catch (IOException e)
                {
                    result = "Es ist ein Fehler aufgetreten: " + e.Message;
                    exit = true;

                }

                if (exit)
                {
                   
                    break;
                }

            }
           
            return result;
        }


        

        

    }
}
