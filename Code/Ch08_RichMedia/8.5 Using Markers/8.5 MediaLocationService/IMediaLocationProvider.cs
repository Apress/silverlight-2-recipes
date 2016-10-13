using System.ServiceModel;
using System.ServiceModel.Web;
using System.Xml;

namespace Ch08_RichMedia.Recipe8_5
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

    [WebGet(UriTemplate="Captions?MediaId={Id}")]
    [OperationContract]
    [XmlSerializerFormat]
    XmlDocument GetCaptionsForMedia(string Id);

    [WebGet(UriTemplate = "Commercial?Marker={MarkerType}")]
    [OperationContract]
    [XmlSerializerFormat]
    XmlDocument GetCommercial(string MarkerType);   
  }  
}
