
namespace project_fo_3
{
    partial class Medicine
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.QtTb = new Guna.UI.WinForms.GunaLineTextBox();
            this.SpriceTb = new Guna.UI.WinForms.GunaLineTextBox();
            this.BpriceTb = new Guna.UI.WinForms.GunaLineTextBox();
            this.MedicineTb = new Guna.UI.WinForms.GunaLineTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.MedicineGV = new Guna.UI2.WinForms.Guna2DataGridView();
            this.ExpDate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.button5 = new System.Windows.Forms.Button();
            this.SelectCompany = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MedicineGV)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DarkRed;
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1463, 100);
            this.panel1.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Tai Le", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label8.Location = new System.Drawing.Point(1418, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 43);
            this.label8.TabIndex = 4;
            this.label8.Text = "×";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label2.Location = new System.Drawing.Point(661, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "Medicine";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(561, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(336, 36);
            this.label1.TabIndex = 2;
            this.label1.Text = "PharmacyManagement";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.label3.Location = new System.Drawing.Point(27, 352);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Quantity";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.label4.Location = new System.Drawing.Point(27, 295);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "SellingPrice";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.label5.Location = new System.Drawing.Point(27, 233);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(110, 20);
            this.label5.TabIndex = 8;
            this.label5.Text = "BuyingPrice";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.label6.Location = new System.Drawing.Point(27, 173);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(132, 20);
            this.label6.TabIndex = 9;
            this.label6.Text = "MedicineName";
            // 
            // QtTb
            // 
            this.QtTb.BackColor = System.Drawing.Color.White;
            this.QtTb.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.QtTb.FocusedLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.QtTb.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.QtTb.LineColor = System.Drawing.Color.DarkRed;
            this.QtTb.Location = new System.Drawing.Point(228, 342);
            this.QtTb.Name = "QtTb";
            this.QtTb.PasswordChar = '\0';
            this.QtTb.SelectedText = "";
            this.QtTb.Size = new System.Drawing.Size(178, 30);
            this.QtTb.TabIndex = 10;
            // 
            // SpriceTb
            // 
            this.SpriceTb.BackColor = System.Drawing.Color.White;
            this.SpriceTb.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.SpriceTb.FocusedLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.SpriceTb.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.SpriceTb.LineColor = System.Drawing.Color.DarkRed;
            this.SpriceTb.Location = new System.Drawing.Point(228, 285);
            this.SpriceTb.Name = "SpriceTb";
            this.SpriceTb.PasswordChar = '\0';
            this.SpriceTb.SelectedText = "";
            this.SpriceTb.Size = new System.Drawing.Size(178, 30);
            this.SpriceTb.TabIndex = 11;
            // 
            // BpriceTb
            // 
            this.BpriceTb.BackColor = System.Drawing.Color.White;
            this.BpriceTb.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.BpriceTb.FocusedLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.BpriceTb.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BpriceTb.LineColor = System.Drawing.Color.DarkRed;
            this.BpriceTb.Location = new System.Drawing.Point(228, 223);
            this.BpriceTb.Name = "BpriceTb";
            this.BpriceTb.PasswordChar = '\0';
            this.BpriceTb.SelectedText = "";
            this.BpriceTb.Size = new System.Drawing.Size(178, 30);
            this.BpriceTb.TabIndex = 12;
            // 
            // MedicineTb
            // 
            this.MedicineTb.BackColor = System.Drawing.Color.White;
            this.MedicineTb.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.MedicineTb.FocusedLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.MedicineTb.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MedicineTb.LineColor = System.Drawing.Color.DarkRed;
            this.MedicineTb.Location = new System.Drawing.Point(228, 163);
            this.MedicineTb.Name = "MedicineTb";
            this.MedicineTb.PasswordChar = '\0';
            this.MedicineTb.SelectedText = "";
            this.MedicineTb.Size = new System.Drawing.Size(178, 30);
            this.MedicineTb.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.DarkRed;
            this.label7.Location = new System.Drawing.Point(938, 116);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(195, 32);
            this.label7.TabIndex = 16;
            this.label7.Text = "Medicine List";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Green;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(228, 668);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(99, 37);
            this.button2.TabIndex = 17;
            this.button2.Text = "Back";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Green;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(286, 608);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(99, 39);
            this.button1.TabIndex = 18;
            this.button1.Text = "Delete";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Green;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(153, 608);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(99, 39);
            this.button3.TabIndex = 19;
            this.button3.Text = "Update";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Green;
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(31, 608);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(91, 39);
            this.button4.TabIndex = 20;
            this.button4.Text = "Add";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DarkRed;
            this.panel2.Controls.Add(this.label9);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 782);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1463, 32);
            this.panel2.TabIndex = 26;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI Symbol", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label9.Location = new System.Drawing.Point(636, 4);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(158, 25);
            this.label9.TabIndex = 5;
            this.label9.Text = "CreatedByOmar";
            // 
            // MedicineGV
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.MedicineGV.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.DarkRed;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Sans Serif Collection", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Firebrick;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MedicineGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.MedicineGV.ColumnHeadersHeight = 30;
            this.MedicineGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.DarkGreen;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.MedicineGV.DefaultCellStyle = dataGridViewCellStyle3;
            this.MedicineGV.GridColor = System.Drawing.Color.Firebrick;
            this.MedicineGV.Location = new System.Drawing.Point(596, 163);
            this.MedicineGV.Name = "MedicineGV";
            this.MedicineGV.RowHeadersVisible = false;
            this.MedicineGV.RowHeadersWidth = 51;
            this.MedicineGV.RowTemplate.Height = 24;
            this.MedicineGV.Size = new System.Drawing.Size(834, 430);
            this.MedicineGV.TabIndex = 29;
            this.MedicineGV.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.MedicineGV.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.MedicineGV.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.MedicineGV.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.MedicineGV.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.MedicineGV.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.MedicineGV.ThemeStyle.GridColor = System.Drawing.Color.Firebrick;
            this.MedicineGV.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.MedicineGV.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.MedicineGV.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MedicineGV.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.MedicineGV.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.MedicineGV.ThemeStyle.HeaderStyle.Height = 30;
            this.MedicineGV.ThemeStyle.ReadOnly = false;
            this.MedicineGV.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.MedicineGV.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.MedicineGV.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MedicineGV.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.MedicineGV.ThemeStyle.RowsStyle.Height = 24;
            this.MedicineGV.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.MedicineGV.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.MedicineGV.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.MedicineGV_CellContentClick);
            this.MedicineGV.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.MedicineGV_CellContentClick);
            this.MedicineGV.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.MedicineGV_CellMouseClick);
            this.MedicineGV.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.MedicineGV_CellMouseClick);
            this.MedicineGV.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.MedicineGV_CellMouseClick);
            // 
            // ExpDate
            // 
            this.ExpDate.Checked = true;
            this.ExpDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ExpDate.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.ExpDate.Location = new System.Drawing.Point(68, 477);
            this.ExpDate.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.ExpDate.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.ExpDate.Name = "ExpDate";
            this.ExpDate.Size = new System.Drawing.Size(290, 36);
            this.ExpDate.TabIndex = 30;
            this.ExpDate.Value = new System.DateTime(2024, 12, 3, 21, 18, 8, 333);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.Green;
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.Location = new System.Drawing.Point(80, 668);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(99, 37);
            this.button5.TabIndex = 31;
            this.button5.Text = "Clear";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // SelectCompany
            // 
            this.SelectCompany.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectCompany.FormattingEnabled = true;
            this.SelectCompany.Location = new System.Drawing.Point(68, 420);
            this.SelectCompany.Name = "SelectCompany";
            this.SelectCompany.Size = new System.Drawing.Size(290, 28);
            this.SelectCompany.TabIndex = 50;
            this.SelectCompany.Text = "Select Company";
            this.SelectCompany.SelectedIndexChanged += new System.EventHandler(this.SelectCompany_SelectedIndexChanged);
            // 
            // Medicine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1463, 814);
            this.Controls.Add(this.SelectCompany);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.ExpDate);
            this.Controls.Add(this.MedicineGV);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.MedicineTb);
            this.Controls.Add(this.BpriceTb);
            this.Controls.Add(this.SpriceTb);
            this.Controls.Add(this.QtTb);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Medicine";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Medicine";
            this.Load += new System.EventHandler(this.Medicine_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MedicineGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private Guna.UI.WinForms.GunaLineTextBox QtTb;
        private Guna.UI.WinForms.GunaLineTextBox SpriceTb;
        private Guna.UI.WinForms.GunaLineTextBox BpriceTb;
        private Guna.UI.WinForms.GunaLineTextBox MedicineTb;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label9;
        private Guna.UI2.WinForms.Guna2DataGridView MedicineGV;
        private Guna.UI2.WinForms.Guna2DateTimePicker ExpDate;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.ComboBox SelectCompany;
    }
}