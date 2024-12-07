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

        private bool isPrinted = false;

        private void button1_Click(object sender, EventArgs e)
        {
            // فحص إذا كانت المعاملة لم تُطبع بعد
            if (tempTransaction.Count > 0 && !isPrinted)
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
                    return;
                }
            }

            this.Hide();
            Home back = new Home();
            back.ShowDialog();
        }




        // الاتصال بقاعدة البيانات
        public SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\PROJECT_OOP\FO_ORGANIZATION\PHARMACY-MANAGEMENT\PROJECT_FO_3\PHARMACYOB.MDF;Integrated Security=True;");

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




        // عند الضغط على زر Add لإضافة الفاتورة
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

        private void button4_Click_1(object sender, EventArgs e)
        {
            try
            {
                // التحقق من اختيار دواء
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
                string billId = GenerateUniqueBillId(); // توليد ID عشوائي للفاتورة
                int sellingPrice = GetSellingPrice(selectedMedicine); // جلب سعر البيع

                if (sellingPrice <= 0)
                {
                    MessageBox.Show("Failed to retrieve the selling price for the selected medicine.");
                    return;
                }

                int totalAmount = sellingPrice * quantity; // حساب المبلغ الإجمالي

                // إضافة صف جديد إلى DataGridView
                BillingGV.Rows.Add(billId, selectedMedicine, quantity, totalAmount);

                // تحديث الكمية المتاحة في قاعدة البيانات
                string updateQuery = "UPDATE MedicineTb1 SET MedQty = MedQty - @Quantity WHERE MedName = @MedName";
                using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                {
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@MedName", selectedMedicine);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                // حفظ البيانات في قاعدة البيانات
                SaveToDatabase(billId, selectedMedicine, quantity, totalAmount);

                // حساب المجموع الكلي بعد إضافة الفاتورة
                CalculateTotalAmount();

                // تنظيف الحقول بعد الإضافة
                MediComboCb.SelectedIndex = -1;
                MediComboCb.Text = "Select Medicine";
                tb1.Clear();
                label4.Text = "Available Stock"; // إعادة النص الأصلي بعد إضافة الفاتورة
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding item: {ex.Message}");
            }
        }

        // حساب المجموع الكلي لجميع الفواتير في الجدول
        private void CalculateTotalAmount()
        {
            try
            {
                int totalAmount = 0;

                foreach (DataGridViewRow row in BillingGV.Rows)
                {
                    if (row.Cells["TotalAmount"].Value != null)
                    {
                        totalAmount += Convert.ToInt32(row.Cells["TotalAmount"].Value);
                    }
                }

                label5.Text = $"Total Amount: {totalAmount}"; // عرض المجموع في label5
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error calculating total amount: {ex.Message}");
            }
        }


        private void SaveToDatabase(string billId, string medName, int quantity, int totalAmount)
        {
            try
            {
                // استعلام الحفظ في قاعدة البيانات
                string query = "INSERT INTO BillsTb1 (BillId, MedName, Quantity, TotalAmount) VALUES (@BillId, @MedName, @Quantity, @TotalAmount)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@BillId", billId);
                    cmd.Parameters.AddWithValue("@MedName", medName);
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





        // توليد كود الفاتورة (مثال بسيط)
        private string GenerateUniqueBillId()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 8);
        }



        private void Billing_Load(object sender, EventArgs e)
        {
            try
            {

                InitializeBillingGrid(); // تأكد من أنك قمت بإعداد الـ DataGridView بشكل صحيح.

                // ملء ComboBox بالأدوية
                LoadMedicineNames();

                // ملء DataGridView بالبيانات من جدول BillsTb1
                PopulateGrid(BillingGV);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
            CustomizeDataGridView();
        }

        private void PopulateGrid(DataGridView BillingGV)
        {
            try
            {
                // مسح البيانات المعروضة في الجدول فقط
                BillingGV.Rows.Clear();
                // حساب المجموع الكلي بعد تحميل البيانات
                UpdateTotalAmount();


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
            BillingGV.Columns.Add("BillId", "Bill ID");
            BillingGV.Columns.Add("MedName", "Medicine Name");
            BillingGV.Columns.Add("Quantity", "Quantity");
            BillingGV.Columns.Add("TotalAmount", "Total Amount");

            // تغيير الخط في رأس الجدول
            BillingGV.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular);

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
        private void RollbackTransaction()
        {
            try
            {
                if (tempTransaction.Count > 0)
                {
                    foreach (var item in tempTransaction)
                    {
                        // استرجاع الكمية إلى المخزون (بإضافة الكميات مرة أخرى)
                        string updateQuery = "UPDATE MedicineTb1 SET MedQty = MedQty + @Quantity WHERE MedName = @MedName";
                        using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                            cmd.Parameters.AddWithValue("@MedName", item.Medicine);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }

                    // بعد التراجع، نعود إلى الحالة الأصلية
                    tempTransaction.Clear();
                    PopulateGrid(BillingGV);  // إعادة ملء الجدول بالبيانات الأصلية
                }
                else
                {
                    MessageBox.Show("No transaction to rollback.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error restoring data: {ex.Message}");
            }
        }

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

                isPrinted = true;
                // استرجاع الكميات إلى المخزون إذا كانت المعاملة لم تُطبع
                // تم مسح البيانات المؤقتة
                tempTransaction.Clear();
            }
            else
            {


 
                RollbackTransaction();

               
            }
        }








        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // فحص إذا كانت المعاملة لم تُطبع بعد
            if (tempTransaction.Count > 0 )
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


        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;

            // استخدام أبعاد الورقة للطباعة
            int startX = e.MarginBounds.Left;
            int startY = e.MarginBounds.Top + 0; // المسافة بين العنوان وأعلى الصفحة تكون 10 نقاط
            int tableWidth = e.MarginBounds.Width; // عرض الجدول بناءً على حجم الورقة
            int rowHeight = 30; // ارتفاع كل صف
            int columnWidth = tableWidth / 4; // تقسيم الأعمدة بالتساوي

            Font headerFont = new Font("Microsoft Sans Serif", 12, FontStyle.Bold);
            Font columnFont = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
            Font rowFont = new Font("Arial", 10);
            Font totalFont = new Font("Arial", 10, FontStyle.Bold);

            // تحديد الألوان حسب التخصيصات في DataGridView
            Brush headerBrush = Brushes.White; // نص الرأس باللون الأبيض
            Brush rowBrush = Brushes.Black; // النص في الصفوف العادية باللون الأسود
            Brush invoiceBackgroundBrush = new SolidBrush(Color.FromArgb(192, 64, 0)); // خلفية العنوان
            Brush invoiceTextBrush = Brushes.White; // نص العنوان باللون الأبيض
            Brush totalBackgroundBrush = Brushes.LightGray; // خلفية المجموع الكلي
            Brush totalTextBrush = Brushes.Black; // نص المجموع الكلي باللون الأسود
            Brush rowBackgroundBrush = Brushes.White; // خلفية الصفوف العادية
            Brush selectedRowBrush = Brushes.DarkGreen; // خلفية الصف المحدد باللون الأخضر الداكن
            Pen tablePen = new Pen(Color.Black); // تحديد اللون الأسود للحدود

            // ** رسم عنوان التقرير (Pharmacy Invoice) **
            Rectangle titleBackground = new Rectangle(startX, startY, tableWidth, rowHeight);
            g.FillRectangle(invoiceBackgroundBrush, titleBackground);
            StringFormat titleFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            g.DrawString("Pharmacy Invoice", headerFont, invoiceTextBrush, titleBackground, titleFormat);
            startY += rowHeight + 100; // المسافة بين العنوان والجدول تكون 100 نقطة

            // ** رسم العناوين (الرأس) **
            Rectangle headerRect = new Rectangle(startX, startY, tableWidth, rowHeight);
            g.FillRectangle(Brushes.DarkSlateBlue, headerRect); // خلفية الرأس باللون الأزرق الداكن
            g.DrawRectangle(tablePen, headerRect);
            StringFormat columnFormat = new StringFormat { LineAlignment = StringAlignment.Center };
            g.DrawString("Bill ID", columnFont, headerBrush, new RectangleF(startX, startY, columnWidth, rowHeight), columnFormat);
            g.DrawString("Medicine", columnFont, headerBrush, new RectangleF(startX + columnWidth, startY, columnWidth, rowHeight), columnFormat);
            g.DrawString("Quantity", columnFont, headerBrush, new RectangleF(startX + 2 * columnWidth, startY, columnWidth, rowHeight), columnFormat);
            g.DrawString("Total Amount", columnFont, headerBrush, new RectangleF(startX + 3 * columnWidth, startY, columnWidth, rowHeight), columnFormat);
            startY += rowHeight; // مباشرة بعد رأس الجدول بدون مسافة إضافية

            // ** رسم البيانات (الصفوف) **
            foreach (DataGridViewRow row in BillingGV.Rows)
            {
                if (row.IsNewRow) continue;

                bool isSelected = row.Selected;
                Brush currentRowBackgroundBrush = isSelected ? selectedRowBrush : rowBackgroundBrush;
                Brush currentRowTextBrush = isSelected ? Brushes.White : rowBrush;

                Rectangle rowRect = new Rectangle(startX, startY, tableWidth, rowHeight);
                g.FillRectangle(currentRowBackgroundBrush, rowRect);
                g.DrawRectangle(tablePen, rowRect);

                g.DrawString(row.Cells["BillId"].Value.ToString(), rowFont, currentRowTextBrush, new RectangleF(startX, startY, columnWidth, rowHeight), columnFormat);
                g.DrawString(row.Cells["MedName"].Value.ToString(), rowFont, currentRowTextBrush, new RectangleF(startX + columnWidth, startY, columnWidth, rowHeight), columnFormat);
                g.DrawString(row.Cells["Quantity"].Value.ToString(), rowFont, currentRowTextBrush, new RectangleF(startX + 2 * columnWidth, startY, columnWidth, rowHeight), columnFormat);
                g.DrawString(row.Cells["TotalAmount"].Value.ToString(), rowFont, currentRowTextBrush, new RectangleF(startX + 3 * columnWidth, startY, columnWidth, rowHeight), columnFormat);

                startY += rowHeight; // المسافة بين الصفوف في الجدول
            }

            // ** طباعة المجموع الكلي (Total Amount) **
            int totalAmount = 0;
            foreach (DataGridViewRow row in BillingGV.Rows)
            {
                if (row.IsNewRow) continue;
                totalAmount += Convert.ToInt32(row.Cells["TotalAmount"].Value);
            }

            Rectangle totalRect = new Rectangle(startX, startY, tableWidth, rowHeight);
            g.FillRectangle(totalBackgroundBrush, totalRect);
            g.DrawRectangle(tablePen, totalRect);
            g.DrawString($"Total Amount: {totalAmount} EGP", totalFont, totalTextBrush, totalRect, titleFormat);
            startY += rowHeight + 50; // زيادة المسافة بعد المجموع الكلي لتصبح 30 نقطة قبل رسالة الشكر

            // ** طباعة رسالة شكر **
            g.DrawString("Thank you for shopping at our pharmacy!", new Font("Arial", 12, FontStyle.Italic), Brushes.Black, startX, startY);
        }



        // Total amount
        private void UpdateTotalAmount()
        {
            try
            {
                int totalAmount = 0;

                // التكرار عبر جميع الصفوف في DataGridView لحساب المجموع
                foreach (DataGridViewRow row in BillingGV.Rows)
                {
                    // التأكد من أن الصف ليس فارغًا
                    if (row.Cells["TotalAmount"].Value != null)
                    {
                        // إضافة القيمة في عمود TotalAmount إلى المجموع
                        totalAmount += Convert.ToInt32(row.Cells["TotalAmount"].Value);
                    }
                }

                // عرض المجموع في label5
                label5.Text = $"Total Amount: {totalAmount} EGP"; // عرض المجموع الإجمالي
                label5.ForeColor = Color.Black; // التأكد من أن النص يظهر بشكل جيد
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error calculating total amount: {ex.Message}");
            }

        }

        private void ExpDate_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
