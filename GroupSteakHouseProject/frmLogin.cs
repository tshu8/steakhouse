using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroupSteakHouseProject
{
    public partial class frmLogin : Form
    {
        frmMenu menu = new frmMenu();

        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            ProgOps.CloseDatabase();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //check for matching email to database
            if (tbxUsername.Text != "" && tbxPassword.Text != "" && tbxUsername.Text != "Username" && tbxPassword.Text != "Password")
            {
                ProgOps.Login(tbxUsername, tbxPassword, this, menu);
            }
        }

        private void btnSign_Click(object sender, EventArgs e)
        {
            frmSignUp frmSignup = new frmSignUp(this);
            frmSignup.Show();
            this.Hide();
        }

        private void tbxUsername_TextChanged(object sender, EventArgs e)
        {
        }

        private void tbxUsername_Click(object sender, EventArgs e)
        {
            if (tbxUsername.Text == "Username")
            {
                tbxUsername.Clear();
            }
        }

        private void tbxPassword_Click(object sender, EventArgs e)
        {
            if (tbxPassword.Text == "Password")
            {
                tbxPassword.Clear();
            }
        }
    }
}
