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
    public partial class reports : UserControl
    {
        public reports()
        {
            InitializeComponent();
        }

        private void dashboard1_Load(object sender, EventArgs e)
        {

        }

        private void reports_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            // Connection string to the Access database
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Admin\Documents\cfsdb.accdb; Persist Security Info=False;";

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    // Simple query to retrieve all records (for testing purposes)
                    string query = "SELECT * FROM containers ORDER BY ArrivalDate DESC";

                    using (OleDbCommand cmd = new OleDbCommand(query, connection))
                    {
                        // Execute the command and fill the DataGridView with the results
                        using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            dataAdapter.Fill(dt);

                            // Check if any rows were returned
                            if (dt.Rows.Count == 0)
                            {
                                MessageBox.Show("No records found in the containers table.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                            dataGridView1.DataSource = dt;
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

        private void btnreport_Click(object sender, EventArgs e)
        {

        }
    }
}
