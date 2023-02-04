using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_HttpServer
{
    public class Request
    {
        public string Type { get; set; }
        public string URL { get; set; }
        public string Host { get; set; }

        public String Referer { get; set; }

        private Request(String type, String url, String host, String referer) 
        {
            Type= type;
            URL= url;
            Host= host;
            Referer=referer;
        }

        public static Request GetRequest(String requst) {

            if (String.IsNullOrEmpty(requst))
                return null;

            String[] tokens = requst.Split(' ');
            String type = tokens[0];
            String url = tokens[1];
            String host = tokens[4];
            String referer = "";

            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i] == "Referer:")
                {
                    referer = tokens[i + 1];
                    break;
                }
            }

            return new Request(type, url, host, referer);
        }
    }
}
