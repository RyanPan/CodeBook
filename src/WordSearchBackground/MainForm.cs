using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordSearchBackground
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // This event handler is called when the background thread finishes.
            // This method runs on the main thread.
            if (e.Error != null)
            {
                MessageBox.Show("Error: " + e.Error.Message);
            }
            else if (e.Cancelled)
            {
                MessageBox.Show("Word counting cancelled.");
            }
            else
            {
                MessageBox.Show("Finished counting words");
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // This method runs on the main thread.
            Words.CurrentState state = (Words.CurrentState)e.UserState;
            this.txtLinesCounted.Text = state.LinesCounted.ToString();
            this.txtWrodsCounted.Text = state.WordsCounted.ToString();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // The event handler is where the actual work is done.
            // This method runs on the background thread.

            // Get the BackgroundWorker object that raised this event.
            BackgroundWorker worker = (BackgroundWorker)sender;

            // Get the Words object and call the main method.
            Words words = (Words)e.Argument;
            words.CountWords(worker, e);
        }

        private void StartThread()
        {
            // This method runs on the main thread.
            this.txtWrodsCounted.Text = "0";

            // Initialize the object that the background worker calls.
            Words words = new Words();
            words.CompareString = this.txtCompareString.Text;
            words.SourceFile = this.txtSourceFile.Text;

            // Start the asynchronous operation.
            backgroundWorker1.RunWorkerAsync(words);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartThread();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Cancel the asynchronous operation.
            this.backgroundWorker1.CancelAsync();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
