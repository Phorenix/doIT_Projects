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

namespace DatabaseConnectedDelete
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int number = 3;
            string connectionString = ConfigurationManager.ConnectionStrings["StringConnection1"].ToString();
            string query = "DELETE FROM Numbers WHERE Number = @Number";


            try
            {
                using (SqlConnection sqlCnn = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(query, sqlCnn))
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
