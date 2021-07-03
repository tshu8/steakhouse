using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Mail;


namespace GroupSteakHouseProject
{
    class ProgOps
    {
        //connection string
        private const string CONNECT_STRING = @"Server=cstnt.tstc.edu;Database=inew2330su21;User Id=group1su212330;password=1587159";
        //build a connection to database
        private static SqlConnection _cntDatabase = new SqlConnection(CONNECT_STRING);

        //Product Information
        public static SqlCommand _sqlProductsCommand;
        //data adaptor
        public static SqlDataAdapter _daProducts = new SqlDataAdapter();
        //data table
        public static DataTable _dtProductsTable = new DataTable();

        public static DataTable _dtProductDescriptionTable = new DataTable();


        public static void OpenDatabase()
        {
            //method to open database
            try
            {
                _cntDatabase.Open();
            }
            catch (Exception ex)
            {
                _cntDatabase.Close();
                OpenDatabase();
            }
        }

        public static void CloseDatabaseDispose()
        {
            //method to close database and dispose of the connection object
            //close connection
            _cntDatabase.Close();
            //dispose of the connection object and command, adapter and table objects
            _cntDatabase.Dispose();
        }

        public static void CloseDatabase()
        {
            //method to close database and dispose of the connection object
            //close connection
            _cntDatabase.Close();
        }

        public static void Login(TextBox tbxUsername, TextBox tbxPassword, frmLogin login, frmMenu menu)
        {

            OpenDatabase();

            //strings command for userId 
            string sqlquery = "SELECT userID, username, password, securityLevel FROM group1su212330.Login WHERE userID = @userID";            

            //command query
            SqlCommand cmd = new SqlCommand(sqlquery, _cntDatabase);
            cmd.Parameters.AddWithValue("@username", tbxUsername.Text);
            SqlDataReader rd = cmd.ExecuteReader();

            //if username is found
            if (rd.Read())
            {
                //sets strings for returned values
                string UserID = rd.GetValue(0).ToString();
                string Username = rd.GetValue(1).ToString();
                string Password = rd.GetValue(2).ToString();
                string SecurityLevel = rd.GetValue(3).ToString();

                int intSecLevel = 0;
                int intUserID = 0;

                int.TryParse(SecurityLevel, out intSecLevel);
                int.TryParse(UserID, out intUserID);

                //if returned username is same as one entered
                if (Username == tbxUsername.Text)
                {
                    //checks if password is same as one entered
                    if (Password == tbxPassword.Text)
                    {
                        //opens up next form and closes reader
                        menu.intSecurityLevel = intSecLevel;
                        menu.UserID = intUserID;
                        menu.Show();
                        login.Hide();
                        rd.Close();
                    }

                    //if password is not same error pops up and reader closes
                    else
                    {
                        MessageBox.Show("Password is Incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        rd.Close();
                    }
                }
                else
                {
                    //username not found message
                    MessageBox.Show("Username is not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    rd.Close();
                }
            }

        }

        public static void SignupCommand(TextBox tbxEmail, TextBox tbxPassword, TextBox tbxReInput, TextBox tbxFirstName, TextBox tbxLastName, TextBox tbxAddress, TextBox tbxPhone)
        {

            OpenDatabase();

            //string commands to create a new user
            string strNewID = "";
            string sqlquery = "SELECT top 1 userID FROM group1su212330.Users ORDER BY userID DESC";
            string sqlquery2 = "INSERT INTO group1su212330.Users (userId, securityLevel, name, phone, email, address, password) VALUES (@userID, 1, @Name, @Phone, @Email, @Address, @Password)";

            //executes query to get last known id
            SqlCommand cmd = new SqlCommand(sqlquery, _cntDatabase);
            SqlDataReader rd = cmd.ExecuteReader();

            //sets last known id to +1
            if (rd.Read())
            {
                strNewID = rd.GetValue(0).ToString();
                int intNewID = int.Parse(strNewID) + 1;
                strNewID = intNewID.ToString();
                rd.Close();
            }

            //sets up query 2 to insert user data
            SqlCommand cmd2 = new SqlCommand(sqlquery2, _cntDatabase);
            cmd2.Parameters.AddWithValue("@userID", strNewID);
            cmd2.Parameters.AddWithValue("@Name",  tbxFirstName.Text + " " + tbxLastName.Text);
            cmd2.Parameters.AddWithValue("@Phone", tbxPhone.Text);
            cmd2.Parameters.AddWithValue("@Email", tbxEmail.Text);
            cmd2.Parameters.AddWithValue("@Address", tbxAddress.Text);
            cmd2.Parameters.AddWithValue("@Password", tbxPassword.Text);

            //executes query2 and then closes reader
            SqlDataReader rd2 = cmd2.ExecuteReader();
            rd2.Close();

            //gets current user's new id
            SqlCommand cmd3 = new SqlCommand(sqlquery, _cntDatabase);
            SqlDataReader rd3 = cmd3.ExecuteReader();

            //displays new id
            if (rd3.Read())
            {
                strNewID = rd3.GetValue(0).ToString();
                MessageBox.Show("Your new Username is " + strNewID, "Username");

                MailMessage message = new MailMessage("group1su212330@gmail.com", tbxEmail.Text);

                //String Builder
                StringBuilder sbEmail = new StringBuilder();
                sbEmail.Append("Dear " + tbxEmail.Text + ",<br/><br/>");
                sbEmail.Append("Thank You for signing up to group one's Steak House.");
                sbEmail.Append("<br/>");
                sbEmail.Append("Your new Username is " + strNewID);

                message.IsBodyHtml = true;

                //email message
                message.Subject = "Steaks SignUp";
                message.Body = sbEmail.ToString();

                //client setup
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.UseDefaultCredentials = false;
                NetworkCredential credential = new NetworkCredential("group1su212330@gmail.com", "inew2330su21");
                client.Credentials = credential;
                client.EnableSsl = true;
                client.Send(message);

                //message displaying that email has been sent
                MessageBox.Show("Email has been sent to you containing your new username.", "Email sent");

                rd3.Close();
            }

            CloseDatabase();


        }

        public static void InitProductDatabaseCommand(DataGridView dgvFood)
        {
            //set command object to null
            _sqlProductsCommand = null;

            //reset data adaptor and datatable to new
            _daProducts = new SqlDataAdapter();
            _dtProductsTable = new DataTable();

            _dtProductDescriptionTable = new DataTable();

            string strDGVCommand = "Select productName, format(ourCost, 'C') as 'Item Price', quantityOnHand from group1su212330.Inventory";
            
            //set command object to null
            _sqlProductsCommand = null;
            //FILLS TABLE AND OBJECTS
            try
            {
                //est cmd obj
                _sqlProductsCommand = new SqlCommand(strDGVCommand, _cntDatabase);
                //establish data adaptor
                _daProducts.SelectCommand = _sqlProductsCommand;
                //fill data table
                _daProducts.Fill(_dtProductsTable);
                //bind dgv to data table
                dgvFood.DataSource = _dtProductsTable;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error in SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //dispose of cmd, adaptor, and table 
            _sqlProductsCommand.Dispose();
            _daProducts.Dispose();
            _dtProductsTable.Dispose();
        }

        public static void DatabaseCommandMakeOrder(List<string> ProductNames, List<int> QuantitiesPurchased, List<double> Prices, double totalPrice, int UserID)
        {

            List<int> id = new List<int>();
            int OrderID = 0;

            if (OrderID == 0)
            {
                int OrderIDConvert = 0;
                string Query = $"Select top 1 orderID from group1su212330.Orders order by orderID desc";
                SqlCommand cmd = new SqlCommand(Query, _cntDatabase);
                SqlDataReader rd = cmd.ExecuteReader();


                if (rd.Read())
                {
                    var intOrderID = rd["orderID"];
                    int.TryParse(intOrderID.ToString(), out OrderIDConvert);
                }

                rd.Close();

                OrderID = OrderIDConvert + 30;
            }

            for (int i = 0; i < ProductNames.Count; i++)
            {
                string Query = $"Select productID from group1su212330.Inventory where productName = '{ProductNames[i]}'";
                SqlCommand cmd = new SqlCommand(Query, _cntDatabase);
                SqlDataReader rd = cmd.ExecuteReader();

                if (rd.Read())
                {
                    var intID = rd["productID"];

                    int IDConvert;

                    int.TryParse(intID.ToString(), out IDConvert);

                    id.Add(IDConvert);
                }
                rd.Close();

            }
            string SaleQuery = $"Insert into group1su212330.Orders (orderID, order_Date, saleTotal, userID) Values ({OrderID}, GetDate(), {totalPrice}, {UserID});";
            for (int i = 0; i < ProductNames.Count; i++)
            {
                SaleQuery += $"\nInsert into group1su212330.OrderDetails (productID, orderID, Price, qtyOrdered, itemTotal) values ({id[i]}, {OrderID}, {Prices[i]}, {QuantitiesPurchased[i]}, {totalPrice})";
            }

            for (int i = 0; i < ProductNames.Count; i++)
            {
                SaleQuery += $"\nUpdate group1su212330.Inventory set quantityOnHand = quantity- {QuantitiesPurchased[i]} where name = '{ProductNames[i]}'";
            }

            SqlCommand myCommand = new SqlCommand(SaleQuery, _cntDatabase);

            myCommand.ExecuteNonQuery();

        }


    }
}
