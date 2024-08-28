using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Container_Freight_Station_Management
{
    public partial class Addcontainer : UserControl
    {
        public Addcontainer()
        {
            InitializeComponent();
            LoadDataGridView();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            containernumber.Text = "";
            sealnumber.Text = "";
            comboBox1.Text = "";


            containernumber.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string containerNumber = containernumber.Text;
            string sealNumber = sealnumber.Text;
            string yardNumber = comboBox1.Text;
            string size = textBox1.Text;
            string arrivalDate = dateTimePicker1.Value.ToString("MM/dd/yyyy");

            if (string.IsNullOrEmpty(containerNumber) ||
        string.IsNullOrEmpty(sealNumber) ||
        string.IsNullOrEmpty(yardNumber) ||
        string.IsNullOrEmpty(size))
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

                    // Check if the container number and seal number combination already exists
                    string checkQuery = "SELECT COUNT(*) FROM containers WHERE [ContainerNumber] = ? AND [SealNumber] = ?";
                    using (OleDbCommand checkCommand = new OleDbCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("?", containerNumber);
                        checkCommand.Parameters.AddWithValue("?", sealNumber);
                        int count = (int)checkCommand.ExecuteScalar();

                        if (count > 0)
                        {
                            // Container number already exists
                            MessageBox.Show("This container already exists in the database.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return; // Exit the method to prevent further processing
                        }
                    }

                    // SQL Insert command
                    string query = "INSERT INTO containers ([ContainerNumber], [SealNumber], [YardNumber], [Size], [ArrivalDate]) VALUES (?, ?, ?, ?, ?)";

                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        // Adding parameters to avoid SQL injection
                        command.Parameters.AddWithValue("?", containerNumber);
                        command.Parameters.AddWithValue("?", sealNumber);
                        command.Parameters.AddWithValue("?", yardNumber);
                        command.Parameters.AddWithValue("?", size);
                        command.Parameters.AddWithValue("?", arrivalDate);

                        // Execute the command
                        int result = command.ExecuteNonQuery();

                        // Check if the insert was successful
                        if (result > 0)
                        {
                            MessageBox.Show("Container added successfully.");
                            LoadDataGridView();
                        }
                        else
                        {
                            MessageBox.Show("Failed to add data.");  
                        }
                    }
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

         private void LoadDataGridView()
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Admin\Documents\cfsdb.accdb; Persist Security Info=False;";
            string query = "SELECT * FROM containers";

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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Get the selected row
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Extract data from the selected row
                string containerNumber = row.Cells["ContainerNumber"].Value.ToString();
                string sealNumber = row.Cells["SealNumber"].Value.ToString();
                string yardNumber = row.Cells["YardNumber"].Value.ToString();
                string size = row.Cells["Size"].Value.ToString();
                string arrivalDate = row.Cells["ArrivalDate"].Value.ToString();

                // Show the details in a message box (for demonstration purposes)
                MessageBox.Show($"Container Number: {containerNumber}\n" +
                                $"Seal Number: {sealNumber}\n" +
                                $"Yard Number: {yardNumber}\n" +
                                $"Size: {size}\n" +
                                $"Arrival Date: {arrivalDate}",
                                "Cell Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Get the selected row
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Extract data from the selected row
                string containerNumber = row.Cells["ContainerNumber"].Value.ToString();
                string sealNumber = row.Cells["SealNumber"].Value.ToString();
                string yardNumber = row.Cells["YardNumber"].Value.ToString();
                string size = row.Cells["Size"].Value.ToString();
                string arrivalDate = row.Cells["ArrivalDate"].Value.ToString();

                // Show the details in a message box (for demonstration purposes)
                MessageBox.Show($"Container Number: {containerNumber}\n" +
                                $"Seal Number: {sealNumber}\n" +
                                $"Yard Number: {yardNumber}\n" +
                                $"Size: {size}\n" +
                                $"Arrival Date: {arrivalDate}",
                                "Cell Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
