using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using System.Xml.Linq;

namespace Ch07_Networking.Recipe7_1.ProductsDataSoapService
{
  [AspNetCompatibilityRequirements(
    RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
  public class ProductManager : IProductManager
  {
    public List<ProductHeader> GetProductHeaders()
    {
      //open the local XML data file for products
      StreamReader stmrdrProductData = new StreamReader(
        new FileStream(HttpContext.Current.Request.MapPath(
          "App_Data/XML/Products.xml"), FileMode.Open));
      //create a Linq To XML Xdocument and load the data
      XDocument xDocProducts = XDocument.Load(stmrdrProductData);
      //close the stream
      stmrdrProductData.Close();
      //transform the XML data to a collection of ProductHeader 
      //using a Linq To XML query
      IEnumerable<ProductHeader> ProductData =
       from elemProduct in xDocProducts.Root.Elements()
       select new ProductHeader
       {
         Name = elemProduct.Attribute("Name") != null ?
          elemProduct.Attribute("Name").Value : null,
         ListPrice = elemProduct.Attribute("ListPrice") != null ?
                    new decimal?(Convert.ToDecimal(
                      elemProduct.Attribute("ListPrice").Value))
                    : null,
         ProductId = elemProduct.Attribute("ProductId") != null ?
                    new ushort?(Convert.ToUInt16(
                      elemProduct.Attribute("ProductId").Value)) :
                    null,
         SellEndDate = elemProduct.Attribute("SellEndDate") != null ?
          elemProduct.Attribute("SellEndDate").Value : null,
         SellStartDate = elemProduct.Attribute("SellStartDate") != null ?
          elemProduct.Attribute("SellStartDate").Value : null
       };
      //return a List<ProductHeader>
      return ProductData.ToList();
    }

    public void UpdateProductHeaders(List<ProductHeader> Updates)
    {
      //open the local data fiel and load into an XDocument
      StreamReader stmrdrProductData = new StreamReader(
        new FileStream(HttpContext.Current.Request.MapPath(
          "App_Data/XML/Products.xml"), FileMode.Open));
      XDocument xDocProducts = XDocument.Load(stmrdrProductData);
      stmrdrProductData.Close();

      //for each of the ProductHeader instances
      foreach (ProductHeader Prod in Updates)
      {
        //find the corresponding XElement in the loaded XDocument
        XElement elemTarget =
         (from elemProduct in xDocProducts.Root.Elements()
          where Convert.ToUInt16(elemProduct.Attribute("ProductId").Value)
          == Prod.ProductId
          select elemProduct).ToList()[0];
        //and updates the attrbiutes with the changes
        if (elemTarget.Attribute("Name") != null)
          elemTarget.Attribute("Name").SetValue(Prod.Name);
        if (elemTarget.Attribute("ListPrice") != null
                && Prod.ListPrice.HasValue)
          elemTarget.Attribute("ListPrice").SetValue(Prod.ListPrice);
        if (elemTarget.Attribute("SellEndDate") != null)
          elemTarget.Attribute("SellEndDate").SetValue(Prod.SellEndDate);
        if (elemTarget.Attribute("SellStartDate") != null)
          elemTarget.Attribute("SellStartDate").SetValue(Prod.SellStartDate);
      }
      //save the XDocument with the changes back to the data file
      StreamWriter stmwrtrProductData = new StreamWriter(
        new FileStream(HttpContext.Current.Request.MapPath(
          "App_Data/XML/Products.xml"), FileMode.Truncate));
      xDocProducts.Save(stmwrtrProductData);
      stmwrtrProductData.Close();

    } 

    public ProductDetail GetProductDetail(ushort ProductId)
    {
      StreamReader stmrdrProductData = new StreamReader(
        new FileStream(
          HttpContext.Current.Request.MapPath("App_Data/XML/Products.xml"),
          FileMode.Open));

      XDocument xDocProducts = XDocument.Load(stmrdrProductData);
      stmrdrProductData.Close();

      IEnumerable<ProductDetail> ProductData =
        from elemProduct in xDocProducts.Root.Elements()
        where elemProduct.Attribute("ProductId").Value == ProductId.ToString()
        select new ProductDetail
        {

          Class = elemProduct.Attribute("Class") != null ?
           elemProduct.Attribute("Class").Value : null,
          Color = elemProduct.Attribute("Color") != null ?
           elemProduct.Attribute("Color").Value : null,
          DaysToManufacture = elemProduct.Attribute("DaysToManufacture") != null ?
           new byte?(
             Convert.ToByte(elemProduct.Attribute("DaysToManufacture").Value))
             : null,
          DiscontinuedDate = elemProduct.Attribute("DiscontinuedDate") != null ?
           elemProduct.Attribute("DiscontinuedDate").Value : null,
          FinishedGoodsFlag = elemProduct.Attribute("FinishedGoodsFlag") != null ?
           elemProduct.Attribute("FinishedGoodsFlag").Value : null,

          MakeFlag = elemProduct.Attribute("MakeFlag") != null ?
           elemProduct.Attribute("MakeFlag").Value : null,
          ProductId = elemProduct.Attribute("ProductId") != null ?
           new ushort?(
             Convert.ToUInt16(elemProduct.Attribute("ProductId").Value)) 
             : null,
          ProductLine = elemProduct.Attribute("ProductLine") != null ?
           elemProduct.Attribute("ProductLine").Value : null,
          ProductNumber = elemProduct.Attribute("ProductNumber") != null ?
           elemProduct.Attribute("ProductNumber").Value : null,
          ReorderPoint = elemProduct.Attribute("ReorderPoint") != null ?
           new ushort?(
             Convert.ToUInt16(elemProduct.Attribute("ReorderPoint").Value)) 
             : null,
          SafetyStockLevel = elemProduct.Attribute("SafetyStockLevel") != null ?
           new ushort?(
             Convert.ToUInt16(elemProduct.Attribute("SafetyStockLevel").Value))
             : null,
          StandardCost = elemProduct.Attribute("StandardCost") != null ?
           new decimal?(Convert.ToDecimal(
             elemProduct.Attribute("StandardCost").Value))
           : null,
          Style = elemProduct.Attribute("Style") != null ?
           elemProduct.Attribute("Style").Value : null

        };

      return ProductData.ToList()[0];
    }

    public void UpdateProductDetail(ProductDetail Update)
    {
      StreamReader stmrdrProductData = new StreamReader(
        new FileStream(
          HttpContext.Current.Request.MapPath("App_Data/XML/Products.xml"),
          FileMode.Open));
      XDocument xDocProducts = XDocument.Load(stmrdrProductData);
      stmrdrProductData.Close();

      XElement elemTarget = 
        (from elemProduct in xDocProducts.Root.Elements()
           where Convert.ToUInt16(elemProduct.Attribute("ProductId").Value) 
           == Update.ProductId
           select elemProduct).ToList()[0];

      if (elemTarget.Attribute("Class") != null)
        elemTarget.Attribute("Class").SetValue(Update.Class);
      if (elemTarget.Attribute("Color") != null)
        elemTarget.Attribute("Color").SetValue(Update.Color);
      if (elemTarget.Attribute("DaysToManufacture") != null 
        && Update.DaysToManufacture.HasValue)
        elemTarget.Attribute("DaysToManufacture").
          SetValue(Update.DaysToManufacture);
      if (elemTarget.Attribute("DiscontinuedDate") != null)
        elemTarget.Attribute("DiscontinuedDate").
          SetValue(Update.DiscontinuedDate);
      if (elemTarget.Attribute("FinishedGoodsFlag") != null)
        elemTarget.Attribute("FinishedGoodsFlag").
          SetValue(Update.FinishedGoodsFlag);
      if (elemTarget.Attribute("MakeFlag") != null)
        elemTarget.Attribute("MakeFlag").
          SetValue(Update.MakeFlag);
      if (elemTarget.Attribute("ProductLine") != null)
        elemTarget.Attribute("ProductLine").
          SetValue(Update.ProductLine);
      if (elemTarget.Attribute("ProductNumber") != null)
        elemTarget.Attribute("ProductNumber").
          SetValue(Update.ProductNumber);
      if (elemTarget.Attribute("ReorderPoint") != null 
        && Update.ReorderPoint.HasValue)
        elemTarget.Attribute("ReorderPoint").
          SetValue(Update.ReorderPoint);
      if (elemTarget.Attribute("SafetyStockLevel") != null 
        && Update.SafetyStockLevel.HasValue)
        elemTarget.Attribute("SafetyStockLevel").
          SetValue(Update.SafetyStockLevel);
      if (elemTarget.Attribute("StandardCost") != null 
        && Update.StandardCost.HasValue)
        elemTarget.Attribute("StandardCost").
          SetValue(Update.StandardCost);
      if (elemTarget.Attribute("Style") != null)
        elemTarget.Attribute("Style").
          SetValue(Update.Style);

      StreamWriter stmwrtrProductData = 
        new StreamWriter(
          new FileStream(
            HttpContext.Current.Request.MapPath("App_Data/XML/Products.xml"),
            FileMode.Truncate));

      xDocProducts.Save(stmwrtrProductData);
      stmwrtrProductData.Close();
    }
  }
}
