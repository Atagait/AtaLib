using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace AtaLib.Net
{
    public class HttpRequest
    {
        public readonly HttpMethod Method;
        public readonly string URI;
        public readonly string Protocol;
        private readonly string[] HeaderKeys;
        public readonly KeyValuePair<string, string>[] Headers;
        public readonly string Body;

        private HttpRequest(HttpMethod method, string uri, string protocol,
                            string[] headerkeys, KeyValuePair<string, string>[] headers,
                            string body)
        { 
            Method = method;
            URI = uri;
            Protocol = protocol;
            HeaderKeys = headerkeys;
            Headers = headers;
            Body = body;
        }

        private static HttpMethod ParseMethod(string method)
        {
            switch(method.ToUpper())
            {
                case "POST": return HttpMethod.Post;
                case "GET": return HttpMethod.Get;
                case "DELETE": return HttpMethod.Delete;
                case "PUT": return HttpMethod.Put;
                case "PATCH": return HttpMethod.Patch;
                case "HEAD": return HttpMethod.Head;
                case "OPTIONS": return HttpMethod.Options;
                case "TRACE": return HttpMethod.Trace;
                default: return null;
            }
        }

        public string Get_Header(string key)
        {
            int index = Array.IndexOf(HeaderKeys, key);
            if (index == -1) return null;
            return Headers[index].Value;
        }

        public static HttpRequest Parse(string HttpRequest)
        {
            // Split the headers from the body
            string[] RequestParts = HttpRequest.Split("\r\n\r\n");
            string[] HeaderLines = RequestParts[0].Split("\r\n");

            // Extract the Method, URI, and Protocol
            string[] URIParts = HeaderLines[0].Split(" ");
            HttpMethod method = ParseMethod(URIParts[0]);
            string uri = URIParts[1];
            string protocol = URIParts[2];

            // Rejig HeaderLines to exclude the first line
            {
                string[] tmp = new string[HeaderLines.Length - 1];
                Array.Copy(HeaderLines, 1, tmp, 0, HeaderLines.Length - 1);
                HeaderLines = tmp;
            }

            // Create the prototype headers collection
            KeyValuePair<string, string>[] Headers_Proto = new KeyValuePair<string, string>[HeaderLines.Length];
            string[] HeaderKeys = new string[HeaderLines.Length];

            for (int i = 0; i < HeaderLines.Length; i++)
            {
                string[] Parts = HeaderLines[i].Split(":");
                Headers_Proto[i] = new(Parts[0], Parts[1].Trim());
                HeaderKeys[i] = Parts[0];
            }

            return new HttpRequest(method, uri, protocol, HeaderKeys, Headers_Proto, RequestParts[1]);
        }

        public T Body_As_Json<T>() => JsonSerializer.Deserialize<T>(Body);
    }
}