using System.Windows.Controls;

namespace Ch05_Controls.Recipe5_8
{
  public partial class Page : UserControl
  {
    public Page()
    {
      InitializeComponent();

    }

    private void PagedGrid_DataItemSelectionChanged(object sender,
      DataItemSelectionChangedEventArgs e)
    {
      if (e.CurrentItem != null)
        ProductCostInfo.Content = e.CurrentItem;
    }
  }
}
