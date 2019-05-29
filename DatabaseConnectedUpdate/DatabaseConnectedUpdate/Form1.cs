using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DatabaseConnectedUpdate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int number = 10;
            int id = 4;
            string connectionString = null;
            string query = null;
            SqlConnection sqlCnn;
            SqlCommand sqlCmd;

            connectionString = ConfigurationManager.ConnectionStrings["StringConnection1"].ToString();
            query = "UPDATE Numbers SET Number = @Number WHERE ID = @Id";


            try
            {
                using (sqlCnn = new SqlConnection(connectionString))
                {
                    using (sqlCmd = new SqlCommand(query, sqlCnn))
                    {
                        // sqlCmd.CommandType = CommandType.Text;
                        sqlCmd.Parameters.AddWithValue("@Number", number);
                        sqlCmd.Parameters.AddWithValue("@ID", id);
                                            
                        sqlCnn.Open();
                        int rowsAffected = sqlCmd.ExecuteNonQuery();
                        sqlCnn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot open connection! Message: {ex.Message}");
            }
        }
    }
}
