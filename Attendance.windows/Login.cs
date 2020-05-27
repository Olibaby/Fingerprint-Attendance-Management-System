using Attendance.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Attendance.windows
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var userName = txtUserName.Text.Trim();
            var password = txtPassword.Text.Trim();
            String message = "";
            if (userName.Length == 0)
            {
                message += "Username is required.\n";
            }
            else if (password.Length == 0)
            {
                message += "Invalid email format supplied.\n";
            }
            if (message.Length > 0)
            {
                MessageBox.Show(message);
            }
            else
            {
                ValidateUser(userName, password);
            }
        }

        void ValidateUser(string username, string password)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                using (var db = new DataEntity())
                {
                    var user = db.Users.Where(u => u.UserName == username && u.Password == password).FirstOrDefault();
                    if (user == null)
                    {
                        MessageBox.Show("invalid user and password combination supplied.");
                    }
                    else
                    {
                        this.txtPassword.Text = "";
                        this.DialogResult = DialogResult.OK;
                        Close();
                    }
                }
            }
            catch (Exception e)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show("Error Connecting to Server. Reason: " + e.Message);
            }

        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
        }
    }
}
