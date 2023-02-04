using System;

namespace C_HttpServer // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server on port 500!");
            HttpServer server = new(500);
            server.Start();
        }
    }
}