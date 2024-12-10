using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Drawing; 
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
                // التأكد من أن الحقول الأساسية ليست فارغة
                if (string.IsNullOrWhiteSpace(MedicineTb.Text) ||
                    string.IsNullOrWhiteSpace(BpriceTb.Text) ||
                    string.IsNullOrWhiteSpace(SpriceTb.Text) ||
                    string.IsNullOrWhiteSpace(QtTb.Text) ||
                    ExpDate.Value == DateTimePicker.MinimumDateTime ||
                    SelectCompany.SelectedIndex == -1) // التأكد من اختيار شركة
                {
                    MessageBox.Show("Please fill all fields.");
                    return;
                }

                // جلب القيم المدخلة
                int bPrice = int.Parse(BpriceTb.Text.Trim());
                int sPrice = int.Parse(SpriceTb.Text.Trim());
                int medQty = int.Parse(QtTb.Text.Trim());
                DateTime expDate = ExpDate.Value;
                string selectedCompany = SelectCompany.SelectedItem.ToString(); // الشركة المختارة

                // استعلام إدخال البيانات
                string query = "INSERT INTO MedicineTb1 (MedName, Bprice, Sprice, MedQty, ExpDate, CompName) " +
                               "VALUES (@MedName, @Bprice, @Sprice, @MedQty, @ExpDate, @CompName)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@MedName", MedicineTb.Text.Trim());
                    cmd.Parameters.AddWithValue("@Bprice", bPrice);
                    cmd.Parameters.AddWithValue("@Sprice", sPrice);
                    cmd.Parameters.AddWithValue("@MedQty", medQty);
                    cmd.Parameters.AddWithValue("@ExpDate", expDate);
                    cmd.Parameters.AddWithValue("@CompName", selectedCompany);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Medicine added successfully!");

                // تحديث الـ DataGridView بعد الإدخال
                PopulateGrid(MedicineGV);

                // مسح الحقول
                MedicineTb.Clear();
                BpriceTb.Clear();
                SpriceTb.Clear();
                QtTb.Clear();
                ExpDate.Value = DateTime.Now;

                // إعادة تعيين ComboBox
                SelectCompany.SelectedIndex = -1;
                SelectCompany.Text = "Select Company";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding medicine: " + ex.Message);
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
        }






        // تعديل بيانات الدواء
        private void button3_Click_1(object sender, EventArgs e)
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

                // جلب القيم المدخلة
                int bPrice = int.Parse(BpriceTb.Text.Trim());
                int sPrice = int.Parse(SpriceTb.Text.Trim());
                int medQty = int.Parse(QtTb.Text.Trim());
                DateTime expDate = ExpDate.Value;
                string selectedCompany = SelectCompany.SelectedItem.ToString(); // الشركة المختارة

                // فتح الاتصال
                con.Open();

                // استعلام للتحقق مما إذا كان اسم الدواء موجود في قاعدة البيانات
                string checkQuery = "SELECT COUNT(*) FROM MedicineTb1 WHERE MedName = @MedName";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                {
                    checkCmd.Parameters.AddWithValue("@MedName", MedicineTb.Text.Trim());

                    int count = (int)checkCmd.ExecuteScalar();
                    if (count == 0)
                    {
                        // الدواء غير موجود
                        MessageBox.Show("Medicine not found. Cannot update.");
                        return;
                    }
                }

                // استعلام التحديث (UPDATE) بدلاً من INSERT
                string query = "UPDATE MedicineTb1 SET " +
                               "Bprice = @Bprice, Sprice = @Sprice, MedQty = @MedQty, ExpDate = @ExpDate, CompName = @CompName " +
                               "WHERE MedName = @MedName";

                // إعداد الأمر مع المعلمات
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@MedName", MedicineTb.Text.Trim());
                    cmd.Parameters.AddWithValue("@Bprice", bPrice);
                    cmd.Parameters.AddWithValue("@Sprice", sPrice);
                    cmd.Parameters.AddWithValue("@MedQty", medQty); // التأكد من أن اسم المعامل هنا هو @MedQty وليس @Quantity
                    cmd.Parameters.AddWithValue("@ExpDate", expDate);
                    cmd.Parameters.AddWithValue("@CompName", selectedCompany);

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
                SelectCompany.Text = "Select Company"; // تعيين النص الافتراضي بعد اختيار شركة
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                // إغلاق الاتصال في الـ finally لضمان إغلاقه في كل الحالات
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
        }





        //  لحذف الدواء
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
                    cmd.Parameters.AddWithValue("@MedName", MedicineTb.Text.Trim()); // تأكد من إزالة المسافات الزائدة

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // مسح الحقول بعد الحذف
                        MedicineTb.Clear();
                        BpriceTb.Clear();
                        SpriceTb.Clear();
                        QtTb.Clear();
                        ExpDate.Value = DateTime.Now;
                        SelectCompany.Text = "Select Company"; // تعيين النص الافتراضي بعد اختيار شركة

                        MessageBox.Show("Medicine Successfully Deleted");
                    }
                    else
                    {
                        MessageBox.Show("Medicine not found.");
                    }
                }

                // تحديث البيانات في DataGridView
                PopulateGrid(MedicineGV);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                // التأكد من إغلاق الاتصال إذا كان مفتوحًا
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
        }




        // ملء DataGridView ببيانات الأدوية
        // ملء DataGridView ببيانات الأدوية
        public void PopulateGrid(DataGridView MedicineGV)
        {
            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();

                // تعديل الاستعلام لإضافة عمود الشركة
               string query = "SELECT MedName, Bprice, Sprice, MedQty, ExpDate, CompName FROM MedicineTb1";

                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // تعيين البيانات إلى DataGridView
                MedicineGV.DataSource = dt;

                // تخصيص التنسيق بعد ملء البيانات
                MedicineGV.Columns["MedName"].HeaderText = "Medicine Name";
                MedicineGV.Columns["Bprice"].HeaderText = "Buying Price";
                MedicineGV.Columns["Sprice"].HeaderText = "Selling Price";
                MedicineGV.Columns["MedQty"].HeaderText = "Quantity";
                MedicineGV.Columns["ExpDate"].HeaderText = "Expiration Date";
                MedicineGV.Columns["CompName"].HeaderText = "Company Name";
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



        private void InitializeMedicinegGrid()
        {
          

            // إضافة الأعمدة مع رؤوس الأعمدة
            MedicineGV.Columns.Add("MedName", "MedName");  // إضافة عمود اسم الدواء
            MedicineGV.Columns.Add("Bprice", "BPrice");  // إضافة عمود سعر الشراء (Bprice)
            MedicineGV.Columns.Add("Sprice", "SPrice");  // إضافة عمود سعر البيع (Sprice)
            MedicineGV.Columns.Add("MedQty", "Quantity");  // إضافة عمود الكمية (MedQty)
            MedicineGV.Columns.Add("ExpDate", "Ex.Date");  // إضافة عمود تاريخ الانتهاء (ExpDate)
            MedicineGV.Columns.Add("Company", "CName");  // إضافة عمود اسم الشركة (Company)

            // تغيير الخط في رأس الجدول
            MedicineGV.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular);

            // جعل الكتابة في النص لجميع الأعمدة
            foreach (DataGridViewColumn column in MedicineGV.Columns)
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // تحديد عرض الأعمدة
            MedicineGV.Columns["MedName"].Width = 120;  // عرض عمود MedName أكبر من الأعمدة الأخرى
            MedicineGV.Columns["Bprice"].Width = 80;  // عرض عمود Buying Price
            MedicineGV.Columns["Sprice"].Width = 80;  // عرض عمود Selling Price
            MedicineGV.Columns["MedQty"].Width = 80;  // عرض عمود Quantity
            MedicineGV.Columns["ExpDate"].Width = 120;  // عرض عمود Expiration Date
            MedicineGV.Columns["Company"].Width = 120;  // عرض عمود Company Name
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


        private void MedicineGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
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
                        row.Cells["ExpDate"].Value != null && row.Cells["CompName"].Value != null) // التأكد من وجود اسم الشركة
                    {
                        // ملء الحقول بالقيم من السطر المحدد
                        MedicineTb.Text = row.Cells["MedName"].Value.ToString();
                        BpriceTb.Text = row.Cells["Bprice"].Value.ToString();
                        SpriceTb.Text = row.Cells["Sprice"].Value.ToString();
                        QtTb.Text = row.Cells["MedQty"].Value.ToString();
                        ExpDate.Value = Convert.ToDateTime(row.Cells["ExpDate"].Value);

                        // تعيين قيمة اسم الشركة في SelectCompany
                        SelectCompany.Text = row.Cells["CompName"].Value.ToString(); // تعيين اسم الشركة
                    }
                }
            }
            catch (Exception ex)
            {
                // عرض رسالة خطأ إذا حدث شيء غير متوقع
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        private void MedicineGV_CellMouseClick(object sender, DataGridViewCellEventArgs e)
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
                        row.Cells["ExpDate"].Value != null && row.Cells["CompName"].Value != null) // التأكد من وجود اسم الشركة
                    {
                        // ملء الحقول بالقيم من السطر المحدد
                        MedicineTb.Text = row.Cells["MedName"].Value.ToString();
                        BpriceTb.Text = row.Cells["Bprice"].Value.ToString();
                        SpriceTb.Text = row.Cells["Sprice"].Value.ToString();
                        QtTb.Text = row.Cells["MedQty"].Value.ToString();
                        ExpDate.Value = Convert.ToDateTime(row.Cells["ExpDate"].Value);

                        // تعيين قيمة اسم الشركة في SelectCompany
                        SelectCompany.Text = row.Cells["CompName"].Value.ToString(); // تعيين اسم الشركة
                    }
                }
            }
            catch (Exception ex)
            {
                // عرض رسالة خطأ إذا حدث شيء غير متوقع
                MessageBox.Show($"Error: {ex.Message}");
            }
        }






        private void Medicine_Load(object sender, EventArgs e)
        {
            try
            {
                // تمرير عنصر التحكم MedicineGV عند استدعاء الدالة
                PopulateGrid(MedicineGV);
                LoadCompanyNames();
            }
            catch (Exception ex)
            {
                // عرض رسالة خطأ إذا فشل التحميل
                MessageBox.Show("Error loading data: " + ex.Message);
            }

        }
        private void LoadCompanyNames()
        {
            try
            {
                con.Open();

                // استعلام لجلب أسماء الشركات
                string query = "SELECT CompName FROM CompanyTb1";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    // إضافة الشركات إلى الـ ComboBox
                    SelectCompany.Items.Clear();
                    while (reader.Read())
                    {
                        SelectCompany.Items.Add(reader["CompName"].ToString());
                    }

                    // ضبط الخيار الافتراضي
                    SelectCompany.SelectedIndex = -1; // عدم اختيار أي شركة عند التحميل
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading company names: " + ex.Message);
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
        }

        private void MedicineGV_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
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
                        row.Cells["ExpDate"].Value != null && row.Cells["CompName"].Value != null) // التأكد من وجود اسم الشركة
                    {
                        // ملء الحقول بالقيم من السطر المحدد
                        MedicineTb.Text = row.Cells["MedName"].Value.ToString();
                        BpriceTb.Text = row.Cells["Bprice"].Value.ToString();
                        SpriceTb.Text = row.Cells["Sprice"].Value.ToString();
                        QtTb.Text = row.Cells["MedQty"].Value.ToString();
                        ExpDate.Value = Convert.ToDateTime(row.Cells["ExpDate"].Value);

                        // تعيين قيمة اسم الشركة في SelectCompany
                        SelectCompany.Text = row.Cells["CompName"].Value.ToString(); // تعيين اسم الشركة
                    }
                }
            }
            catch (Exception ex)
            {
                // عرض رسالة خطأ إذا حدث شيء غير متوقع
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void SelectCompany_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        //لعرض معلومات عن الشركة المختارة 
        //private void SelectCompany_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        // التأكد من اختيار شركة
        //        if (SelectCompany.SelectedIndex == -1) return;

        //        // جلب اسم الشركة المختارة
        //        string selectedCompanyName = SelectCompany.SelectedItem.ToString(); // تأكد من أنك تستخدم النص وليس الكائن

        //        MessageBox.Show("Selected Company: " + selectedCompanyName); // تأكد من القيمة المختارة

        //        // استعلام لجلب بيانات الشركة
        //        string query = "SELECT CompId, CompPhone, CompAddress FROM CompanyTb1 WHERE CompName = @CompName";

        //        using (SqlCommand cmd = new SqlCommand(query, con))
        //        {
        //            cmd.Parameters.AddWithValue("@CompName", selectedCompanyName);
        //            con.Open();

        //            SqlDataReader reader = cmd.ExecuteReader();
        //            if (reader.Read())
        //            {
        //                // التعامل مع البيانات المسترجعة
        //                string compId = reader["CompId"].ToString();
        //                string compPhone = reader["CompPhone"] != DBNull.Value ? reader["CompPhone"].ToString() : "N/A";
        //                string compAddress = reader["CompAddress"] != DBNull.Value ? reader["CompAddress"].ToString() : "N/A";

        //                // عرض أو استخدام البيانات كما تريد
        //                MessageBox.Show($"Company ID: {compId}\nPhone: {compPhone}\nAddress: {compAddress}");
        //            }
        //            reader.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error loading company details: " + ex.Message);
        //    }
        //    finally
        //    {
        //        if (con.State == System.Data.ConnectionState.Open)
        //            con.Close();
        //    }
        //}

    }

}
