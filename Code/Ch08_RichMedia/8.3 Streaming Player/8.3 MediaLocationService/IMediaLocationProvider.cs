using System.ServiceModel;
using System.ServiceModel.Web;
using System.Xml;

namespace Ch08_RichMedia.Recipe8_3
{  
  [ServiceContract]
  public interface IMediaLocationProvider
  {
    [WebGet]
    [OperationContract]
    [XmlSerializerFormat]
    XmlDocument GetDownloadsList();

    [WebGet]
    [OperationContract]
    [XmlSerializerFormat]
    XmlDocument GetOnDemandStreamsList();

    [WebGet]
    [OperationContract]
    [XmlSerializerFormat]
    XmlDocument GetBroadcastStreamsList();   
  }  
}
