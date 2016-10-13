using System;
using System.Net;
using System.Net.Sockets;

namespace Ch07_Networking.Recipe7_5.ChatBroker
{
  internal class ConnectionListener
  {
    //the socket used for listening to incoming connections
    Socket ListenerSocket { get; set; }
    SocketAsyncEventArgs sockEvtArgs = null;
    //new server connection manager
    ServerConnectionManager ConnManager = new ServerConnectionManager();
    //run the connection listener
    internal void Run(int Port)
    {
      //create a new IP endpoint at the specific port, 
      //and on any available IP address
      IPEndPoint ListenerEndPoint = new IPEndPoint(IPAddress.Any, Port);
      //create the listener socket
      ListenerSocket = new Socket(AddressFamily.InterNetwork,
        SocketType.Stream, ProtocolType.Tcp);
      //bind to the endpoint
      ListenerSocket.Bind(ListenerEndPoint);
      //listen with a backlog of 20
      ListenerSocket.Listen(20);
      Console.WriteLine("Waiting for incoming connection ...");
      //start accepting connections
      AcceptIncoming();
    }
    //accept incoming connections
    internal void AcceptIncoming()
    {
      //pass in the server connection manager
      sockEvtArgs = new SocketAsyncEventArgs { UserToken = ConnManager };
      sockEvtArgs.Completed += new EventHandler<SocketAsyncEventArgs>(
          delegate(object Sender, SocketAsyncEventArgs e)
          {
            Console.WriteLine("Accepted connection..." +
              "Assigning to Connection Manager...." +
              "Waiting for more connections...");
            //pass the connected socket to the server connectino manager
            ConnManager.Manage(e.AcceptSocket);
            //keep listening
            AcceptIncoming();
          });
      //accept an incoming connection
      ListenerSocket.AcceptAsync(sockEvtArgs);
    }
  }
}