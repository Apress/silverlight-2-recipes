using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Ch02_ProgrammingModel.Recipe2_14
{
  public partial class Page : UserControl
  {
    public Page()
    {
      InitializeComponent();
    }

    private void RetrieveResourceNames_Click(object sender, RoutedEventArgs e)
    {
      Assembly app = Assembly.GetExecutingAssembly();
      string[] resources = app.GetManifestResourceNames();
      ResourceNames.Items.Clear();
      foreach (string s in resources)
      {
        ResourceNames.Items.Add(s);
      }
    }

    private void ResourceNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!(ResourceNames.SelectedItem.ToString().Contains("resources")))
      {
        Assembly app = Assembly.GetExecutingAssembly();
        using (Stream stream = app.GetManifestResourceStream(ResourceNames.SelectedItem.ToString()))
        {
          BitmapImage bImage = new BitmapImage();
          bImage.SetSource(stream);
          ImageDisplay.Source = bImage;
          ImageBorder.Visibility = Visibility.Visible;
        }
      }
    }
  }
}
