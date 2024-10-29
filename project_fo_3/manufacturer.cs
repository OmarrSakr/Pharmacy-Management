using System;
using System.IO;
using System.Windows.Forms;

namespace project_fo_3
{
    public partial class Manufacturer : Form
    {
        public Manufacturer()
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
            DataGridView dgvEmp = dataGridView1;
            dgvEmp.ColumnCount = 4;
            dgvEmp.Columns[0].Name = "ID";
            dgvEmp.Columns[1].Name = "Name";
            dgvEmp.Columns[2].Name = "Phone";
            dgvEmp.Columns[3].Name = "Address";

            string id = tb1.Text;
            string name = tb2.Text;
            string phone = tb3.Text;
            string address = tb4.Text;

            object[] data = { id, name, phone, address };
            dgvEmp.Rows.Add(data);

            // Save data to file
            string filename = @"D:\Project_OOP\FO_organization\project_fo_3\project_fo_3\Manufacturer.txt";
            using (FileStream myfile = new FileStream(filename, FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(myfile))
            {
                string record = $"{id}\t{name}\t{phone}\t{address}\r\n";
                sw.WriteLine(record);
            }

            MessageBox.Show("Your data has been added");

            tb1.Clear();
            tb2.Clear();
            tb3.Clear();
            tb4.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string filename = @"D:\Project_OOP\FO_organization\project_fo_3\project_fo_3\Manufacturer.txt";
            using (FileStream myfile = new FileStream(filename, FileMode.Open, FileAccess.Read))
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
                        return;
                    }
                }
            }
            MessageBox.Show("Item not found");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tb2.Clear();
            tb3.Clear();
            tb4.Clear();

            string filename = @"D:\Project_OOP\FO_organization\project_fo_3\project_fo_3\Manufacturer.txt";
            using (FileStream myfile = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
            using (StreamReader sr = new StreamReader(myfile))
            using (StreamWriter sw = new StreamWriter(myfile))
            {
                string record;
                int step = 0;
                while ((record = sr.ReadLine()) != null)
                {
                    string[] fields = record.Split('\t');
                    if (tb1.Text == fields[0])
                    {
                        myfile.Seek(step, SeekOrigin.Begin);
                        sw.Write("*" + record); // Add a mark to the existing line
                        sw.Flush();
                        return;
                    }
                    step = (int)myfile.Position; // Update step for the next iteration
                }
            }
            MessageBox.Show("Item not found for marking");
        }

        private void tb2_TextChanged(object sender, EventArgs e) { }
    }
}
