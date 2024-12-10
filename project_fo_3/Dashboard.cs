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





        // الاتصال بقاعدة البيانات
        public SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\PROJECT_OOP\FO_ORGANIZATION\PHARMACY-MANAGEMENT\PROJECT_FO_3\PHARMACYOB.MDF;Integrated Security=True;");



        private void Dashboard_Load(object sender, EventArgs e)
        {
            UpdateCompanyCountLabel();
            UpdateEmployeeCountLabel();
            UpdateSalesTotalLabel();
            LoadCustomerBillData();
            CustomizeCustomerGridView();
            LoadCustomerData();
        }
        private void CustomizeCustomerGridView()
        {
            // لون خلفية الـ Header
            CustomerGV.EnableHeadersVisualStyles = false;
            CustomerGV.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSlateBlue;
            CustomerGV.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            CustomerGV.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);

            // لون الصفوف العادية
            CustomerGV.DefaultCellStyle.BackColor = Color.White;
            CustomerGV.DefaultCellStyle.ForeColor = Color.Black;

            // لون التحديد للصفوف والأعمدة
            CustomerGV.DefaultCellStyle.SelectionBackColor = Color.DarkGreen; // خلفية الصف المحدد
            CustomerGV.DefaultCellStyle.SelectionForeColor = Color.White; // لون النص في الصف المحدد
        }

        private void UpdateCompanyCountLabel()
        {
            try
            {
                // فتح الاتصال بقاعدة البيانات
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                // استعلام للحصول على عدد الشركات
                string query = "SELECT COUNT(*) FROM CompanyTb1";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // تنفيذ الاستعلام واسترداد عدد الشركات
                    int companyCount = Convert.ToInt32(cmd.ExecuteScalar());

                    // تحديث النص في Label وإزالة أي نص سابق
                    NumberOfEmp.Text = companyCount.ToString();
                }
            }
            catch (Exception ex)
            {
                // التعامل مع الأخطاء
                MessageBox.Show($"Error fetching company count: {ex.Message}");
            }
            finally
            {
                // التأكد من إغلاق الاتصال إذا كان مفتوحًا
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        private void UpdateEmployeeCountLabel()
        {
            try
            {
                // فتح الاتصال بقاعدة البيانات
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                // استعلام للحصول على عدد الموظفين
                string query = "SELECT COUNT(*) FROM EmployeeTab1";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // تنفيذ الاستعلام واسترداد العدد
                    int employeeCount = Convert.ToInt32(cmd.ExecuteScalar());

                    // تحديث النص في Label
                    EmployeeNumber.Text = employeeCount.ToString();
                }
            }
            catch (Exception ex)
            {
                // التعامل مع الأخطاء
                MessageBox.Show($"Error fetching employee count: {ex.Message}");
            }
            finally
            {
                // التأكد من إغلاق الاتصال إذا كان مفتوحًا
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        // دالة لتحديث الإجمالي الإجمالي للمبيعات
        private void UpdateSalesTotalLabel()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                string query = "SELECT SUM(CAST(TotalSpent AS INT)) FROM CustomersBillsTb";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    object result = cmd.ExecuteScalar();
                    int totalSales = result != DBNull.Value ? Convert.ToInt32(result) : 0;

                    // تحديث الإجمالي في الـ Label
                    SellsNumber.Text = $"{totalSales} EGY";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating total sales: {ex.Message}");
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        // دالة لتحميل بيانات فواتير العملاء
        private void LoadCustomerBillData()
        {
            try
            {
                // فتح الاتصال
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                // استعلام لجلب بيانات العملاء والفواتير مع تاريخ الإصدار واسم الموظف ورقم الهاتف والعنوان
                string query = "SELECT CustomerId, CustomerName, CustomerAddress, PhoneNumber, TotalSpent, InvoiceDate, EmployeeName FROM CustomersBillsTb";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // عرض البيانات في الـ DataGridView
                CustomerGV.DataSource = dataTable;

                // تخصيص عرض الأعمدة يدويًا بعد ملء الـ DataGridView
                CustomerGV.Columns["CustomerId"].Width = 100;
                CustomerGV.Columns["CustomerName"].Width = 150;
                CustomerGV.Columns["CustomerAddress"].Width = 150;  // تخصيص عرض عمود العنوان
                CustomerGV.Columns["PhoneNumber"].Width = 110;      // تخصيص عرض عمود رقم الهاتف
                CustomerGV.Columns["TotalSpent"].Width = 100;
                CustomerGV.Columns["InvoiceDate"].Width = 130;
                CustomerGV.Columns["EmployeeName"].Width = 150;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customer bills: {ex.Message}");
            }
            finally
            {
                // غلق الاتصال
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        // دالة لتحميل البيانات إلى الجدول
        private void LoadCustomerData()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                // استعلام لتحميل البيانات من قاعدة البيانات (يشمل CustomerAddress)
                string query = "SELECT CustomerId, CustomerName, CustomerAddress, PhoneNumber, TotalSpent, InvoiceDate, EmployeeName FROM CustomersBillsTb";
                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // تعيين البيانات إلى الـ DataGridView
                CustomerGV.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customer data: {ex.Message}");
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }




        private void CustomerGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    // استخراج معلومات العميل من الجدول
                    string customerId = CustomerGV.Rows[e.RowIndex].Cells["CustomerId"].Value.ToString();
                    string customerName = CustomerGV.Rows[e.RowIndex].Cells["CustomerName"].Value.ToString();
                    string customerAddress = CustomerGV.Rows[e.RowIndex].Cells["CustomerAddress"].Value.ToString(); // إضافة العنوان

                    // تأكيد الحذف
                    DialogResult result = MessageBox.Show(
                        $"Are you sure you want to delete customer '{customerName}' with ID '{customerId}' and address '{customerAddress}'?",
                        "Delete Confirmation",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    // إذا وافق المستخدم على الحذف
                    if (result == DialogResult.Yes)
                    {
                        // حذف العميل من قاعدة البيانات
                        DeleteCustomerFromDatabase(customerId);

                        // تحديث المجموع الإجمالي للمبيعات بعد الحذف
                        UpdateSalesTotalLabel();

                        // تحديث الجدول بعد الحذف
                        LoadCustomerData();

                        // عرض رسالة تأكيد
                        MessageBox.Show("Customer record deleted successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting customer: {ex.Message}");
            }
        }


        // دالة لحذف العميل من قاعدة البيانات
      
        private void DeleteCustomerFromDatabase(string customerId)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                // استعلام لحذف العميل
                string query = "DELETE FROM CustomersBillsTb WHERE CustomerId = @CustomerId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", customerId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting customer from database: {ex.Message}");
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }


        // دالة لتحميل البيانات إلى الجدول
       


        private void CustomerGV_CellContentClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    // استخراج معلومات العميل من الجدول
                    string customerId = CustomerGV.Rows[e.RowIndex].Cells["CustomerId"].Value.ToString();
                    string customerName = CustomerGV.Rows[e.RowIndex].Cells["CustomerName"].Value.ToString();
                    string customerAddress = CustomerGV.Rows[e.RowIndex].Cells["CustomerAddress"].Value.ToString(); // إضافة العنوان

                    // تأكيد الحذف
                    DialogResult result = MessageBox.Show(
                        $"Are you sure you want to delete customer '{customerName}' with ID '{customerId}' and address '{customerAddress}'?",
                        "Delete Confirmation",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    // إذا وافق المستخدم على الحذف
                    if (result == DialogResult.Yes)
                    {
                        // حذف العميل من قاعدة البيانات
                        DeleteCustomerFromDatabase(customerId);

                        // تحديث المجموع الإجمالي للمبيعات بعد الحذف
                        UpdateSalesTotalLabel();

                        // تحديث الجدول بعد الحذف
                        LoadCustomerData();

                        // عرض رسالة تأكيد
                        MessageBox.Show("Customer record deleted successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting customer: {ex.Message}");
            }
        }


    }

}

