using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Ch07_Networking.Recipe7_5.ChatClient
{
  public class ClientConnectionManager : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    //create a new socket
    Socket ClientSocket = new Socket(AddressFamily.InterNetwork,
      SocketType.Stream, ProtocolType.Tcp);
    //reference to the parent page
    public Page ParentPage { get; set; }
    //participants collection
    private ObservableCollection<string> _Participants;
    public ObservableCollection<string> Participants
    {
      get { return _Participants; }
      set
      {
        _Participants = value;
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs("Participants"));
      }
    }
    //collection of all messages exchanged in a particular conversation
    private ObservableCollection<TextMessage> _Conversation;
    public ObservableCollection<TextMessage> Conversation
    {
      get { return _Conversation; }
      set
      {
        _Conversation = value;
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs("Conversation"));
      }
    }
    //IP Address of the server connected to
    private string _IP;
    public string IP
    {
      get { return _IP; }
      set
      {
        _IP = value;
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs("IP"));
      }
    }
    //Port connected to
    private string _Port;
    public string Port
    {
      get { return _Port; }
      set
      {
        _Port = value;
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs("Port"));
      }
    }
    //name of the person logged in
    private string _Me;
    public string Me
    {
      get { return _Me; }
      set
      {
        _Me = value;
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs("Me"));

      }
    }
    //the other person in a conversation
    private string _TalkingTo;
    public string TalkingTo
    {
      get { return _TalkingTo; }
      set
      {
        _TalkingTo = value;
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs("TalkingTo"));
      }
    }
    //the body of a conversation message
    private string _MessageBody;
    public string MessageBody
    {
      get { return _MessageBody; }
      set
      {
        _MessageBody = value;
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs("MessageBody"));
      }
    }
    //buffer used to receive messages
    private const int RECEIVEBUFFERSIZE = 10 * 1024;
    private byte[] ReceiveBuffer = new Byte[RECEIVEBUFFERSIZE];
    //constructor
    public ClientConnectionManager()
    {
      //initialize the collections
      Participants = new ObservableCollection<string>();
      Conversation = new ObservableCollection<TextMessage>();
    }
    //called when the login button is clicked
    public void Join()
    {
      //create a new SocketEventArgs, specify the remote endpoint details
      SocketAsyncEventArgs sockEvtArgs =
        new SocketAsyncEventArgs
        {
          RemoteEndPoint = new IPEndPoint(IPAddress.Parse(IP),
            Convert.ToInt32(Port)),
          UserToken = Me
        };
      //connect a completion handler
      sockEvtArgs.Completed +=
        new EventHandler<SocketAsyncEventArgs>(Connection_Completed);
      //connect asynchronously
      ClientSocket.ConnectAsync(sockEvtArgs);

    }
    //connection completion handler
    void Connection_Completed(object sender, SocketAsyncEventArgs e)
    {
      //connected successfully, send a 
      //ConnectionDisconnectionRequest with Connect=true
      if (e.SocketError == SocketError.Success)
      {
        SocketAsyncEventArgs sockEvtArgs =
          new SocketAsyncEventArgs { UserToken = e.UserToken };
        //serialize a new ConnectionDisconnectionMessage into a MemoryStream
        MemoryStream SerializedStream =
          MessageWrapper.SerializeMessage(
          new MessageWrapper
          {
            Message = new ConnectionDisconnectionRequest
            {
              From = e.UserToken as string,
              Connect = true
            }
          });
        //set buffer to the contents of the memorystream
        sockEvtArgs.SetBuffer(SerializedStream.GetBuffer(),
          0, (int)SerializedStream.Length);
        sockEvtArgs.Completed +=
          new EventHandler<SocketAsyncEventArgs>(ConnectionRequestSend_Completed);
        //send
        ClientSocket.SendAsync(sockEvtArgs);
      }
    }
    //ConnectionDisconnectionRequest send completion handler
    void ConnectionRequestSend_Completed(object sender, SocketAsyncEventArgs e)
    {
      //sent successfully
      if (e.SocketError == SocketError.Success)
      {
        //start receiving messages
        ReceiveMessage();
        //switch context
        ParentPage.Dispatcher.BeginInvoke(new Action(delegate
        {
          //switch view to participants
          ParentPage.ShowParticipantsView();
        }));
      }
    }
    //receive a message
    private void ReceiveMessage()
    {
      SocketAsyncEventArgs sockEvtArgsReceive = new SocketAsyncEventArgs();
      sockEvtArgsReceive.SetBuffer(ReceiveBuffer, 0, RECEIVEBUFFERSIZE);
      sockEvtArgsReceive.Completed +=
        new EventHandler<SocketAsyncEventArgs>(Receive_Completed);
      ClientSocket.ReceiveAsync(sockEvtArgsReceive);
    }
    //receive completion handler
    void Receive_Completed(object sender, SocketAsyncEventArgs e)
    {
      if (e.SocketError == SocketError.Success)
      {
        ParentPage.Dispatcher.BeginInvoke(new Action(delegate
        {
          //copy the message to a temporary buffer - this is 
          //because we reuse the same buffer for all SocketAsyncEventArgs,
          //and message lengths may vary
          byte[] Message = new byte[e.BytesTransferred];
          Array.Copy(e.Buffer, 0, Message, 0, e.BytesTransferred);
          //process the message
          ProcessMessage(Message);
          //keep receiving
          ReceiveMessage();
        }));
      }
    }
    //process a message
    internal void ProcessMessage(byte[] Message)
    {
      //deserialize the message into the wrapper
      MessageWrapper mw = MessageWrapper.DeserializeMessage(Message);
      //check type of the contained message
      //correct type resolution is ensured through the 
      //usage of KnownTypeAttribute on the MessageWrapper 
      //data contract declaration
      if (mw.Message is TextMessage)
      {
        //receiving a text message from someone - 
        //switch to chat view if not there already
        ParentPage.ShowChatView();
        //remember the other party in the conversation
        if (this.TalkingTo == null)
          this.TalkingTo = (mw.Message as TextMessage).From;
        //data bind the text of the message
        Conversation.Add(mw.Message as TextMessage);
      }
      //some one has ended an ongoing chat
      else if (mw.Message is ChatEndNotification)
      {
        //reset
        this.TalkingTo = null;
        //reset
        Conversation.Clear();
        //go back to participants list
        ParentPage.ShowParticipantsView();
      }
      //server has sent a reply to your connection request
      else if (mw.Message is ConnectionReply)
      {
        //reset
        Participants.Clear();
        //get the list of the other participants
        List<string> ReplyList = (mw.Message as ConnectionReply).Participants;
        //data bind
        foreach (string s in ReplyList)
          Participants.Add(s);

      }
      //soneone has connected or disconnected
      else if (mw.Message is ConnectionDisconnectionNotification)
      {
        ConnectionDisconnectionNotification notif =
          mw.Message as ConnectionDisconnectionNotification;
        //if it is a connection
        if (notif.Connect)
          //add to participants list
          Participants.Add(notif.Participant);
        else
        {
          //remove from participants list
          Participants.Remove(notif.Participant);
          //if you were in a conversation with this person, 
          //go back to the participants view
          if (notif.Participant == TalkingTo)
          {
            ParentPage.ShowParticipantsView();
          }
        }
      }
    }
    //send a text message
    internal void SendTextMessage()
    {
      //package the From,To and Text of the message 
      //into a TextMessage, and then into a wrapper
      MessageWrapper mwSend =
        new MessageWrapper
        {
          Message = new TextMessage { 
            From = Me, To = TalkingTo, Body = MessageBody }
        };
      //serialize
      MemoryStream SerializedStream = MessageWrapper.SerializeMessage(mwSend);
      SocketAsyncEventArgs sockEvtArgsSend =
        new SocketAsyncEventArgs { UserToken = mwSend.Message };
      //grab the byte[] and set the buffer
      sockEvtArgsSend.SetBuffer(
        SerializedStream.GetBuffer(), 0, (int)SerializedStream.Length);
      //attach handler
      sockEvtArgsSend.Completed +=
        new EventHandler<SocketAsyncEventArgs>(SendTextMessage_Completed);
      //send
      ClientSocket.SendAsync(sockEvtArgsSend);
    }
    //send completed
    void SendTextMessage_Completed(object sender, SocketAsyncEventArgs e)
    {
      //scuccess
      if (e.SocketError == SocketError.Success)
      {
        //switch context
        ParentPage.Dispatcher.BeginInvoke(new Action(delegate
        {
          //send was successful, add message to ongoing conversation
          Conversation.Add(e.UserToken as TextMessage);
          //reset edit box
          MessageBody = "";
        }));
      }
    }
    //disconnect
    internal void Disconnect()
    {
      SocketAsyncEventArgs sockEvtArgs = new SocketAsyncEventArgs();
      //package a ConnectionDisconnectionRequest with Connect=false
      MemoryStream SerializedStream =
        MessageWrapper.SerializeMessage(
        new MessageWrapper
        {
          Message = new ConnectionDisconnectionRequest
          {
            From = Me,
            Connect = false
          }
        });
      sockEvtArgs.SetBuffer(
        SerializedStream.GetBuffer(), 0, (int)SerializedStream.Length);
      sockEvtArgs.Completed +=
        new EventHandler<SocketAsyncEventArgs>(DisconnectRequest_Completed);
      ClientSocket.SendAsync(sockEvtArgs);
    }
    //disconnect completed
    void DisconnectRequest_Completed(object sender, SocketAsyncEventArgs e)
    {
      //success
      if (e.SocketError == SocketError.Success)
      {
        //reset my identity
        this.Me = null;
        //clear all participants
        Participants.Clear();
        //show login screen
        ParentPage.ShowLoginView();
      }
    }
    //end a chat
    internal void SendChatEnd()
    {
      MessageWrapper mwSend =
        new MessageWrapper
        {
          Message = new ChatEndNotification { From = Me, To = TalkingTo }
        };
      MemoryStream SerializedStream =
        MessageWrapper.SerializeMessage(mwSend);
      SocketAsyncEventArgs sockEvtArgsSend =
        new SocketAsyncEventArgs { UserToken = mwSend.Message };
      sockEvtArgsSend.SetBuffer(
        SerializedStream.GetBuffer(), 0, (int)SerializedStream.Length);
      sockEvtArgsSend.Completed +=
        new EventHandler<SocketAsyncEventArgs>(SendChatEnd_Completed);
      ClientSocket.SendAsync(sockEvtArgsSend);
    }
    //chat ended
    void SendChatEnd_Completed(object sender, SocketAsyncEventArgs e)
    {
      //success
      if (e.SocketError == SocketError.Success)
      {
        //switch context
        ParentPage.Dispatcher.BeginInvoke(new Action(delegate
        {
          //reset identity of the other participant
          this.TalkingTo = null;
          //clear the conversation
          Conversation.Clear();
          //switch back to the participants view
          ParentPage.ShowParticipantsView();
        }));
      }
    }
  }
}