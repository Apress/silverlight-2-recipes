using System.ServiceModel;
using System.ServiceModel.Web;
using System.Xml;

namespace Ch08_RichMedia.Recipe8_2
{  
  [ServiceContract]
  public interface IMediaLocationProvider
  {
    [WebGet]
    [OperationContract]
    [XmlSerializerFormat]
    XmlDocument GetLocationList();    
  }  
}
