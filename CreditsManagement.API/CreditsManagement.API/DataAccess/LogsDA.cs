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

        //------------------------------------------
        // Methods to simplify the code
        //------------------------------------------

        private List<Log> ReadAllLogsFromCmd(SqlCommand sqlCmd)
        {
            List<Log> logs = new List<Log>();
            using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
            {
                while (sqlReader.Read())
                {
                    logs.Add(ReadLog(sqlReader));
                }
            }

            return logs;
        
        }

        private Log ReadLog(SqlDataReader sqlReader)
        {
            Log res = new Log()
            {
                Id = int.Parse(sqlReader["Id"].ToString()),
                CustomerId = int.Parse(sqlReader["CustomerId"].ToString()),
                CustomerName = sqlReader["CustomerName"].ToString(),
                CustomerSurname = sqlReader["CustomerSurname"].ToString(),
                OperationType = int.Parse(sqlReader["OperationType"].ToString()),
                Amount = int.Parse(sqlReader["Amount"].ToString()),
                Date = (DateTime)sqlReader["OperationDate"]
                // Date = sqlReader["OperationDate"] != DBNull.Value ? (DateTime) sqlReader["OperationDate"] : (DateTime?) null
            };
            return res;
        }

        private List<Log> GetLogsSingleDate(string query, DateTime date)
        {
            List<Log> logs = new List<Log>();

            using (SqlConnection sqlCnn = new SqlConnection(_connectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCnn))
                {
                    sqlCnn.Open();
                    sqlCmd.Parameters.AddWithValue("@Date", date);

                    logs = ReadAllLogsFromCmd(sqlCmd);

                    sqlCnn.Close();
                }
            }

            return logs;

        }

        private List<Log> ReadLogsByIdSingleDate(int customerId, string query, DateTime date)
        {
            List<Log> logs = new List<Log>();

            using (SqlConnection sqlCnn = new SqlConnection(_connectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCnn))
                {
                    sqlCnn.Open();
                    sqlCmd.Parameters.AddWithValue("@Date", date);
                    sqlCmd.Parameters.AddWithValue("@CustomerId", customerId);

                    logs = ReadAllLogsFromCmd(sqlCmd);

                    sqlCnn.Close();
                }
            }

            return logs;

        }

        private string ShortQuerySelect()
        {
            string query = @"SELECT L.Id
                                , L.CustomerId
	                            ,C.Name AS CustomerName
	                            ,C.Surname AS CustomerSurname
	                            ,L.OperationType
	                            ,L.Amount
	                            ,L.OperationDate
                            FROM OperationLog AS L
                            INNER JOIN Customers AS C ON L.CustomerID = C.ID ";
            return query;
        }

        //------------------------------------------
        // Methods to Get all logs
        //------------------------------------------

        public List<Log> GetAllLogs()
        {
            List<Log> logs = new List<Log>();

            // Name, Surname
            string query = $@"{ShortQuerySelect()}
                              ORDER BY L.OperationDate DESC";

            using (SqlConnection sqlCnn = new SqlConnection(_connectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCnn))
                {
                    sqlCnn.Open();

                    logs = ReadAllLogsFromCmd(sqlCmd);

                    sqlCnn.Close();
                }
            }
            return logs;
        }

        public List<Log> GetAllLogsInSpecificPeriod(DateTime fromDate, DateTime toDate)
        {
            List<Log> logs = new List<Log>();

            string query = $@"{ShortQuerySelect()}
                              WHERE @From <= L.OperationDate AND L.OperationDate <= @To
                              ORDER BY L.OperationDate DESC";

            using (SqlConnection sqlCnn = new SqlConnection(_connectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCnn))
                {
                    sqlCnn.Open();
                    sqlCmd.Parameters.AddWithValue("@From", fromDate);
                    sqlCmd.Parameters.AddWithValue("@To", toDate);

                    logs = ReadAllLogsFromCmd(sqlCmd);

                    sqlCnn.Close();
                }
            }
            return logs;
        }

        public List<Log> GetAllLogsUntilDate(DateTime toDate)
        {
            List<Log> logs = new List<Log>();

            string query = $@"{ShortQuerySelect()}
                              WHERE L.OperationDate <= @Date
                              ORDER BY L.OperationDate DESC";

            logs = GetLogsSingleDate(query, toDate);

            return logs;
        }

        public List<Log> GetAllLogsFromDate(DateTime fromDate)
        {
            List<Log> logs = new List<Log>();

            string query = $@"{ShortQuerySelect()}
                              WHERE @Date <= L.OperationDate
                              ORDER BY L.OperationDate DESC";

            logs = GetLogsSingleDate(query, fromDate);

            return logs;
        }

        //------------------------------------------
        // Methods to Get logs by customerId
        //------------------------------------------

        public List<Log> GetLogsById(int customerId)
        {
            List<Log> logs = new List<Log>();

            string query = $@"{ShortQuerySelect()}
                              WHERE L.CustomerId = @CustomerId
                              ORDER BY L.OperationDate DESC";

            using (SqlConnection sqlCnn = new SqlConnection(_connectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCnn))
                {
                    sqlCnn.Open();
                    sqlCmd.Parameters.AddWithValue("@CustomerId", customerId);

                    logs = ReadAllLogsFromCmd(sqlCmd);

                    sqlCnn.Close();
                }
            }
            return logs;
        }

        public List<Log> GetLogsInSpecificPeriod(int customerId, DateTime fromDate, DateTime toDate)
        {
            List<Log> logs = new List<Log>();

            string query = $@"{ShortQuerySelect()}
                              WHERE @From <= L.OperationDate AND L.OperationDate <= @To AND L.CustomerId = @CustomerId
                              ORDER BY L.OperationDate DESC";

            using (SqlConnection sqlCnn = new SqlConnection(_connectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCnn))
                {
                    sqlCnn.Open();
                    sqlCmd.Parameters.AddWithValue("@From", fromDate);
                    sqlCmd.Parameters.AddWithValue("@To", toDate);
                    sqlCmd.Parameters.AddWithValue("@CustomerId", customerId);

                    logs = ReadAllLogsFromCmd(sqlCmd);

                    sqlCnn.Close();
                }
            }
            return logs;
        }

        public List<Log> GetLogsByIdUntilDate(int customerId, DateTime toDate)
        {
            List<Log> logs = new List<Log>();

            string query = $@"{ShortQuerySelect()}
                              WHERE L.OperationDate <= @Date AND L.CustomerId = @CustomerId
                              ORDER BY L.OperationDate DESC";

            logs = ReadLogsByIdSingleDate(customerId, query, toDate);

            return logs;
        }

        public List<Log> GetLogsByIdFromDate(int customerId, DateTime fromDate)
        {
            List<Log> logs = new List<Log>();

            string query = $@"{ShortQuerySelect()}
                              WHERE @Date <= L.OperationDate AND L.CustomerId = @CustomerId
                              ORDER BY L.OperationDate DESC";

            logs = ReadLogsByIdSingleDate(customerId, query, fromDate);

            return logs;
        }

    }
}
