using CreditsManagement.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CreditsManagement.API.DataAccess
{
    public class CustomersDA
    {
        private string _connectionString;

        public CustomersDA(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<CustomerModelInput> GetAllNames()
        {
            List<CustomerModelInput> names = new List<CustomerModelInput>();

            string query = @"SELECT Id
                                ,Name
                                ,Surname
                                ,Credits
                            FROM Customers";

            using (SqlConnection sqlCnn = new SqlConnection(_connectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCnn))
                {
                    sqlCnn.Open();
                    using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                    {
                        while (sqlReader.Read())
                        {
                            names.Add(new CustomerModelInput()
                            {
                                Id = int.Parse(sqlReader["Id"].ToString()),
                                Name = sqlReader["Name"].ToString(),
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

        public CustomerModelInput GetById(int id)
        {
            CustomerModelInput result = null;

            string query = @"SELECT Id
                                ,Name
                                ,Surname
                                ,Credits
                            FROM Customers
                            WHERE Id = @Id";

            using (SqlConnection sqlCnn = new SqlConnection(_connectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCnn))
                {
                    sqlCnn.Open();
                    sqlCmd.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                    {
                        if (sqlReader.HasRows)
                        {
                            sqlReader.Read();
                            result = new CustomerModelInput()
                            {
                                Id = int.Parse(sqlReader["Id"].ToString()),
                                Name = sqlReader["Name"].ToString(),
                                Surname = sqlReader["Surname"].ToString(),
                                Credits = int.Parse(sqlReader["Credits"].ToString())
                            };
                        }
                    }
                    sqlCnn.Close();
                }
            }
            return result;
        }

        public bool AddCustomer(CustomerModelOutput customer)
        {
            string query = @"INSERT INTO Customers (Name, Surname, Credits) VALUES (@Name, @Surname, @Credits)";

            bool result = true;

            try
            {
                using (SqlConnection sqlCnn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(query, sqlCnn))
                    {
                        sqlCmd.Parameters.AddWithValue("@Name", customer.Name);
                        sqlCmd.Parameters.AddWithValue("@Surname", customer.Surname);
                        sqlCmd.Parameters.AddWithValue("@Credits", customer.Credits);

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

        public bool DeleteById(int id)
        {
            string query = @"DELETE FROM Customers WHERE Id = @Id";

            bool result = true;

            try
            {
                using (SqlConnection sqlCnn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(query, sqlCnn))
                    {
                        sqlCmd.Parameters.AddWithValue("@Id", id);

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

        public bool UpdateCustomer(int id, CustomerModelUpdate customer)
        {
            string query = @"UPDATE Customers SET Name = @Name
	                            ,Surname = @Surname
	                            ,Credits = @Credits
                            WHERE Id = @Id";

            bool result = true;

            try
            {
                using (SqlConnection sqlCnn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(query, sqlCnn))
                    {
                        sqlCmd.Parameters.AddWithValue("@Id", id);
                        sqlCmd.Parameters.AddWithValue("@Name", customer.Name);
                        sqlCmd.Parameters.AddWithValue("@Surname", customer.Surname);
                        sqlCmd.Parameters.AddWithValue("@Credits", customer.Credits);

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
