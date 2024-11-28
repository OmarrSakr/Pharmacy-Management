using System;
using System.IO;
using System.Windows.Forms;

namespace project_fo_3
{
    public partial class Agent : Form
    {
        public Agent()
        {
            InitializeComponent();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            Home back = new Home();
            back.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // إعداد DataGridView
            DataGridView dgvEmp = dataGridView1;
            dgvEmp.ColumnCount = 6;
            dgvEmp.Columns[0].Name = "ID";
            dgvEmp.Columns[1].Name = "Name";
            dgvEmp.Columns[2].Name = "Age";
            dgvEmp.Columns[3].Name = "Salary";
            dgvEmp.Columns[4].Name = "Number";
            dgvEmp.Columns[5].Name = "Password";

            string id = tb1.Text;
            string name = tb2.Text;
            string age = tb3.Text;
            string salary = tb4.Text;
            string number = tb5.Text;
            string password = tb6.Text;

            // إضافة البيانات إلى DataGridView
            object[] data = { id, name, age, salary, number, password };
            dgvEmp.Rows.Add(data);

            // حفظ البيانات إلى الملف
            string filename = @"E:\Project_OOP\FO_organization\Pharmacy-Management\Agent.txt";
            using (FileStream myfile = new FileStream(filename, FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(myfile))
            {
                string record = $"{id}\t{name}\t{age}\t{salary}\t{number}\t{password}\r\n";
                sw.WriteLine(record);
            }

            MessageBox.Show("Your data has been added");

            // مسح الحقول
            tb1.Clear();
            tb2.Clear();
            tb3.Clear();
            tb4.Clear();
            tb5.Clear();
            tb6.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // البحث عن العنصر
            string filename = @"E:\Project_OOP\FO_organization\Pharmacy-Management\Agent.txt";

            using (FileStream myfile = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Read))
            using (StreamReader sr = new StreamReader(myfile))
            {
                string record;
                while ((record = sr.ReadLine()) != null)
                {
                    string[] fields = record.Split('\t');

                    if (tb1.Text == fields[0])
                    {
                        tb2.Text = fields[1];
                        tb3.Text = fields[2];
                        tb4.Text = fields[3];
                        tb5.Text = fields[4];
                        tb6.Text = fields[5];

                        return;
                    }
                }
            }

            MessageBox.Show("Item not found");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // وضع علامة على العنصر
            string filename = @"E:\Project_OOP\FO_organization\Pharmacy-Management\Agent.txt";

            using (FileStream myfile = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            using (StreamReader sr = new StreamReader(myfile))
            using (StreamWriter sw = new StreamWriter(myfile))
            {
                string record;
                int step = 0;

                while ((record = sr.ReadLine()) != null)
                {
                    if (tb1.Text == record.Split('\t')[0])
                    {
                        myfile.Seek(step, SeekOrigin.Begin);
                        sw.Write("*");
                        sw.Flush();
                        return;
                    }
                    step += record.Length + Environment.NewLine.Length;
                }
            }

            MessageBox.Show("Item marked");
        }

        private void tb2_TextChanged(object sender, EventArgs e)
        {
            // يمكنك إضافة أي معالجات إضافية هنا إذا لزم الأمر
        }

        private void Agent_Load(object sender, EventArgs e)
        {

        }
    }
}
