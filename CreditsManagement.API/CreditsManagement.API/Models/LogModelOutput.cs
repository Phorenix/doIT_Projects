using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreditsManagement.API.Models
{
    public class LogModelOutput
    {
        public int CustomerId { get; set; }
        public int OperationType { get; set; }
        public int Amount { get; set; }
    }
}
