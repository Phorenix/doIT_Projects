using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreditsManagement.API.DataAccess;
using CreditsManagement.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;

namespace CreditsManagement.API.Controllers
{
    [Route("api/[controller]")]
    public class LogsController : Controller
    {
        private CustomersDA _customerDA;
        private LogsDA _logDA;

        public LogsController(LogsDA logDA, CustomersDA customerDA)
        {
            _logDA = logDA;
            _customerDA = customerDA;
        }

        [HttpGet("{customerId}")]
        public IActionResult Get(int customerId, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            List<CustomerModelInput> customerList = _customerDA.GetAllNames();

            if (!customerList.Any())
            {
                return NotFound("No customers has been added.");
            }

            
        }
    }
}
