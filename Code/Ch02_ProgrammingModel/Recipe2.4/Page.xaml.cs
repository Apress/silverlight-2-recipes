using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Ch02_ProgrammingModel.Recipe2_5
{
  public partial class Page : UserControl
  {
    public Page()
    {
      InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      string xamlString = "<Ellipse xmlns=\"http://schemas.microsoft.com/client/2007\"  xmlns:x=\"xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml\" Height=\"200\" Width=\"200\" Fill=\"Navy\" Grid.Column=\"1\" Grid.Row=\"1\" />";
      UIElement element = (UIElement)XamlReader.Load(xamlString);
      LayoutRoot.Children.Add(element);
    }
  }
}