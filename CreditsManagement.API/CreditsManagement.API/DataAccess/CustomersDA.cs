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

            if (sort.Split(' ').Length > 1 
                 || sort.Contains('\''))
                throw new Exception("Simone non mi freghi!");

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

        //public void AddCustomerAndLog(Customer customerToAdd)
        //{
        //    string query = @"BEGIN TRY
        //                        BEGIN TRANSACTION
        //                            DECLARE @NewCId int
        //                            INSERT INTO Customers (Name, Surname, Credits) VALUES (@Name, @Surname, @Credits)
        //                            SET @NewCId = @@IDENTITY

        //                            INSERT INTO OperationLog (CustomerId, OperationType, Amount) VALUES (@NewCId, 2, @Credits)
        //                            COMMIT
        //                    END TRY

        //                    BEGIN CATCH
        //                        ROLLBACK
        //                    END CATCH";

        //    try
        //    {
        //        using (SqlConnection sqlCnn = new SqlConnection(_connectionString))
        //        {
        //            using (SqlCommand sqlCmd = new SqlCommand(query, sqlCnn))
        //            {
        //                sqlCmd.Parameters.AddWithValue("@Name", customerToAdd.Name);
        //                sqlCmd.Parameters.AddWithValue("@Surname", customerToAdd.Surname);
        //                sqlCmd.Parameters.AddWithValue("@Credits", customerToAdd.Credits);

        //                sqlCnn.Open();
        //                int rowsAffected = sqlCmd.ExecuteNonQuery();
        //                sqlCnn.Close();
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw new Exception("A problem happened with headling your request.");
        //    }
        //}

        public void AddCustomerAndLogWithExec(Customer customerToAdd)
        {
            string query = @"EXEC uspInsertNewCustomer @Name, @Surname, @Credits";

            try
            {
                using (SqlConnection sqlCnn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(query, sqlCnn))
                    {
                        sqlCmd.Parameters.AddWithValue("@Name", customerToAdd.Name);
                        sqlCmd.Parameters.AddWithValue("@Surname", customerToAdd.Surname);
                        sqlCmd.Parameters.AddWithValue("@Credits", customerToAdd.Credits);

                        sqlCnn.Open();
                        int rowsAffected = sqlCmd.ExecuteNonQuery();
                        sqlCnn.Close();
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("A problem happened with headling your request.");
            }
        }

        public bool DeleteById(int id)
        {
            string query = @"BEGIN TRY
                                BEGIN TRANSACTION
                                    DELETE FROM OperationLog WHERE CustomerId = @Id
                                    DELETE FROM Customers WHERE Id = @Id                           
                                    COMMIT
                            END TRY

                            BEGIN CATCH
                                ROLLBACK
                            END CATCH";

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

        public bool UpdateCustomer(int id, Customer customerToUpdate)
        {
            string query = @"UPDATE Customers SET Name = @Name
	                                    ,Surname = @Surname
                             WHERE Id = @Id";

            bool result = true;

            try
            {
                using (SqlConnection sqlCnn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(query, sqlCnn))
                    {
                        sqlCmd.Parameters.AddWithValue("@Id", id);
                        sqlCmd.Parameters.AddWithValue("@Name", customerToUpdate.Name);
                        sqlCmd.Parameters.AddWithValue("@Surname", customerToUpdate.Surname);

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

        public bool UpdateCustomerAndLog(int id, Customer customerToUpdate, Log logToUpdate)
        {
            string query = @"BEGIN TRY
                                BEGIN TRANSACTION
                                    UPDATE Customers SET Name = @Name
	                                    ,Surname = @Surname
	                                    ,Credits = @Credits
                                    WHERE Id = @Id
                                    INSERT INTO OperationLog (CustomerId, OperationType, Amount) VALUES (@CustomerId, @OperationType, @Amount)
                                    COMMIT
                            END TRY

                            BEGIN CATCH
                                ROLLBACK
                            END CATCH";

            bool result = true;

            try
            {
                using (SqlConnection sqlCnn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(query, sqlCnn))
                    {
                        sqlCmd.Parameters.AddWithValue("@Id", id);
                        sqlCmd.Parameters.AddWithValue("@Name", customerToUpdate.Name);
                        sqlCmd.Parameters.AddWithValue("@Surname", customerToUpdate.Surname);
                        sqlCmd.Parameters.AddWithValue("@Credits", customerToUpdate.Credits);

                        sqlCmd.Parameters.AddWithValue("@CustomerId", logToUpdate.CustomerId);
                        sqlCmd.Parameters.AddWithValue("@OperationType", logToUpdate.OperationType);
                        sqlCmd.Parameters.AddWithValue("@Amount", logToUpdate.Amount);

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
