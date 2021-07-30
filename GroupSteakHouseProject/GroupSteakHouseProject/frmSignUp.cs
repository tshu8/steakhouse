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
    public partial class frmSignUp : Form
    {
        frmLogin login;
        public frmSignUp(frmLogin Login)
        {
            InitializeComponent();
            login = Login;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            login.Show();
            this.Close();
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            if (!tbxEmailInput.Text.Contains("@"))
            {
                MessageBox.Show("Please enter a valid Email Address!", "Invalid email", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tbxEmailInput.Text.LastIndexOf(".") < tbxEmailInput.Text.LastIndexOf("@"))
            {
                MessageBox.Show("Please enter a valid Email Address!", "Invalid email", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //sees if passwords are the same
            if (tbxPassword.Text == tbxPasswordRenter.Text && tbxPassword.Text != "")
            {
                ProgOps.SignupCommand(tbxEmailInput, tbxPassword, tbxPasswordRenter, tbxFirstName, tbxLastName, tbxAddress, tbxPhone);
                this.Close();
                frmLogin frmLogin = new frmLogin();
                frmLogin.Show();
            }

            else if (tbxPassword.Text == "")
            {
                MessageBox.Show("Cannot leave password blank", "Error");
            }

            else
            {
                MessageBox.Show("Passwords do not match", "Error");
            }

        }

        private void tbxPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Controls the textbox so only numbers can be applied
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '-')
            {
                e.Handled = true;
            }
        }
    }
}
