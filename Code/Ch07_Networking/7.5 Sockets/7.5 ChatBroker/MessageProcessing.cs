using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

namespace Ch07_Networking.Recipe7_5.ChatBroker
{
  internal class ServerConnectionManager
  {

    //list of participants
    private List<Participant> _Participants = new List<Participant>();
    internal List<Participant> Participants
    {
      get { return _Participants; }
    }
    //accept and manage a client socket
    internal void Manage(Socket socket)
    {
      //create a new Participant around the client socket
      Participant p = new Participant { ClientSocket = socket, Parent = this };
      //add it to the list
      _Participants.Add(p);
      //start up the participant
      p.StartUp();
    }
    //broadcast a message from a participant to all other participants
    internal void Broadcast(string From, MessageWrapper Message)
    {
      //get a list of all participants other than the one sending the message
      List<Participant> targets = (from p in Participants
                                   where p.Name != From
                                   select p).ToList();
      //iterate and add to the Send queue for each
      foreach (Participant p in targets)
      {
        lock (p.QueueSyncRoot)
        {
          p.SendQueue.Enqueue(Message);
        }
      }
    }
    //send a message to a specific participant 
    internal void Send(string To, MessageWrapper Message)
    {
      //get the Participant from the list
      Participant target = (from p in Participants
                            where p.Name == To
                            select p).ToList()[0];
      //add to the send queue for the participant
      lock (target.QueueSyncRoot)
      {
        target.SendQueue.Enqueue(Message);
      }
    }
  }

  internal class Participant
  {
    //lock target
    internal object QueueSyncRoot = new object();
    //name as specified at the client
    internal string Name { get; set; }
    //the connected client socket
    internal Socket ClientSocket { get; set; }
    //a reference back tot he ServerConnectionManager instance
    internal ServerConnectionManager Parent { get; set; }
    //are we currently receiving a message from this participant ?
    bool Receiving = false;
    //are we currently sending a message to this participant ?
    bool Sending = false;
    //a queue to hold messages being sent to this participant
    private Queue<MessageWrapper> _SendQueue = new Queue<MessageWrapper>();
    internal Queue<MessageWrapper> SendQueue
    {
      get { return _SendQueue; }
      set { _SendQueue = value; }
    }
    //check to see if there are messages in the queue
    private int HasMessage()
    {
      lock (QueueSyncRoot)
      {
        return SendQueue.Count;
      }
    }
    //start the participant up
    internal void StartUp()
    {
      //create the receiver thread
      Thread thdParticipantReceiver = new Thread(new ThreadStart(
        //thread start delegate
          delegate
          {
            //loop while the socket is valid
            while (ClientSocket != null)
            {
              //if there is  no data available OR 
              //we are currently receiving, continue
              if (ClientSocket.Available <= 0 || Receiving) continue;
              //set receiving to true
              Receiving = true;
              //begin to receive the next message
              ReceiveMessage();
            }
          }));
      //set thread to background
      thdParticipantReceiver.IsBackground = true;
      //start receiver thread
      thdParticipantReceiver.Start();
      //create the sender thread
      Thread thdParticipantSender = new Thread(new ThreadStart(
        //thread start delegate
          delegate
          {
            //loop while the socket is valid
            while (ClientSocket != null)
            {
              //if there are no messages to be sent OR 
              //we are currently sending, continue
              if (HasMessage() == 0 || Sending) continue;
              //set sending to true
              Sending = true;
              //begin sending
              SendMessage();
            }
          }));
      //set thread to background
      thdParticipantSender.IsBackground = true;
      //start sender thread
      thdParticipantSender.Start();
    }
    //receive a message
    private void ReceiveMessage()
    {
      SocketAsyncEventArgs sockEvtArgs = new SocketAsyncEventArgs();
      //allocate a bufer as large as the available data
      sockEvtArgs.SetBuffer(
        new byte[ClientSocket.Available], 0, ClientSocket.Available);
      sockEvtArgs.Completed += new EventHandler<SocketAsyncEventArgs>(
        //completion handler
          delegate(object sender, SocketAsyncEventArgs e)
          {
            //process the message
            ProcessMessage(e.Buffer);
            //done receiving, thread loop will look for next
            Receiving = false;
          });
      //start receiving
      ClientSocket.ReceiveAsync(sockEvtArgs);
    }
    internal void ProcessMessage(byte[] Message)
    {
      //deserialize message
      MessageWrapper mw = MessageWrapper.DeserializeMessage(Message);
      //if text message
      if (mw.Message is TextMessage)
      {
        //send it to the target participant
        Parent.Send((mw.Message as TextMessage).To, mw);
      }
      //if it is a ConnectionDisconnectionRequest
      else if (mw.Message is ConnectionDisconnectionRequest)
      {
        ConnectionDisconnectionRequest connDisconnReq =
          mw.Message as ConnectionDisconnectionRequest;
        //if connecting
        if (connDisconnReq.Connect)
        {
          this.Name = connDisconnReq.From;
          //broadcast to everyone else
          Parent.Broadcast(this.Name, new MessageWrapper
          {
            Message = new ConnectionDisconnectionNotification
            {
              Participant = this.Name,
              Connect = true
            }
          });
          //send the list of all participants other than 
          //the one connecting to the connecting client
          Parent.Send(this.Name, new MessageWrapper
          {
            Message = new ConnectionReply
            {
              Participants =
              (from part in Parent.Participants
               where part.Name != this.Name
               select part.Name).ToList()
            }
          });
        }
        else //disconnecting
        {
          //remove from the participants list
          Parent.Participants.Remove(this);
          //close socket
          this.ClientSocket.Close();
          //reset
          this.ClientSocket = null;
          //broadcast to everyone else
          Parent.Broadcast(this.Name, new MessageWrapper
          {
            Message = new ConnectionDisconnectionNotification
            {
              Participant = this.Name,
              Connect = false
            }
          });
        }
      }
      //chat end
      else if (mw.Message is ChatEndNotification)
      {
        //send it to the other participant
        Parent.Send((mw.Message as ChatEndNotification).To, mw);
      }
    }
    //send a message
    private void SendMessage()
    {
      MessageWrapper mw = null;
      //dequeue a message from the send queue
      lock (QueueSyncRoot)
      {
        mw = SendQueue.Dequeue();
      }
      SocketAsyncEventArgs sockEvtArgs =
        new SocketAsyncEventArgs { UserToken = mw };
      //serialize and pack into the send buffer
      MemoryStream SerializedMessage =
        MessageWrapper.SerializeMessage(mw);
      sockEvtArgs.SetBuffer(
        SerializedMessage.GetBuffer(), 0, (int)SerializedMessage.Length);
      sockEvtArgs.Completed += new EventHandler<SocketAsyncEventArgs>(
        //completion handler
          delegate(object sender, SocketAsyncEventArgs e)
          {
            //not sending anymore
            Sending = false;
          });
      //begin send
      ClientSocket.SendAsync(sockEvtArgs);
    }
  }
}