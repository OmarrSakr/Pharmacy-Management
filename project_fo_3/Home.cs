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
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Hide();
            Medicine md = new Medicine();
            md.ShowDialog();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Hide();
            
           
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Hide();
            Agent ag = new Agent();
            ag.ShowDialog();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Hide();
            Billing bl=new Billing();
            bl.ShowDialog();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Hide();
            Login back = new Login();
            back.ShowDialog();
        }
    }
}
