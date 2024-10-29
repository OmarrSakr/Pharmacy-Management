using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project_fo_3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void InitializeTimer()
        {
            timer1.Interval = 100;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Enabled = true;
        }
       
        private void timer1_Tick(object sender, EventArgs e)
        {
            gunaCircleProgressBar1.Increment(1);
            if (gunaCircleProgressBar1.Value == 100)
            {
                Hide();
                Login Login = new Login();
                Login.Show();
                timer1.Enabled = false;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

    }
}
