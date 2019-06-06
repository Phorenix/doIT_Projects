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

        private Customer ReadCustomer(SqlDataReader sqlReader)
        {
            Customer res = new Customer()
            {
                Id = int.Parse(sqlReader["Id"].ToString()),
                Name = sqlReader["Name"].ToString(),
                Surname = sqlReader["Surname"].ToString(),
                Credits = int.Parse(sqlReader["Credits"].ToString())
            };
            return res;
        }

        //public List<CustomerModelInput> GetAllNames(string sort = "id")
        //{
        //    List<CustomerModelInput> names = new List<CustomerModelInput>();


        //    string query = @"SELECT Id
        //                        ,Name
        //                        ,Surname
        //                        ,Credits
        //                    FROM Customers
        //                    ORDER BY CASE WHEN @Sort = 'id' THEN Id
        //                                  WHEN @Sort = 'credits' THEN Credits
        //                                  END,
        //                                  CASE
        //                                      WHEN @Sort = 'name' THEN Name
        //                                      WHEN @Sort = 'surname' THEN Surname
        //                                  END";

        //    using (SqlConnection sqlCnn = new SqlConnection(_connectionString))
        //    {
        //        using (SqlCommand sqlCmd = new SqlCommand(query, sqlCnn))
        //        {
        //            sqlCnn.Open();
        //            sqlCmd.Parameters.AddWithValue("@Sort", sort.ToLower());
        //            using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
        //            {
        //                while (sqlReader.Read())
        //                {
        //                    names.Add(new CustomerModelInput()
        //                    {
        //                        Id = int.Parse(sqlReader["Id"].ToString()),
        //                        Name = sqlReader["Name"].ToString(),
        //                        Surname = sqlReader["Surname"].ToString(),
        //                        Credits = int.Parse(sqlReader["Credits"].ToString())
        //                    });
        //                }
        //            }
        //            sqlCnn.Close();
        //        }
        //    }
        //    return names;
        //}

        public List<Customer> GetAllNames(string sort = "id")
        {
            List<Customer> names = new List<Customer>(); 


            string query = @"SELECT Id
                                ,Name
                                ,Surname
                                ,Credits
                            FROM Customers
                            ORDER BY " + sort ;

            using (SqlConnection sqlCnn = new SqlConnection(_connectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCnn))
                {
                    sqlCnn.Open();
                    //sqlCmd.Parameters.AddWithValue("@Sort", sort.ToLower());
                    using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                    {
                        while (sqlReader.Read())
                        {
                            names.Add(ReadCustomer(sqlReader));
                        }
                    }
                    sqlCnn.Close();
                }
            }
            return names;
        }

        public Customer GetById(int id)
        {
            Customer customerToReturn = null;

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
                            customerToReturn = ReadCustomer(sqlReader);
                        }
                    }
                    sqlCnn.Close();
                }
            }
            return customerToReturn;
        }

        public bool AddCustomer(Customer customer)
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

        public bool UpdateCustomer(int id, Customer customer)
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
