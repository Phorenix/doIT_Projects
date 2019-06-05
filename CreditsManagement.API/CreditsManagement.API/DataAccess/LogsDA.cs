using CreditsManagement.API.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CreditsManagement.API.DataAccess
{
    public class LogsDA
    {
        private string _connectionString;

        public LogsDA(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<LogModelInput> GetAllNames()
        {
            List<LogModelInput> names = new List<LogModelInput>();

            string query = @"SELECT Id
                                ,CustomerId
                                ,OperationType
                                ,Amount
                                ,OperationDate
                            FROM OperationLog
                            WHERE CustomerId = @CustomerId";

            using (SqlConnection sqlCnn = new SqlConnection(_connectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCnn))
                {
                    sqlCnn.Open();
                    using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                    {
                        while (sqlReader.Read())
                        {
                            names.Add(new LogModelInput()
                            {
                                Id = int.Parse(sqlReader["Id"].ToString()),
                                CustomerId = sqlReader["Name"].ToString(),
                                Surname = sqlReader["Surname"].ToString(),
                                Credits = int.Parse(sqlReader["Credits"].ToString())
                            });
                        }
                    }
                    sqlCnn.Close();
                }
            }
            return names;
        }
    }
}
