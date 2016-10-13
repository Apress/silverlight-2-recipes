using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ch07_Networking.Recipe7_1.ProductsDataViewer.ProductsDataSoapService;

namespace Ch07_Networking.Recipe7_1.ProductsDataViewer
{
  public partial class Page : UserControl
  {
    ProductsDataSoapService.ProductManagerClient client = null;
    bool InEdit = false;
    public Page()
    {
      InitializeComponent();
      //create a new instance of the proxy
      client = new ProductsDataSoapService.ProductManagerClient();
      //add a handler for the GetProductHeadersCompleted event
      client.GetProductHeadersCompleted +=
        new EventHandler<GetProductHeadersCompletedEventArgs>(
          client_GetProductHeadersCompleted);
      //add a handler for the UpdateProductHeadersCompleted event
      client.UpdateProductHeadersCompleted +=
        new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(
          client_UpdateProductHeadersCompleted);
      //add a handler for GetProductDetailCompleted
      client.GetProductDetailCompleted +=
        new EventHandler<GetProductDetailCompletedEventArgs>(
          client_GetProductDetailCompleted);
      //invoke the GetProductHeaders() service operation
      client.GetProductHeadersAsync();
    }

    void ProductHeaderDataGrid_SelectionChanged(object sender, EventArgs e)
    {
      if (ProductHeaderDataGrid.SelectedItem != null)
        //invoke the GetProductDetails() service operation, 
        //using the ProductId of the currently selected ProductHeader
        client.GetProductDetailAsync(
          (ProductHeaderDataGrid.SelectedItem as
          ProductsDataSoapService.ProductHeader).ProductId.Value);
    }

    void client_GetProductDetailCompleted(object sender,
      GetProductDetailCompletedEventArgs e)
    {
      //set the datacontext of the containing grid
      ProductDetailsGrid.DataContext = e.Result;
    }
    void client_UpdateProductHeadersCompleted(object sender,
      System.ComponentModel.AsyncCompletedEventArgs e)
    {
      client.GetProductHeadersAsync();
    }

    void client_GetProductHeadersCompleted(object sender,
      GetProductHeadersCompletedEventArgs e)
    {
      //bind the data of form List<ProductHeader> to the ProductHeaderDataGrid
      ProductHeaderDataGrid.ItemsSource = e.Result;
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
      //get all the header items
      List<ProductHeader> AllItems =
        ProductHeaderDataGrid.ItemsSource as List<ProductHeader>;
      //use LINQ to filter out the ones with their dirty flag set to true
      List<ProductHeader> UpdateList =
          new List<ProductHeader>
          (
            from Prod in AllItems
            where Prod.Dirty == true
            select Prod
          );
      //send in the updates
      client.UpdateProductHeadersAsync(UpdateList);
    }

    void Click_Btn_SendDetailUpdate(object Sender, RoutedEventArgs e)
    {
      //send the ProductDetail update
      client.UpdateProductDetailAsync(ProductDetailsGrid.DataContext as
        ProductsDataSoapService.ProductDetail);
    }

    
  }
}

namespace Ch07_Networking.Recipe7_1.ProductsDataViewer.ProductsDataSoapService
{
  public partial class ProductHeader
  {
    //dirty flag
    public bool Dirty { get; set; }
  }
}
