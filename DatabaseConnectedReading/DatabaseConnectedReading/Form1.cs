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

namespace DatabaseConnectedReading
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string connectionString = null;
            string query = null;
            SqlConnection sqlCnn;
            SqlCommand sqlCmd;
            SqlDataReader sqlReader = null;


            connectionString = ConfigurationManager.ConnectionStrings["StringConnection1"].ToString();
            query = "SELECT [ID]" +
                ",[Number]" +
                "FROM [TESTDB].[dbo].[Numbers]" +
                "ORDER BY [ID]";


            try
            {
                using (sqlCnn = new SqlConnection(connectionString))
                {
                    using (sqlCmd = new SqlCommand(query, sqlCnn))
                    {
                        sqlCnn.Open();
                        using (sqlReader = sqlCmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(sqlReader);

                            dgvOutput.DataSource = dt;
                        }
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
