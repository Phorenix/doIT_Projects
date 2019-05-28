﻿using System;
using System.Collections.Generic;

namespace test1
{
    class Program
    {
        static void Main(string[] args)
        {
            var clients = new List<Client>();
            
            while (true)
            {
                Console.WriteLine("\n\n\n\nMenù:\n" +
                    "1. Input client\n" +
                    "2. Output client\n" +
                    "0. End\n" +
                    "Choose an option: ");
                var chosen = int.Parse(Console.ReadLine());
                switch (chosen)
                {
                    case 1:
                        Input(ref clients);
                        break;
                    case 2:
                        Output(clients);
                        break;
                    case 0:
                        System.Environment.Exit(1);
                        break;
                    default:
                        Console.WriteLine("Invalid option");
                        break;
                }
            }
        }

        public static void Input(ref List<Client> clients)
        {
            Console.WriteLine("\n\n\nName: ");
            var name = Console.ReadLine();
            Console.WriteLine("\nAddress: ");
            var address = Console.ReadLine();
            Console.WriteLine("\nTelephone number: ");
            var tel_number = int.Parse(Console.ReadLine());
            var client = new Client(name, address, tel_number);
            clients.Add(client);
        }

        public static void Output(List<Client> clients)
        {
            foreach (Client client in clients)
            {
                Console.WriteLine($"\n\nName: {client.Name}");
                Console.WriteLine($"\nAddress: {client.Address}");
                Console.WriteLine($"\nTelephone number: {client.Tel_number}");
            }
        }
    }
}
