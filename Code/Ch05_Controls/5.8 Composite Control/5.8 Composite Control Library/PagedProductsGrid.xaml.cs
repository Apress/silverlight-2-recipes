using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ch05_Controls.Recipe5_8.AdvWorks;

namespace Ch05_Controls.Recipe5_8
{
  public partial class PagedProductsGrid : UserControl
  {
    //WCF service proxy
    AdvWorksDataServiceClient client = new AdvWorksDataServiceClient();
    //raise an event when current record selection changes
    public event EventHandler<DataItemSelectionChangedEventArgs> 
      DataItemSelectionChanged;
    //RecordsPerPage DP
    DependencyProperty RecordsPerPageProperty =
      DependencyProperty.Register("RecordsPerPage",
      typeof(int),
      typeof(PagedProductsGrid),
      new PropertyMetadata(20,
        new PropertyChangedCallback(
          PagedProductsGrid.RecordsPerPageChangedHandler)
        ));
    //CLR DP Wrapper
    public int RecordsPerPage
    {
      get
      {
        return (int)GetValue(RecordsPerPageProperty);
      }
      set
      {
        SetValue(RecordsPerPageProperty, value);
      }
    }
    //RecordPerPage DP value changed
    internal static void RecordsPerPageChangedHandler(DependencyObject sender,
      DependencyPropertyChangedEventArgs e)
    {
      PagedProductsGrid dg = sender as PagedProductsGrid;
      //call init data 
      dg.InitData();
    }


    public PagedProductsGrid()
    {
      InitializeComponent();     
    }

    internal void InitData()
    {
      //got data
      client.GetProductPageCompleted += 
        new EventHandler<GetProductPageCompletedEventArgs>(
          delegate(object Sender, GetProductPageCompletedEventArgs e)
          {
            //bind grid
            dgProductPage.ItemsSource = e.Result;
          });

      //got the count
      client.GetProductsCountCompleted += 
        new EventHandler<GetProductsCountCompletedEventArgs>(
          delegate(object Sender, GetProductsCountCompletedEventArgs e)
          {
            //set the pager control
            lbxPager.ItemsSource = new List<int>(Enumerable.Range(1, 
              (int)Math.Ceiling(e.Result / RecordsPerPage)));
            //get the first page of data
            client.GetProductPageAsync(0, RecordsPerPage);
          });
      //get the product count
      client.GetProductsCountAsync();
    }
    //page selection changed
    private void lbxPager_SelectionChanged(object sender,
      SelectionChangedEventArgs e)
    {
      //get page number
      int PageNum = (int)(lbxPager.SelectedItem);
      //fetch that page - handler defined in InitData()
      client.GetProductPageAsync(RecordsPerPage * (PageNum - 1), RecordsPerPage);

    }
    //record selection changed
    private void dgProductPage_SelectionChanged(object sender, EventArgs e)
    {      
      if (this.DataItemSelectionChanged != null)
      {
        //raise DataItemSelectionChanged
        this.DataItemSelectionChanged(this, 
          new DataItemSelectionChangedEventArgs { 
            CurrentItem = dgProductPage.SelectedItem as Product 
          });
      }
    }
  }

  public class DataItemSelectionChangedEventArgs : EventArgs
  {
    public Product CurrentItem { get; internal set; }
  }
}
