using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatabaseConnectedWriting
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int number = 4;            
            string connectionString = null;
            string query = null;
            SqlConnection sqlCnn;
            SqlCommand sqlCmd;

            connectionString = ConfigurationManager.ConnectionStrings["StringConnection1"].ToString();
            query = "INSERT INTO Numbers (Number) VALUES (@Number)";
  

            try
            {
                using (sqlCnn = new SqlConnection(connectionString))
                {
                    using (sqlCmd = new SqlCommand(query, sqlCnn))
                    {
                        // sqlCmd.CommandType = CommandType.Text;
                        sqlCmd.Parameters.AddWithValue("@Number", number);

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
