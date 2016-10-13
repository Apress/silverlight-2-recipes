using System.Windows;
using System.Windows.Controls;

namespace Ch03_DevelopingUX.Recipe3_9
{
  public partial class Page : UserControl
  {
    public Page()
    {
      InitializeComponent();
    }

    private void AddText_Click(object sender, RoutedEventArgs e)
    {
      TextBlock text = new TextBlock();
      text.Text = typedText.Text;
      text.Margin = new Thickness(2, 2, 2, 2);
      spText.Children.Add(text);
    }
  }
}
