using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Container_Freight_Station_Management
{
    public partial class clearingagent : UserControl
    {
        public clearingagent()
        {
            InitializeComponent();
            LoadDataGridView(); // Ensure the DataGridView is loaded initially
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Clear all text boxes and set focus to the portpassNumber textbox
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";

            textBox2.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Retrieve input values
            string firstName = textBox1.Text;
            string secondName = textBox4.Text;
            string companyName = textBox3.Text;
            string portpassNumber = textBox2.Text;

            // Validate that all fields are filled in
            if (string.IsNullOrEmpty(firstName) ||
                string.IsNullOrEmpty(secondName) ||
                string.IsNullOrEmpty(companyName) ||
                string.IsNullOrEmpty(portpassNumber))
            {
                MessageBox.Show("Please fill in all the fields.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Admin\Documents\cfsdb.accdb;Persist Security Info=False;";

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    // Check if the portpassNumber already exists
                    string checkQuery = "SELECT COUNT(*) FROM clearingAgents WHERE portpassNumber = ?";
                    using (OleDbCommand checkCommand = new OleDbCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("?", portpassNumber);
                        int count = (int)checkCommand.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show("This portpass number is already in use. Please use a different one.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // SQL Insert command to add new clearing agent
                    string query = "INSERT INTO clearingAgents ([firstName], [secondName], [companyName], [portpassNumber]) VALUES (?, ?, ?, ?)";

                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("?", firstName);
                        command.Parameters.AddWithValue("?", secondName);
                        command.Parameters.AddWithValue("?", companyName);
                        command.Parameters.AddWithValue("?", portpassNumber);

                        int result = command.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Clearing Agent added successfully.");
                            LoadDataGridView(); // Reload data to reflect the new entry
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
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Admin\Documents\cfsdb.accdb;Persist Security Info=False;";
            string query = "SELECT * FROM clearingAgents";

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
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                string firstName = row.Cells["firstName"].Value.ToString();
                string secondName = row.Cells["secondName"].Value.ToString();
                string portpassNumber = row.Cells["portpassNumber"].Value.ToString();
                string companyName = row.Cells["companyName"].Value.ToString();

                MessageBox.Show($"First Name: {firstName}\n" +
                                $"Second Name: {secondName}\n" +
                                $"Portpass Number: {portpassNumber}\n" +
                                $"Company Name: {companyName}",
                                "Cell Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && dataGridView1.SelectedRows.Count > 0)
            {
                string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Admin\Documents\cfsdb.accdb;Persist Security Info=False;";
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                string portpassNumber = selectedRow.Cells["portpassNumber"].Value.ToString();

                var confirmResult = MessageBox.Show("Are you sure to delete this item?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                        using (OleDbConnection connection = new OleDbConnection(connectionString))
                        {
                            connection.Open();

                            string deleteQuery = "DELETE FROM clearingAgents WHERE portpassNumber = ?";
                            using (OleDbCommand command = new OleDbCommand(deleteQuery, connection))
                            {
                                command.Parameters.AddWithValue("?", portpassNumber);
                                int result = command.ExecuteNonQuery();

                                if (result > 0)
                                {
                                    MessageBox.Show("Clearing Agent deleted successfully.");
                                    LoadDataGridView(); // Reload the DataGridView to remove the deleted row
                                }
                                else
                                {
                                    MessageBox.Show("Failed to delete data.");
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
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
