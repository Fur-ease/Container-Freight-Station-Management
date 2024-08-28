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
    public partial class Dashboard : UserControl
    {
        public Dashboard()
        {
            InitializeComponent();
            UpdateTotalContainerCount();  // Load total container count when the form loads
        }

        private void UpdateTotalContainerCount()
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Admin\Documents\cfsdb.accdb; Persist Security Info=False;";
            string query = "SELECT COUNT(*) FROM containers";

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        int totalContainers = (int)command.ExecuteScalar();
                        label2.Text = $" {totalContainers}";
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

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateTotalContainerCount(); // Update the label when the button is clicked
        }
    }
}
