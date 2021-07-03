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
    public partial class frmMenu : Form
    {
        public int intSecurityLevel = 0;
        public int UserID = 0;

        public frmMenu()
        {
            InitializeComponent();
        }

        private void frmMenu_Load(object sender, EventArgs e)
        {
            if (intSecurityLevel < 2)
            {
                btnChange.Hide();
            }
            if (intSecurityLevel < 3)
            {
                btnReports.Hide();
                btnCusInfo.Hide();
            }

        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            frmOrder order = new frmOrder(this);
            order.intUserID = UserID;
            order.Show();
        }
    }
}
