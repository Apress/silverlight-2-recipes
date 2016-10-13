using System.Windows;
using System.Windows.Controls;

namespace Ch03_DevelopingUX.Recipe3_14
{
  public partial class Page : UserControl
  {
    public Page()
    {
      InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      FallingBoulderStoryboard.Begin();
    }
    
  }
}
