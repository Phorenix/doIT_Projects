using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreditsManagement.API.Models
{
    public class LogModelInput
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int OperationType { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
