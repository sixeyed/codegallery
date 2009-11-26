using System;
using System.Collections.Generic;
using System.Text;

namespace TriathlonResults.Central.Console
{
    /// <summary>
    /// Console app for monitoring file drops, extracting file contents
    /// and persisting to TriathlonResults db
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            FileWatcher.FileWatcher.Start();
            System.Console.WriteLine("TriathlonResults.Central.Console started.");
            System.Console.WriteLine("Enter to exit.");
            System.Console.ReadLine();
            FileWatcher.FileWatcher.Stop();
        }
    }
}
