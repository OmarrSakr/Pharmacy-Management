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
       

            private void button1_Click_1(object sender, EventArgs e)
            {
                string name = username.Text;
                string pass = password.Text;

            //for save data in your file
            string filename = @"E:\Project_OOP\FO_organization\Pharmacy-Management\savedLogin.txt";

            // استخدام using لضمان إغلاق الموارد تلقائيًا
            using (FileStream myfile = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(myfile))
                {
                    sw.WriteLine($"username: {username.Text}\t password:{password.Text}");
                }
            }

            //MessageBox.Show("Your data has been saved");


            if (name == "admin" && pass == "123")
                {
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


        private void button2_Click_1(object sender, EventArgs e)
        {
            username.Text = password.Text = null;
        }

    

        private void button3_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void password_TextChanged(object sender, EventArgs e)
        {
            password.PasswordChar = '●'; 
            password.Font = new Font("Segoe UI Semibold", 12);
        }


        private void username_TextChanged(object sender, EventArgs e)
        {
            username.Font = new Font("Microsoft Sans Serif", 12);
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }


     
    }

}
