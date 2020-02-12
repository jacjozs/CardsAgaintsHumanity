using CAHClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAHClient
{
    class Program
    {
        static void Main(string[] args)
        {
            NetworkService.Start();
            string command;
            Console.Write("Command > ");
            while ((command = Console.ReadLine()) != "exit")
            {
                string[] commandargs = command.Split(' ');
                switch (commandargs[0])
                {
                    case "account":

                        break;
                }
                Console.WriteLine("");
                Console.Write("Command > ");
            }
        }
    }
}
