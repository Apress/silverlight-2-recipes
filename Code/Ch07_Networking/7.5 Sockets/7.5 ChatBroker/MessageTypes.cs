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

[DataContract]
[KnownType(typeof(ConnectionDisconnectionRequest))]
[KnownType(typeof(ConnectionReply))]
[KnownType(typeof(ConnectionDisconnectionNotification))]
[KnownType(typeof(TextMessage))]
[KnownType(typeof(ChatEndNotification))]
public class MessageWrapper
{ 
    [DataMember]
    public object Message { get; set; }

    public static MessageWrapper DeserializeMessage(byte[] Message)
    {
        MemoryStream ms = new MemoryStream(Message);
        DataContractJsonSerializer dcSer = new DataContractJsonSerializer(typeof(MessageWrapper));
        MessageWrapper mw = dcSer.ReadObject(ms) as MessageWrapper;
        return mw;
    }

    public static MemoryStream SerializeMessage(MessageWrapper Message)
    {
        MemoryStream ms = new MemoryStream();
        DataContractJsonSerializer dcSer = new DataContractJsonSerializer(typeof(MessageWrapper));
        dcSer.WriteObject(ms, Message);
        return ms;
    }
}

[DataContract]
public class ConnectionDisconnectionRequest
{
    [DataMember]
    public string From { get; set; }
    [DataMember]
    public bool Connect { get; set; }

}
[DataContract]
public class ConnectionReply
{
    [DataMember]
    public List<string> Participants;
}

[DataContract]
public class ConnectionDisconnectionNotification
{
    [DataMember]
    public string Participant { get; set; }
    [DataMember]
    public bool Connect { get; set; }

}
[DataContract]
public class ChatEndNotification
{
    [DataMember]
    public string From { get; set; }
    [DataMember]
    public string To { get; set; }
    

}

[DataContract]
public class TextMessage
{
    [DataMember]
    public string From { get; set; }
    [DataMember]
    public string To { get; set; }
    [DataMember]
    public string Body { get; set; }
}

 