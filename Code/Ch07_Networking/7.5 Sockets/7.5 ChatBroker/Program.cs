using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Ch07_Networking.Recipe7_5.ChatBroker
{

    
    class Program
    {
        internal static ConnectionListener _ConnListener = new ConnectionListener();
      
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: ChatBroker.exe <port#>");
                Console.WriteLine("Press any key to end...");
                Console.ReadLine();
            }
           
            _ConnListener.Run(Convert.ToInt32(args[0]));
            
            Console.WriteLine("Press any key to terminate server...");
            
            Console.ReadLine();
        }


    }

    
    
    


}
