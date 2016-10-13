using System.ServiceModel;
using System.ServiceModel.Web;
using System.Xml;

namespace Ch07_Networking.Recipe7_2.ProductsDataPOXService
{
  [ServiceContract]
  public interface IProductManager
  {
    [OperationContract]
    [XmlSerializerFormat()]
    [WebGet()]
    XmlDocument GetProductHeaders();

    [OperationContract]
    [XmlSerializerFormat()]
    [WebInvoke()]
    void UpdateProductHeaders(XmlDocument Updates);

    [OperationContract]
    [XmlSerializerFormat()]
    [WebGet()]
    XmlDocument GetProductDetail(ushort ProductId);

    [OperationContract]
    [XmlSerializerFormat()]
    [WebInvoke()]
    void UpdateProductDetail(XmlDocument Update);
  }
}


