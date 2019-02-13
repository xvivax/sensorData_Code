using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            string ip = @"http://192.168.0.103:9980/test/";
            //string ip = @"http://+:9980/test/";
            Server s1 = new Server(ip);
            s1.Start();
        }
    }
}
