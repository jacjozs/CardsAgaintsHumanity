using CAHServer.Data;
using CAHServer.Services;
using System;

namespace CAHServer
{
    class Program
    {
        static void Main(string[] args)
        {
            DataStore.Initialize();
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
