using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace Ch07_Networking.Recipe7_5.PolicyServer
{

  internal class PolicyListener
  {
    Socket ListenerSocket { get; set; }
    SocketAsyncEventArgs sockEvtArgs = null;
    //valid policy request string
    public static string ValidPolicyRequest = "<policy-file-request/>";

    public PolicyListener()
    {
      //bind to all available addresses and port 943      
      IPEndPoint ListenerEndPoint = 
        new IPEndPoint(IPAddress.Any, 943);
      ListenerSocket = 
        new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      ListenerSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, 0);
      ListenerSocket.Bind(ListenerEndPoint);
      ListenerSocket.Listen(20);
    }
    internal void ListenForPolicyRequest()
    {
      sockEvtArgs = new SocketAsyncEventArgs();
      sockEvtArgs.Completed += new EventHandler<SocketAsyncEventArgs>(
          delegate(object Sender, SocketAsyncEventArgs e)
          {
            //process this request
            ReadPolicyRequest(e.AcceptSocket);
            //go back to listening
            ListenForPolicyRequest();
          });
      ListenerSocket.AcceptAsync(sockEvtArgs);
    }
    private bool ReadPolicyRequest(Socket ClientSocket)
    {
      SocketAsyncEventArgs sockEvtArgs = 
        new SocketAsyncEventArgs { UserToken = ClientSocket };
      sockEvtArgs.SetBuffer(
        new byte[ValidPolicyRequest.Length], 0, ValidPolicyRequest.Length);
      sockEvtArgs.Completed += new EventHandler<SocketAsyncEventArgs>(
          delegate(object Sender, SocketAsyncEventArgs e)
          {
            if (e.SocketError == SocketError.Success)
            {
              //get policy request string
              string PolicyRequest = new UTF8Encoding().
                GetString(e.Buffer, 0, e.BytesTransferred);
              //check for valid format
              if (PolicyRequest.CompareTo(ValidPolicyRequest) == 0)
                //valid request-send policy
                SendPolicy(e.UserToken as Socket);
            }
          });
      return ClientSocket.ReceiveAsync(sockEvtArgs);
    }
    private void SendPolicy(Socket ClientSocket)
    {
      //read the policy file
      FileStream fs = new FileStream("clientaccesspolicy.xml", FileMode.Open);
      byte[] PolicyBuffer = new byte[(int)fs.Length];
      fs.Read(PolicyBuffer, 0, (int)fs.Length);
      fs.Close();

      SocketAsyncEventArgs sockEvtArgs = 
        new SocketAsyncEventArgs { UserToken = ClientSocket };
      //send the policy
      sockEvtArgs.SetBuffer(PolicyBuffer, 0, PolicyBuffer.Length);
      sockEvtArgs.Completed += new EventHandler<SocketAsyncEventArgs>(
          delegate(object Sender, SocketAsyncEventArgs e)
          {
            //close this connection
            if (e.SocketError == SocketError.Success)
              (e.UserToken as Socket).Close();
          });
      ClientSocket.SendAsync(sockEvtArgs);
    }
  }
}