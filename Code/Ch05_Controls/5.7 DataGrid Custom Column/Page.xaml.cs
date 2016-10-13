using System;
using System.Windows.Controls;
using Ch05_Controls.Recipe5_7.AdvWorks;

namespace Ch05_Controls.Recipe5_7
{
  public partial class Page : UserControl
  {
    AdvWorksDataServiceClient client = 
      new AdvWorksDataServiceClient();


    public Page()
    {
      InitializeComponent();
      GetData();
    }

    private void GetData()
    {
      client.GetProductsCompleted += 
        new EventHandler<GetProductsCompletedEventArgs>(
          delegate(object sender, GetProductsCompletedEventArgs e)
          {
            dgProducts.ItemsSource = e.Result;
          });

      client.GetProductsAsync();
    }

    private void DatePicker_LostFocus(object sender, System.Windows.RoutedEventArgs e)
    {
      string a = "b";
    }
  }   
}



namespace Ch05_Controls.Recipe5_7.AdvWorks
{
  public partial class Product
  {    
    public DateTime DisplayDateEnd { get { 
      return DateTime.Today + new TimeSpan(365, 0, 0, 0); } }     
  }
}


