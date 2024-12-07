using System;
using System.IO;
using System.Data.SqlClient;
using System.Data;
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

        private void button2_Click_1(object sender, EventArgs e)
        {
            // مسح الحقول بعد إضافة البيانات
            tb1.Clear();
            tb2.Clear();
            tb3.Clear();
            tb4.Clear();
            tb5.Clear();
            tb6.Clear();

            Hide(); 
            Home back = new Home();
            back.ShowDialog();
;  
        }


        // الاتصال بقاعدة البيانات
        public SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\PROJECT_OOP\FO_ORGANIZATION\PHARMACY-MANAGEMENT\PROJECT_FO_3\PHARMACYOB.MDF;Integrated Security=True;");

        // زر لإضافة موظف جديد
        private void button4_Click_1(object sender, EventArgs e)
        {
            try
            {
                // التأكد من أن الحقول ليست فارغة
                if (string.IsNullOrEmpty(tb1.Text) || string.IsNullOrEmpty(tb2.Text) ||
                    string.IsNullOrEmpty(tb3.Text) || string.IsNullOrEmpty(tb4.Text) ||
                    string.IsNullOrEmpty(tb5.Text) || string.IsNullOrEmpty(tb6.Text))
                {
                    MessageBox.Show("Please fill all fields.");
                    return;
                }

                // التحقق من القيم المدخلة
                int empAge, empSal;
                if (!int.TryParse(tb3.Text, out empAge) || !int.TryParse(tb4.Text, out empSal))
                {
                    MessageBox.Show("Please enter valid numeric values for Age and Salary.");
                    return;
                }

                // فتح الاتصال بقاعدة البيانات
                con.Open();

                // استعلام الإدخال (Insert) مع تضمين EmpId و EmpName كـ NVARCHAR
                string query = "INSERT INTO EmployeeTab1 (EmpId, EmpName, EmpSal, EmpAge, EmpPhone, EmpPassword) " +
                               "VALUES (@EmpId, @EmpName, @EmpSal, @EmpAge, @EmpPhone, @EmpPassword)";

                // إعداد الأمر مع المعلمات
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // التأكد من إدخال قيمة EmpId يدويًا (يجب أن تكون فريدة)
                    cmd.Parameters.AddWithValue("@EmpId", tb1.Text.Trim());  // قم بإدخال القيمة الخاصة بـ EmpId هنا
                    cmd.Parameters.AddWithValue("@EmpName", tb2.Text.Trim()); // اسم الموظف يمكن أن يحتوي على حروف عربية
                    cmd.Parameters.AddWithValue("@EmpSal", empSal);
                    cmd.Parameters.AddWithValue("@EmpAge", empAge);
                    cmd.Parameters.AddWithValue("@EmpPhone", tb5.Text.Trim());
                    cmd.Parameters.AddWithValue("@EmpPassword", tb6.Text.Trim());

                    // تنفيذ الأمر
                    cmd.ExecuteNonQuery();
                }

                // عرض رسالة نجاح
                MessageBox.Show("Employee Successfully Added");

                // تحديث البيانات في DataGridView
                PopulateGrid(EmployeeGV); // استدعاء دالة تحديث الـ DataGridView

                // مسح الحقول بعد إضافة البيانات
                tb1.Clear();
                tb2.Clear();
                tb3.Clear();
                tb4.Clear();
                tb5.Clear();
                tb6.Clear();
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



        // زر لتحديث بيانات موظف
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // التأكد من أن الحقول ليست فارغة
                if (string.IsNullOrEmpty(tb1.Text) || string.IsNullOrEmpty(tb2.Text) ||
                    string.IsNullOrEmpty(tb3.Text) || string.IsNullOrEmpty(tb4.Text) ||
                    string.IsNullOrEmpty(tb5.Text) || string.IsNullOrEmpty(tb6.Text))
                {
                    MessageBox.Show("Please fill all fields.");
                    return;
                }

                // التحقق من القيم المدخلة
                int empAge, empSal;
                if (!int.TryParse(tb3.Text, out empAge) || !int.TryParse(tb4.Text, out empSal))
                {
                    MessageBox.Show("Please enter valid numeric values for Age and Salary.");
                    return;
                }

                con.Open();

                // استعلام التحديث (Update)
                string query = "UPDATE EmployeeTab1 SET EmpName = @EmpName, EmpSal = @EmpSal, EmpAge = @EmpAge, EmpPhone = @EmpPhone, EmpPassword = @EmpPassword " +
                               "WHERE EmpId = @EmpId";

                // إعداد الأمر مع المعلمات
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@EmpId", tb1.Text.Trim());  // التعامل مع EmpId كـ VARCHAR
                    cmd.Parameters.AddWithValue("@EmpName", tb2.Text.Trim());
                    cmd.Parameters.AddWithValue("@EmpSal", empSal);
                    cmd.Parameters.AddWithValue("@EmpAge", empAge);
                    cmd.Parameters.AddWithValue("@EmpPhone", tb5.Text.Trim());
                    cmd.Parameters.AddWithValue("@EmpPassword", tb6.Text.Trim());

                    // تنفيذ الأمر
                    cmd.ExecuteNonQuery();
                }

                // عرض رسالة نجاح
                MessageBox.Show("Employee Successfully Updated");

                PopulateGrid(EmployeeGV); // تحديث DataGridView بعد التحديث

                // مسح الحقول بعد التحديث
                tb1.Clear();
                tb2.Clear();
                tb3.Clear();
                tb4.Clear();
                tb5.Clear();
                tb6.Clear();
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


        // زر لحذف موظف
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tb1.Text))
                {
                    MessageBox.Show("Please enter Employee ID to delete.");
                    return;
                }

                con.Open();

                // استعلام الحذف (Delete)
                string query = "DELETE FROM EmployeeTab1 WHERE EmpId = @EmpId";

                // إعداد الأمر مع المعلمات
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@EmpId", tb1.Text.Trim());  // التعامل مع EmpId كـ VARCHAR

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Employee Successfully Deleted");
                    }
                    else
                    {
                        MessageBox.Show("Employee not found.");
                    }
                }

                PopulateGrid(EmployeeGV); // تحديث DataGridView بعد الحذف
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

        // ملء DataGridView ببيانات الموظفين
        public void PopulateGrid(DataGridView EmployeeGV)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                // جلب البيانات من الجدول
                string query = "SELECT EmpId, EmpName, EmpSal, EmpAge, EmpPhone, EmpPassword FROM EmployeeTab1";
                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // إنشاء نسخة جديدة من الجدول لإظهار كلمة المرور كنجوم
                foreach (DataRow row in dt.Rows)
                {
                    if (row["EmpPassword"] != null)
                        row["EmpPassword"] = new string('*', row["EmpPassword"].ToString().Length);
                }

                EmployeeGV.DataSource = dt;
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





        private void Ageny_Load(object sender, EventArgs e)
        {
            try
            {
                    // تمرير عنصر التحكم MedicineGV عند استدعاء الدالة
                    PopulateGrid(EmployeeGV);
                
            }
            catch (Exception ex)
            {
                // عرض رسالة خطأ إذا فشل التحميل
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        private void EmployeeGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // التأكد من أن الصف الذي تم الضغط عليه هو صف صالح
                if (e.RowIndex >= 0)
                {
                    // الحصول على البيانات من السطر المحدد
                    DataGridViewRow row = EmployeeGV.Rows[e.RowIndex];

                    // التأكد من أن السطر يحتوي على قيم قبل ملء الحقول
                    if (row.Cells["EmpId"].Value != null && row.Cells["EmpName"].Value != null &&
                        row.Cells["EmpSal"].Value != null && row.Cells["EmpAge"].Value != null &&
                        row.Cells["EmpPhone"].Value != null && row.Cells["EmpPassword"].Value != null)
                    {
                        // ملء الحقول بالقيم من السطر المحدد
                        tb1.Text = row.Cells["EmpId"].Value.ToString();      // Employee Id
                        tb2.Text = row.Cells["EmpName"].Value.ToString();    // Employee Name
                        tb3.Text = row.Cells["EmpAge"].Value.ToString();     // Employee Age
                        tb4.Text = row.Cells["EmpSal"].Value.ToString();     // Employee Salary
                        tb5.Text = row.Cells["EmpPhone"].Value.ToString();   // Phone Number

                        // إخفاء كلمة المرور كنجوم في الحقل (tb6)
                        tb6.Text = new string('*', row.Cells["EmpPassword"].Value.ToString().Length); // Hide password with stars
                    }
                }
            }
            catch (Exception ex)
            {
                // عرض رسالة خطأ إذا حدث شيء غير متوقع
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        private string GetOriginalPassword(string empId)
        {
            string password = string.Empty;
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                // جلب كلمة المرور الأصلية بناءً على EmpId
                string query = "SELECT EmpPassword FROM EmployeeTab1 WHERE EmpId = @EmpId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@EmpId", empId);

                object result = cmd.ExecuteScalar();
                if (result != null)
                    password = result.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching password: {ex.Message}");
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            return password;
        }


        private void button5_Click_1(object sender, EventArgs e)
        { // مسح الحقول بعد إضافة البيانات
            tb1.Clear();
            tb2.Clear();
            tb3.Clear();
            tb4.Clear();
            tb5.Clear();
            tb6.Clear();
        }

        private void EmployeeGV_CellContentClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                // التأكد من أن الصف الذي تم الضغط عليه هو صف صالح
                if (e.RowIndex >= 0)
                {
                    // الحصول على البيانات من السطر المحدد
                    DataGridViewRow row = EmployeeGV.Rows[e.RowIndex];

                    // التأكد من أن السطر يحتوي على قيم قبل ملء الحقول
                    if (row.Cells["EmpId"].Value != null && row.Cells["EmpName"].Value != null &&
                        row.Cells["EmpSal"].Value != null && row.Cells["EmpAge"].Value != null &&
                        row.Cells["EmpPhone"].Value != null && row.Cells["EmpPassword"].Value != null)
                    {
                        // ملء الحقول بالقيم من السطر المحدد
                        tb1.Text = row.Cells["EmpId"].Value.ToString();      // Employee Id
                        tb2.Text = row.Cells["EmpName"].Value.ToString();    // Employee Name
                        tb3.Text = row.Cells["EmpAge"].Value.ToString();     // Employee Age
                        tb4.Text = row.Cells["EmpSal"].Value.ToString();     // Employee Salary
                        tb5.Text = row.Cells["EmpPhone"].Value.ToString();   // Phone Number

                        // إخفاء كلمة المرور كنجوم في الحقل (tb6)
                        tb6.Text = new string('*', row.Cells["EmpPassword"].Value.ToString().Length); // Hide password with stars
                    }
                }
            }
            catch (Exception ex)
            {
                // عرض رسالة خطأ إذا حدث شيء غير متوقع
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }


    
}
