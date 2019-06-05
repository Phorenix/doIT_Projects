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
        public IActionResult Get(int customerId, [FromQuery] string fromDate, [FromQuery] string toDate)
        {
            CustomerModelInput result = _customerDA.GetById(customerId);

            if (result == null)
            {
                return NotFound($"Customer with id {customerId} not found.");
            }

            List<LogModelInput> logs = new List<LogModelInput>();

            if (fromDate != null && toDate != null)
            {
                logs = _logDA.GetLogsInSpecificPeriod(customerId, DateTime.Parse(fromDate), DateTime.Parse(toDate));
            }
            else
            {
                logs = _logDA.GetLogsById(customerId);
            }
            return Ok(logs);
        }
    }
}
