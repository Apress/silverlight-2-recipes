using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;

namespace AdventureWorksDataService
{
  // NOTE: If you change the interface name "IService" here, you must also update the reference to "IService" in Web.config.
  [ServiceContract]
  public interface IAdvWorksDataService
  {
    [OperationContract]
    List<Product> GetProducts();
    [OperationContract]
    List<ProductInventory> GetInventory(Product product);
    [OperationContract]
    List<ProductCostHistory> GetProductCostHistory(Product product);
    [OperationContract]
    ProductPhoto GetPhotos(Product product);
    [OperationContract]
    ProductDescription GetDescription(Product product);
    [OperationContract]
    ProductSubcategory GetSubcategory(Product product);
    [OperationContract]
    ProductCategory GetCategory(Product product);
    [OperationContract]
    ProductReview GetReview(Product product);
    [OperationContract]
    List<ProductCategory> GetAllCategories();
    [OperationContract]
    List<ProductSubcategory> GetAllSubCategories();
    [OperationContract]
    int GetProductsCount();
    [OperationContract]
    List<Product> GetProductPage(int SkipCount, int TakeCount);
    [OperationContract]
    List<int> GetPhotoIds();



  }

  [ServiceContract]
  public interface IPhotoDownload
  {
    [OperationContract]
    [WebGet(UriTemplate = "Photos?Id={ProductPhotoId}")]
    //get a full resolution image 
    byte[] GetPhoto(int ProductPhotoId);
  }
}


