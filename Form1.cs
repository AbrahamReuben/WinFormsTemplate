using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsTemplate
{
    public partial class Form1 : Form
    {
        // Delegate to Update TextBox1
        private delegate void UpdateTextBox1Handler(string msg);
        // Delegate to Update TextBox2
        private delegate void UpdateTextBox2Handler(string msg);

        public Form1()
        {
            InitializeComponent();

            // File to be placed in the same folder as the executable
            string filename = "test.txt";

            Thread MD5HashingThread = new Thread(() => ComputeMD5Hash(filename));
            MD5HashingThread.Start();
        }

        public void UpdateTextBox1(string text1)
        {
            if (InvokeRequired)
                BeginInvoke(new UpdateTextBox1Handler(UpdateTextBox1), new object[] { text1 });
            else
                textBox1.Text = text1;
        }

        public void UpdateTextBox2(string text2)
        {
            if (InvokeRequired)
                BeginInvoke(new UpdateTextBox2Handler(UpdateTextBox2), new object[] { text2 });
            else
                textBox2.Text = text2;
        }

        private void ComputeMD5Hash(string filename)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    UpdateTextBox1(BitConverter.ToString(hash).Replace("-", ""));
                    //return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
            stopWatch.Stop();
            // Ticks/Frequency gives the seconds
            string elapsedtime = ((double)stopWatch.ElapsedTicks / Stopwatch.Frequency).ToString() + " sec";
            UpdateTextBox2(elapsedtime);

            // To gracefully end the thread
            return;
        }
    }
}
