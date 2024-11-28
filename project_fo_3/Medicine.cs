using System;
using System.IO;
using System.Windows.Forms;

namespace project_fo_3
{
    public partial class Medicine : Form
    {
        public Medicine()
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

            dgvEmp.Columns[0].Name = "Name";
            dgvEmp.Columns[1].Name = "Buying";
            dgvEmp.Columns[2].Name = "Selling";
            dgvEmp.Columns[3].Name = "Quantity";

            string Name = tb1.Text;
            string Buying = tb2.Text;
            string Selling = tb3.Text;
            string Quantity = tb4.Text;

            object[] data = { Name, Buying, Selling, Quantity };
            dgvEmp.Rows.Add(data);

            // Save data to file
            string filename = @"E:\Project_OOP\FO_organization\Pharmacy-Management\Medicine.txt";
            using (FileStream myfile = new FileStream(filename, FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(myfile))
            {
                string record = $"{Name}\t{Buying}\t{Selling}\t{Quantity}\r\n";
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
            string filename = @"E:\Project_OOP\FO_organization\Pharmacy-Management\Medicine.txt";
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

            string filename = @"E:\Project_OOP\FO_organization\Pharmacy-Management\Medicine.txt";
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
                    step = (int)myfile.Position; // Update the step for next iteration
                }
            }
            MessageBox.Show("Item not found for marking");
        }

        private void gunaDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle cell click if needed
        }

        private void tb1_TextChanged(object sender, EventArgs e) { }

        private void tb2_TextChanged(object sender, EventArgs e) { }

        private void Medicine_Load(object sender, EventArgs e)
        {

        }
    }
}
