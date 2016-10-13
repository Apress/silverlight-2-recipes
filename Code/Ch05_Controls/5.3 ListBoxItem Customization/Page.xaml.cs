using System;
using System.Windows.Controls;
using System.Windows.Media;
using Ch05_Controls.Recipe5_3.AdvWorks;

namespace Ch05_Controls.Recipe5_3
{
  public partial class Page : UserControl
  {
    //WCF service client
    AdvWorksDataServiceClient client =
      new AdvWorksDataServiceClient();
    public Page()
    {
      InitializeComponent();
      GetData();
    }

    private void GetData()
    {
      client.GetInventoryCompleted += 
        new EventHandler<GetInventoryCompletedEventArgs>(
          delegate(object sender, GetInventoryCompletedEventArgs e)
          {
            Product product = e.UserState as Product;
            product.ProductInventories = e.Result;
            product.InventoryLevelBrush = null;
            product.InventoryLevelMessage = null;

          });
      client.GetProductsCompleted += 
        new EventHandler<GetProductsCompletedEventArgs>(
          delegate(object sender, GetProductsCompletedEventArgs e)
          {

            lbxStandard.ItemsSource = e.Result;
            lbxCustom.ItemsSource = e.Result;

            foreach (Product p in e.Result)
            {
              client.GetInventoryAsync(p, p);
            }
          });

      client.GetProductsAsync();
    }
  }
}

namespace Ch05_Controls.Recipe5_3.AdvWorks
{

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
          new SolidColorBrush(Colors.Yellow) : new SolidColorBrush(Colors.Red)));
      }
      set
      {
        //no actual value set here - just property change raised
        //can be set to null in code to cause rebinding, when 
        //ProductInventories changes
        RaisePropertyChanged("InventoryLevelBrush");
      }

    }
    private string _InventoryLevelMessage;

    public string InventoryLevelMessage
    {
      get
      {
        return (this.ProductInventories == null
          || this.ProductInventories.Count == 0) ? "Stock Level Unknown"
          : (this.ProductInventories[0].Quantity > this.SafetyStockLevel
          ? "In Stock" :
          (this.ProductInventories[0].Quantity > this.ReorderPoint ?
          "Low Stock" : "Reorder Now"));
      }
      set
      {
        //no actual value set here - just property change raised
        //can be set to null in code to cause rebinding, 
        //when ProductInventories changes
        RaisePropertyChanged("InventoryLevelMessage");
      }
    }
  }
}