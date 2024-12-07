
# Pharmacy Management ( C# ) 💊

<div style="display: flex; justify-content: space-between; margin-bottom:2%;">
  <img src="https://github.com/user-attachments/assets/c5412f6f-df6c-4cb4-a8f0-657f66a05f35" alt="Screenshot 1" style="width: 24%; margin-right: 1%;" />
  <img src="https://github.com/user-attachments/assets/f76113d1-ef6f-40d4-b2a9-0085dbcc7c85" alt="Screenshot 3"style="width: 24%;" />
  <img src="https://github.com/user-attachments/assets/e297474e-e151-4b69-9ed3-f9b1087aabf4" alt="Screenshot 4"style="width: 24%; margin-right: 1%;" />
  <img src="https://github.com/user-attachments/assets/fd46f1b4-daa8-42a9-976b-86ada5dcea99" alt="Screenshot 5"style="width: 24%;" />
  <img src="https://github.com/user-attachments/assets/c6a0ba51-9616-41cb-83a3-58518a628acb" alt="Screenshot 6"style="width: 24%;" />
  <img src="https://github.com/user-attachments/assets/acdec246-0486-48c8-b64c-b7950648de85" alt="Screenshot 7"style="width: 24%;" />
  <img src="https://github.com/user-attachments/assets/12a0cb9d-a85b-46b9-86ac-65326f0271a0" alt="Screenshot 8"style="width: 24%;" />
  <img src="https://github.com/user-attachments/assets/f7e681be-5c63-4e7e-8676-01a734b0ed0a" alt="Screenshot 9"style="width: 24%;" />
</div>

A `static desktop application` built using `C#`, designed for managing pharmacy operations. It features
multiple modules to handle various aspects of pharmacy management, including Employees
management, billing, medicine tracking, and Companies records.

## 🛠 Technologies Used:

- `C# (Windows Forms)` for building the user interface and implementing the business logic.
- File Handling for saving and retrieving data using `.txt` files.

### 📂 Features:

*`1. Agent Management`*
- Add, search, edit, and manage agent details with ease.
- Maintain comprehensive records to streamline operations.

*`2. Billing Module`*
- Record customer transactions accurately.
- Generate detailed bills for enhanced financial tracking.

*`3. Medicine Inventory Management`*
- Track available stock and manage inventory efficiently.
- Monitor medicine details, including batch, expiry, and pricing.

*`4. Manufacturer Management`*
- Maintain detailed records of manufacturers.
- Simplify tracking and communication with suppliers.
 
⚠ Current Status:
- This project is currently functional but relies on a database. The UI is responsive and designed primarily for desktop usage.

---

## 🔑 Login Information:

- Username: `admin`
- Password: `123`

⚠ Important Notice:
If any attempt is made to change the username or password in the code, a warning message will appear.
This helps ensure system security by preventing unauthorized modifications to login credentials.
Below is an example of how the warning message is implemented:
```
 //for save data in your file
   string filename = @"E:\Project_OOP\FO_organization\Pharmacy-Management\savedLogin.txt";   // استخدام using لضمان إغلاق الموارد تلقائيًا
  using (FileStream myfile = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write))
 {
   using (StreamWriter sw = new StreamWriter(myfile))
   {
      sw.WriteLine($"username: {username.Text}\t password:{password.Text}");
   }
}
 //MessageBox.Show("Your data has been saved");
 if (name == "admin" && pass == "123")
 {
     Hide();
     Home basic = new Home();
     basic.ShowDialog();
 }
 else
 {
    MessageBox.Show("Error,your Username or Password is incorrect");
    username.Text = password.Text = null;
 }
}
  
```

💡 How to Disable the Warning Message:
To disable the warning message or modify the login credentials, you can update them directly in the code. Below is where the username and password are hardcoded:
```
// Example of where login credentials are set
string username = "admin";
string password = "123";
```
If you need to change the credentials, simply update the `username` and `password` variables in the code.
Make sure to test the program after making changes to ensure proper functionality.

---
## How to Use 🚀  

We welcome `contributions` to **Pharmacy Management**! Here’s how you can help:
1. *Fork the repository* - Click the "Fork" button at the top right of the repository page.
2. *Clone your fork* - Use the command:
   
   ```bash
   git clone https://github.com/OmarrSakr/Pharmacy-Management.git

---
## 🔄 Future Enhancements:

- User Authentication System for `secure` access to the application.
- Responsive UI with potential dark mode support for improved user experience.
- This project demonstrates how C# Windows Forms can be used for building structured and efficient pharmacy management systems, with easy data handling and modular design.
