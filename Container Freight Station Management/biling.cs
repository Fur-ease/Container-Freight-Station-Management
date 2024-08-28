using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace Container_Freight_Station_Management
{
    public partial class billing : UserControl
    {
        private string staffID; // Assuming you have a way to set this value when the user logs in

        public billing()
        {
            InitializeComponent();
        }

        public billing(string loggedInStaffID)
        {
            InitializeComponent();
            staffID = loggedInStaffID;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string containerNumber = textBox1.Text;
            string yardNumber = textBox3.Text;
            DateTime arrivalDate = DateTime.Parse(dateTimePicker1.Text);
            string paymentMethod = textBox4.Text; // Retrieve payment method from the textbox

            if (string.IsNullOrEmpty(containerNumber) ||
                string.IsNullOrEmpty(yardNumber) ||
                string.IsNullOrEmpty(paymentMethod))
            {
                // Display a message to fill all the fields
                MessageBox.Show("Please fill in all the fields.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Exit the method to prevent further processing
            }

            // Connection string to the Access database
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Admin\Documents\cfsdb.accdb; Persist Security Info=False;";

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    // Retrieve the logged-in staff's name
                    string staffQuery = "SELECT Name FROM users WHERE Name = @Name";
                    string staffName = string.Empty;

                    using (OleDbCommand staffCommand = new OleDbCommand(staffQuery, connection))
                    {
                        staffCommand.Parameters.AddWithValue("@Name", Name); // Use the correct parameter for StaffID
                        staffName = staffCommand.ExecuteScalar()?.ToString();
                    }

                    // Calculate the days stayed
                    DateTime currentDate = DateTime.Now;
                    int daysStayed = (currentDate - arrivalDate).Days;

                    // Calculate the amount paid (rate of $50 per day)
                    decimal amountPaid = daysStayed * 50m;

                    // Generate a unique receipt number
                    string receiptNumber = GenerateReceiptNumber();

                    // Generate the receipt message
                    string receiptMessage = $"RECEIPT\n\n" +
                                            $"Receipt Number: {receiptNumber}\n" +
                                            $"Container Number: {containerNumber}\n" +
                                            $"Yard Number: {yardNumber}\n" +
                                            $"Arrival Date: {arrivalDate.ToShortDateString()}\n" +
                                            $"Days Stayed: {daysStayed}\n" +
                                            $"Amount Paid: ${amountPaid}\n" +
                                            $"Payment Method: {paymentMethod}\n\n" +
                                            $"Processed by: {staffName}";

                    // Display the receipt
                    MessageBox.Show(receiptMessage, "Payment Receipt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (OleDbException oleDbEx)
            {
                MessageBox.Show("Database error: " + oleDbEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadClearedContainersDataGridView()
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Admin\Documents\cfsdb.accdb;Persist Security Info=False;";
            string query = "SELECT * FROM clearedContainers";

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
            }
            catch (OleDbException oleDbEx)
            {
                MessageBox.Show("Database error: " + oleDbEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private string GenerateReceiptNumber()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[8];
                rng.GetBytes(randomBytes);
                return BitConverter.ToUInt64(randomBytes, 0).ToString("X8");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Admin\Documents\cfsdb.accdb;Persist Security Info=False;";
            string query = "SELECT * FROM clearedContainers";

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
            }
            catch (OleDbException oleDbEx)
            {
                MessageBox.Show("Database error: " + oleDbEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
