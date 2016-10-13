using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Ch07_Networking.Recipe7_3.ProductsDataJSONService
{
  [ServiceContract]
  public interface IProductManager
  {
    [OperationContract]
    [WebGet(ResponseFormat = WebMessageFormat.Json)]
    List<ProductHeader> GetProductHeaders();

    [OperationContract]
    [WebInvoke(RequestFormat = WebMessageFormat.Json)]
    void UpdateProductHeaders(List<ProductHeader> Updates);

    [OperationContract]
    [WebGet(ResponseFormat = WebMessageFormat.Json)]
    ProductDetail GetProductDetail(ushort ProductId);

    [OperationContract]
    [WebInvoke(RequestFormat = WebMessageFormat.Json)]
    void UpdateProductDetail(ProductDetail Update);
  }
}



