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
    public partial class frmOrder : Form
    {
        frmMenu frmMenu;
        public int intUserID;

        public frmOrder(frmMenu menu)
        {
            InitializeComponent();
            frmMenu = menu;
        }

        public List<string> strCart = new List<string>();
        public List<double> dblPrice = new List<double>();

        private void btnAddtoCart_Click(object sender, EventArgs e)
        {
            string strPrice;
            double dblCost;

            strCart.Add(dgvFood.CurrentRow.Cells[0].Value.ToString());

            strPrice = dgvFood.CurrentRow.Cells[1].Value.ToString();

            string strProdPrice = strPrice.Substring(strPrice.IndexOf("$") + 1, strPrice.Length - (strPrice.IndexOf("$") + 1));

            double.TryParse(strProdPrice, out dblCost);

            dblPrice.Add(dblCost);
            MessageBox.Show("1 " + dgvFood.CurrentRow.Cells[0].Value.ToString() + " has been added to your cart", "Item added", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public List<int> intProdCount = new List<int>();
        public List<string> strCop = new List<string>();
        public List<double> dblProdPrice = new List<double>();

        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (strCart.Count == 0)
            {
                MessageBox.Show("Sorry, it looks like you don't have any items in your cart! Add some goodies and feel free to come check out here!");
                return;
            }

            frmCheckout checkout = new frmCheckout(this);
            int Count = 0;

            for (int i = 0; i < strCart.Count; i++)
            {
                if (strCop.Contains(strCart[i]))
                {
                    intProdCount[strCop.IndexOf(strCart[i])]++;
                }
                if (!strCop.Contains(strCart[i]))
                {
                    intProdCount.Add(Count + 1);
                    strCop.Add(strCart[i]);
                    dblProdPrice.Add(dblPrice[i]);
                }
            }

            for (int i = 0; i < strCop.Count; i++)
            {
                dblProdPrice[i] = dblPrice[strCart.IndexOf(strCop[i])] * intProdCount[i];
            }

            for (int i = 0; i < strCop.Count(); i++)
            {
                checkout.lbxCart.Items.Add(intProdCount[i].ToString() + " " + strCop[i] + " $" + dblProdPrice[i].ToString("0.##"));
                MessageBox.Show(checkout.lbxCart.Items[i].ToString());
            }

            checkout.Show();
        }

        private void frmOrder_Load(object sender, EventArgs e)
        {
            ProgOps.OpenDatabase();
            ProgOps.InitProductDatabaseCommand(dgvFood);

            if (frmMenu.intSecurityLevel > 1)
            {
                btnAddtoCart.Enabled = false;
                btnCheck.Enabled = false;
            }

        }

        private void ProductClick()
        {
            int intRowIndex = dgvFood.CurrentRow.Index;

            string SelectedItem = dgvFood.CurrentRow.Cells[0].Value.ToString();

        }

        private void dgvFood_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ProductClick();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            frmMenu.Show();
            this.Close();
        }
    }
}
