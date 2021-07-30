using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO;

namespace GroupSteakHouseProject
{
    class HtmlReports
    {

        public static StringBuilder GenerateOrder(SqlCommand command)
        {
            StringBuilder html = new StringBuilder();
            StringBuilder css = new StringBuilder();

            SqlDataReader reader;
            string info;

            command.ExecuteNonQuery();

            reader = command.ExecuteReader();

            css.Append("<style>");
            css.Append("td {padding:5px;text-align:center;font-weight:bold;text-align:center}");
            css.Append("</style>");

            html.Append("<html>");
            html.Append($"<head>{css}<title>{"Your Orders"}</title></head>");
            html.Append($"<body>");
            html.Append($"<h1>{"Your Orders"}</h1>");

            html.Append("<table>");
            html.Append("<tr><td colspan=4></td></tr>");

            html.Append("<tr>");
            html.Append("<td>OrderID</td>");
            html.Append("<td>Product Name</td>");
            html.Append("<td>Order Price</td>");
            html.Append("<td>Quantity Purchased</td>");
            html.Append("</tr>");

            html.Append("<tr>");
            while (reader.Read())
            {                
                html.Append($"<td>{reader.GetInt32(0)}</td>");
                html.Append($"<td>{reader.GetString(1)}</td>");
                html.Append($"<td>{reader.GetString(2)}</td>");
                html.Append($"<td>{reader.GetInt32(3)}</td>");
                html.Append("</tr>");
                html.Append($"<p>Order Total :{reader.GetString(4)}</p>");
            }

            html.Append("<tr><td colspan=5></td></tr>");
            html.Append("</table>");

            html.Append("</body></html>");

            reader.Close();
            return html;
        }

        public static void PrintOrder(StringBuilder html)
        {
            try
            {
                using (StreamWriter wr = new StreamWriter("Order.html"))
                {
                    wr.WriteLine(html);
                }
                System.Diagnostics.Process.Start(@"Order.html");
            }
            catch (Exception ex)
            {
                MessageBox.Show("You don't have write permissions", "Error System Permissions", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            DateTime today = DateTime.Now;
            using (StreamWriter wr = new StreamWriter($"{today.ToString("yyyy-MM-dd-HHmmss")} - OrderReport.html"))
            {
                wr.WriteLine(html);
            }
        }

        public static StringBuilder GenerateReceipt(List<string> strListItems, List<int> intQuantity, List<double> dblProductPrice, double dblPriceOfPurchase, double dblPriceOfPurchaseTax)
        {
            StringBuilder html = new StringBuilder();
            StringBuilder css = new StringBuilder();


            css.Append("<style>");
            css.Append("td {padding:5px;text-align:center;font-weight:bold;text-align:center}");
            css.Append("</style>");

            html.Append("<html>");
            html.Append($"<head>{css}<title>{"Order Receipt"}</title></head>");
            html.Append($"<body>");
            html.Append($"<h1>{"Your Order has been processed"}</h1>");


            html.Append($"<ul>");

            for (int i = 0; i < strListItems.Count; i++)
            {
                html.Append($"<li>{intQuantity[i].ToString()} {strListItems[i]}----{dblProductPrice[i].ToString("C2")}</li>");
            }
            html.Append("</ul>");
            html.Append($"<p>Subtotal: {dblPriceOfPurchase.ToString("C2")}</p>");
            html.Append($"<p>Total After Taxes: {dblPriceOfPurchaseTax.ToString("C2")}");

            html.Append("</body></html>");

            return html;
        }

        public static void PrintReceiept(StringBuilder html)
        {
            try
            {
                using (StreamWriter wr = new StreamWriter("Receipt.html"))
                {
                    wr.WriteLine(html);
                }
                System.Diagnostics.Process.Start(@"Receipt.html");
            }
            catch (Exception ex)
            {
                MessageBox.Show("You don't have write permissions", "Error System Permissions", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            using (StreamWriter wr = new StreamWriter("Receipt.html"))
            {
                wr.WriteLine(html);
            }
        }

        public static StringBuilder GenerateDailySales(SqlCommand command)
        {
            StringBuilder html = new StringBuilder();
            StringBuilder css = new StringBuilder();
            string info = "";
            SqlDataReader reader;

            command.ExecuteNonQuery();

            reader = command.ExecuteReader();

            css.Append("<style>");
            css.Append("td {padding:5px;text-align:center;font-weight:bold;text-align:center}");
            css.Append("</style>");

            html.Append("<html>");
            html.Append($"<head>{css}<title>{"Daily Sales"}</title></head>");
            html.Append($"<body>");
            html.Append($"<h1>{"Daily Sales"}</h1>");

            html.Append("<table>");
            html.Append("<tr><td colspan=6></td></tr>");

            html.Append("<tr>");
            html.Append("<td>OrderID</td>");
            html.Append("<td>Order Date</td>");
            html.Append("<td>Customer ID</td>");
            html.Append("<td>Product Name</td>");
            html.Append("<td>Quantity</td>");
            html.Append("<td>Price of Order</td>");
            html.Append("</tr>");

            html.Append("<tr>");
            while (reader.Read())
            {
                info = reader.GetString(6);
                html.Append($"<td>{reader.GetInt32(0)}</td>");
                html.Append($"<td>{Convert.ToDateTime(reader.GetDateTime(1)).ToString("dd/MM/yyyy")}</td>");
                html.Append($"<td>{reader.GetInt32(2)}</td>");
                html.Append($"<td>{reader.GetString(3)}</td>");
                html.Append($"<td>{reader.GetInt32(4)}</td>");
                html.Append($"<td>{reader.GetString(5)}</td>");


                html.Append("</tr>");
            }


            html.Append("<tr><td colspan=7></td></tr>");
            html.Append("</table>");
            if (info != "")
            {
                html.Append("<p>Total Daily Sales</p>");
                html.Append($"<p>{info}</p>");
            }
            html.Append("</body></html>");

            reader.Close();
            return html;
        }

        public static void PrintDailySales(StringBuilder html)
        {

            DateTime today = DateTime.Now;
            try
            {
                using (StreamWriter wr = new StreamWriter($"{today.ToString("yyyy-MM-dd-HHmmss")}DailySales.html"))
                {
                    wr.WriteLine(html);
                }
                System.Diagnostics.Process.Start($@"{today.ToString("yyyy-MM-dd-HHmmss")}DailySales.html");
            }
            catch (Exception ex)
            {
                MessageBox.Show("You don't have write permissions", "Error System Permissions", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            using (StreamWriter wr = new StreamWriter($"{today.ToString("yyyy-MM-dd-HHmmss")} - DailySales.html"))
            {
                wr.WriteLine(html);
            }
        }

        public static StringBuilder GenerateWeeklySales(SqlCommand command)
        {
            StringBuilder html = new StringBuilder();
            StringBuilder css = new StringBuilder();
            string info = "";

            SqlDataReader reader;

            command.ExecuteNonQuery();

            reader = command.ExecuteReader();

            css.Append("<style>");
            css.Append("td {padding:5px;text-align:center;font-weight:bold;text-align:center}");
            css.Append("</style>");

            html.Append("<html>");
            html.Append($"<head>{css}<title>{"Weeekly Sales"}</title></head>");
            html.Append($"<body>");
            html.Append($"<h1>{"Weekly Sales"}</h1>");

            html.Append("<table>");
            html.Append("<tr><td colspan=7></td></tr>");

            html.Append("<tr>");
            html.Append("<td>OrderID</td>");
            html.Append("<td>Order Date</td>");
            html.Append("<td>Customer ID</td>");
            html.Append("<td>Product Name</td>");
            html.Append("<td>Quantity</td>");
            html.Append("<td>Price of Order</td>");
            html.Append("</tr>");

            html.Append("<tr>");
            while (reader.Read())
            {
                info = reader.GetString(6);
                html.Append($"<td>{reader.GetInt32(0)}</td>");
                html.Append($"<td>{Convert.ToDateTime(reader.GetDateTime(1)).ToString("dd/MM/yyyy")}</td>");
                html.Append($"<td>{reader.GetInt32(2)}</td>");
                html.Append($"<td>{reader.GetString(3)}</td>");
                html.Append($"<td>{reader.GetInt32(4)}</td>");
                html.Append($"<td>{reader.GetString(5)}</td>");
                html.Append("</tr>");
            }


            html.Append("<tr><td colspan=8 ></td></tr>");
            html.Append("</table>");

            if (info != "")
            {
                html.Append("<p>Total Weekly Sales</p>");
                html.Append($"<p>{info}</p>");
            }


            html.Append("</body></html>");

            reader.Close();
            return html;
        }

        public static void PrintWeeklySales(StringBuilder html)
        {

            DateTime today = DateTime.Now;
            try
            {
                using (StreamWriter wr = new StreamWriter($"{today.ToString("yyyy-MM-dd-HHmmss")}WeeklySales.html"))
                {
                    wr.WriteLine(html);
                }
                System.Diagnostics.Process.Start($@"{today.ToString("yyyy-MM-dd-HHmmss")}WeeklySales.html");
            }
            catch (Exception ex)
            {
                MessageBox.Show("You don't have write permissions", "Error System Permissions", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            using (StreamWriter wr = new StreamWriter($"{today.ToString("yyyy-MM-dd-HHmmss")} - WeeklySales.html"))
            {
                wr.WriteLine(html);
            }
        }

        public static StringBuilder GenerateMonthlySales(SqlCommand command)
        {
            StringBuilder html = new StringBuilder();
            StringBuilder css = new StringBuilder();
            string info = "";
            SqlDataReader reader;

            command.ExecuteNonQuery();

            reader = command.ExecuteReader();

            css.Append("<style>");
            css.Append("td {padding:5px;text-align:center;font-weight:bold;text-align:center}");
            css.Append("</style>");

            html.Append("<html>");
            html.Append($"<head>{css}<title>{"Monthly Sales"}</title></head>");
            html.Append($"<body>");
            html.Append($"<h1>{"Monthly Sales"}</h1>");

            html.Append("<table>");
            html.Append("<tr><td colspan=7></td></tr>");

            html.Append("<tr>");
            html.Append("<td>OrderID</td>");
            html.Append("<td>Order Date</td>");
            html.Append("<td>Customer ID</td>");
            html.Append("<td>Product Name</td>");
            html.Append("<td>Quantity</td>");
            html.Append("<td>Price of Order</td>");
            html.Append("</tr>");

            html.Append("<tr>");
            while (reader.Read())
            {
                info = reader.GetString(6);
                html.Append($"<td>{reader.GetInt32(0)}</td>");
                html.Append($"<td>{Convert.ToDateTime(reader.GetDateTime(1)).ToString("dd/MM/yyyy")}</td>");
                html.Append($"<td>{reader.GetInt32(2)}</td>");
                html.Append($"<td>{reader.GetString(3)}</td>");
                html.Append($"<td>{reader.GetInt32(4)}</td>");
                html.Append($"<td>{reader.GetString(5)}</td>");


                html.Append("</tr>");
            }


            html.Append("<tr><td colspan=8 ></td></tr>");
            html.Append("</table>");

            if (info != "")
            {
                html.Append("<p>Total Monthly Sales</p>");
                html.Append($"<p>{info}</p>");
            }
            html.Append("</body></html>");

            reader.Close();
            return html;
        }

        public static void PrintMonthlySales(StringBuilder html)
        {

            DateTime today = DateTime.Now;
            try
            {
                using (StreamWriter wr = new StreamWriter($"{today.ToString("yyyy-MM-dd-HHmmss")}MonthlySales.html"))
                {
                    wr.WriteLine(html);
                }
                System.Diagnostics.Process.Start($@"{today.ToString("yyyy-MM-dd-HHmmss")}MonthlySales.html");
            }
            catch (Exception ex)
            {
                MessageBox.Show("You don't have write permissions", "Error System Permissions", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            using (StreamWriter wr = new StreamWriter($"{today.ToString("yyyy-MM-dd-HHmmss")} - MonthlySales.html"))
            {
                wr.WriteLine(html);
            }
        }

    }
}
