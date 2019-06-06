using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreditsManagement.API.DataAccess;
using CreditsManagement.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;

namespace LibraryV2.API.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private CustomersDA _customerDA;
        private LogsDA _logDA;

        /// <summary>
        /// This is the controller for customers
        /// </summary>
        /// <param name="customerDA">Data access to read from database the customers</param>
        public CustomersController(CustomersDA customerDA, LogsDA logDA)
        {
            _customerDA = customerDA;
            _logDA = logDA;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string orderBy = "id")
        {
            List<Customer> customerList = _customerDA.GetAllNames(orderBy);

            if (!customerList.Any())
            {
                return NotFound("No customers has been added.");
            }
            return Ok(customerList);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Customer result = _customerDA.GetById(id);

            if (result == null)
            {
                return NotFound($"Customer with id {id} not found.");
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Customer customer)
        {
            if (customer == null)
            {
                return BadRequest("A problem happened with headling your request.");
            }

            bool resultCustomerAdded = _customerDA.AddCustomerAndLog(customer, new Log()
            {
                CustomerId = customer.Id,
                OperationType = 2,
                Amount = customer.Credits
            });

            if (resultCustomerAdded)
            {
                return Ok("Customer added without any problems.");
            }
            else
            {
                return StatusCode(500, "A problem happened with headling your request.");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            Customer exist = _customerDA.GetById(id);

            if (exist == null)
            {
                return NotFound("Customer not found.");
            }

            bool resultDeleteCustomer = _customerDA.DeleteById(id);

            if (resultDeleteCustomer)
            {
                return Ok("Customer deleted without any problems.");
            }
            else
            {
                return StatusCode(500, "A problem happened with headling your request.");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, 
            [FromBody] Customer customer)
        {
            if (customer == null)
            {
                return BadRequest("A problem happened with headling your request.");
            }

            Customer exist = _customerDA.GetById(id);

            if (exist == null)
            {
                //return NotFound("Customer not found.");
                bool added = _customerDA.AddCustomerAndLog(new Customer()
                {
                    Name = customer.Name,
                    Surname = customer.Surname,
                    Credits = customer.Credits
                }, new Log()
                {
                    CustomerId = customer.Id,
                    OperationType = 2,
                    Amount = customer.Credits
                });

                if (added)
                {
                    return Ok("Customer added without any problems.");
                }
                else
                {
                    return StatusCode(500, "A problem happened with headling your request.");
                }
            }

            if (exist.Credits != customer.Credits)
            {
                return BadRequest("You can't modify the credits in this way");
            }

            bool result = _customerDA.UpdateCustomer(id, customer);

            if (result)
            {
                return Ok("Customer updated without any problems.");
            }
            else
            {
                return StatusCode(500, "A problem happened with headling your request.");
            }
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, 
            [FromBody] JsonPatchDocument<Customer> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("A problem happened with headling your request.");
            }

            Customer currentCustomer = _customerDA.GetById(id);

            if (currentCustomer == null)
            {
                return NotFound("Customer not found.");
            }

            Customer customerToPatch = new Customer()
            {
                Name = currentCustomer.Name,
                Surname = currentCustomer.Surname,
                Credits = currentCustomer.Credits
            };

            patchDoc.ApplyTo(new Customer()
            {
                Name = currentCustomer.Name,
                Surname = currentCustomer.Surname,
                Credits = currentCustomer.Credits
            });

            if (customerToPatch.Credits != currentCustomer.Credits)
            {
                return BadRequest("You can't modify the credits in this way");
            }

            bool result = _customerDA.UpdateCustomer(id, customerToPatch);

            if (result)
            {
                return Ok("Customer updated without any problems.");
            }
            else
            {
                return StatusCode(500, "A problem happened with headling your request.");
            }
        }

        [HttpPost("{customerId}/consume")]
        public IActionResult ConsumeCredits(int customerId,
            [FromBody] ConsumeRequest credits)
        {
            Customer currentCustomer = _customerDA.GetById(customerId);

            if (currentCustomer == null)
            {
                return NotFound($"Customer with id {customerId} not found.");
            }

            if (currentCustomer.Credits < credits.Amount)
            {
                return BadRequest("Credits can't be consumed because the customer hasn't enough credits.");
            }

            bool resultLogConsumed = _customerDA.UpdateCustomerAndLog(customerId, new Customer()
            {
                Name = currentCustomer.Name,
                Surname = currentCustomer.Surname,
                Credits = currentCustomer.Credits - credits.Amount
            }, new Log()
            {
                CustomerId = customerId,
                OperationType = 1,
                Amount = credits.Amount
            });

            if (resultLogConsumed)
            {
                return Ok("Credits consumed without any problems.");
            }
            else
            {
                return StatusCode(500, "A problem happened with headling your request.");
            }
        }

        [HttpPost("{customerId}/charge")]
        public IActionResult ChargeCredits(int customerId,
            [FromBody] ChargeRequest credits)
        {
            Customer currentCustomer = _customerDA.GetById(customerId);

            if (currentCustomer == null)
            {
                return NotFound($"Customer with id {customerId} not found.");
            }

            bool resultLogAdded = _customerDA.UpdateCustomerAndLog(customerId, new Customer()
            {
                Name = currentCustomer.Name,
                Surname = currentCustomer.Surname,
                Credits = currentCustomer.Credits + credits.Amount
            }, new Log()
            {
                CustomerId = customerId,
                OperationType = 2,
                Amount = credits.Amount
            });

            if (resultLogAdded)
            {
                return Ok("Credits added without any problems.");
            }
            else
            {
                return StatusCode(500, "A problem happened with headling your request.");
            }
        }
    }
}
