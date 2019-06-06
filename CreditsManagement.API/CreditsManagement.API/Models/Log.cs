using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreditsManagement.API.Models
{
    public class Log
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public int OperationType { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
