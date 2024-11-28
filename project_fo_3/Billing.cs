using System;
using System.IO;
using System.Windows.Forms;

namespace project_fo_3
{
    public partial class Billing : Form
    {
        public Billing()
        {
            InitializeComponent();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            Home back = new Home();
            back.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // إعداد DataGridView
            DataGridView dgvEmp = new DataGridView();
            dgvEmp.ColumnCount = 1;
            dgvEmp.Columns[0].Name = "Quantity";

            string quantity = tb1.Text;

            // إضافة البيانات إلى DataGridView
            object[] data = { quantity };
            dgvEmp.Rows.Add(data);

            // حفظ البيانات إلى الملف
            string filename = @"E:\Project_OOP\FO_organization\Pharmacy-Management\Billing.txt";
            using (FileStream myfile = new FileStream(filename, FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(myfile))
            {
                string record = $"{quantity}\r\n";
                sw.WriteLine(record);
            }

            MessageBox.Show("Your data has been added");

            tb1.Clear(); // استخدام Clear بدلاً من تعيين النص إلى null
        }

        private void tb1_TextChanged(object sender, EventArgs e)
        {
            // يمكنك إضافة أي معالجات إضافية هنا إذا لزم الأمر
        }

        private void Billing_Load(object sender, EventArgs e)
        {

        }
    }
}
