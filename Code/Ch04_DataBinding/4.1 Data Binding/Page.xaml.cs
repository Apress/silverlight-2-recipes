using System.Windows.Controls;
using System.Windows.Data;

namespace Ch04_DataBinding.Recipe4_1
{
  public partial class Page : UserControl
  {
    public Page()
    {
      InitializeComponent();
      //In case you want to set the datacontext in code...
      //LayoutRoot.DataContext = new Company();
      //create a new Binding
      Binding CompanyNameBinding = new Binding("Name");
      //set properties on the Binding as needed
      CompanyNameBinding.Mode = BindingMode.OneWay;
      //apply the Binding to the DependencyProperty of 
      //choice on the appropriate object
      tbxCompanyName.SetBinding(TextBlock.TextProperty,
        CompanyNameBinding);


    }
  }
}
