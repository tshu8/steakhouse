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
            this.Hide();
        }

        private void btnViewOrders_Click(object sender, EventArgs e)
        {
            frmView view = new frmView(this);
            view.Show();
            this.Hide();
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            frmChange change = new frmChange(this);
            change.Show();
            this.Hide();
        }

        private void btnCusInfo_Click(object sender, EventArgs e)
        {
            frmCustomer cusInfo = new frmCustomer(this);
            cusInfo.Show();
            this.Hide();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.Yes == MessageBox.Show("Do you wish to print a Daily Report?", "Daily Reports", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                {
                    ProgOps.DatabaseCommandLoadDailySales();
                    HtmlReports.PrintDailySales(HtmlReports.GenerateDailySales(ProgOps._sqlDailySales));
                }
                else
                {
                    if (DialogResult.Yes == MessageBox.Show("Do you wish to print a Weekly Report?", "Weekly Reports", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                    {
                        ProgOps.DatabaseCommandLoadWeeklySales();
                        HtmlReports.PrintWeeklySales(HtmlReports.GenerateWeeklySales(ProgOps._sqlWeeklySales));
                    }
                    else
                    {
                        if (DialogResult.Yes == MessageBox.Show("Do you wish to print a Monthly Report?", "Monthly Reports", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                        {
                            ProgOps.DatabaseCommandLoadMonthlySales();
                            HtmlReports.PrintMonthlySales(HtmlReports.GenerateMonthlySales(ProgOps._sqlMonthlySales));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There has been an error processing all sales information", "Error Processing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            frmLogin login = new frmLogin();
            login.Show();
            this.Close();
        }
    }
}
