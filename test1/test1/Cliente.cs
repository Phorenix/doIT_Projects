using System;
using System.Collections.Generic;
using System.Text;

namespace test1
{
    class Client
    {
        public Client(string name, string address, int tel_number)
        {
            Name = name;
            Address = address;
            Tel_number = tel_number;
        }

        public string Name { get; set; }
        public string Address { get; set; }
        public int Tel_number { get; set; }
    }
}
