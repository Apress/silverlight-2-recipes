using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace Ch02_ProgrammingModel.Recipe2_8
{
  public partial class Page : UserControl
  {
    public Page()
    {
      InitializeComponent();
    }

    private void ButtonReadXML_Click(object sender, RoutedEventArgs e)
    {

      XmlReaderSettings XmlRdrSettings = new XmlReaderSettings();
      XmlRdrSettings.XmlResolver = new XmlXapResolver();
      XmlReader reader = XmlReader.Create("ApressBooks.xml", XmlRdrSettings);

      // Moves the reader to the root element.
      reader.MoveToContent();

      while (!reader.EOF)
      {
        reader.ReadToFollowing("ApressBook");
        // Note that ReadInnerXml only returns the markup of the node's children
        // so the book's attributes are not returned.
        XmlData.Items.Add(reader.ReadInnerXml());
      }
      reader.Close();
    }
  }
}
