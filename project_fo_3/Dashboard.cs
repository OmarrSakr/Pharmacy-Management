using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace project_fo_3
{
    public partial class Dashboard : Form
    {
        public Dashboard()
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






        public SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\YourFolder\PHARMACYOB.MDF;Integrated Security=True;");

        //private void TestConnection()
        //{
        //    try
        //    {
        //        con.Open();
        //        MessageBox.Show("Connection successful!");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error: " + ex.Message);
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //}

        //private void LoadData()
        //{
        //    // الاتصال بقاعدة البيانات للحصول على عدد الموظفين
        //    string employeeQuery = "SELECT COUNT(*) FROM EmployeeTab1";
        //    int employeeCount = ExecuteScalarQuery(employeeQuery);
        //    guna2TextBox1.Text = employeeCount.ToString();
        //    guna2TextBox1.BackColor = Color.LightBlue;  // تغيير اللون الخلفي للـ TextBox الأول

        //    // الاتصال بقاعدة البيانات للحصول على عدد الشركات
        //    string companyQuery = "SELECT COUNT(*) FROM CompanyTb1";
        //    int companyCount = ExecuteScalarQuery(companyQuery);
        //    guna2TextBox3.Text = companyCount.ToString();
        //    guna2TextBox3.BackColor = Color.LightGreen;  // تغيير اللون الخلفي للـ TextBox الثاني

        //    // الاتصال بقاعدة البيانات للحصول على إجمالي المبيعات
        //    string sellsQuery = "SELECT SUM(TotalAmount) FROM BillsTb1";
        //    int totalSells = ExecuteScalarQuery(sellsQuery);
        //    guna2TextBox2.Text = totalSells.ToString();
        //    guna2TextBox2.BackColor = Color.LightYellow;  // تغيير اللون الخلفي للـ TextBox الثالث
        //}

        //private void InitializeCheckBoxes()
        //{
        //    // تهيئة الألوان للخلفيات في الـ CheckBox أيضًا إذا رغبت في ذلك
        //    CheckBox checkBoxEmployees = new CheckBox();
        //    checkBoxEmployees.Location = new Point(20, 100);
        //    checkBoxEmployees.Text = "Employees";
        //    checkBoxEmployees.BackColor = Color.AliceBlue; // تغيير خلفية الشيك بوكس
        //    checkBoxEmployees.CheckedChanged += new EventHandler(CheckBoxEmployees_CheckedChanged);

        //    CheckBox checkBoxCompanies = new CheckBox();
        //    checkBoxCompanies.Location = new Point(20, 150);
        //    checkBoxCompanies.Text = "Companies";
        //    checkBoxCompanies.BackColor = Color.Beige; // تغيير خلفية الشيك بوكس
        //    checkBoxCompanies.CheckedChanged += new EventHandler(CheckBoxCompanies_CheckedChanged);

        //    CheckBox checkBoxSells = new CheckBox();
        //    checkBoxSells.Location = new Point(20, 200);
        //    checkBoxSells.Text = "Sells";
        //    checkBoxSells.BackColor = Color.Lavender; // تغيير خلفية الشيك بوكس
        //    checkBoxSells.CheckedChanged += new EventHandler(CheckBoxSells_CheckedChanged);

        //    // إضافة الـ CheckBoxes إلى النموذج
        //    this.Controls.Add(checkBoxEmployees);
        //    this.Controls.Add(checkBoxCompanies);
        //    this.Controls.Add(checkBoxSells);
        //}


        //// دالة لتنفيذ الاستعلام وإرجاع القيمة
        //private int ExecuteScalarQuery(string query)
        //{
        //    int result = 0;
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection("YourConnectionString"))
        //        {
        //            connection.Open();
        //            using (SqlCommand command = new SqlCommand(query, connection))
        //            {
        //                result = (int)command.ExecuteScalar();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error: " + ex.Message);
        //    }
        //    return result;
        //}

       

        //// دالة تحقق من التغيير في CheckBox للموظفين
        //private void CheckBoxEmployees_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (((CheckBox)sender).Checked)
        //    {
        //        MessageBox.Show("Employee data selected.");
        //    }
        //}

        //// دالة تحقق من التغيير في CheckBox للشركات
        //private void CheckBoxCompanies_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (((CheckBox)sender).Checked)
        //    {
        //        MessageBox.Show("Company data selected.");
        //    }
        //}

        //// دالة تحقق من التغيير في CheckBox للمبيعات
        //private void CheckBoxSells_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (((CheckBox)sender).Checked)
        //    {
        //        MessageBox.Show("Sales data selected.");
        //    }
        //}

        //// Employees TextBox
        //private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        //{
        //}

        //// Companies TextBox
        //private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        //{
        //}

        //// Sells TextBox
        //private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        //{
        //}

        //private void Dashboard_Load(object sender, EventArgs e)
        //{
        //    LoadData();
        //    InitializeCheckBoxes();
        //}
    }



  
}

