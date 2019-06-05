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

        public List<LogModelInput> GetLogsById(int customerId)
        {
            List<LogModelInput> logs = new List<LogModelInput>();

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
                    sqlCmd.Parameters.AddWithValue("@CustomerId", customerId);
                    using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                    {
                        while (sqlReader.Read())
                        {
                            logs.Add(new LogModelInput()
                            {
                                Id = int.Parse(sqlReader["Id"].ToString()),
                                CustomerId = int.Parse(sqlReader["CustomerId"].ToString()),
                                OperationType = int.Parse(sqlReader["OperationType"].ToString()),
                                Amount = int.Parse(sqlReader["Amount"].ToString()),
                                Date = DateTime.Parse(sqlReader["OperationDate"].ToString())
                            });
                        }
                    }
                    sqlCnn.Close();
                }
            }
            return logs;
        }

        public List<LogModelInput> GetLogsInSpecificPeriod(int customerId, DateTime from, DateTime to)
        {
            List<LogModelInput> logs = new List<LogModelInput>();

            string query = @"SELECT Id
                                ,CustomerId
                                ,OperationType
                                ,Amount
                                ,OperationDate
                            FROM OperationLog
                            WHERE @From <= OperationDate AND OperationDate <= @To AND CustomerId = @CustomerId";

            using (SqlConnection sqlCnn = new SqlConnection(_connectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCnn))
                {
                    sqlCnn.Open();
                    sqlCmd.Parameters.AddWithValue("@From", from);
                    sqlCmd.Parameters.AddWithValue("@To", to);
                    sqlCmd.Parameters.AddWithValue("@CustomerId", customerId);
                    using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                    {
                        while (sqlReader.Read())
                        {
                            logs.Add(new LogModelInput()
                            {
                                Id = int.Parse(sqlReader["Id"].ToString()),
                                CustomerId = int.Parse(sqlReader["CustomerId"].ToString()),
                                OperationType = int.Parse(sqlReader["OperationType"].ToString()),
                                Amount = int.Parse(sqlReader["Amount"].ToString()),
                                Date = DateTime.Parse(sqlReader["OperationDate"].ToString())
                            });
                        }
                    }
                    sqlCnn.Close();
                }
            }
            return logs;
        }

        public bool AddLog(LogModelOutput logToAdd)
        {
            string query = @"INSERT INTO OperationLog (CustomerId, OperationType, Amount) VALUES (@CustomerId, @OperationType, @Amount)";

            bool result = true;

            try
            {
                using (SqlConnection sqlCnn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(query, sqlCnn))
                    {
                        sqlCmd.Parameters.AddWithValue("@CustomerId", logToAdd.CustomerId);
                        sqlCmd.Parameters.AddWithValue("@OperationType", logToAdd.OperationType);
                        sqlCmd.Parameters.AddWithValue("@Amount", logToAdd.Amount);

                        sqlCnn.Open();
                        int rowsAffected = sqlCmd.ExecuteNonQuery();
                        sqlCnn.Close();
                    }
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
    }
}
