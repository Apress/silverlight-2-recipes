using System.Collections.Generic;
using System.ServiceModel;

namespace Ch07_Networking.Recipe7_1.ProductsDataSoapService
{
  [ServiceContract]
  public interface IProductManager
  {
    [OperationContract]
    List<ProductHeader> GetProductHeaders();
    [OperationContract]
    void UpdateProductHeaders(List<ProductHeader> Updates);

    [OperationContract]
    ProductDetail GetProductDetail(ushort ProductId);

    [OperationContract]
    void UpdateProductDetail(ProductDetail Update);
  }
}


