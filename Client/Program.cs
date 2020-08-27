using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Proxies;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            InstanceContext context = new InstanceContext(new MyCallback());
            Proxy.ChatServiceClient server = new Proxy.ChatServiceClient(context);

            Console.WriteLine("Enter username");
            var username = Console.ReadLine();
            server.Join(username);

            Console.WriteLine();
            Console.WriteLine("Enter Message");
            Console.WriteLine("Press Q to Exit");

            var message = Console.ReadLine();

            while (message != "Q")
            {
                if (!string.IsNullOrEmpty(message))
                    server.SendMessage(message);
                message = Console.ReadLine();
            }

        }
    }
}
