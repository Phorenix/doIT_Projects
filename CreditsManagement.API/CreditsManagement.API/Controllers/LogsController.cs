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

        [HttpGet()]
        public IActionResult Get([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {
            List<Log> logs = _logDA.GetAllLogs();

            if (!logs.Any())
            {
                return NotFound("No logs has been added.");
            }

            if (fromDate != DateTime.MinValue || toDate != DateTime.MinValue)
            {
                if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                {
                    if (fromDate > toDate)
                    {
                        DateTime temporary = toDate;
                        toDate = fromDate;
                        fromDate = temporary;
                    }

                    logs = _logDA.GetAllLogsInSpecificPeriod(fromDate, toDate);
                }
                else
                {
                    if (toDate != DateTime.MinValue)
                    {
                        logs = _logDA.GetAllLogsUntilDate(toDate);
                    }
                    else
                    {
                        logs = _logDA.GetAllLogsFromDate(fromDate);
                    }
                }
            }
            else
            {
                logs = _logDA.GetAllLogs();
            }
            return Ok(logs);
        }

        [HttpGet("{customerId}")]
        public IActionResult Get(int customerId, 
            [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {
            Customer result = _customerDA.GetById(customerId);

            if (result == null)
            {
                return NotFound($"Customer with id {customerId} not found.");
            }

            List<Log> logs = new List<Log>();

            if (fromDate != DateTime.MinValue || toDate != DateTime.MinValue)
            {
                if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                {
                    if (fromDate > toDate)
                    {
                        DateTime temporary = toDate;
                        toDate = fromDate;
                        fromDate = temporary;
                    }

                    logs = _logDA.GetLogsInSpecificPeriod(customerId, fromDate, toDate);
                }
                else
                {
                    if (toDate != DateTime.MinValue)
                    {
                        logs = _logDA.GetLogsByIdUntilDate(customerId, toDate);
                    }
                    else
                    {
                        logs = _logDA.GetLogsByIdFromDate(customerId, fromDate);
                    }
                }
            }
            else
            {
                logs = _logDA.GetLogsById(customerId);
            }
            return Ok(logs);
        }
    }
}
