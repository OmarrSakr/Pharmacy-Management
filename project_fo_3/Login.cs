using System;
using System.IO;
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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
       

            private void button1_Click(object sender, EventArgs e)
            {
                string name = username.Text;
                string pass = password.Text;


            //for save data in your file
            string filename = @"E:\Project_OOP\FO_organization\Pharmacy-Management\savedLogin.txt";
            FileStream myfile = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            StreamWriter sw = new StreamWriter(myfile);
            sw.WriteLine($"username: {username.Text}\t password:{password.Text}");
            sw.Flush();
            MessageBox.Show("Your data has been saved");


            if (name == "omar" && pass == "123")
                {
                    MessageBox.Show("hi,poss");
                    Hide();
                    Home basic = new Home();
                    basic.ShowDialog();


                }
                else
                {
                    MessageBox.Show("Error,your Username or Password is incorrect");
                    username.Text = password.Text = null;
                }

            }


        private void button2_Click(object sender, EventArgs e)
        {
            username.Text = password.Text = null;
        }

    

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void password_TextChanged(object sender, EventArgs e)
        {
            password.PasswordChar = '*';
            password.Font = new Font("Microsoft Sans Serif", 14);
        }

        private void username_TextChanged(object sender, EventArgs e)
        {
            username.Font = new Font("Microsoft Sans Serif", 12);
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }

}
