using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CAHLib.Utils
{
    public class Log
    {
        static StreamWriter writer;

        public static void Initialize(string filename)
        {
            try
            {
                writer = new StreamWriter(filename, true);
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot initialize the Log!");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        static void AppendLog(string message)
        {
            if (writer == null) return;
            string result = DateTime.Now.ToString() + " " + message;
            lock (writer)
            {
                writer.WriteLine(message);
                writer.Flush();
            }
        }

        static object console_mutex = new object();

        static void OutputColor(string message, ConsoleColor clr)
        {
            lock (console_mutex)
            {
                ConsoleColor old = Console.ForegroundColor;
                Console.ForegroundColor = clr;
                Console.WriteLine(DateTime.Now.ToString() + " " + message);
                Console.ForegroundColor = old;
            }
        }

        public static void Info(string message)
        {
            string result = "[INFO] " + message;
            AppendLog(result);
            OutputColor(result, ConsoleColor.Green);
        }

        public static void Error(string message)
        {
            Error(message, null);
        }

        public static void Error(string message, Exception e)
        {
            string result = "[ERROR] " + message;
            if (e != null)
                result += "\n" + e.Message + "\n" + e.StackTrace;
            AppendLog(result);
            OutputColor(result, ConsoleColor.Red);
        }

        public static void Warn(string message)
        {
            Warn(message, null);
        }

        public static void Warn(string message, Exception e)
        {
            string result = "[WARN] " + message;
            if (e != null)
                result += "\n" + e.Message + "\n" + e.StackTrace;
            AppendLog(result);
            OutputColor(result, ConsoleColor.Yellow);
        }

        public static void Debug(string message)
        {
            string result = "[DEBUG] " + message;
            AppendLog(result);
            OutputColor(result, ConsoleColor.White);
        }
    }
}
