using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CSharpSortingAlgos
{
    public partial class mainWindow : Form
    {
        int[] arr;
        Graphics g;

        BackgroundWorker bgw = null; // running sorting algorithm on a different thread

        bool Paused = false;

        public mainWindow()
        {
            InitializeComponent();

        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (arr == null)
                BtnReset_Click(null, null);

            bgw = new BackgroundWorker
            {
                WorkerSupportsCancellation = true // allowing you to cancel the background workers task
            };

            bgw.DoWork += new DoWorkEventHandler(Bgw_DoWork); // add an event handler that add to a list of things that happen when the event fires
            bgw.RunWorkerAsync(argument: comboBox1.SelectedItem); // run the background worker on whichever sorting algo is selected
        }


        private void BtnPause_Click(object sender, EventArgs e)
        {
            // null check
            if (bgw == null)
            {
                return;
            }


            if (!Paused)
            {
                // pause worker
                bgw.CancelAsync(); 
                Paused = true;
            }
            else
            {
                Thread.Sleep(100);

                // resume worker if background worker isnt busy
                if (bgw.IsBusy)
                    return;

                int NumEntries = panel1.Width;
                int MaxVal = panel1.Height;

                Paused = false;

                for (int i = 0; i < NumEntries; i++)
                {
                    g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Black), i, 0, 1, MaxVal);
                    g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.White), i, MaxVal - arr[i], 1, MaxVal);
                }
                bgw.RunWorkerAsync(argument: comboBox1.SelectedItem);
            }

        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            // dispose of old instances
            g?.Dispose();

            g = panel1.CreateGraphics(); 

            int NumEntries = panel1.Width;
            int maxVal = panel1.Height;

            arr = new int[NumEntries];

            // creating a black panel background
            g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Black), 0, 0, NumEntries, maxVal); 
            Random rand = new Random();

            // initializing each memeber of the array to a random number between 0 and the max number
            for (int i = 0; i < NumEntries; i++)
            {
                arr[i] = rand.Next(0, maxVal);
            }

            // drawing bars to represent the integers
            for (int i = 0; i < NumEntries; i++)
            {
                g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.White), i, maxVal - arr[i], 1, maxVal);
            }
        }

        public void Bgw_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            // explicitly identify the sender as a background worker
            BackgroundWorker bgw = sender as BackgroundWorker;

            // extract the name of the sorting algo to use
            string SortEngineName = (string)e.Argument;

            // figure out the type of the class that will implement the algo
            Type type = Type.GetType("CSharpSortingAlgos." + SortEngineName);

            // get the constructors of that type
            var ctors = type.GetConstructors();

            try
            {
                // creating a sort engine, create an instance of the constructor, and pass the 3 parameters they need
                ISortEngine se = (ISortEngine)ctors[0].Invoke(new object[] { arr, g, panel1.Height});

                while (!se.IsSorted() && (!bgw.CancellationPending))
                {
                    se.NextStep();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
