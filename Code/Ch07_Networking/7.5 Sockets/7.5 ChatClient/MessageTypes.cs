using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

[DataContract]
[KnownType(typeof(ConnectionDisconnectionRequest))]
[KnownType(typeof(ConnectionReply))]
[KnownType(typeof(ConnectionDisconnectionNotification))]
[KnownType(typeof(TextMessage))]
[KnownType(typeof(ChatEndNotification))]
// a wrapper message that contaisn the actual message, 
// facilitating easy serialization and deserialization
public class MessageWrapper
{
  [DataMember]
  public object Message { get; set; }

  //Deserialize a byte[] into a MessageWrapper
  public static MessageWrapper DeserializeMessage(byte[] Message)
  {
    MemoryStream ms = new MemoryStream(Message);
    DataContractJsonSerializer dcSer =
      new DataContractJsonSerializer(typeof(MessageWrapper));
    MessageWrapper mw = dcSer.ReadObject(ms) as MessageWrapper;
    return mw;
  }
  //serialize a MessageWrapper into a MemoryStream
  public static MemoryStream SerializeMessage(MessageWrapper Message)
  {
    MemoryStream ms = new MemoryStream();
    DataContractJsonSerializer dcSer =
      new DataContractJsonSerializer(typeof(MessageWrapper));
    dcSer.WriteObject(ms, Message);
    return ms;
  }
}

//a request from a client to the server for either a connection or a disconnection
[DataContract]
public class ConnectionDisconnectionRequest
{
  [DataMember]
  public string From { get; set; }
  [DataMember]
  public bool Connect { get; set; }

}
//a reply from the server on successful connection
[DataContract]
public class ConnectionReply
{
  [DataMember]
  public List<string> Participants;
}
//a broadcast style notification to all connected clients about a 
//specific client's connection/disconnection activity
[DataContract]
public class ConnectionDisconnectionNotification
{
  [DataMember]
  public string Participant { get; set; }
  [DataMember]
  public bool Connect { get; set; }
}
//a notification from a client to the srever that it has ended a chat
[DataContract]
public class ChatEndNotification
{
  [DataMember]
  public string From { get; set; }
  [DataMember]
  public string To { get; set; }
}
//a chat message
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


