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
    public partial class frmCheckout : Form
    {
        frmOrder ordering;

        public frmCheckout(frmOrder order)
        {
            ordering = order;
            InitializeComponent();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lbxCart.SelectedIndex == -1)
                MessageBox.Show("Please select an Item first!", "Error Deleting Item", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (lbxCart.Items.Count == 0)
            {
                MessageBox.Show("Sorry, it looks like you don't have any items in your cart! Add some goodies and feel free to come check out here!");
                return;
            }
            if (!lbxCart.Focus())
            {
                lbxCart.SelectedIndex = 0;
            }
            if (lbxCart.Items.Count > 0)
            {
                string strProductName = lbxCart.SelectedItem.ToString().Substring(lbxCart.SelectedItem.ToString().IndexOf(" ") + 1, lbxCart.SelectedItem.ToString().Length - (lbxCart.SelectedItem.ToString().Length - lbxCart.SelectedItem.ToString().LastIndexOf("$") + 3));

                ordering.dblProdPrice.Clear();

                ordering.strCop.Clear();

                ordering.intProdCount.Clear();
                while (ordering.strCart.Contains(strProductName))
                {
                    for (int i = 0; i < ordering.strCart.Count; i++)
                    {
                        ordering.dblPrice.RemoveAt(ordering.strCart.IndexOf(strProductName));
                        ordering.strCart.Remove(strProductName);
                    }
                }

                lbxCart.Items.RemoveAt(lbxCart.SelectedIndex);
            }

        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            if (lbxCart.Items.Count == 0)
            {
                MessageBox.Show("Sorry, it looks like you don't have any items in your cart. Order some items and feel free to come check out here!");
                return;
            }
            List<string> Prodname = new List<string>();
            List<int> Quantities = new List<int>();
            List<double> ProdPrice = new List<double>();
            int intQuantity = 0;
            double dblPrice = 0;
            double totalPrice = 0;


            for (int i = 0; i < lbxCart.Items.Count; i++)
            {
                string placeholder = lbxCart.Items[i].ToString();

                string ProdName = placeholder.Substring(placeholder.IndexOf(" ") + 1, placeholder.Length - ((placeholder.Length - placeholder.IndexOf("$")) + 2));

                string ProdQuantity = placeholder.Substring(0, placeholder.IndexOf(" "));

                string strProdPrice = placeholder.Substring(placeholder.LastIndexOf("$") + 1, placeholder.Length - (placeholder.LastIndexOf("$") + 1));

                Prodname.Add(ProdName);
                int.TryParse(ProdQuantity, out intQuantity);
                Quantities.Add(intQuantity);
                double.TryParse(strProdPrice, out dblPrice);
                ProdPrice.Add(dblPrice);
            }

            for (int i = 0; i < ProdPrice.Count; i++)
            {
                totalPrice += ProdPrice[i];
            }

            for (int i = 0; i < Prodname.Count; i++)
            {
                Prodname[i].Replace("'", "''");
            }

            ProgOps.DatabaseCommandMakeOrder(Prodname, Quantities, ProdPrice, totalPrice, ordering.intUserID);

            if (MessageBox.Show($"Your total comes to {(totalPrice + (totalPrice * .0825)).ToString("C2")} is this okay?", "Total Price of Order", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
            {
                //HTML RECEPEIT
                MessageBox.Show("Order Placed. Thank you for your Order! \nPlease come again soon!");
                HtmlReports.PrintReceiept(HtmlReports.GenerateReceipt(Prodname, Quantities, ProdPrice, totalPrice, totalPrice + (totalPrice * .0825)));

                if (ordering.strCart.Count() > 0)
                {
                    ordering.strCart.Clear();
                    ordering.dblProdPrice.Clear();
                    ordering.dblPrice.Clear();
                    ordering.strCop.Clear();
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Remove whatever items from your cart and come on back to complete your order!", "Transaction suspended", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
