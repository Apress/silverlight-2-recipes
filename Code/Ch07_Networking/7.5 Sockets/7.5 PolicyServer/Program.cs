using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System;

using System.IO;

using System.Net;

using System.Net.Sockets;


namespace Ch07_Networking.Recipe7_5.PolicyServer
{
   
    public class Program
    {

        static void Main(string[] args)
        {
 
            PolicyListener ps = new PolicyListener();
            ps.ListenForPolicyRequest();
            Console.WriteLine("Press any key to terminate server...");
            Console.ReadLine();
             

        }


    }
} 

