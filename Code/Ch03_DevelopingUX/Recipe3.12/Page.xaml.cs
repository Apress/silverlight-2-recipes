using System.Windows;
using System.Windows.Controls;

namespace Ch03_DevelopingUX.Recipe3_12
{
  public partial class Page : UserControl
  {
    public Page()
    {
      InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      BouncingBallStoryboard.Begin();
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
      MultipleAnimations.Begin();
    }
  }
}
