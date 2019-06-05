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
        public IActionResult Get()
        {
            List<CustomerModelInput> customerList = _customerDA.GetAllNames();

            if (!customerList.Any())
            {
                return NotFound("No customers has been added.");
            }
            return Ok(customerList);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            CustomerModelInput result = _customerDA.GetById(id);

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
        public IActionResult Post([FromBody] CustomerModelOutput customer)
        {
            if (customer == null)
            {
                return BadRequest("A problem happened with headling your request.");
            }

            bool result = _customerDA.AddCustomer(customer);

            if (result)
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
            CustomerModelInput exist = _customerDA.GetById(id);

            if (exist == null)
            {
                return NotFound("Customer not found.");
            }

            bool resultDeleteCustomer = _customerDA.DeleteById(id);

            bool resultDeleteLogs = _logDA.DeleteById(id);

            if (resultDeleteCustomer && resultDeleteLogs)
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
            [FromBody] CustomerModelUpdate customer)
        {
            if (customer == null)
            {
                return BadRequest("A problem happened with headling your request.");
            }

            CustomerModelInput exist = _customerDA.GetById(id);

            if (exist == null)
            {
                //return NotFound("Customer not found.");
                bool added = _customerDA.AddCustomer(new CustomerModelOutput()
                {
                    Name = customer.Name,
                    Surname = customer.Surname,
                    Credits = customer.Credits
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
            [FromBody] JsonPatchDocument<CustomerModelUpdate> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("A problem happened with headling your request.");
            }

            CustomerModelInput currentCustomer = _customerDA.GetById(id);

            if (currentCustomer == null)
            {
                return NotFound("Customer not found.");
            }

            CustomerModelUpdate customerToPatch = new CustomerModelUpdate()
            {
                Name = currentCustomer.Name,
                Surname = currentCustomer.Surname,
                Credits = currentCustomer.Credits
            };

            patchDoc.ApplyTo(customerToPatch);

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
            CustomerModelInput currentCustomer = _customerDA.GetById(customerId);

            if (currentCustomer == null)
            {
                return NotFound($"Customer with id {customerId} not found.");
            }

            if (currentCustomer.Credits < credits.Amount)
            {
                return BadRequest("Credits can't be consumed because the customer hasn't enough credits.");
            }

            CustomerModelUpdate customerToPatchCredits = new CustomerModelUpdate()
            {
                Name = currentCustomer.Name,
                Surname = currentCustomer.Surname,
                Credits = currentCustomer.Credits - credits.Amount
            };

            bool resultCustomerUpdate = _customerDA.UpdateCustomer(customerId, customerToPatchCredits);

            bool resultLogConsumed = _logDA.AddLog(new LogModelOutput()
            {
                CustomerId = customerId,
                OperationType = 1,
                Amount = credits.Amount
            });

            if (resultCustomerUpdate && resultLogConsumed)
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
            CustomerModelInput currentCustomer = _customerDA.GetById(customerId);

            if (currentCustomer == null)
            {
                return NotFound($"Customer with id {customerId} not found.");
            }

            CustomerModelUpdate customerToPatchCredits = new CustomerModelUpdate()
            {
                Name = currentCustomer.Name,
                Surname = currentCustomer.Surname,
                Credits = currentCustomer.Credits + credits.Amount
            };

            bool resultCustomerUpdate = _customerDA.UpdateCustomer(customerId, customerToPatchCredits);

            bool resultLogAdded = _logDA.AddLog(new LogModelOutput()
            {
                CustomerId = customerId,
                OperationType = 2,
                Amount = credits.Amount
            });

            if (resultCustomerUpdate && resultLogAdded)
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
