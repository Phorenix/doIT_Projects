using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreditsManagement.API.DataAccess
{
    public class OperationsDA
    {
        private string _connectionString;

        public OperationsDA(string connectionString)
        {
            _connectionString = connectionString;
        }
    }
}
