using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Activation;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Web;

namespace AdventureWorksDataService
{

  // NOTE: If you change the class name "Service" here, you must also update the reference to "Service" in Web.config and in the associated .svc file.
  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
  public class DataService : IAdvWorksDataService 
  {
    public List<Product> GetProducts()
    {
      ProductsDataContext dc = new ProductsDataContext();
      List<Product> retval = (from p in dc.Products
                              join ppp in dc.ProductProductPhotos on p.ProductID equals ppp.ProductID
                              where ppp.ProductPhotoID != 1
                              select p).ToList();

      return retval;
    }
    public List<ProductInventory> GetInventory(Product product)
    {
      ProductsDataContext dc = new ProductsDataContext();
      dc.Products.Attach(product);
      return product.ProductInventories.ToList();
    }
    public ProductPhoto GetPhotos(Product product)
    {
      ProductsDataContext dc = new ProductsDataContext();
      dc.Products.Attach(product);
      if (product.ProductProductPhotos.Count == 0)
        return null;
      else
        return PhotoConverter.GIFtoPNG(product.ProductProductPhotos[0].ProductPhoto);

    }
    

    #region IAdvWorksDataService Members


    public ProductDescription GetDescription(Product product)
    {
      ProductsDataContext dc = new ProductsDataContext();
      dc.Products.Attach(product);
      if (product.ProductModel == null || product.ProductModel.ProductModelProductDescriptionCultures.Count == 0)
        return null;
      else
        return product.ProductModel.ProductModelProductDescriptionCultures[0].ProductDescription;
    }

    public ProductSubcategory GetSubcategory(Product product)
    {
      ProductsDataContext dc = new ProductsDataContext();
      dc.Products.Attach(product);
      return product.ProductSubcategory;

    }

    public ProductCategory GetCategory(Product product)
    {
      ProductsDataContext dc = new ProductsDataContext();
      dc.Products.Attach(product);
      if (product.ProductSubcategory == null)
        return null;
      else
        return product.ProductSubcategory.ProductCategory;
    }
    public ProductReview GetReview(Product product)
    {
      ProductsDataContext dc = new ProductsDataContext();
      dc.Products.Attach(product);
      if (product.ProductReviews.Count == 0)
        return null;
      else
        return product.ProductReviews[0];
    }

    #endregion

    #region IAdvWorksDataService Members


    public List<ProductCategory> GetAllCategories()
    {
      ProductsDataContext dc = new ProductsDataContext();
      return dc.ProductCategories.ToList();
    }
    public List<ProductSubcategory> GetAllSubCategories()
    {
      ProductsDataContext dc = new ProductsDataContext();
      return dc.ProductSubcategories.ToList();
    }


    #endregion

    #region IAdvWorksDataService Members


    public int GetProductsCount()
    {
      ProductsDataContext dc = new ProductsDataContext();

      return dc.Products.Count();
    }

    public List<Product> GetProductPage(int SkipCount, int TakeCount)
    {
      ProductsDataContext dc = new ProductsDataContext();

      return (from Prod in dc.Products select Prod).Skip(SkipCount).Take(TakeCount).ToList();
    }

    #endregion

    #region IAdvWorksDataService Members


    public List<ProductCostHistory> GetProductCostHistory(Product product)
    {
      ProductsDataContext dc = new ProductsDataContext();
      dc.Products.Attach(product);
      return product.ProductCostHistories.ToList();
    }

    #endregion

    

    #region IAdvWorksDataService Members


    public List<int> GetPhotoIds()
    {
      ProductsDataContext dc = new ProductsDataContext();
      return (from pPhoto in dc.ProductPhotos select pPhoto.ProductPhotoID).ToList();
    }

    #endregion
  }
  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
  public class PhotoService : IPhotoDownload
  {
    #region IPhotoDownload Members

    public byte[] GetPhoto(int ProductPhotoId)
    {
      ProductsDataContext dc = new ProductsDataContext();
      ProductPhoto pPhotoPng =
        PhotoConverter.GIFtoPNG((from pPhoto in dc.ProductPhotos where pPhoto.ProductPhotoID == ProductPhotoId select pPhoto).ToList()[0]);
      HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
      return pPhotoPng.LargePhoto.ToArray();

    }

    #endregion
  }
  internal static class PhotoConverter
  {
    internal static ProductPhoto GIFtoPNG(ProductPhoto photo)
    {
      MemoryStream streamThumbGif = new MemoryStream(photo.ThumbNailPhoto.ToArray());
      GifBitmapDecoder gifDecoder = new GifBitmapDecoder(streamThumbGif, BitmapCreateOptions.None, BitmapCacheOption.OnDemand);
      BitmapSource src = gifDecoder.Frames[0];


      MemoryStream streamThumbPng = new MemoryStream();
      PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
      pngEncoder.Interlace = PngInterlaceOption.On;
      pngEncoder.Frames.Add(BitmapFrame.Create(src));
      pngEncoder.Save(streamThumbPng);
      photo.ThumbNailPhoto = new System.Data.Linq.Binary(streamThumbPng.GetBuffer());
      streamThumbPng.Close();
      streamThumbGif.Close();

      MemoryStream streamLargeGif = new MemoryStream(photo.LargePhoto.ToArray());
      gifDecoder = new GifBitmapDecoder(streamLargeGif, BitmapCreateOptions.None, BitmapCacheOption.OnDemand);
      src = gifDecoder.Frames[0];


      MemoryStream streamLargePng = new MemoryStream();
      pngEncoder = new PngBitmapEncoder();
      pngEncoder.Interlace = PngInterlaceOption.On;
      pngEncoder.Frames.Add(BitmapFrame.Create(src));
      pngEncoder.Save(streamLargePng);
      photo.LargePhoto = new System.Data.Linq.Binary(streamLargePng.GetBuffer());
      streamLargePng.Close();
      streamLargeGif.Close();

      return photo;


    }
  }
}
