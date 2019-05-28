using System;
using System.Collections.Generic;
using System.Text;

namespace test1
{
    class Client
    {
        public Client(string name, string address, string telNumber)
        {
            Name = name;
            Address = address;
            TelNumber = telNumber;
        }

        public string Name { get; set; }
        public string Address { get; set; }
        public string TelNumber { get; set; }
    }
}
