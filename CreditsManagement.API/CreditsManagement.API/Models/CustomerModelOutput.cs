using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreditsManagement.API.Models
{
    public class CustomerModelOutput
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Credits { get; set; }
    }
}
