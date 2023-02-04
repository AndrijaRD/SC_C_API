using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace C_HttpServer
{
    public class Response
    {
        private Byte[] data;
        private String status;
        private String mime;

        private Response(String status, String mime, Byte[] data) { 
            this.status= status;
            this.data = data;
            this.mime = mime;
        }
        public static Response From(Request request)
        {

            if (request == null)
                return MakeNullRequest();

            if (request.Type == "GET")
            {
                
                String filename = Environment.CurrentDirectory + HttpServer.WEB_DIR + "data.json";
                if (request.URL.Contains("get"))
                {
                    Console.WriteLine("METHODE: GET");
                    FileInfo f = new(filename);
                    MakeFromFile(f);
                }
                else
                {
                    try
                    {
                        Console.WriteLine("METHODE: POST");
                        string[] values = request.URL.Split('_');
                        values[0] = values[0].Split('/')[1];
                        for (int i = 0; i < values.Length; i++)
                            Console.WriteLine("\t value: " + values[i]);

                        int O2 = Int32.Parse(values[0]);
                        int CO2 = Int32.Parse(values[1]);
                        int CH4 = Int32.Parse(values[2]);


                        FileStream fs = new(filename, FileMode.Create, FileAccess.Write);
                        if (fs.CanWrite)
                        {
                            byte[] buffer = Encoding.ASCII.GetBytes("{\n\"O2\": " + O2 + ",\n\"CO2\": " + CO2 + ",\n\"CH4\": " + CH4 + "}");
                            fs.Write(buffer, 0, buffer.Length);
                        }

                        fs.Flush();
                        fs.Close();
                        byte[] returnReport = Encoding.ASCII.GetBytes("{\n\"status\": \"CHANGES SECCEFULLY APPLIED.\"\n}");
                        return new Response("200 OK", "application/json", returnReport);
                    }
                    catch
                    {
                        return MakePageNotFound();
                    }
                }
            }
            
            else
                return MakeMethodeNotAllowed();

            return MakePageNotFound();
        }

        private static Response MakeFromFile(FileInfo f)
        {
            FileStream fs = f.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] d = new Byte[fs.Length];
            reader.Read(d, 0, d.Length);
            fs.Close();
            return new Response("200 OK", "application/json", d);
        }

        private static Response MakeNullRequest()
        {

            String file = Environment.CurrentDirectory + HttpServer.MSG_DIR + "400.html";
            FileInfo fi = new FileInfo(file);
            FileStream fs = fi.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] d = new Byte[fs.Length];
            reader.Read(d, 0, d.Length);
            fs.Close();
            return new Response("400 Bad Request", "text/html", d);
        }

        private static Response MakeMethodeNotAllowed()
        {

            String file = Environment.CurrentDirectory + HttpServer.MSG_DIR + "405.html";
            FileInfo fi = new FileInfo(file);
            FileStream fs = fi.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] d = new Byte[fs.Length];
            reader.Read(d, 0, d.Length);
            fs.Close();
            return new Response("405 Methode Not Allowed", "text/html", d);
        }

        private static Response MakePageNotFound()
        {

            String file = Environment.CurrentDirectory + HttpServer.MSG_DIR + "404.html";
            FileInfo fi = new FileInfo(file);
            FileStream fs = fi.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] d = new Byte[fs.Length];
            reader.Read(d, 0, d.Length);
            fs.Close();
            return new Response("404 Page Not Found", "text/html", d);
        }

        public void Post(NetworkStream stream)
        {

            StreamWriter writer  = new StreamWriter(stream);
            writer.WriteLine(String.Format("{0} {1}\r\nServer: {2}\r\nContent-Type: {3}\r\nAccept-Ranges: bytes\r\nContent-Length: {4}\r\n", 
                HttpServer.VERSION, status, HttpServer.NAME, mime, data.Length));
            writer.Flush();
            try
            {
                stream.Write(data, 0, data.Length);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
               
        }
    }
}
/*
 
GET / HTTP/1.1
Host: localhost:500
Connection: keep-alive
Cache-Control: max-age=0
sec-ch-ua: "Not_A Brand";v="99", "Microsoft Edge";v="109", "Chromium";v="109"
sec-ch-ua-mobile: ?0
sec-ch-ua-platform: "Windows"
Upgrade-Insecure-Requests: 1
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/109.0.0.0 Safari/537.36 Edg/109.0.1518.70
Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/          /*
*; q = 0.8,application / signed - exchange; v = b3; q = 0.9
Sec - Fetch - Site: none
Sec-Fetch-Mode: navigate
Sec-Fetch-User: ? 1
Sec - Fetch - Dest: document
Accept-Encoding: gzip, deflate, br
Accept-Language: en - US,en; q = 0.9

 */