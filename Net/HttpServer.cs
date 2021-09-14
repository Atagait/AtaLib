using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AtaLib.Logging;

namespace AtaLib.Net
{
    /// <summary>
    /// HttpServer is a basic embeddable HTTP server
    /// </summary>
    public class HttpServer
    {
        private TcpListener listener;
        public readonly string ServerName;
        public readonly ILogger Logger;

        Dictionary<string, HttpResponder> Routes;

        CancellationTokenSource Cancellation;

        public delegate HttpResponse HttpResponder(HttpRequest request, HttpResponse response);

        public HttpServer(int Port, string ServerName = "HttpServer", ILogger logger = null)
        {
            listener = new TcpListener(System.Net.IPAddress.Any, Port);
            this.ServerName = ServerName;
            Routes = new();
            Cancellation = new();

            Logger = logger;
            if(logger == null)
                Logger = new BasicLogger("HttpServer", LogLevel.Information);
        }

        /// <summary>
        /// Adds a route for the server to process
        /// </summary>
        /// <param name="route">The route you want to add (E.G /index)</param>
        /// <param name="RequestHandler"></param>
        public void Route(string route, HttpResponder RequestHandler)
            => Routes.Add(route, RequestHandler);

        /// <summary>
        /// The method the server uses when a matching route cannot be found
        /// </summary>
        /// <param name="request">The request being processed</param>
        /// <param name="response">The response that will be sent</param>
        public virtual HttpResponse NotFound(HttpRequest request, HttpResponse response)
        {
            return response.Text($"<h1>Not found.</h1><hr><p>{ServerName}</p>")
                .Status(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Sets up basic response headers
        /// </summary>
        /// <param name="response"></param>
        private HttpResponse SetupResponseHeaders(HttpResponse response)
        {
            response.AddHeader("Server", ServerName)
                    .AddHeader("Last-Modified", DateTime.Now.ToString());
            if(!response.Headers.ContainsKey("Content-Type"))
                response.AddHeader("Content-Type", "text/html");
            response.AddHeader("Content-Length", "0");

            var Bytes = Encoding.UTF8.GetBytes(response.ToString());
            response.Headers["Content-Length"] = Bytes.Length.ToString();

            return response;
        }

        public async void HandleRequest(TcpClient client)
        {
            // Get and read the request
            using NetworkStream ns = client.GetStream();
            using StreamReader sr = new(ns);
            string message = "";

            int asyncResult = 1;
            while(asyncResult != 0 && sr.Peek() != -1 )
            {
                char[] b = new char[1];
                asyncResult = await sr.ReadAsync(b, 0, 1);
                message += b[0];
            }
            var Request = HttpRequest.Parse(message);

            Logger.Info($"Connection {client.Client.RemoteEndPoint} - {Request.Method} {Request.URI}");

            // Set up the response
            HttpResponse response = new();
            if(Routes.ContainsKey(Request.URI))
                response = Routes[Request.URI](Request, response);
            else
                response = NotFound(Request, response);
                
            response = SetupResponseHeaders(response);

            // Write the response and close the connection
            byte[] responseBytes = Encoding.UTF8.GetBytes(response.ToString());
            await ns.WriteAsync(responseBytes);
            ns.Close();
            client.Close();
        }

        public async void Start()
        {
            var token = Cancellation.Token;
            listener.Start();

            Logger.Info($"Listening on {listener.LocalEndpoint}");
            while(!token.IsCancellationRequested)
            {
                TcpClient client = listener.AcceptTcpClient();
                HandleRequest(client);
                await Task.Delay(10);
            }

            listener.Stop();
        }

        public void Stop()
        {
            Cancellation.Cancel();
        }
    }
}