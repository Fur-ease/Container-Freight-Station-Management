using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Container_Freight_Station_Management
{
    public partial class Form1 : Form
    {
        OleDbConnection conn = new OleDbConnection();

        public Form1()
        {
            InitializeComponent();
            conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Admin\Documents\cfsdb.accdb; 
Persist Security Info=False;";

            txtpassword.PasswordChar = '*';
            txtusername.Focus();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = conn;


                // Use parameterized queries to prevent SQL injection
                command.CommandText = "SELECT * FROM users WHERE username = @username AND password = @password";
                command.Parameters.AddWithValue("@username", txtusername.Text);
                command.Parameters.AddWithValue("@password", txtpassword.Text);

                OleDbDataReader reader = command.ExecuteReader();
                int count = 0;
                while (reader.Read())
                {
                    count++;
                }

                if (count == 1)
                {
                    MessageBox.Show("Login successful");
                    this.Hide();
                    Home h = new Home();
                    h.Show();
                }
                else
                {
                    MessageBox.Show("Wrong username or password");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show( $"Error occureddd:  {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Clear the textboxes
            txtusername.Text = "";
            txtpassword.Text = "";

            // Set the focus to the username textbox
            txtusername.Focus();
        }
    }
    
}
