using System.IO;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Ch07_Networking.Recipe7_2.ProductsDataPOXService
{
  [AspNetCompatibilityRequirements(
    RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
  public class ProductManager : IProductManager
  {

    public XmlDocument GetProductHeaders()
    {
      //open the local data file
      StreamReader stmrdrProductData = 
        new StreamReader(
          new FileStream(
            HttpContext.Current.Request.MapPath("App_Data/XML/Products.xml"),
            FileMode.Open));
      //creat and load an XmlDocument
      XmlDocument xDoc = new XmlDocument();
      xDoc.LoadXml(stmrdrProductData.ReadToEnd());
      stmrdrProductData.Close();

      //return the document
      HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
      return xDoc;
    }

    public void UpdateProductHeaders(XmlDocument Updates)
    {
      //load the XmlDocument containing the updates into a LINQ XDocument 
      XDocument xDocProductUpdates = XDocument.Parse(Updates.OuterXml);
      //load the local data file
      StreamReader stmrdrProductData = 
        new StreamReader(
          new FileStream(
            HttpContext.Current.Request.MapPath("App_Data/XML/Products.xml"),
            FileMode.Open));
      XDocument xDocProducts = XDocument.Load(stmrdrProductData);
      stmrdrProductData.Close();
      //for each of the updated records, find the matching record in the local data using a LINQ query
      //and update the appropriate fields
      foreach (XElement elemProdUpdate in xDocProductUpdates.Root.Elements())
      {
        XElement elemTarget = 
          (from elemProduct in xDocProducts.Root.Elements() 
           where elemProduct.Attribute("ProductId").Value == 
           elemProdUpdate.Attribute("ProductId").Value 
           select elemProduct).ToList()[0];

       

        if (elemTarget.Attribute("Name") != null)
          elemTarget.Attribute("Name").
            SetValue(elemProdUpdate.Attribute("Name").Value);
        if (elemTarget.Attribute("ListPrice") != null)
          elemTarget.Attribute("ListPrice").
            SetValue(elemProdUpdate.Attribute("ListPrice").Value);
        if (elemTarget.Attribute("SellEndDate") != null)
          elemTarget.Attribute("SellEndDate").
            SetValue(elemProdUpdate.Attribute("SellEndDate").Value);
        if (elemTarget.Attribute("SellStartDate") != null)
          elemTarget.Attribute("SellStartDate").
            SetValue(elemProdUpdate.Attribute("SellStartDate").Value);

         
      }
      //save the changes
      StreamWriter stmwrtrProductData = 
        new StreamWriter(
          new FileStream(
            HttpContext.Current.Request.MapPath("App_Data/XML/Products.xml"),
            FileMode.Truncate));
      xDocProducts.Save(stmwrtrProductData);
      stmwrtrProductData.Close();
    }

    public XmlDocument GetProductDetail(ushort ProductId)
    {
      StreamReader stmrdrProductData = 
        new StreamReader(
          new FileStream(
            HttpContext.Current.Request.MapPath("App_Data/XML/Products.xml"),
            FileMode.Open));
      XDocument xDocProducts = XDocument.Load(stmrdrProductData);
      XDocument xDocProdDetail = new XDocument(
        (from xElem in xDocProducts.Root.Elements()
          where xElem.Attribute("ProductId").Value == ProductId.ToString()
          select xElem).ToList()[0]);

      XmlDocument xDoc = new XmlDocument();
      xDoc.LoadXml(xDocProdDetail.ToString());
      stmrdrProductData.Close();

      HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
      return xDoc;
    }

    public void UpdateProductDetail(XmlDocument Update)
    {
      XDocument xDocProductUpdates = XDocument.Parse(Update.OuterXml);
      XElement elemProdUpdate = xDocProductUpdates.Root;
      StreamReader stmrdrProductData = 
        new StreamReader(
          new FileStream(
            HttpContext.Current.Request.MapPath("App_Data/XML/Products.xml"),
            FileMode.Open));

      XDocument xDocProducts = XDocument.Load(stmrdrProductData);
      stmrdrProductData.Close();

      XElement elemTarget = 
        (from elemProduct in xDocProducts.Root.Elements()
         where elemProduct.Attribute("ProductId").Value == 
         elemProdUpdate.Attribute("ProductId").Value
         select elemProduct).ToList()[0];

      if (elemTarget.Attribute("Class") != null)
        elemTarget.Attribute("Class").
          SetValue(elemProdUpdate.Attribute("Class").Value);
      if (elemTarget.Attribute("Color") != null)
        elemTarget.Attribute("Color").
          SetValue(elemProdUpdate.Attribute("Color").Value);
      if (elemTarget.Attribute("DaysToManufacture") != null)
        elemTarget.Attribute("DaysToManufacture").
          SetValue(elemProdUpdate.Attribute("DaysToManufacture").Value);
      if (elemTarget.Attribute("DiscontinuedDate") != null)
        elemTarget.Attribute("DiscontinuedDate").
          SetValue(elemProdUpdate.Attribute("DiscontinuedDate").Value);
      if (elemTarget.Attribute("FinishedGoodsFlag") != null)
        elemTarget.Attribute("FinishedGoodsFlag").
          SetValue(elemProdUpdate.Attribute("FinishedGoodsFlag").Value);
      if (elemTarget.Attribute("MakeFlag") != null)
        elemTarget.Attribute("MakeFlag").
          SetValue(elemProdUpdate.Attribute("MakeFlag").Value);
      if (elemTarget.Attribute("ProductLine") != null)
        elemTarget.Attribute("ProductLine").
          SetValue(elemProdUpdate.Attribute("ProductLine").Value);
      if (elemTarget.Attribute("ProductNumber") != null)
        elemTarget.Attribute("ProductNumber").
          SetValue(elemProdUpdate.Attribute("ProductNumber").Value);
      if (elemTarget.Attribute("ReorderPoint") != null)
        elemTarget.Attribute("ReorderPoint").
          SetValue(elemProdUpdate.Attribute("ReorderPoint").Value);
      if (elemTarget.Attribute("SafetyStockLevel") != null)
        elemTarget.Attribute("SafetyStockLevel").
          SetValue(elemProdUpdate.Attribute("SafetyStockLevel").Value);
      if (elemTarget.Attribute("StandardCost") != null)
        elemTarget.Attribute("StandardCost").
          SetValue(elemProdUpdate.Attribute("StandardCost").Value);
      if (elemTarget.Attribute("Style") != null)
        elemTarget.Attribute("Style").
          SetValue(elemProdUpdate.Attribute("Style").Value);

      StreamWriter stmwrtrProductData = new StreamWriter(new FileStream(HttpContext.Current.Request.MapPath("App_Data/XML/Products.xml"), FileMode.Truncate));
      xDocProducts.Save(stmwrtrProductData);
      stmwrtrProductData.Close();
    }
  }
}
