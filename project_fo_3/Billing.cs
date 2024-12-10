using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
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


        // Back button
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // فحص إذا كانت المعاملة لم تُطبع بعد
                if (tempTransaction.Count > 0)
                {
                    // عرض رسالة للمستخدم قبل إغلاق النموذج
                    DialogResult result = MessageBox.Show(
                        "هل أنت متأكد أنك تريد الخروج؟ لو ضغطت ( نعم) سيتم استرجاع الكميات إلى المخزون إذا لم تتم الطباعة.",
                        "تأكيد الخروج",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    // إذا اختار المستخدم "نعم"، يتم التراجع عن التغييرات
                    if (result == DialogResult.Yes)
                    {
                        // إعادة الكميات المخفضة إلى قاعدة البيانات ومسح البيانات من الجدول
                        RollbackTransaction();

                        // مسح البيانات من الجدول
                        BillingGV.Rows.Clear();

                        // إعادة تخصيص الجدول بعد مسح البيانات
                        CustomizeDataGridView();

                        MessageBox.Show("تم استرجاع البيانات و تم إلغاء الفاتورة.");
                    }
                    else
                    {
                        // إذا اختار "لا"، يتم إلغاء عملية الإغلاق
                        return;
                    }
                }
                else
                {
                    // إذا كانت المعاملة قد تم طباعتها بالفعل أو لا توجد معاملات مضافة
                    //MessageBox.Show("تمت الطباعة بالفعل، لا يمكن التراجع عن البيانات.");
                }

                // الانتقال إلى الصفحة الرئيسية
                this.Hide();
                Home back = new Home();
                back.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while clearing the grid or rolling back transaction: " + ex.Message);
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






        // الاتصال بقاعدة البيانات
        public SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\PROJECT_OOP\FO_ORGANIZATION\PHARMACY-MANAGEMENT\PROJECT_FO_3\PHARMACYOB.MDF;Integrated Security=True;");

        // عند اختيار دواء من ComboBox

        private int GetSellingPrice(string medName)
        {
            int sellingPrice = 0;

            try
            {
                string query = "SELECT Sprice FROM MedicineTb1 WHERE MedName = @MedName";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@MedName", medName);
                    con.Open();

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                        sellingPrice = Convert.ToInt32(result);

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching selling price: {ex.Message}");
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return sellingPrice;
        }


        // Employee name (تم إلغاء التحديث التلقائي هنا)
        private void SelectEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            // لا تفعل شيء هنا الآن، فقط عند الضغط على زر Add سيحدث التحديث
        }







        // دالة لملء ComboBox بأسماء الأدوية
        private void LoadMedicineNames()
        {
            try
            {
                con.Open();
                string query = "SELECT MedName FROM MedicineTb1"; // استعلام لجلب أسماء الأدوية

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        // إضافة اسم الدواء إلى الـ ComboBox
                        MediComboCb.Items.Add(reader["MedName"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading medicine names: " + ex.Message);
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
        }

        // عند تغيير الاختيار في ComboBox
        private void MediComboCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // التحقق من اختيار دواء
                if (MediComboCb.SelectedItem == null)
                {
                    label4.Text = "Available Stock"; // عرض متاح 0 إذا لم يتم اختيار دواء
                    label4.ForeColor = Color.Black; // إعادة اللون إلى الأسود
                    tb1.Clear(); // مسح الكمية المدخلة
                    return;
                }

                // جلب اسم الدواء الذي تم اختياره
                string selectedMedicine = MediComboCb.SelectedItem.ToString();

                // استعلام لجلب الكمية المتوفرة للدواء من قاعدة البيانات
                string query = "SELECT MedQty FROM MedicineTb1 WHERE MedName = @MedName";
                int availableQuantity = 0;

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@MedName", selectedMedicine);
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        availableQuantity = Convert.ToInt32(result);
                    }
                    con.Close();
                }

                // عرض الكمية المتاحة في المخزن في Label4 فور اختيار الدواء
                label4.Text = $"Available Stock: {availableQuantity} Units";
                label4.ForeColor = Color.Black; // إعادة اللون إلى الأسود

                // إذا كانت الكمية المدخلة مسبقاً في tb1، تحقق من صحتها
                if (!string.IsNullOrEmpty(tb1.Text))
                {
                    int enteredQuantity;
                    if (int.TryParse(tb1.Text, out enteredQuantity))
                    {
                        // إذا كانت الكمية المدخلة أكبر من المتاحة، أظهر رسالة خطأ
                        if (enteredQuantity > availableQuantity)
                        {
                            label4.Text = $"Error: Only {availableQuantity} units available.";
                            label4.ForeColor = Color.Red;
                            tb1.Clear(); // مسح الكمية المدخلة
                        }
                        else
                        {
                            // إذا كانت الكمية المدخلة صحيحة، أظهر الكمية المتاحة بعد طرح الكمية المدخلة
                            label4.Text = $"Available Stock: {availableQuantity} Units";
                            label4.ForeColor = Color.Green;
                        }
                    }
                    else
                    {
                        // في حالة إدخال قيمة غير صحيحة، أظهر رسالة تحذير
                        MessageBox.Show("Please enter a valid quantity.");
                        tb1.Clear(); // مسح الكمية المدخلة
                    }
                }
            }
            catch (Exception ex)
            {
                label4.Text = $"Error: {ex.Message}";
                label4.ForeColor = Color.Red;
            }
        }

        // دالة لحفظ الفاتورة في قاعدة البيانات
        private void SaveToDatabase(string billId, string medName, int quantity, int totalAmount)
        {
            try
            {
                // استعلام للحصول على السعر من جدول الأدوية
                string getSpriceQuery = "SELECT Sprice FROM MedicineTb1 WHERE MedName = @MedName";
                int sprice = 0;

                using (SqlCommand cmd = new SqlCommand(getSpriceQuery, con))
                {
                    cmd.Parameters.AddWithValue("@MedName", medName);

                    con.Open();
                    var result = cmd.ExecuteScalar(); // إرجاع أول نتيجة (السعر)
                    con.Close();

                    if (result != DBNull.Value)
                    {
                        sprice = Convert.ToInt32(result);
                    }
                    else
                    {
                        MessageBox.Show("Medicine not found.");
                        return; // إذا لم يتم العثور على الدواء، لا نكمل العملية
                    }
                }

                // استعلام لإدخال البيانات في جدول الفواتير
                string insertQuery = "INSERT INTO BillsTb1 (BillId, MedName, Sprice, Quantity, TotalAmount) VALUES (@BillId, @MedName, @Sprice, @Quantity, @TotalAmount)";

                using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                {
                    cmd.Parameters.AddWithValue("@BillId", billId);
                    cmd.Parameters.AddWithValue("@MedName", medName);
                    cmd.Parameters.AddWithValue("@Sprice", sprice);  // إدخال السعر المستخرج من جدول الأدوية
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                // تسجيل الكمية المؤقتة في القائمة
                tempTransaction.Add((medName, quantity));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}");
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
        }









        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // التحقق من أن جميع الحقول تم تعبئتها (البيانات الخاصة بالدواء)
                if (MediComboCb.SelectedItem == null)
                {
                    MessageBox.Show("Please select a medicine.");
                    return;
                }

                // التحقق من إدخال كمية صحيحة
                if (!int.TryParse(tb1.Text, out int quantity) || quantity <= 0)
                {
                    MessageBox.Show("Please enter a valid quantity.");
                    return;
                }

                // جلب القيم اللازمة
                string selectedMedicine = MediComboCb.SelectedItem.ToString();
                int sellingPrice = GetSellingPrice(selectedMedicine); // جلب سعر البيع من جدول الأدوية

                if (sellingPrice <= 0)
                {
                    MessageBox.Show("Failed to retrieve the selling price for the selected medicine.");
                    return;
                }

                int totalAmount = sellingPrice * quantity; // حساب المبلغ الإجمالي

                // تحقق من وجود الدواء في الفاتورة الحالية
                bool medicineExistsInBill = false;
                string billId = string.Empty;
                int oldQuantity = 0; // لتخزين الكمية القديمة قبل التعديل
                foreach (DataGridViewRow row in BillingGV.Rows)
                {
                    if (row.Cells["MedName"].Value != null && row.Cells["MedName"].Value.ToString() == selectedMedicine)
                    {
                        // الدواء موجود بالفعل في الفاتورة
                        medicineExistsInBill = true;
                        oldQuantity = Convert.ToInt32(row.Cells["Quantity"].Value); // تخزين الكمية القديمة
                        billId = row.Cells["BillId"].Value.ToString(); // جلب الـ billId للفاتورة

                        // قم بتعديل الكمية في نفس الصف بدلاً من إضافة صف جديد
                        DialogResult dialogResult = MessageBox.Show("This medicine is already added. Do you want to modify the quantity?", "Modify Quantity", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            // أولاً، أعد الكمية القديمة إلى المخزون في قاعدة البيانات
                            string returnOldQuantityQuery = "UPDATE MedicineTb1 SET MedQty = MedQty + @OldQuantity WHERE MedName = @MedName";
                            using (SqlCommand cmd = new SqlCommand(returnOldQuantityQuery, con))
                            {
                                cmd.Parameters.AddWithValue("@OldQuantity", oldQuantity); // إضافة الكمية القديمة مرة أخرى
                                cmd.Parameters.AddWithValue("@MedName", selectedMedicine);
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }

                            // بعد إعادة الكمية القديمة، نحديث الكمية الجديدة في الفاتورة
                            row.Cells["Quantity"].Value = quantity; // تحديث الكمية
                            row.Cells["TotalAmount"].Value = totalAmount; // تحديث المبلغ الإجمالي
                        }
                        break;
                    }
                }

                if (!medicineExistsInBill)
                {
                    // إذا كان الدواء غير موجود في الفاتورة الحالية، أضفه كصف جديد
                    billId = GenerateUniqueBillId(); // توليد ID عشوائي للفاتورة
                    BillingGV.Rows.Add(billId, selectedMedicine, sellingPrice, quantity, totalAmount);  // إضافة صف جديد
                }

                // خصم الكمية الجديدة من المخزون في قاعدة البيانات
                string updateQuery = "UPDATE MedicineTb1 SET MedQty = MedQty - @Quantity WHERE MedName = @MedName";
                using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                {
                    cmd.Parameters.AddWithValue("@Quantity", quantity); // خصم الكمية الجديدة
                    cmd.Parameters.AddWithValue("@MedName", selectedMedicine); // اسم الدواء
                    con.Open();
                    cmd.ExecuteNonQuery(); // تنفيذ الاستعلام
                    con.Close();
                }

                // تحديث الـ PriceBDLabel لعرض المجموع الإجمالي بعد إضافة الدواء
                UpdateTotalAmount();

                // تنظيف الحقول بعد إضافة الدواء
                MediComboCb.SelectedIndex = -1;
                MediComboCb.Text = "Select Medicine";
                tb1.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding item: {ex.Message}");
            }
        }




        // دالة للتحقق إذا كان اسم الدواء موجودًا في الفاتورة بناءً على اسم الدواء
        private bool CheckMedicineExistsInBillByName(string medicineName)
        {
            foreach (DataGridViewRow row in BillingGV.Rows)
            {
                if (row.Cells["MedName"].Value != null && row.Cells["MedName"].Value.ToString() == medicineName)
                {
                    return true; // الدواء موجود في الفاتورة
                }
            }
            return false; // الدواء غير موجود في الفاتورة
        }

        // دالة لتحديث الكمية في الفاتورة إذا كان اسم الدواء موجودًا
        private void UpdateMedicineQuantityInBillByName(string medicineName, int quantity, int totalAmount)
        {
            // استعلام لتحديث الكمية في الفاتورة، حيث سيتم استبدال الكمية القديمة بالكمية الجديدة
            string updateQuery = "UPDATE BillsTb1 SET Quantity = @Quantity, TotalAmount = @TotalAmount WHERE MedName = @MedName";
            using (SqlCommand cmd = new SqlCommand(updateQuery, con))
            {
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                cmd.Parameters.AddWithValue("@MedName", medicineName);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            // بعد تحديث الكمية في قاعدة البيانات، نقوم بتحديث العرض في DataGridView
            foreach (DataGridViewRow row in BillingGV.Rows)
            {
                if (row.Cells["MedName"].Value != null && row.Cells["MedName"].Value.ToString() == medicineName)
                {
                    row.Cells["Quantity"].Value = quantity; // استبدال الكمية القديمة بالكمية الجديدة
                    row.Cells["TotalAmount"].Value = totalAmount; // استبدال المبلغ الإجمالي بالكمية الجديدة
                    break;
                }
            }

            // تحديث المجموع الإجمالي بعد تعديل الكمية
            UpdateTotalAmount();
        }








        // تعريف الدالة لتوليد ID عشوائي
        private string GenerateUniqueBillId()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 8); // توليد معرّف فريد
        }









        private void Billing_Load(object sender, EventArgs e)
        {
            try
            {

                InitializeBillingGrid(); // تأكد من إعداد DataGridView بشكل صحيح
                LoadMedicineNames(); // تحميل بيانات الأدوية
                PopulateGrid(BillingGV); // تحميل بيانات الفواتير
                PriceADLabel.Text = $"Price A.D"; // أو القيمة الافتراضية
                LoadEmployeeNames(); // تحميل أسماء الموظفين في ComboBox
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
            CustomizeDataGridView(); // تخصيص عرض DataGridView إذا لزم الأمر
        }

        private void LoadEmployeeNames()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                string query = "SELECT EmpId, EmpName FROM EmployeeTab1";
                using (SqlCommand cmd = new SqlCommand(query, con))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // إضافة الأسماء والـ ID إلى ComboBox
                        SelectEmployee.Items.Add(new ComboBoxItem(reader["EmpName"].ToString(), reader["EmpId"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading employee names: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }


        public class ComboBoxItem
        {
            public string Text { get; set; }
            public string Value { get; set; }

            public ComboBoxItem(string text, string value)
            {
                Text = text; // الاسم
                Value = value; // ID
            }

            public override string ToString()
            {
                return Text; // يتم عرض الاسم فقط في ComboBox
            }
        }

        private string GetEmployeeId(string employeeName)
        {
            string empId = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(employeeName))
                {
                    MessageBox.Show("Employee name is missing.");
                    return empId;
                }

                using (SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\PROJECT_OOP\FO_ORGANIZATION\PHARMACY-MANAGEMENT\PROJECT_FO_3\PHARMACYOB.MDF;Integrated Security=True;"))
                {
                    con.Open();

                    string query = "SELECT EmpId FROM EmployeeTab1 WHERE EmpName = @EmpName";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add(new SqlParameter("@EmpName", SqlDbType.NVarChar) { Value = employeeName });

                        var result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            empId = result.ToString();
                        }
                        else
                        {
                            MessageBox.Show($"No employee found with the name: {employeeName}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

            return empId;
        }

        private void PopulateGrid(DataGridView BillingGV)
        {
            try
            {
                // مسح البيانات المعروضة في الجدول فقط
                BillingGV.Rows.Clear();
                Clear();
                // حساب المجموع الكلي بعد تحميل البيانات
                //UpdateTotalAmount();


                //// استعلام لجلب البيانات من جدول BillsTb1
                //string query = "SELECT BillId, MedName, Quantity, TotalAmount FROM BillsTb1";

                //using (SqlCommand cmd = new SqlCommand(query, con))
                //{
                //    con.Open();
                //    SqlDataReader reader = cmd.ExecuteReader();

                //    // إذا لم يتم العثور على بيانات
                //    if (!reader.HasRows)
                //    {
                //        MessageBox.Show("No data found in BillsTb1");
                //        return;
                //    }

                //    // إضافة البيانات الجديدة فقط إلى DataGridView
                //    while (reader.Read())
                //    {
                //        BillingGV.Rows.Add(
                //            reader["BillId"].ToString(),
                //            reader["MedName"].ToString(),
                //            Convert.ToInt32(reader["Quantity"]),
                //            Convert.ToInt32(reader["TotalAmount"])
                //        );
                //    }
                //}

                //// تأكيد أن البيانات تم إضافتها بشكل صحيح
                //MessageBox.Show("Data populated successfully!");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error populating grid: " + ex.Message);
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }


        // دالة لملء DataGridView بالفواتير
        private void InitializeBillingGrid()
        {
            // مسح الأعمدة السابقة إذا كانت موجودة
            BillingGV.Columns.Clear();

            // إضافة الأعمدة مع رؤوس الأعمدة
            BillingGV.Columns.Add("BillId", "MedID");
            BillingGV.Columns.Add("MedName", "MedName");
            BillingGV.Columns.Add("Sprice", "Seller Price");  // إضافة عمود سعر الدواء
            BillingGV.Columns.Add("Quantity", "Quantity");
            BillingGV.Columns.Add("TotalAmount", "Total Amount");

            // تغيير الخط في رأس الجدول
            BillingGV.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular);

            // جعل الكتابة في النص لجميع الأعمدة
            foreach (DataGridViewColumn column in BillingGV.Columns)
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // تحديد عرض الأعمدة
            BillingGV.Columns["BillId"].Width = 80;  // عرض عمود MedID
            BillingGV.Columns["MedName"].Width = 120;  // عرض عمود MedName أكبر من الأعمدة الأخرى
            BillingGV.Columns["Sprice"].Width = 100;  // عرض عمود S.P. OfOneMed
            BillingGV.Columns["Quantity"].Width = 80;  // عرض عمود Quantity
            BillingGV.Columns["TotalAmount"].Width = 120;  // عرض عمود TotalAmount
        }




        // عند تغيير النص في TextBox (tb1)

        private void tb1_TextChanged_1(object sender, EventArgs e)
        {
            try
            {
                // التحقق من أن الـ TextBox يحتوي على كمية صحيحة
                if (MediComboCb.SelectedItem == null || string.IsNullOrEmpty(tb1.Text))
                {
                    return; // لا تفعل شيء إذا لم يتم اختيار دواء أو لم يتم إدخال كمية
                }

                int enteredQuantity;
                if (int.TryParse(tb1.Text, out enteredQuantity))
                {
                    // جلب اسم الدواء الذي تم اختياره
                    string selectedMedicine = MediComboCb.SelectedItem.ToString();

                    // استعلام لجلب الكمية المتوفرة للدواء من قاعدة البيانات
                    string query = "SELECT MedQty FROM MedicineTb1 WHERE MedName = @MedName";
                    int availableQuantity = 0;

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@MedName", selectedMedicine);
                        con.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            availableQuantity = Convert.ToInt32(result);
                        }
                        con.Close();
                    }

                    // إذا كانت الكمية المدخلة أكبر من الكمية المتاحة، أظهر رسالة خطأ
                    if (enteredQuantity > availableQuantity)
                    {
                        label4.Text = $"Error: Only {availableQuantity} units available.";
                        label4.ForeColor = Color.Red;
                        tb1.Clear(); // مسح الكمية المدخلة
                    }
                    else if (enteredQuantity <= 0)
                    {
                        label4.Text = $"Error: Only {availableQuantity} units available.";
                        label4.ForeColor = Color.Red;
                        tb1.Clear(); // مسح الكمية المدخلة
                    }
                    else
                    {
                        // إذا كانت الكمية المدخلة صحيحة، أظهر الكمية المتاحة بعد طرح الكمية المدخلة
                        label4.Text = $"Available Stock: {availableQuantity} Units";
                        label4.ForeColor = Color.Green;
                    }
                }
            }
            catch (Exception ex)
            {
                label4.Text = $"Error: {ex.Message}";
                label4.ForeColor = Color.Red;
            }
        }

        // Print Button

        // تعريف المتغير على مستوى الفئة أو الـ class
        private List<(string Medicine, int Quantity)> tempTransaction = new List<(string Medicine, int Quantity)>();

        // عند الضغط على زر إضافة البيانات أو الطباعة
        private void button2_Click(object sender, EventArgs e)
        {
            if (BillingGV.Rows.Count > 0) // التحقق من وجود بيانات للطباعة
            {
                try
                {
                    // تجهيز البيانات للخصم المؤقت
                    PrepareTransaction();

                    // إنشاء مستند للطباعة
                    PrintDocument printDoc = new PrintDocument();
                    printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                    printDoc.EndPrint += new PrintEventHandler(PrintCompleted);

                    // عرض معاينة الطباعة
                    PrintPreviewDialog preview = new PrintPreviewDialog
                    {
                        Document = printDoc
                    };

                    DialogResult result = preview.ShowDialog();

                    // التحقق مما إذا تم إغلاق نافذة الطباعة بدون طباعة
                    if (result != DialogResult.OK)
                    {
                        // في حالة إلغاء الطباعة، نقوم بإعادة ملء الجدول بالبيانات الأصلية
                        PopulateGrid(BillingGV);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error during printing: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("No data available to print.");
            }
        }



        // التراجع عن المعاملات عند إلغاء الطباعة
      




        // تطبيق المعاملات على قاعدة البيانات
        private void ApplyTransaction()
        {
            try
            {
                foreach (var item in tempTransaction)
                {
                    string updateQuery = "UPDATE MedicineTb1 SET MedQty = MedQty - @Quantity WHERE MedName = @MedName";
                    using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                        cmd.Parameters.AddWithValue("@MedName", item.Medicine);
                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        con.Close();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show($"Quantity of {item.Medicine} has been deducted.");
                        }
                        else
                        {
                            MessageBox.Show($"Failed to deduct {item.Medicine} quantity.");
                        }
                    }
                }
                tempTransaction.Clear(); // تفريغ المعاملة بعد إتمام الخصم
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error applying transaction: {ex.Message}");
            }
        }

        // تجهيز المعاملات المؤقتة
        private void PrepareTransaction()
        {
            tempTransaction.Clear();  // التأكد من مسح المعاملات السابقة

            foreach (DataGridViewRow row in BillingGV.Rows)
            {
                // تأكد من وجود القيم في الخلايا قبل إضافتها
                if (row.Cells["MedName"].Value != null && row.Cells["Quantity"].Value != null)
                {
                    string medicine = row.Cells["MedName"].Value.ToString();
                    int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);

                    // إضافة البيانات إلى المعاملة المؤقتة
                    if (quantity > 0)  // التأكد من أن الكمية أكبر من صفر
                    {
                        tempTransaction.Add((medicine, quantity));
                    }
                }
            }

            // تحقق من إضافة البيانات إلى tempTransaction
            if (tempTransaction.Count == 0)
            {
                MessageBox.Show("No items to print.");
            }
        }

        // عند إتمام عملية الطباعة
        private void PrintCompleted(object sender, PrintEventArgs e)
        {
            if (!e.Cancel)
            {


                // إغلاق نافذة المعاينة للطباعة

                // استرجاع الكميات إلى المخزون إذا كانت المعاملة لم تُطبع
                // تم مسح البيانات المؤقتة
                tempTransaction.Clear();
            }
            else
            {



                RollbackTransaction();


            }
        }



        private void Clear()
        {
            PriceBDLabel.Text = $"Price B.D: 0 EGP"; // أو القيمة الافتراضية
            PriceADLabel.Text = $"Price A.D: 0 EGP"; // أو القيمة الافتراضية
            Discounttext.Clear();
            CustomerNameLabel.Text = $"Customer Name:";  // مسح النص في CustomerNameLabel
            CustomerPhoneLabel.Text = $"Customer Phone:"; // مسح النص في CustomerPhoneLabel
            CustomerAddressLabel.Text = $"Customer address:"; // مسح النص في CustomerAddressLabel
            EmployeeLabel.Text = $"Employee Name:";
            EmployeeIdLabel.Text = $"Employee Id:";
            CustomerIdLabel.Text = $"Customer Id:";
            TimingLabel.Text = $"Timing:";
            gunaLineTextBox3.Clear();
            gunaLineTextBox2.Clear();
            gunaLineTextBox1.Clear();
        }




        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // فحص إذا كانت المعاملة لم تُطبع بعد
            if (tempTransaction.Count > 0)
            {
                // عرض رسالة للمستخدم قبل إغلاق النموذج
                DialogResult result = MessageBox.Show(
                    "هل أنت متأكد أنك تريد الخروج؟ سيتم استرجاع الكميات إلى المخزون إذا لم تتم الطباعة.",
                    "تأكيد الخروج",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                // إذا اختار المستخدم "نعم"، يتم التراجع عن التغييرات
                if (result == DialogResult.Yes)
                {
                    RollbackTransaction();  // استرجاع الكميات إلى المخزون
                    MessageBox.Show("تم استرجاع البيانات.");
                }
                else
                {
                    // إذا اختار "لا"، يتم إلغاء عملية الإغلاق
                    e.Cancel = true;
                }
            }
        }

       

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;

            // استخدام أبعاد الورقة للطباعة
            int startX = e.MarginBounds.Left;
            int startY = e.MarginBounds.Top + 0;
            int tableWidth = e.MarginBounds.Width;
            int rowHeight = 30;

            Font headerFont = new Font("Microsoft Sans Serif", 12, FontStyle.Bold);
            Font rowFont = new Font("Arial", 10);
            Font employeeFont = new Font("Arial", 12, FontStyle.Bold);
            Font totalFont = new Font("Arial", 10, FontStyle.Bold);
            Font smallFont = new Font("Arial", 9, FontStyle.Bold);
            Font priceFont = new Font("Arial", 10, FontStyle.Bold);

            Brush rowBrush = Brushes.Black;
            Brush invoiceBackgroundBrush = new SolidBrush(Color.FromArgb(192, 64, 0));
            Brush invoiceTextBrush = Brushes.White;
            Brush priceBeforeDiscountBackgroundBrush = Brushes.LightYellow; // لون الخلفية قبل الخصم
            Brush priceAfterDiscountBackgroundBrush = Brushes.DarkGray; // لون الخلفية بعد الخصم
            Brush priceTextBrush = Brushes.Black; // لون النص الأسود

            Pen tablePen = new Pen(Color.Black);

            // رسم عنوان التقرير (Pharmacy Invoice)
            Rectangle titleBackground = new Rectangle(startX, startY, tableWidth, rowHeight);
            g.FillRectangle(invoiceBackgroundBrush, titleBackground);
            StringFormat titleFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            g.DrawString("Al Mohandes Pharmacy Invoice", headerFont, invoiceTextBrush, titleBackground, titleFormat);
            startY += rowHeight + 10;

            // وضع تاريخ الفاتورة على اليسار
            g.DrawString($"Date: {DateTime.Now.ToShortDateString()}", smallFont, Brushes.Black, startX, startY);

            // تحريك العبارة "Your trusted pharmacy for health and well-being." للأعلى بمقدار 4 نقاط
            StringFormat rightAlignFormat = new StringFormat
            {
                Alignment = StringAlignment.Far, // لمحاذاة النص إلى اليمين
                LineAlignment = StringAlignment.Center // في نفس الصف مع تاريخ الفاتورة
            };
            startY -= 10; // رفع العبارة 4 نقاط للأعلى
            g.DrawString("Your trusted pharmacy for health and well-being.", smallFont, Brushes.DarkGreen,
                new RectangleF(startX, startY, tableWidth, rowHeight), rightAlignFormat);

            // زيادة المسافة بين تاريخ الفاتورة وبقية البيانات
            startY += rowHeight + 15; // زيادة المسافة قليلاً بين البيانات

            // رسم خلفية مميزة للبيانات الشخصية
            Rectangle customerInfoBackground = new Rectangle(startX, startY, tableWidth, rowHeight * 4); // خلفية تغطي البيانات الشخصية
            g.FillRectangle(Brushes.LightBlue, customerInfoBackground); // تغيير اللون هنا حسب رغبتك

            // طباعة بيانات العميل مع خلفية مميزة
            g.DrawString($" {CustomerIdLabel.Text}", rowFont, rowBrush, startX, startY);
            startY += rowHeight;
            g.DrawString($" {CustomerNameLabel.Text}", rowFont, rowBrush, startX, startY);
            startY += rowHeight;
            g.DrawString($" {CustomerPhoneLabel.Text}", rowFont, rowBrush, startX, startY);
            startY += rowHeight;
            g.DrawString($" {CustomerAddressLabel.Text}", rowFont, rowBrush, startX, startY);
            startY += rowHeight;

            // رسم خلفية مميزة لاسم الموظف وID الموظف
            Rectangle employeeInfoBackground = new Rectangle(startX, startY, tableWidth, rowHeight * 2); // خلفية تغطي اسم الموظف والID
            g.FillRectangle(Brushes.LightCoral, employeeInfoBackground); // لون خلفية الموظف (يمكن تغييره)

            // طباعة اسم الموظف من الـ Label
            g.DrawString($" {EmployeeLabel.Text}", employeeFont, rowBrush, startX, startY);
            startY += rowHeight;
            // طباعة الـ ID من EmployeeIdLabel
            g.DrawString($" {EmployeeIdLabel.Text}", smallFont, rowBrush, startX, startY);
            startY += rowHeight;

            // طباعة تاريخ الفاتورة
            g.DrawString($" {TimingLabel.Text}", rowFont, rowBrush, startX, startY);
            startY += rowHeight;

            // رسم عناوين الجدول
            Rectangle headerRect = new Rectangle(startX, startY, tableWidth, rowHeight);
            g.FillRectangle(Brushes.DarkSlateBlue, headerRect);
            g.DrawRectangle(tablePen, headerRect);
            StringFormat columnFormat = new StringFormat { LineAlignment = StringAlignment.Center };
            g.DrawString("Med ID", rowFont, Brushes.White, new RectangleF(startX, startY, tableWidth / 5, rowHeight), columnFormat);
            g.DrawString("Med Name", rowFont, Brushes.White, new RectangleF(startX + tableWidth / 5, startY, tableWidth / 5, rowHeight), columnFormat);
            g.DrawString("Seller price (1)Unit", rowFont, Brushes.White, new RectangleF(startX + 2 * tableWidth / 5, startY, tableWidth / 5, rowHeight), columnFormat);
            g.DrawString("Quantity", rowFont, Brushes.White, new RectangleF(startX + 3 * tableWidth / 5, startY, tableWidth / 5, rowHeight), columnFormat);
            g.DrawString("Total Amount", rowFont, Brushes.White, new RectangleF(startX + 4 * tableWidth / 5, startY, tableWidth / 5, rowHeight), columnFormat);
            startY += rowHeight;

            // رسم بيانات الجدول
            foreach (DataGridViewRow row in BillingGV.Rows)
            {
                if (row.IsNewRow) continue;

                Rectangle rowRect = new Rectangle(startX, startY, tableWidth, rowHeight);
                g.FillRectangle(Brushes.White, rowRect);
                g.DrawRectangle(tablePen, rowRect);

                g.DrawString(row.Cells["BillId"].Value.ToString(), rowFont, rowBrush, new RectangleF(startX, startY, tableWidth / 5, rowHeight), columnFormat);
                g.DrawString(row.Cells["MedName"].Value.ToString(), rowFont, rowBrush, new RectangleF(startX + tableWidth / 5, startY, tableWidth / 5, rowHeight), columnFormat);
                g.DrawString(row.Cells["Sprice"].Value.ToString(), rowFont, rowBrush, new RectangleF(startX + 2 * tableWidth / 5, startY, tableWidth / 5, rowHeight), columnFormat);
                g.DrawString(row.Cells["Quantity"].Value.ToString(), rowFont, rowBrush, new RectangleF(startX + 3 * tableWidth / 5, startY, tableWidth / 5, rowHeight), columnFormat);
                g.DrawString(row.Cells["TotalAmount"].Value.ToString(), rowFont, rowBrush, new RectangleF(startX + 4 * tableWidth / 5, startY, tableWidth / 5, rowHeight), columnFormat);

                startY += rowHeight;
            }


            // قراءة السعر قبل الخصم (المجموع الكلي)
            int totalAmount = 0;
            foreach (DataGridViewRow row in BillingGV.Rows)
            {
                if (row.IsNewRow) continue;
                totalAmount += Convert.ToInt32(row.Cells["TotalAmount"].Value);
            }

            // طباعة السعر قبل الخصم
            Rectangle priceBeforeDiscountRect = new Rectangle(startX, startY, tableWidth, rowHeight);
            g.FillRectangle(priceBeforeDiscountBackgroundBrush, priceBeforeDiscountRect); // تعبئة الخلفية
            g.DrawRectangle(tablePen, priceBeforeDiscountRect);
            g.DrawString($"Price B.D: {totalAmount} EGP", priceFont, priceTextBrush, priceBeforeDiscountRect, new StringFormat
            {
                Alignment = StringAlignment.Center, // النص في المنتصف
                LineAlignment = StringAlignment.Center
            });
            startY += rowHeight;

            // قراءة النص الموجود في PriceADLabel
            string priceAfterDiscountText = PriceADLabel.Text.Trim(); // النص الكامل الموجود في الليبل

            // التحقق إذا كان النص يساوي "Price A.D: 0 EGP"
            if (priceAfterDiscountText == "Price A.D: 0 EGP" || priceAfterDiscountText == "Price A.D")
            {
                // إذا لم يكن هناك خصم
                Rectangle noDiscountRect = new Rectangle(startX, startY, tableWidth, rowHeight);
                g.FillRectangle(priceAfterDiscountBackgroundBrush, noDiscountRect); // تعبئة الخلفية
                g.DrawRectangle(tablePen, noDiscountRect);
                g.DrawString("Unfortunately, no discount. We wish you a happy day!", priceFont, Brushes.White, noDiscountRect, new StringFormat
                {
                    Alignment = StringAlignment.Center, // النص في المنتصف
                    LineAlignment = StringAlignment.Center // النص في المنتصف عموديًا
                });
            }
            else
            {
                // إذا كان هناك خصم
                Rectangle priceAfterDiscountRect = new Rectangle(startX, startY, tableWidth, rowHeight);
                g.FillRectangle(priceAfterDiscountBackgroundBrush, priceAfterDiscountRect); // تعبئة الخلفية
                g.DrawRectangle(tablePen, priceAfterDiscountRect);
                g.DrawString(priceAfterDiscountText, priceFont, priceTextBrush, priceAfterDiscountRect, new StringFormat
                {
                    Alignment = StringAlignment.Center, // النص في المنتصف
                    LineAlignment = StringAlignment.Center // النص في المنتصف عموديًا
                });
            }


            startY += rowHeight + 50; // إضافة مسافة بين الجدول والعبارة الأخيرة


            // طباعة رسالة شكر
            g.DrawString("Thank you for choosing Al Mohandes Pharmacy, Your health is our top priority.", new Font("Arial", 12, FontStyle.Italic), Brushes.Black, startX, startY);
        }






      


        private void ExpDate_ValueChanged(object sender, EventArgs e)
        {

        }



        private void LoadCustomerData(string customerName)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                // استعلام للحصول على بيانات الزبون بناءً على الاسم
                string query = "SELECT CustomerPhone, CustomerAddress FROM CustomerTab WHERE CustomerName = @CustomerName";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CustomerName", customerName);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // تحديث البيانات في الـ Labels
                            CustomerPhoneLabel.Text = $"Phone: {reader["CustomerPhone"].ToString()}";
                            CustomerAddressLabel.Text = $"Address: {reader["CustomerAddress"].ToString()}";
                        }
                        else
                        {
                            // في حالة عدم العثور على الزبون
                            CustomerPhoneLabel.Text = "Phone: Not found";
                            CustomerAddressLabel.Text = "Address: Not found";
                        }
                    }
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

        private void gunaLineTextBox3_TextChanged(object sender, EventArgs e)
        {
            // لا تقم بتحديث الـ Label هنا، فقط احتفظ بالقيمة في المتغيرات.
        }

        // Customer Phone
        private void gunaLineTextBox2_TextChanged(object sender, EventArgs e)
        {
            // لا تقم بتحديث الـ Label هنا، فقط احتفظ بالقيمة في المتغيرات.
        }

        // Customer Address
        private void gunaLineTextBox1_TextChanged(object sender, EventArgs e)
        {
            // لا تقم بتحديث الـ Label هنا، فقط احتفظ بالقيمة في المتغيرات.
        }

        // Date with details
        private void ExpDate_ValueChanged_1(object sender, EventArgs e)
        {
            // لا تقم بتحديث الـ Label هنا، فقط احتفظ بالقيمة في المتغيرات.
        }






        // زر إضافة معلومات العميل
        private void CustomerInfo_Click(object sender, EventArgs e)
        {
            try
            {
                // التحقق من أن جميع الحقول تم تعبئتها (البيانات الأساسية للعميل)
                if (string.IsNullOrWhiteSpace(gunaLineTextBox3.Text) || // Customer Name
                    string.IsNullOrWhiteSpace(gunaLineTextBox2.Text) || // Customer Phone
                    string.IsNullOrWhiteSpace(gunaLineTextBox1.Text) || // Customer Address
                    SelectEmployee.SelectedItem == null) // Employee
                {
                    MessageBox.Show("Please fill all customer fields before adding.");
                    return;
                }

                // إضافة بيانات العميل في الـ Labels بعد الضغط على الزر
                CustomerNameLabel.Text = $"Customer Name: {gunaLineTextBox3.Text}";
                CustomerPhoneLabel.Text = $"Phone: {gunaLineTextBox2.Text}";
                CustomerAddressLabel.Text = $"Address: {gunaLineTextBox1.Text}";
                TimingLabel.Text = $"Date: {ExpDate.Value.ToString("dddd, dd MMMM yyyy hh:mm tt")}";

                // توليد Customer ID باستخدام GUID واستخراج أول 8 خانات
                string generatedCustomerId = Guid.NewGuid().ToString("N").Substring(0, 8);
                // عرض الرقم في CustomerIdLabel
                CustomerIdLabel.Text = $"Customer ID: {generatedCustomerId}";
                // تغيير اللون أو أي تعديلات أخرى إذا لزم الأمر
                CustomerIdLabel.ForeColor = Color.Black;
                CustomerIdLabel.BackColor = Color.FromArgb(255, 128, 0);

                // تغيير الألوان
                CustomerNameLabel.ForeColor = Color.Black;
                CustomerPhoneLabel.ForeColor = Color.Black;
                CustomerAddressLabel.ForeColor = Color.Black;
                TimingLabel.ForeColor = Color.Black;

                CustomerNameLabel.BackColor = Color.FromArgb(192, 64, 0);
                CustomerPhoneLabel.BackColor = Color.FromArgb(192, 64, 0);
                CustomerAddressLabel.BackColor = Color.FromArgb(192, 64, 0);
                TimingLabel.BackColor = Color.FromArgb(192, 64, 0);

                // عرض اسم الموظف و ID الموظف بعد إضافة بيانات العميل
                if (SelectEmployee.SelectedItem != null)
                {
                    ComboBoxItem selectedItem = (ComboBoxItem)SelectEmployee.SelectedItem;
                    EmployeeLabel.Text = $"Employee Name: {selectedItem.Text}";

                    // عرض EmployeeId الخاص بالموظف
                    EmployeeIdLabel.Text = $"Employee ID: {selectedItem.Value}";
                }
                else
                {
                    EmployeeLabel.Text = "Employee not selected.";
                    EmployeeIdLabel.Text = string.Empty; // تفريغ الـ Label الخاص بالـ ID
                }

                // **إفراغ الـ TextBox بعد إضافة معلومات العميل**
                gunaLineTextBox3.Clear();  // مسح اسم العميل
                gunaLineTextBox2.Clear();  // مسح رقم التليفون
                gunaLineTextBox1.Clear();  // مسح العنوان
                SelectEmployee.SelectedIndex = -1; // مسح اختيار الموظف
                SelectEmployee.Text = "Select Employee"; // إعادة النص إلى "Select Employee"

                // استخراج البيانات من النصوص
                string customerIdText = CustomerIdLabel.Text;   // النص الخاص بـ ID
                string customerNameText = CustomerNameLabel.Text;   // النص الخاص بـ اسم الزبون
                string customerPhoneText = CustomerPhoneLabel.Text;  // النص الخاص برقم الهاتف
                string priceAdText = PriceADLabel.Text;         // النص الخاص بالسعر النهائي
                string EmployeeNameText = EmployeeLabel.Text;      // اسم الموظف
                DateTime invoiceDate = DateTime.Now;           // تاريخ إصدار الفاتورة

                // استخراج القيم فقط من النصوص
                string customerId = ExtractValue(customerIdText);    // "5b9d9828"
                string customerName = ExtractValue(customerNameText); // "Omar Hussein abdallah"
                string employeeName = ExtractEmployeeName(EmployeeNameText);  // اسم الموظف
                string customerPhone = ExtractCustomerPhone(customerPhoneText);  // رقم الهاتف
                decimal totalAmount;

                // محاولة تحويل النص الخاص بالسعر النهائي إلى عدد عشري
                if (decimal.TryParse(ExtractNumericValue(priceAdText), out totalAmount))
                {
                    // حفظ البيانات في قاعدة البيانات
                    SaveDetailsToDatabase(customerId, customerName, customerPhone, (int)totalAmount, employeeName, invoiceDate);

                    // عرض رسالة تأكيد
                    MessageBox.Show($"Customer Details Saved:\nID: {customerId}\nName: {customerName}\nPhone: {customerPhone}\nTotal Amount: {totalAmount} EGY");
                }
                else
                {
                    // إذا كانت القيمة غير صحيحة
                    MessageBox.Show("Invalid total amount.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding customer info: {ex.Message}");
            }
        }





        private void EmployeeeId_Click(object sender, EventArgs e)
        {
            if (SelectEmployee.SelectedItem is ComboBoxItem selectedItem)
            {
                // استرجاع الـ EmpId الخاص بالاسم المحدد
                string empId = GetEmployeeId(selectedItem.Text);

                if (!string.IsNullOrEmpty(empId))
                {
                    // عرض الـ EmpId في الـ Label
                    EmployeeIdLabel.Text = $"Employee ID: {empId}";
                }
            }
            else
            {
                MessageBox.Show("Please select an employee.");
            }
        }



        


        // دالة للتحقق من وجود الدواء في الفاتورة (جدول الفاتورة)
        private bool CheckMedicineExistsInBill(string medicineName)
        {
            string query = "SELECT COUNT(*) FROM BillsTb1 WHERE MedName = @MedName";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@MedName", medicineName);
                con.Open();
                int count = (int)cmd.ExecuteScalar();
                con.Close();
                return count > 0; // إذا كان الدواء موجودًا في الفاتورة
            }
        }

        
        



        // دالة لتحديث المجموع الإجمالي
        private void UpdateTotalAmount()
        {
            try
            {
                int totalAmount = 0;

                // التكرار عبر جميع الصفوف في DataGridView لحساب المجموع
                foreach (DataGridViewRow row in BillingGV.Rows)
                {
                    if (row.Cells["TotalAmount"].Value != null)
                    {
                        totalAmount += Convert.ToInt32(row.Cells["TotalAmount"].Value);
                    }
                }

                // عرض المجموع في PriceBDLabel
                PriceBDLabel.Text = $"Price B.D: {totalAmount} EGP"; // عرض المجموع الإجمالي
                PriceBDLabel.ForeColor = Color.Black; // التأكد من أن النص يظهر بشكل جيد

                // تحديث الخصم بعد تحديث المجموع الإجمالي
                UpdateDiscount(totalAmount);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error calculating total amount: {ex.Message}");
            }
        }

        private void UpdateDiscount(int totalAmount)
        {
            try
            {
                // التحقق من أن حقل الخصم يحتوي على قيمة صحيحة
                if (decimal.TryParse(Discounttext.Text, out decimal discountPercentage) && discountPercentage > 0)
                {
                    // حساب الخصم
                    decimal discountAmount = (discountPercentage / 100) * totalAmount;
                    decimal amountAfterDiscount = totalAmount - discountAmount;

                    // عرض المجموع بعد الخصم
                    PriceADLabel.Text = $"Price A.D: {amountAfterDiscount} EGP"; // عرض المجموع بعد الخصم
                    PriceADLabel.ForeColor = Color.Black; // التأكد من أن النص يظهر بشكل جيد
                }
                else
                {
                    // إذا لم يكن هناك خصم أو الخصم غير صالح
                    PriceADLabel.Text = $"Price A.D: {totalAmount} EGP"; // عرض المجموع الإجمالي بدون خصم
                    PriceADLabel.ForeColor = Color.Black;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating discount: {ex.Message}");
            }
        }


        private void Discountbutton_Click(object sender, EventArgs e)
        {
            try
            {
                // التحقق من أن قيمة الخصم المدخلة صحيحة
                if (decimal.TryParse(Discounttext.Text, out decimal discountPercentage) && discountPercentage > 0)
                {
                    // حساب المجموع الإجمالي للفاتورة
                    int totalAmount = 0;

                    // التكرار عبر جميع الصفوف في DataGridView لحساب المجموع
                    foreach (DataGridViewRow row in BillingGV.Rows)
                    {
                        if (row.Cells["TotalAmount"].Value != null)
                        {
                            totalAmount += Convert.ToInt32(row.Cells["TotalAmount"].Value);
                        }
                    }

                    // إذا كان المجموع الإجمالي أكبر من 0
                    if (totalAmount > 0)
                    {
                        // حساب قيمة الخصم بناءً على النسبة المدخلة
                        decimal discountAmount = (discountPercentage / 100) * totalAmount;
                        decimal amountAfterDiscount = totalAmount - discountAmount;

                        // عرض المجموع بعد الخصم
                        PriceADLabel.Text = $"Price A.D: {amountAfterDiscount} EGP"; // عرض المجموع بعد الخصم
                        PriceADLabel.ForeColor = Color.Black; // التأكد من أن النص يظهر بشكل جيد
                    }
                    else
                    {
                        // إذا كان المجموع الإجمالي يساوي 0، عرض رسالة
                        MessageBox.Show("Total amount cannot be zero.");
                    }

                    // مسح محتويات حقل الخصم بعد الحساب
                    Discounttext.Clear();
                }
                else
                {
                    // إذا كانت قيمة الخصم المدخلة غير صالحة
                    MessageBox.Show("Please enter a valid discount percentage.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error applying discount: {ex.Message}");
            }
        }









        private void clearbutton_Click(object sender, EventArgs e)
        {
            // عرض رسالة تأكيد للمستخدم قبل مسح البيانات
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to clear the bill and restore the quantities to the stock?",
                                                        "Clear Bill Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    // استرجاع الكميات المخصومة من جدول الفاتورة
                    RollbackTransaction();  // استدعاء الدالة الخاصة بإرجاع الكميات

                    // مسح البيانات من الجدول في الواجهة
                    //BillingGV.Rows.Clear();

                    // مسح بيانات الفاتورة في قاعدة البيانات
                    ClearPreviousBillData();

                    // مسح النصوص والبيانات المعروضة على الواجهة
                    ResetForm();

                    // إعادة تخصيص الجدول بناءً على وجود أو عدم وجود بيانات
                    CustomizeDataGridView();

                    MessageBox.Show("Bill cleared and quantities restored.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while clearing the grid or rolling back transaction: " + ex.Message);
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
            else
            {
                // لو المستخدم اختار عدم مسح البيانات
                MessageBox.Show("Data clearing operation was canceled.");
            }
        }

        private void RollbackTransaction()
        {
            try
            {
                if (tempTransaction.Count > 0) // إذا كانت هناك معاملات تم خصم الكمية منها
                {
                    foreach (var item in tempTransaction)
                    {
                        // استعلام لإرجاع الكمية المسحوبة إلى المخزن
                        string updateQuery = "UPDATE MedicineTb1 SET MedQty = MedQty + @Quantity WHERE MedName = @MedName";
                        using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@Quantity", item.Quantity); // إضافة الكمية المسحوبة
                            cmd.Parameters.AddWithValue("@MedName", item.Medicine); // اسم الدواء الذي تم إضافته
                            con.Open();
                            cmd.ExecuteNonQuery(); // تنفيذ الاستعلام لإرجاع الكمية إلى المخزن
                            con.Close();
                        }
                    }

                    // تفريغ قائمة المعاملات بعد التراجع
                    //tempTransaction.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error restoring data: {ex.Message}");
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void ClearPreviousBillData()
        {
            // حذف جميع بيانات الفاتورة القديمة من جدول الفاتورات
            string deleteQuery = "DELETE FROM BillsTb1";
            using (SqlCommand cmd = new SqlCommand(deleteQuery, con))
            {
                con.Open();
                cmd.ExecuteNonQuery();  // تنفيذ الاستعلام لحذف الفاتورة
                con.Close();
            }
        }

        private void ResetForm()
        {
            // مسح جميع الحقول والبيانات المعروضة في الواجهة
            PriceBDLabel.Text = $"Price B.D: 0 EGP";
            PriceADLabel.Text = $"Price A.D: 0 EGP";
            Discounttext.Clear();
            tb1.Clear();
            CustomerNameLabel.Text = $"Customer Name:";
            CustomerPhoneLabel.Text = $"Customer Phone:";
            CustomerAddressLabel.Text = $"Customer address:";
            EmployeeLabel.Text = $"Employee Name:";
            EmployeeIdLabel.Text = $"Employee Id:";
            CustomerIdLabel.Text = $"Customer Id:";
            TimingLabel.Text = $"Timing:";
            gunaLineTextBox3.Clear();
            gunaLineTextBox2.Clear();
            gunaLineTextBox1.Clear();
            SelectEmployee.Text = "Select Employee";
            MediComboCb.Text = "Select Medicine";
            label4.Text = "AvailableStock";
        }

        private void BillingGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // تأكد من أن النقر تم على خلايا البيانات وليس على الأعمدة (مثل الأزرار أو رؤوس الأعمدة)
            if (e.RowIndex >= 0) // إذا كان السطر المحدد هو صف بيانات حقيقي
            {
                // الحصول على البيانات من الصف المحدد
                string medName = BillingGV.Rows[e.RowIndex].Cells["MedName"].Value.ToString(); // اسم الدواء
                int quantity = Convert.ToInt32(BillingGV.Rows[e.RowIndex].Cells["Quantity"].Value); // الكمية

                // عرض رسالة تأكيد للمستخدم قبل حذف الصف
                DialogResult dialogResult = MessageBox.Show($"Are you sure you want to remove {quantity} of {medName} from the bill and restore it to stock?",
                                                            "Remove Item Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        // استرجاع الكمية إلى قاعدة البيانات
                        string updateQuery = "UPDATE MedicineTb1 SET MedQty = MedQty + @Quantity WHERE MedName = @MedName";
                        using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@Quantity", quantity); // الكمية التي تم حذفها
                            cmd.Parameters.AddWithValue("@MedName", medName); // اسم الدواء

                            con.Open();
                            cmd.ExecuteNonQuery(); // تنفيذ الاستعلام لتحديث الكمية في قاعدة البيانات
                            con.Close();
                        }

                        // حذف الصف من DataGridView
                        BillingGV.Rows.RemoveAt(e.RowIndex);

                        // إعادة تخصيص DataGridView بعد حذف الصف
                        CustomizeDataGridView();

                        MessageBox.Show($"{medName} has been removed from the bill and restored to stock.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error occurred while restoring quantity: {ex.Message}");
                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                        {
                            con.Close();
                        }
                    }
                }
                else
                {
                    // إذا اختار المستخدم عدم الحذف
                    MessageBox.Show("Item removal canceled.");
                }
            }
        }

        private void CustomizeDataGridView()
        {
            // لون خلفية الـ Header
            BillingGV.EnableHeadersVisualStyles = false;
            BillingGV.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSlateBlue;
            BillingGV.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            BillingGV.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);

            // لون الصفوف العادية
            BillingGV.DefaultCellStyle.BackColor = Color.White;
            BillingGV.DefaultCellStyle.ForeColor = Color.Black;

            // لون التحديد للصفوف والأعمدة
            BillingGV.DefaultCellStyle.SelectionBackColor = Color.DarkGreen; // خلفية الصف المحدد
            BillingGV.DefaultCellStyle.SelectionForeColor = Color.White; // لون النص في الصف المحدد
        }








        // دالة لحفظ بيانات العميل في قاعدة البيانات
        private void SaveDetailsToDatabase(string customerId, string customerName, string phoneNumber, int totalAmount, string employeeName, DateTime invoiceDate)
        {
            try
            {
                // فتح الاتصال بقاعدة البيانات
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                // استعلام للتحقق إذا كان العميل موجود مسبقًا في قاعدة البيانات
                string checkQuery = "SELECT COUNT(*) FROM CustomersBillsTb WHERE CustomerId = @CustomerId";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                {
                    checkCmd.Parameters.AddWithValue("@CustomerId", customerId);
                    int exists = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (exists > 0)
                    {
                        // إذا كان العميل موجودًا بالفعل، تحديث الإجمالي
                        string updateQuery = "UPDATE CustomersBillsTb SET TotalSpent = TotalSpent + @TotalAmount, EmployeeName = @EmployeeName, InvoiceDate = @InvoiceDate, PhoneNumber = @PhoneNumber WHERE CustomerId = @CustomerId";
                        using (SqlCommand updateCmd = new SqlCommand(updateQuery, con))
                        {
                            updateCmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                            updateCmd.Parameters.AddWithValue("@CustomerId", customerId);
                            updateCmd.Parameters.AddWithValue("@EmployeeName", employeeName);
                            updateCmd.Parameters.AddWithValue("@InvoiceDate", invoiceDate);
                            updateCmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // إذا لم يكن العميل موجودًا، إضافة عميل جديد
                        string insertQuery = "INSERT INTO CustomersBillsTb (CustomerId, CustomerName, PhoneNumber, TotalSpent, EmployeeName, InvoiceDate) VALUES (@CustomerId, @CustomerName, @PhoneNumber, @TotalAmount, @EmployeeName, @InvoiceDate)";
                        using (SqlCommand insertCmd = new SqlCommand(insertQuery, con))
                        {
                            insertCmd.Parameters.AddWithValue("@CustomerId", customerId);
                            insertCmd.Parameters.AddWithValue("@CustomerName", customerName);
                            insertCmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                            insertCmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                            insertCmd.Parameters.AddWithValue("@EmployeeName", employeeName);
                            insertCmd.Parameters.AddWithValue("@InvoiceDate", invoiceDate);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }

                // عرض رسالة تأكيد بعد حفظ البيانات
                MessageBox.Show("Invoice details saved to the database.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving to database: {ex.Message}");
            }
            finally
            {
                // التأكد من غلق الاتصال
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }





        //private void SaveDetailsCustomer_Click_1(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        // استخراج البيانات من النصوص
        //        string customerIdText = CustomerIdLabel.Text;   // النص الخاص بـ ID
        //        string customerNameText = CustomerNameLabel.Text;   // النص الخاص بـ اسم الزبون
        //        string customerPhoneText = CustomerPhoneLabel.Text;  // النص الخاص برقم الهاتف
        //        string priceAdText = PriceADLabel.Text;         // النص الخاص بالسعر النهائي
        //        string employeeName = EmployeeLabel.Text;      // اسم الموظف
        //        DateTime invoiceDate = DateTime.Now;           // تاريخ إصدار الفاتورة

        //        // استخراج القيم فقط من النصوص
        //        string customerId = ExtractValue(customerIdText);    // "5b9d9828"
        //        string customerName = ExtractValue(customerNameText); // "Omar Hussein abdallah"
        //        string customerPhone = ExtractCustomerPhone(customerPhoneText);  // رقم الهاتف
        //        decimal totalAmount;

        //        // محاولة تحويل النص الخاص بالسعر النهائي إلى عدد عشري
        //        if (decimal.TryParse(ExtractNumericValue(priceAdText), out totalAmount))
        //        {
        //            // حفظ البيانات في قاعدة البيانات
        //            SaveDetailsToDatabase(customerId, customerName, customerPhone, (int)totalAmount, employeeName, invoiceDate);

        //            // عرض رسالة تأكيد
        //            MessageBox.Show($"Customer Details Saved:\nID: {customerId}\nName: {customerName}\nPhone: {customerPhone}\nTotal Amount: {totalAmount} EGY");
        //        }
        //        else
        //        {
        //            // إذا كانت القيمة غير صحيحة
        //            MessageBox.Show("Invalid total amount.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error saving customer details: {ex.Message}");
        //    }
        ////}




        // دالة لاستخراج القيمة بعد العلامة ":"
        private string ExtractValue(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            // تقسيم النص بناءً على ":"
            string[] parts = input.Split(':');
            if (parts.Length > 1)
            {
                return parts[1].Trim(); // القيمة بعد ":"
            }

            return input; // إذا لم يكن النص يحتوي على ":"
        }

        // دالة لاستخراج القيم الرقمية فقط (مثل "810.0" من "Price A.D: 810.0 EGP")
        private string ExtractNumericValue(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            // استخدام Regex لاستخراج الأرقام فقط (بما في ذلك الأرقام العشرية)
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\d+(\.\d+)?");
            var match = regex.Match(input);
            if (match.Success)
            {
                return match.Value; // النص الرقمي فقط
            }

            return string.Empty; // إذا لم يتم العثور على أي رقم
        }

        // دالة لاستخراج اسم الموظف بعد "Employee Name:"

        private string ExtractEmployeeName(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            // تقسيم النص بناءً على "Employee Name:" فقط
            string[] parts = input.Split(':');
            if (parts.Length > 1)
            {
                return parts[1].Trim(); // القيمة بعد ":" ستكون اسم الموظف
            }

            return input; // إذا لم يحتوي النص على "Employee Name:"، يتم إرجاع النص كما هو
        }

        // دالة لاستخراج رقم الهاتف بعد "Customer Phone:"
        private string ExtractCustomerPhone(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            // تقسيم النص بناءً على "Customer Phone:"
            string[] parts = input.Split(':');
            if (parts.Length > 1)
            {
                return parts[1].Trim(); // القيمة بعد ":"
            }

            return input; // إذا لم يكن النص يحتوي على ":"
        }



        private void SaveInvoice()
        {
            // التأكد من أن النص يحتوي على قيمة قابلة للتحويل إلى عدد صحيح
            string customerName = CustomerNameLabel.Text; // اسم العميل
            string employeeNameText = EmployeeLabel.Text; // النص الخاص بـ Employee Name
            string employeeName = ExtractEmployeeName(employeeNameText); // استخراج اسم الموظف
            string customerPhoneText = CustomerPhoneLabel.Text; // النص الخاص بـ Customer Phone
            string customerPhone = ExtractCustomerPhone(customerPhoneText); // استخراج رقم الهاتف
            int invoiceTotal;

            // محاولة تحويل النص إلى عدد صحيح
            if (int.TryParse(PriceADLabel.Text, out invoiceTotal))
            {
                // حفظ بيانات العميل والفاتورة مع اسم الموظف وتاريخ الفاتورة ورقم الهاتف
                SaveDetailsToDatabase(customerName, customerName, customerPhone, invoiceTotal, employeeName, DateTime.Now);

                // رسالة تأكيد
                MessageBox.Show($"Invoice saved for {customerName} with total {invoiceTotal} EGY");
            }
            else
            {
                // إذا كانت القيمة غير قابلة للتحويل إلى عدد صحيح
                MessageBox.Show("Invalid total amount.");
            }
        }





    }
}

