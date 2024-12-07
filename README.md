
# Pharmacy Management ( C# ) 💊

<div style="display: flex; justify-content: space-between; margin-bottom:2%;">
  <img src="https://github.com/user-attachments/assets/b700f8ec-2495-484c-88d9-d2da26b3f505" alt="Screenshot 20" style="width: 24%; margin-right: 1%;" />
  <img src="https://github.com/user-attachments/assets/52ff42ed-1b4c-4cb8-a3be-01614ae629d4" alt="Screenshot 21"style="width: 24%;" />
  <img src="https://github.com/user-attachments/assets/9eb103d5-9620-46f1-b9f6-8aeea7176326" alt="Screenshot 22"style="width: 24%; margin-right: 1%;" />
  <img src="https://github.com/user-attachments/assets/8dbc8b31-a1b3-479c-9064-d28185592d53" alt="Screenshot 23"style="width: 24%;" />
  <img src="https://github.com/user-attachments/assets/535e3863-a2f0-4084-ab48-2676be9b123d" alt="Screenshot 24"style="width: 24%;" />
  <img src="https://github.com/user-attachments/assets/690f4bfe-5339-4008-8e22-3bb65f0e7ec1" alt="Screenshot 25"style="width: 24%;" />
  <img src="https://github.com/user-attachments/assets/d59ce6ed-f25f-4f37-b81f-eb562ccb0976" alt="Screenshot 25"style="width: 24%;" />
</div>


A `static desktop application` built using `C#`, designed for managing pharmacy operations. It features
multiple modules to handle various aspects of pharmacy management, including agent
management, billing, medicine tracking, and manufacturer records.

## 🛠 Technologies Used:

- `C# (Windows Forms)` for building the user interface and implementing the business logic.
- File Handling for saving and retrieving data using `.txt` files.

### 📂 Features:

- Agent Management: Add, search, edit, and manage agent details.
- Billing Module: Record transactions and generate bills for customers.
- Medicine Tracking: Manage medicine inventory and track available stock.
- Manufacturer Records: Store and manage manufacturer details efficiently.
 
⚠ Current Status:
This project is currently functional but relies on file handling instead of a database. The UI is non-responsive and designed primarily for desktop usage.

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
  string filename = @"D:\Project_OOP\FO_organization\project_fo_3\project_fo_3\Login.txt";
  FileStream myfile = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);

  StreamWriter sw = new StreamWriter(myfile);
  sw.WriteLine($"username: {username.Text}\t password:{password.Text}");
  sw.Flush();
  MessageBox.Show("Your data has been saved");


  if (name == "admin" && pass == "123")
      {
          MessageBox.Show("hi,poss");
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

- `Database` Integration (e.g., SQL Server) for more reliable and scalable data management.
- User Authentication System for `secure` access to the application.
- Responsive UI with potential dark mode support for improved user experience.
- This project demonstrates how C# Windows Forms can be used for building structured and efficient pharmacy management systems, with easy data handling and modular design.
