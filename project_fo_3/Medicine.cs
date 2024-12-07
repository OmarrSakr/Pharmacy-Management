using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
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
            // مسح الحقول بعد إضافة البيانات
            MedicineTb.Clear();
            BpriceTb.Clear();
            SpriceTb.Clear();
            QtTb.Clear();
            ExpDate.Value = DateTime.Now;

            Hide();
            Home back = new Home();
            back.ShowDialog();
        }




        // الاتصال بقاعدة البيانات
        public SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\PROJECT_OOP\FO_ORGANIZATION\PHARMACY-MANAGEMENT\PROJECT_FO_3\PHARMACYOB.MDF;Integrated Security=True;");

        // Add Input
        // إضافة دواء جديد
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // التأكد من أن الحقول الأساسية ليست فارغة أو تحتوي على مسافات فارغة
                if (string.IsNullOrWhiteSpace(MedicineTb.Text) ||
                    string.IsNullOrWhiteSpace(BpriceTb.Text) ||
                    string.IsNullOrWhiteSpace(SpriceTb.Text) ||
                    string.IsNullOrWhiteSpace(QtTb.Text) ||
                    ExpDate.Value == DateTimePicker.MinimumDateTime)
                {
                    MessageBox.Show("Please fill all fields.");
                    return;
                }

                // التحقق من القيم المدخلة
                int bPrice, sPrice, medQty;
                DateTime expDate;

                // التحقق من صحة القيم المدخلة
                if (!int.TryParse(BpriceTb.Text.Trim(), out bPrice) ||
                    !int.TryParse(SpriceTb.Text.Trim(), out sPrice) ||
                    !int.TryParse(QtTb.Text.Trim(), out medQty) ||
                    !DateTime.TryParse(ExpDate.Value.ToString(), out expDate))
                {
                    MessageBox.Show("Please enter valid values.");
                    return;
                }

                // فتح الاتصال بقاعدة البيانات
                con.Open();

                // استعلام الإدخال (Insert)
                string query = "INSERT INTO MedicineTb1 (MedName, Bprice, Sprice, MedQty, ExpDate) " +
                               "VALUES (@MedName, @Bprice, @Sprice, @MedQty, @ExpDate)";

                // إعداد الأمر مع المعلمات
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // التأكد من أن MedName يمكن أن يحتوي على حروف عربية
                    cmd.Parameters.Add("@MedName", SqlDbType.NVarChar).Value = MedicineTb.Text.Trim();  // مدخل اسم الدواء
                    cmd.Parameters.Add("@Bprice", SqlDbType.Int).Value = bPrice;
                    cmd.Parameters.Add("@Sprice", SqlDbType.Int).Value = sPrice;
                    cmd.Parameters.Add("@MedQty", SqlDbType.Int).Value = medQty;
                    cmd.Parameters.Add("@ExpDate", SqlDbType.DateTime).Value = ExpDate.Value;

                    // تنفيذ الأمر
                    cmd.ExecuteNonQuery();
                }

                // عرض رسالة نجاح
                MessageBox.Show("Medicine Successfully Added");

                // تحديث البيانات في DataGridView
                PopulateGrid(MedicineGV); // تأكد من أن PopulateGrid تعمل بشكل صحيح

                // مسح الحقول بعد إضافة البيانات
                MedicineTb.Clear();
                BpriceTb.Clear();
                SpriceTb.Clear();
                QtTb.Clear();
                ExpDate.Value = DateTime.Now;
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


        // تعديل بيانات الدواء
        // زر لتحديث بيانات الدواء
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // التأكد من أن الحقول ليست فارغة
                if (string.IsNullOrEmpty(MedicineTb.Text) || string.IsNullOrEmpty(BpriceTb.Text) ||
                    string.IsNullOrEmpty(SpriceTb.Text) || string.IsNullOrEmpty(QtTb.Text))
                {
                    MessageBox.Show("Please fill all fields.");
                    return;
                }

                // التحقق من القيم المدخلة
                decimal bprice, sprice;
                int quantity;
                if (!decimal.TryParse(BpriceTb.Text, out bprice) || bprice <= 0)
                {
                    MessageBox.Show("Please enter a valid Purchase Price.");
                    return;
                }

                if (!decimal.TryParse(SpriceTb.Text, out sprice) || sprice <= 0)
                {
                    MessageBox.Show("Please enter a valid Selling Price.");
                    return;
                }

                if (!int.TryParse(QtTb.Text, out quantity) || quantity < 0)
                {
                    MessageBox.Show("Please enter a valid Quantity.");
                    return;
                }

                con.Open();

                // استعلام التحديث (Update)
                string query = "UPDATE MedicineTb1 SET " +
                               "Bprice = @Bprice, Sprice = @Sprice, MedQty = @Quantity, ExpDate = @ExpDate " +
                               "WHERE MedName = @MedName";

                // إعداد الأمر مع المعلمات
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@MedName", MedicineTb.Text.Trim()); // التعامل مع MedName كـ VARCHAR
                    cmd.Parameters.AddWithValue("@Bprice", bprice);
                    cmd.Parameters.AddWithValue("@Sprice", sprice);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@ExpDate", ExpDate.Value);

                    // تنفيذ الأمر
                    cmd.ExecuteNonQuery();
                }

                // عرض رسالة نجاح
                MessageBox.Show("Medicine Successfully Updated");

                // تحديث البيانات في DataGridView
                PopulateGrid(MedicineGV);

                // مسح الحقول بعد التحديث
                MedicineTb.Clear();
                BpriceTb.Clear();
                SpriceTb.Clear();
                QtTb.Clear();
                ExpDate.Value = DateTime.Now;
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


        // حذف دواء
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(MedicineTb.Text))
                {
                    MessageBox.Show("Please enter the Medicine Name to delete.");
                    return;
                }

                con.Open();

                // استعلام الحذف (Delete)
                string query = "DELETE FROM MedicineTb1 WHERE MedName = @MedName";

                // إعداد الأمر مع المعلمات
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@MedName", MedicineTb.Text);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {   // مسح الحقول بعد إضافة البيانات
                        MedicineTb.Clear();
                        BpriceTb.Clear();
                        SpriceTb.Clear();
                        QtTb.Clear();
                        ExpDate.Value = DateTime.Now;

                        MessageBox.Show("Medicine Successfully Deleted");

                    }
                    else
                    {
                        MessageBox.Show("Medicine not found.");
                    }
                }

                PopulateGrid(MedicineGV);

             
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



        // ملء DataGridView ببيانات الأدوية
        public void PopulateGrid(DataGridView MedicineGV)
        {
            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();

                string query = "SELECT MedName, Bprice, Sprice, MedQty, ExpDate FROM MedicineTb1";
                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                MedicineGV.DataSource = dt;
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

        private void Medicine_Load(object sender, EventArgs e)
        {
            try
            {
                // تمرير عنصر التحكم MedicineGV عند استدعاء الدالة
                PopulateGrid(MedicineGV);
            }
            catch (Exception ex)
            {
                // عرض رسالة خطأ إذا فشل التحميل
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }



        private void button5_Click(object sender, EventArgs e)
        {
            // مسح الحقول بعد إضافة البيانات
            MedicineTb.Clear();
            BpriceTb.Clear();
            SpriceTb.Clear();
            QtTb.Clear();
            ExpDate.Value = DateTime.Now;
        }

        private void MedicineGV_CellContentContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // التأكد من أن الصف الذي تم الضغط عليه هو صف غير فارغ
                if (e.RowIndex >= 0)
                {
                    // الحصول على البيانات من السطر المحدد
                    DataGridViewRow row = MedicineGV.Rows[e.RowIndex];

                    // التأكد من أن السطر يحتوي على قيم قبل ملء الحقول
                    if (row.Cells["MedName"].Value != null && row.Cells["Bprice"].Value != null &&
                        row.Cells["Sprice"].Value != null && row.Cells["MedQty"].Value != null &&
                        row.Cells["ExpDate"].Value != null)
                    {
                        // ملء الحقول بالقيم من السطر المحدد
                        MedicineTb.Text = row.Cells["MedName"].Value.ToString();
                        BpriceTb.Text = row.Cells["Bprice"].Value.ToString();
                        SpriceTb.Text = row.Cells["Sprice"].Value.ToString();
                        QtTb.Text = row.Cells["MedQty"].Value.ToString();
                        ExpDate.Value = Convert.ToDateTime(row.Cells["ExpDate"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                // عرض رسالة خطأ إذا حدث شيء غير متوقع
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void MedicineGV_CellContentClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                // التأكد من أن الصف الذي تم الضغط عليه هو صف غير فارغ
                if (e.RowIndex >= 0)
                {
                    // الحصول على البيانات من السطر المحدد
                    DataGridViewRow row = MedicineGV.Rows[e.RowIndex];

                    // التأكد من أن السطر يحتوي على قيم قبل ملء الحقول
                    if (row.Cells["MedName"].Value != null && row.Cells["Bprice"].Value != null &&
                        row.Cells["Sprice"].Value != null && row.Cells["MedQty"].Value != null &&
                        row.Cells["ExpDate"].Value != null)
                    {
                        // ملء الحقول بالقيم من السطر المحدد
                        MedicineTb.Text = row.Cells["MedName"].Value.ToString();
                        BpriceTb.Text = row.Cells["Bprice"].Value.ToString();
                        SpriceTb.Text = row.Cells["Sprice"].Value.ToString();
                        QtTb.Text = row.Cells["MedQty"].Value.ToString();
                        ExpDate.Value = Convert.ToDateTime(row.Cells["ExpDate"].Value);
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
