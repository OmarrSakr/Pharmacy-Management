using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
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
            // مسح الحقول بعد إضافة البيانات
            tb1.Clear();
            tb2.Clear();
            tb3.Clear();
            tb4.Clear();
            Hide();
            Home back = new Home();
            back.ShowDialog();
        }


        // الاتصال بقاعدة البيانات
        public SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\PROJECT_OOP\FO_ORGANIZATION\PHARMACY-MANAGEMENT\PROJECT_FO_3\PHARMACYOB.MDF;Integrated Security=True;");
        // Add Input
        // إضافة اسم شركة جديد
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // التأكد من أن الحقول الأساسية ليست فارغة أو تحتوي على مسافات فارغة
                if (string.IsNullOrWhiteSpace(tb1.Text) ||  // Company Id
                    string.IsNullOrWhiteSpace(tb2.Text) ||  // Company Name
                    string.IsNullOrWhiteSpace(tb3.Text) ||  // Phone Number
                    string.IsNullOrWhiteSpace(tb4.Text))    // Address
                {
                    MessageBox.Show("Please fill all fields.");
                    return;
                }

                // فتح الاتصال بقاعدة البيانات
                con.Open();

                // استعلام الإدخال (Insert)
                string query = "INSERT INTO CompanyTb1 (CompId, CompName, CompPhone, CompAddress) " +
                               "VALUES (@CompId, @CompName, @CompPhone, @CompAddress)";

                // إعداد الأمر مع المعلمات
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // تغيير البيانات المدخلة إلى NVARCHAR فقط للحقلين الذين يدعمان اللغة العربية
                    cmd.Parameters.Add("@CompId", SqlDbType.NVarChar).Value = tb1.Text.Trim();   // يجب أن يدعم الأرقام والرموز
                    cmd.Parameters.Add("@CompName", SqlDbType.NVarChar).Value = tb2.Text.Trim(); // يدعم النصوص العربية
                    cmd.Parameters.Add("@CompPhone", SqlDbType.NVarChar).Value = tb3.Text.Trim(); // يدعم الأرقام
                    cmd.Parameters.Add("@CompAddress", SqlDbType.NVarChar).Value = tb4.Text.Trim(); // يدعم النصوص العربية

                    // تنفيذ الأمر
                    cmd.ExecuteNonQuery();
                }

                // عرض رسالة نجاح
                MessageBox.Show("Company Successfully Added");

                // تحديث البيانات في DataGridView
                PopulateGrid(CompanyGV);  // تأكد من أن PopulateGrid تعمل بشكل صحيح

                // مسح الحقول بعد إضافة البيانات
                tb1.Clear();
                tb2.Clear();
                tb3.Clear();
                tb4.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                // التأكد من إغلاق الاتصال
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
        }
        // تعديل بيانات الشركة
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tb1.Text))
                {
                    MessageBox.Show("Please enter the Company Id to update.");
                    return;
                }

                con.Open();

                // التحقق من وجود الشركة
                string checkQuery = "SELECT COUNT(*) FROM CompanyTb1 WHERE CompId = @CompId";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                {
                    checkCmd.Parameters.AddWithValue("@CompId", tb1.Text);
                    int count = (int)checkCmd.ExecuteScalar();
                    if (count == 0)
                    {
                        MessageBox.Show("Company not found.");
                        return;
                    }
                }

                // استعلام التحديث (Update)
                string query = "UPDATE CompanyTb1 SET " +
                               "CompName = @CompName, CompPhone = @CompPhone, CompAddress = @CompAddress " +
                               "WHERE CompId = @CompId";

                // إعداد الأمر مع المعلمات
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CompId", tb1.Text);
                    cmd.Parameters.AddWithValue("@CompName", tb2.Text);
                    cmd.Parameters.AddWithValue("@CompPhone", tb3.Text);
                    cmd.Parameters.AddWithValue("@CompAddress", tb4.Text);

                    cmd.ExecuteNonQuery();
                }

                // عرض رسالة نجاح
                MessageBox.Show("Company Successfully Updated");

                // تحديث البيانات في DataGridView
                PopulateGrid(CompanyGV);

                // مسح الحقول بعد التحديث
                tb1.Clear();
                tb2.Clear();
                tb3.Clear();
                tb4.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                con.Close();
            }
        }

        // حذف بيانات الشركة
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tb1.Text))
                {
                    MessageBox.Show("Please enter the Company Id to delete.");
                    return;
                }

                // نافذة تأكيد قبل الحذف
                DialogResult result = MessageBox.Show("Are you sure you want to delete this company?",
                                                      "Confirm Deletion",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // فتح الاتصال بقاعدة البيانات
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    // استعلام الحذف
                    string query = "DELETE FROM CompanyTb1 WHERE CompId = @CompId";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@CompId", tb1.Text.Trim());

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // تفريغ الحقول بعد نجاح الحذف
                            tb1.Clear();
                            tb2.Clear();
                            tb3.Clear();
                            tb4.Clear();

                            MessageBox.Show("Company Successfully Deleted");
                        }
                        else
                        {
                            MessageBox.Show("Company not found.");
                        }
                    }

                    // تحديث DataGridView بعد الحذف
                    PopulateGrid(CompanyGV);
                }
                else
                {
                    // إذا اختار "No"، إلغاء العملية
                    MessageBox.Show("Deletion Cancelled.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }


        public void PopulateGrid(DataGridView CompanyGV)
        {
            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();

                string query = "SELECT CompId, CompName, CompPhone, CompAddress FROM CompanyTb1";
                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                CompanyGV.DataSource = dt;
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
        }

        private void Company_Load(object sender, EventArgs e)
        {
            try
            {
                // تمرير عنصر التحكم CompanyGV عند استدعاء الدالة
                PopulateGrid(CompanyGV);
            }
            catch (Exception ex)
            {
                // عرض رسالة خطأ إذا فشل التحميل
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        private void CompanyGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // التأكد من أن الصف الذي تم الضغط عليه هو صف غير فارغ
                if (e.RowIndex >= 0)
                {
                    // الحصول على البيانات من السطر المحدد
                    DataGridViewRow row = CompanyGV.Rows[e.RowIndex];

                    // التأكد من أن السطر يحتوي على قيم قبل ملء الحقول
                    if (row.Cells["CompId"].Value != DBNull.Value && row.Cells["CompName"].Value != DBNull.Value &&
                        row.Cells["CompPhone"].Value != DBNull.Value && row.Cells["CompAddress"].Value != DBNull.Value)
                    {
                        // ملء الحقول بالقيم من السطر المحدد
                        tb1.Text = row.Cells["CompId"].Value.ToString();      // Company ID
                        tb2.Text = row.Cells["CompName"].Value.ToString();    // Company Name
                        tb3.Text = row.Cells["CompPhone"].Value.ToString();   // Phone Number
                        tb4.Text = row.Cells["CompAddress"].Value.ToString(); // Company Address
                    }
                    else
                    {
                        MessageBox.Show("Some fields are empty in the selected row.");
                    }
                }
            }
            catch (Exception ex)
            {
                // عرض رسالة خطأ إذا حدث شيء غير متوقع
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void CompanyGV_CellContentClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                // التأكد من أن الصف الذي تم الضغط عليه هو صف غير فارغ
                if (e.RowIndex >= 0)
                {
                    // الحصول على البيانات من السطر المحدد
                    DataGridViewRow row = CompanyGV.Rows[e.RowIndex];

                    // التأكد من أن السطر يحتوي على قيم قبل ملء الحقول
                    if (row.Cells["CompId"].Value != DBNull.Value && row.Cells["CompName"].Value != DBNull.Value &&
                        row.Cells["CompPhone"].Value != DBNull.Value && row.Cells["CompAddress"].Value != DBNull.Value)
                    {
                        // ملء الحقول بالقيم من السطر المحدد
                        tb1.Text = row.Cells["CompId"].Value.ToString();      // Company ID
                        tb2.Text = row.Cells["CompName"].Value.ToString();    // Company Name
                        tb3.Text = row.Cells["CompPhone"].Value.ToString();   // Phone Number
                        tb4.Text = row.Cells["CompAddress"].Value.ToString(); // Company Address
                    }
                    else
                    {
                        MessageBox.Show("Some fields are empty in the selected row.");
                    }
                }
            }
            catch (Exception ex)
            {
                // عرض رسالة خطأ إذا حدث شيء غير متوقع
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            // مسح الحقول بعد إضافة البيانات
            tb1.Clear();
            tb2.Clear();
            tb3.Clear();
            tb4.Clear();
        }

       
    }
    
}
