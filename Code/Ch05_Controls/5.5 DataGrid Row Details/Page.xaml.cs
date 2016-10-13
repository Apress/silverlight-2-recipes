using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Ch05_Controls.Recipe5_5.AdvWorks;

namespace Ch05_Controls.Recipe5_5
{
  public partial class Page : UserControl
  {
    AdvWorksDataServiceClient client =
      new AdvWorksDataServiceClient();


    public Page()
    {
      InitializeComponent();
      //async completion callbacks for the web service calls to get data
      client.GetPhotosCompleted +=
        new EventHandler<GetPhotosCompletedEventArgs>(
          delegate(object s1, GetPhotosCompletedEventArgs e1)
          {
            (e1.UserState as Product).ProductPhoto = e1.Result;
          });
      client.GetInventoryCompleted +=
        new EventHandler<GetInventoryCompletedEventArgs>(
          delegate(object s2, GetInventoryCompletedEventArgs e2)
          {
            (e2.UserState as Product).ProductInventories = e2.Result;
            (e2.UserState as Product).InventoryLevelBrush = null;
            (e2.UserState as Product).InventoryLevelMessage = null;
          });
      client.GetCategoryCompleted +=
        new EventHandler<GetCategoryCompletedEventArgs>(
         delegate(object s3, GetCategoryCompletedEventArgs e3)
         {
           (e3.UserState as Product).ProductCategory = e3.Result;
         });
      client.GetSubcategoryCompleted +=
        new EventHandler<GetSubcategoryCompletedEventArgs>(
          delegate(object s4, GetSubcategoryCompletedEventArgs e4)
          {
            (e4.UserState as Product).ProductSubCategory = e4.Result;
          });
      client.GetDescriptionCompleted +=
        new EventHandler<GetDescriptionCompletedEventArgs>(
          delegate(object s5, GetDescriptionCompletedEventArgs e5)
          {
            (e5.UserState as Product).ProductDescription = e5.Result;
          });
      client.GetProductCostHistoryCompleted +=
        new EventHandler<GetProductCostHistoryCompletedEventArgs>(
          delegate(object s6, GetProductCostHistoryCompletedEventArgs e6)
          {
            (e6.UserState as Product).ProductCostHistories = e6.Result;
          });

      //LoadingRowDetails handler - here we make the calls to load 
      //row details data on demand
      dgProducts.LoadingRowDetails +=
        new EventHandler<DataGridRowDetailsEventArgs>(
          delegate(object sender, DataGridRowDetailsEventArgs e)
          {
            Product prod = e.Row.DataContext as Product;
            if (prod.ProductInventories == null)
              client.GetInventoryAsync(prod, prod);
            if (prod.ProductCategory == null && prod.ProductSubcategoryID != null)
              client.GetCategoryAsync(prod, prod);
            if (prod.ProductSubCategory == null && 
              prod.ProductSubcategoryID != null)
              client.GetSubcategoryAsync(prod, prod);
            if (prod.ProductDescription == null)
              client.GetDescriptionAsync(prod, prod);
            if (prod.ProductPhoto == null)
              client.GetPhotosAsync(prod, prod);
            if (prod.ProductCostHistories == null)
              client.GetProductCostHistoryAsync(prod, prod);

          });

      GetData();
    }

    private void GetData()
    {
      //get the top level product data
      client.GetProductsCompleted +=
        new EventHandler<GetProductsCompletedEventArgs>(
          delegate(object sender, GetProductsCompletedEventArgs e)
          {
            dgProducts.ItemsSource = e.Result;
          });
      client.GetProductsAsync();
    }


    private void ShowDetails_Click(object sender, RoutedEventArgs e)
    {
      DataGridRow row = DataGridRow.GetRowContainingElement(sender as Button);
      row.DetailsVisibility =
        (row.DetailsVisibility == Visibility.Collapsed ?
        Visibility.Visible : Visibility.Collapsed);
    }
  }
}

namespace Ch05_Controls.Recipe5_5.AdvWorks
{
  public partial class ProductPhoto
  {
    private BitmapImage _LargePhotoPNG;

    public BitmapImage LargePhotoPNG
    {
      get
      {
        BitmapImage bim = new BitmapImage();
        MemoryStream ms = new MemoryStream(this.LargePhoto.Bytes);
        bim.SetSource(ms);
        ms.Close();
        return bim;
      }
      set
      {
        RaisePropertyChanged("LargePhotoPNG");
      }
    }
  }

  public partial class Product
  {
    private SolidColorBrush _InventoryLevelBrush;
    public SolidColorBrush InventoryLevelBrush
    {
      get
      {
        return (this.ProductInventories == null
          || this.ProductInventories.Count == 0) ?
          new SolidColorBrush(Colors.Gray) :
            (this.ProductInventories[0].Quantity > this.SafetyStockLevel ?
              new SolidColorBrush(Colors.Green) :
                (this.ProductInventories[0].Quantity > this.ReorderPoint ?
                   new SolidColorBrush(Colors.Yellow) :
                    new SolidColorBrush(Colors.Red)));
      }
      set
      {
        //no actual value set here - just property change raised
        RaisePropertyChanged("InventoryLevelBrush");
      }

    }
    private string _InventoryLevelMessage;
    public string InventoryLevelMessage
    {
      get
      {
        return (this.ProductInventories == null
          || this.ProductInventories.Count == 0) ?
          "Stock Level Unknown" :
            (this.ProductInventories[0].Quantity > this.SafetyStockLevel ?
            "In Stock" :
              (this.ProductInventories[0].Quantity > this.ReorderPoint ?
                "Low Stock" : "Reorder Now"));
      }
      set
      {
        //no actual value set here - just property change raised
        RaisePropertyChanged("InventoryLevelMessage");
      }
    }
    private ProductSubcategory _productSubCategory;
    public ProductSubcategory ProductSubCategory
    {
      get { return _productSubCategory; }
      set
      {
        _productSubCategory = value;
        RaisePropertyChanged("ProductSubCategory");
      }
    }
    private ProductCategory _productCategory;
    public ProductCategory ProductCategory
    {
      get { return _productCategory; }
      set { _productCategory = value; RaisePropertyChanged("ProductCategory"); }
    }
    private ProductDescription _productDescription;
    public ProductDescription ProductDescription
    {
      get { return _productDescription; }
      set
      {
        _productDescription = value;
        RaisePropertyChanged("ProductDescription");
      }
    }
    private ProductReview _productReview;
    public ProductReview ProductReview
    {
      get { return _productReview; }
      set { _productReview = value; RaisePropertyChanged("ProductReview"); }
    }
    private ProductPhoto _productPhoto;
    public ProductPhoto ProductPhoto
    {
      get { return _productPhoto; }
      set { _productPhoto = value; RaisePropertyChanged("ProductPhoto"); }
    }
  }
}
