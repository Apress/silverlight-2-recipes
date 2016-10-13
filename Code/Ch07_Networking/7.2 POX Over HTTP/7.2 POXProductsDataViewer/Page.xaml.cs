using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using System.Windows.Browser;

namespace Ch07_Networking.Recipe7_2.POXProductsDataViewer
{
    public partial class Page : UserControl
    {
      private const string ServiceUri = 
          "http://localhost:9292/ProductsPOXService.svc";
      bool InEdit = false;
        
        public Page()
        {
            InitializeComponent();

            RequestProductHeaders();
        }

        
        private List<ProductHeader> DeserializeProductHeaders(string HeaderXml)
        {
            //load into a LINQ To XML Xdocument
            XDocument xDocProducts = XDocument.Parse(HeaderXml);          
            //for each Product Xelement, project a new ProductHeader
            List<ProductHeader> ProductList =  
              (from elemProduct in xDocProducts.Root.Elements()
                select new ProductHeader
                {
                    Name = elemProduct.Attribute("Name") != null ? 
                      elemProduct.Attribute("Name").Value : null,                                                              
                    ListPrice = elemProduct.Attribute("ListPrice") != null ? 
                      new decimal?(
                        Convert.ToDecimal(elemProduct.Attribute("ListPrice").
                        Value)) : null,                                                              
                    ProductId = elemProduct.Attribute("ProductId") != null ? 
                      new ushort?(Convert.ToUInt16(elemProduct.Attribute("ProductId").
                        Value)) : null,                                                              
                    SellEndDate = elemProduct.Attribute("SellEndDate") != null ? 
                      elemProduct.Attribute("SellEndDate").Value : null,
                    SellStartDate = elemProduct.Attribute("SellStartDate") != null ? 
                      elemProduct.Attribute("SellStartDate").Value : null                                                              

                }).ToList();
            //return the list
            return ProductList;
        }
        

        private void RequestProductHeaders()
        {
            //create and initialize an HttpWebRequest
            WebRequest webReq = HttpWebRequest.Create(
              new Uri(string.Format("{0}/GetProductHeaders",ServiceUri))); 
           
            //GET a response, passing in OnProductHeadersReceived as the completion callback, and the WebRequest as state
            webReq.BeginGetResponse(
              new AsyncCallback(OnProductHeadersReceived), webReq);
        }

        private void OnProductHeadersReceived(IAsyncResult target)
        {
            //reacquire the WebRequest from the passed in state
            WebRequest webReq = target.AsyncState as WebRequest;
            //get the WebResponse
            WebResponse webResp = webReq.EndGetResponse(target);
            
            //get the response stream, and wrap in a StreamReader for reading as text
            StreamReader stmReader = new StreamReader(webResp.GetResponseStream());
            //read the incoming POX into a string
            string ProductHeadersXml = stmReader.ReadToEnd();
            stmReader.Close();
           
          
            //use the Dispatcher to switch context to the main thread
            //deserialize the POX into a Product Header collection, and bind to the DataGrid
            Dispatcher.BeginInvoke(new Action(delegate
            {
                ProductHeaderDataGrid.ItemsSource = 
                  DeserializeProductHeaders(ProductHeadersXml);
            }), null);

        }
        private void UpdateProductHeaders()
        {
          //create and initialize an HttpWebRequest
          WebRequest webReq = HttpWebRequest.Create(
            new Uri(string.Format("{0}/UpdateProductHeaders", ServiceUri)));
          //set the VERB to POST
          webReq.Method = "POST";
          //set the MIME type to send POX
          webReq.ContentType = "text/xml";
          //begin acquiring the request stream
          webReq.BeginGetRequestStream(
            new AsyncCallback(OnProdHdrUpdReqStreamAcquired), webReq);
        }

        private void OnProdHdrUpdReqStreamAcquired(IAsyncResult target)
        {
          //get the passed in the WebRequest
          HttpWebRequest webReq = target.AsyncState as HttpWebRequest;
          //get the request stream, wrap in a writer
          StreamWriter stmUpdates = 
            new StreamWriter(webReq.EndGetRequestStream(target));
         
          Dispatcher.BeginInvoke(new Action(delegate
            {
              //select all the updated records
              List<ProductHeader> AllItems =
                ProductHeaderDataGrid.ItemsSource as List<ProductHeader>;
              List<ProductHeader> UpdateList = new List<ProductHeader>
                                          (
                                            from Prod in AllItems
                                            where Prod.Dirty == true
                                            select Prod
                                          );

              //use LINQ To XML to transform to XML
              XElement Products = new XElement("Products",
                from Prod in UpdateList
                select new XElement("Product",
                   new XAttribute("Name", Prod.Name),
                   new XAttribute("ListPrice", Prod.ListPrice),
                   new XAttribute("ProductId", Prod.ProductId),
                   new XAttribute("SellEndDate", Prod.SellEndDate),
                   new XAttribute("SellStartDate", Prod.SellStartDate)));

              //write the XML into the request stream
              Products.Save(stmUpdates);
              stmUpdates.Close();
              //start acquiring the response
              webReq.BeginGetResponse(
                new AsyncCallback(OnProdHdrsUpdateCompleted), webReq);
            }));
          
        }

        private void OnProdHdrsUpdateCompleted(IAsyncResult target)
        {
          HttpWebRequest webResp = target.AsyncState as HttpWebRequest;
          HttpWebResponse resp = 
            webResp.EndGetResponse(target) as HttpWebResponse;
          //if response is OK, refresh the grid to 
          //show that the changes actually happended on the server
          
          if (resp.StatusCode == HttpStatusCode.OK)
            RequestProductHeaders();
        }
        void ProductHeaderDataGrid_SelectionChanged(object sender, EventArgs e)
        {
          if (ProductHeaderDataGrid.SelectedItem != null)
          {
            //invoke the GetProductDetails() service operation, 
            //using the ProductId of the currently selected ProductHeader
            RequestProductDetail(
              (ProductHeaderDataGrid.SelectedItem 
              as ProductHeader).ProductId.Value);
          }
        }
        void ProductHeaderDataGrid_CurrentCellChanged(object sender,
     EventArgs e)
        {
          //changing the dirty flag on a cell edit for the ProductHeader data grid
          if (InEdit)
          {
            ((sender as DataGrid).SelectedItem as ProductHeader).Dirty = true;
            InEdit = false;
          }
        }
        private void ProductHeaderDataGrid_BeginningEdit(object sender,
         DataGridBeginningEditEventArgs e)
        {
          InEdit = true;
        }


        void Click_Btn_SendHeaderUpdates(object Sender, RoutedEventArgs e)
        {
          UpdateProductHeaders();
        }
        void Click_Btn_SendDetailUpdate(object Sender, RoutedEventArgs e)
        {
          UpdateProductDetail();
        }
        
      //Product detail functionality omitted - please refer to sample code for full listing

        private ProductDetail DeserializeProductDetails(string DetailXml)
        {

          XDocument xDocProducts = null;
          try
          {
            xDocProducts = XDocument.Parse(DetailXml);
          }
          catch (Exception Ex)
          {
            string abc = Ex.Message;
          }

          ProductDetail productDetail = new ProductDetail
          {
            Class = xDocProducts.Root.Attribute("Class") != null ? 
              xDocProducts.Root.Attribute("Class").Value : null,
            Color = xDocProducts.Root.Attribute("Color") != null ? 
              xDocProducts.Root.Attribute("Color").Value : null,
            DaysToManufacture = 
              xDocProducts.Root.Attribute("DaysToManufacture") != null ? 
                new byte?(Convert.ToByte(
                  xDocProducts.Root.Attribute("DaysToManufacture").Value)) : null,
            DiscontinuedDate = 
              xDocProducts.Root.Attribute("DiscontinuedDate") != null ? 
                xDocProducts.Root.Attribute("DiscontinuedDate").Value : null,
            FinishedGoodsFlag = xDocProducts.Root.Attribute("FinishedGoodsFlag") 
            != null ? 
              xDocProducts.Root.Attribute("FinishedGoodsFlag").Value : null,
            MakeFlag = xDocProducts.Root.Attribute("MakeFlag") != null ? 
              xDocProducts.Root.Attribute("MakeFlag").Value : null,
            ProductId = xDocProducts.Root.Attribute("ProductId") != null ? 
              new ushort?(
                Convert.ToUInt16(
                  xDocProducts.Root.Attribute("ProductId").Value)) : null,
            ProductLine = xDocProducts.Root.Attribute("ProductLine") != null ?
              xDocProducts.Root.Attribute("ProductLine").Value : null,
            ProductNumber = xDocProducts.Root.Attribute("ProductNumber")
              != null ? 
                xDocProducts.Root.Attribute("ProductNumber").Value : null,
            ReorderPoint = xDocProducts.Root.Attribute("ReorderPoint") != null ? 
              new ushort?(
                Convert.ToUInt16(
                  xDocProducts.Root.Attribute("ReorderPoint").Value)) : null,
            SafetyStockLevel = xDocProducts.Root.Attribute("SafetyStockLevel") != null ?
              new ushort?(
                Convert.ToUInt16(
                  xDocProducts.Root.Attribute("SafetyStockLevel").Value)) : null,
            StandardCost = xDocProducts.Root.Attribute("StandardCost") != null ? 
              new decimal?(
                Convert.ToDecimal(
                  xDocProducts.Root.Attribute("StandardCost").Value)) : null,
            Style = xDocProducts.Root.Attribute("Style") != null ? 
              xDocProducts.Root.Attribute("Style").Value : null
          };
          return productDetail;

        }

        private void RequestProductDetail(ushort ProductId)
        {
            WebRequest webReq = HttpWebRequest.Create(
              new Uri(string.Format(
                "{0}/GetProductDetail?ProductId={1}",ServiceUri,ProductId)));
            webReq.BeginGetResponse(
              new AsyncCallback(OnProductDetailReceived), webReq);
        }

       
        private void OnProductDetailReceived(IAsyncResult target)
        {

            WebRequest webReq = target.AsyncState as WebRequest;
            WebResponse webResp = webReq.EndGetResponse(target);

            StreamReader stmReader = 
              new StreamReader(webResp.GetResponseStream());

            string ProductDetailXml = stmReader.ReadToEnd();

            stmReader.Close();

            Dispatcher.BeginInvoke(new Action(delegate
            {
                ProductDetailsGrid.DataContext = 
                  DeserializeProductDetails(ProductDetailXml);
            }), null);

        }
        
        private void UpdateProductDetail()
        {
            WebRequest webReq = HttpWebRequest.Create(new Uri(string.Format("{0}/UpdateProductDetail",ServiceUri)));
            webReq.Method = "POST";
            webReq.ContentType = "text/xml";
            webReq.BeginGetRequestStream(new AsyncCallback(OnProductDetailUpdateRequestStreamAcquired), new object[] { webReq, ProductDetailsGrid.DataContext as ProductDetail});
        }

        private void OnProductDetailUpdateRequestStreamAcquired(IAsyncResult target)
        {
            HttpWebRequest webReq = (target.AsyncState as object[])[0] as HttpWebRequest;
            StreamWriter stmUpdates = new StreamWriter(webReq.EndGetRequestStream(target));


            ProductDetail Prod = (target.AsyncState as object[])[1] as ProductDetail;

            XElement ProductDetails = new XElement("Product", 
                                           new XAttribute("Class", Prod.Color),
                                           new XAttribute("Color", Prod.Color),
                                           new XAttribute("DaysToManufacture", Prod.DaysToManufacture),
                                           new XAttribute("DiscontinuedDate", Prod.DiscontinuedDate),
                                           new XAttribute("FinishedGoodsFlag", Prod.FinishedGoodsFlag),                                                                          
                                           new XAttribute("MakeFlag", Prod.MakeFlag),
                                           new XAttribute("ProductId", Prod.ProductId),
                                           new XAttribute("ProductLine", Prod.ProductLine),
                                           new XAttribute("ProductNumber", Prod.ProductNumber),
                                           new XAttribute("ReorderPoint", Prod.ReorderPoint),
                                           new XAttribute("SafetyStockLevel", Prod.SafetyStockLevel),                                                                           
                                           new XAttribute("StandardCost", Prod.StandardCost),
                                           new XAttribute("Style", Prod.Style));


            ProductDetails.Save(stmUpdates);

            stmUpdates.Close();

            webReq.BeginGetResponse(new AsyncCallback(OnProductDetailsUpdateCompleted), webReq);
        }

        private void OnProductDetailsUpdateCompleted(IAsyncResult target)
        {
            HttpWebRequest webResp = target.AsyncState as HttpWebRequest;
            HttpWebResponse resp = webResp.EndGetResponse(target) as HttpWebResponse;

            //do nothing else
        }

         
    }    
}

 