using System.Net;
using System.Web;
using System.Collections.Generic;

namespace AtaLib.Net
{
    public class HttpResponse
    {
        public string Protocol = "HTTP/1.1";
        public HttpStatusCode StatusCode;
        public Dictionary<string, string> Headers;
        public string Body;

        public HttpResponse(HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            StatusCode = statusCode;
            Headers = new();
        }

        public HttpResponse AddHeader(string Header, string Value)
        {
            Headers.Add(Header, Value);
            return this;
        }

        public HttpResponse Status(HttpStatusCode code)
        {
            StatusCode = code;
            return this;
        }

        public HttpResponse Text(string Body)
        {
            Headers.Add("Content-Type", "text/html");
            this.Body = Body;
            return this;
        }

        public HttpResponse Json(string Json)
        {
            Headers.Add("Content-Type", "application/json");
            this.Body = Json;
            return this;
        }

        public override string ToString()
        {
            string Complete = $"{Protocol} {(int)StatusCode} {StatusCode.ToString()}\r\n";
            foreach(KeyValuePair<string, string> Header in Headers)
            {
                Complete += $"{Header.Key}: {Header.Value}\r\n";
            }
            return $"{Complete}\r\n{Body}";
        }
    }
}