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
            string connectionString = null;
            string query = null;
            SqlConnection sqlCnn;
            SqlCommand sqlCmd;


            //SqlDataAdapter adapter = new SqlDataAdapter();

            query = "Insert into Numbers (Number) values (3)";

            connectionString = ConfigurationManager.ConnectionStrings["StringConnection1"].ToString();
            sqlCnn = new SqlConnection(connectionString);


            try
            {
                sqlCnn.Open();
                sqlCmd = new SqlCommand(query, sqlCnn);
                sqlCmd.ExecuteNonQuery();

                //adapter.InsertCommand = new SqlCommand(query, sqlCnn);
                //adapter.InsertCommand.ExecuteNonQuery();


                sqlCmd.Dispose();
                sqlCnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot open connection! Message: {ex.Message}");
            }
        }
    }
}
