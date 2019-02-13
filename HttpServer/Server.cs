using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace HttpServer
{
    public class Server
    {
        private string ip;
        private HttpListener listener;
        private bool flag = true;

        public Server(string ip)
        {
            this.ip = ip;
        }

        public void Start()
        {
            listener = new HttpListener();

            if (!HttpListener.IsSupported) return;
            
            listener.Prefixes.Add(ip);

            try
            {
                listener.Start();
                Console.WriteLine("Server started successfully");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
            

            while (listener.IsListening)
            {
                Console.WriteLine("Waiting for request..");
                HttpListenerContext context = listener.GetContext();
                Console.WriteLine("Request received");
                Console.WriteLine(context.Request.RemoteEndPoint);
                // receive incoming request
                HttpListenerRequest request = context.Request;

                if (request.HttpMethod == "POST")
                {
                    ShowRequestData(request);
                }

                if (!flag) return;

                //string responseString = @"<!DOCTYPE HTML>
                //                        <html><head></head><body>
                //                        <form method=""post"" action="""">
                //                        <p><b>Name: </b><br>
                //                        <input type=""text"" name=""myname"" size=""40""></p>
                //                        <p><input type=""submit"" value=""send""></p>
                //                        </form></body></html>";

                //HttpListenerResponse response = context.Response;
                //response.ContentType = "text/html; charset=UTF-8";

                //byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                //response.ContentLength64 = buffer.Length;

                HttpListenerResponse response = context.Response;
                TextReader tr = new StreamReader("data.json");
                string msg = tr.ReadToEnd();
                byte[] buffer = Encoding.UTF8.GetBytes(msg);
                response.ContentType = "application/json";
                response.ContentLength64 = buffer.Length;


                using (Stream output = response.OutputStream)
                {
                    output.Write(buffer, 0, buffer.Length);
                }
            }

        }

        private void ShowRequestData(HttpListenerRequest request)
        {
            if (!request.HasEntityBody) return;

            using (Stream body = request.InputStream)
            {
                using (StreamReader reader = new StreamReader(body))
                {
                    string text = reader.ReadToEnd();
                    Console.WriteLine(text);

                    if (text == "stop")
                    {
                        listener.Stop();
                        flag = false;
                        Console.WriteLine("Server stopped");
                    }
                }
            }
        }
    }
}