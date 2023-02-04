using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace C_HttpServer
{
    public class HttpServer
    {
        public const String MSG_DIR = "/root/msg/";
        public const String WEB_DIR = "/root/web/";
        public const String VERSION = "HTTP/1.1";
        public const String NAME = "MatthiWare HTTP Server v0.1";

        private bool running = false;
        private readonly TcpListener listener;


        public HttpServer(int port) {
            listener = new TcpListener(IPAddress.Any, port);
        }

        public void Start() {
            Thread serverThread = new(new ThreadStart(Run));
            serverThread.Start();
        }

        private void Run()
        {   
            running = true;
            listener.Start();

            while (running) {
                Console.WriteLine("\nWaiting for connection...");
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client connected!");

                HandleClient(client);

                client.Close();
            }

            running = false;
            listener.Stop();
        }

        private static void HandleClient(TcpClient client)
        {
            StreamReader reader = new(client.GetStream());

            string msg = "";
            while (reader.Peek() != -1)
                msg += reader.ReadLine() + "\n\t";

            Console.WriteLine("REQUEST: \n\t" + msg);

            Request req = Request.GetRequest(msg);
            Response resp = Response.From(req);
            resp.Post(client.GetStream());

        }

    }
}
