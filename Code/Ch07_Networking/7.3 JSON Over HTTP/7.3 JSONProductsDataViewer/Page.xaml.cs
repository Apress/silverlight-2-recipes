using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Controls;

namespace Ch07_Networking.Recipe7_3.JSONProductsDataViewer
{
  public partial class Page : UserControl
  {
    private const string ServiceUri =
      "http://localhost:9393/ProductsJSONService.svc";
    bool InEdit = false;

    public Page()
    {
      InitializeComponent();

      GetProductHeaders();
    }


    private List<ProductHeader> DeserializeProductHeaders(Stream HeaderJson)
    {
      //create and initialize a new DataContractJsonSerializer
      DataContractJsonSerializer jsonSer =
        new DataContractJsonSerializer(typeof(List<ProductHeader>));
      //Deserialize - root object returned and cast
      List<ProductHeader> ProductList =
        jsonSer.ReadObject(HeaderJson) as List<ProductHeader>;
      return ProductList;
    }
    private void OnProdHdrUpdReqStreamAcquired(IAsyncResult target)
    {
      HttpWebRequest webReq = target.AsyncState as HttpWebRequest;
      Stream stmUpdates = webReq.EndGetRequestStream(target);
      Dispatcher.BeginInvoke(new Action(delegate
        {
          List<ProductHeader> AllItems =
            ProductHeaderDataGrid.ItemsSource as List<ProductHeader>;

          List<ProductHeader> UpdateList =
            new List<ProductHeader>
            (
              from Prod in AllItems
              where Prod.Dirty == true
              select Prod
            );
          //create and initialize a DataContractJsonSerializer
          DataContractJsonSerializer jsonSer =
            new DataContractJsonSerializer(typeof(List<ProductHeader>));
          //write object tree out to the stream
          jsonSer.WriteObject(stmUpdates, UpdateList);
          stmUpdates.Close();

          webReq.BeginGetResponse(
            new AsyncCallback(OnProductHeadersUpdateCompleted), webReq);
        }));
    }

    private ProductDetail DeserializeProductDetails(Stream DetailJson)
    {

      DataContractJsonSerializer jsonSer =
        new DataContractJsonSerializer(typeof(ProductDetail));
      ProductDetail Detail =
        jsonSer.ReadObject(DetailJson) as ProductDetail;
      return Detail;
    }
    private void OnProductDetailUpdateRequestStreamAcquired(IAsyncResult target)
    {
      HttpWebRequest webReq =
        (target.AsyncState as object[])[0] as HttpWebRequest;
      Stream stmUpdates = webReq.EndGetRequestStream(target);


      ProductDetail Detail =
        (target.AsyncState as object[])[1] as ProductDetail;

      DataContractJsonSerializer jsonSer =
        new DataContractJsonSerializer(typeof(ProductDetail));
      jsonSer.WriteObject(stmUpdates, Detail);
      stmUpdates.Close();
      webReq.BeginGetResponse(new AsyncCallback(OnProductDetailsUpdateCompleted), webReq);
    }
    private void OnProductHeadersReceived(IAsyncResult target)
    {

      WebRequest webReq = target.AsyncState as WebRequest;
      WebResponse webResp = webReq.EndGetResponse(target);
      Stream stmHeaders = webResp.GetResponseStream();
      List<ProductHeader> ProductHeaders =
        DeserializeProductHeaders(stmHeaders);
      stmHeaders.Close();

      Dispatcher.BeginInvoke(new Action(delegate
      {
        ProductHeaderDataGrid.ItemsSource = ProductHeaders;
      }), null);
    }

    private void OnProductDetailReceived(IAsyncResult target)
    {

      WebRequest webReq = target.AsyncState as WebRequest;
      WebResponse webResp = webReq.EndGetResponse(target);
      Stream stmDetail = webResp.GetResponseStream();
      ProductDetail Detail =
        DeserializeProductDetails(stmDetail);
      stmDetail.Close();

      Dispatcher.BeginInvoke(new Action(delegate
      {
        ProductDetailsGrid.DataContext = Detail;
      }), null);
    }

    private void GetProductHeaders()
    {
      WebRequest webReq = HttpWebRequest.Create(new Uri(string.Format("{0}/GetProductHeaders", ServiceUri)));
      webReq.BeginGetResponse(new AsyncCallback(OnProductHeadersReceived), webReq);
    }

    private void GetProductDetail(ushort ProductId)
    {
      WebRequest webReq = HttpWebRequest.Create(new Uri(string.Format("{0}/GetProductDetail?ProductId={1}", ServiceUri, ProductId)));
      webReq.BeginGetResponse(new AsyncCallback(OnProductDetailReceived), webReq);
    }


    private void UpdateProductHeaders()
    {
      WebRequest webReq = HttpWebRequest.Create
        (new Uri(string.Format("{0}/UpdateProductHeaders", ServiceUri)));
      webReq.Method = "POST";
      webReq.ContentType = "application/json";
      webReq.BeginGetRequestStream(new AsyncCallback(OnProdHdrUpdReqStreamAcquired), webReq);
    }



    private void OnProductHeadersUpdateCompleted(IAsyncResult target)
    {
      HttpWebRequest webResp = target.AsyncState as HttpWebRequest;
      HttpWebResponse resp = webResp.EndGetResponse(target) as HttpWebResponse;

      if (resp.StatusCode == HttpStatusCode.OK)
        GetProductHeaders();
    }
    private void UpdateProductDetail()
    {
      WebRequest webReq = HttpWebRequest.Create(new Uri(string.Format("{0}/UpdateProductDetail", ServiceUri)));
      webReq.Method = "POST";
      webReq.ContentType = "application/json";
      webReq.BeginGetRequestStream(new AsyncCallback(OnProductDetailUpdateRequestStreamAcquired), new object[] { webReq, ProductDetailsGrid.DataContext as ProductDetail });
    }



    private void OnProductDetailsUpdateCompleted(IAsyncResult target)
    {
      HttpWebRequest webResp = target.AsyncState as HttpWebRequest;
      HttpWebResponse resp = webResp.EndGetResponse(target) as HttpWebResponse;

      //do nothing else
    }

    void ProductHeaderDataGrid_SelectionChanged(object sender, EventArgs e)
    {
      if (ProductHeaderDataGrid.SelectedItem != null)
      {
        //invoke the GetProductDetails() service operation, using the ProductId of the currently selected ProductHeader
        GetProductDetail((ProductHeaderDataGrid.SelectedItem as ProductHeader).ProductId.Value);
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

  }



}
