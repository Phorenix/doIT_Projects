using CreditsManagement.API.DataAccess;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreditsManagement.API.Controllers
{
    public class OperationsController : Controller
    {
        private OperationsDA _operationsDA;

        public OperationsController(OperationsDA operationsDA)
        {
            _operationsDA = operationsDA;
        }
    }
}
