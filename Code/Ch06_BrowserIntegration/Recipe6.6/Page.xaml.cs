using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Ch06_BrowserIntegration.Recipe6_5
{
  public partial class Page : UserControl
  {
    HtmlDocument doc;
    public Page()
    {
      InitializeComponent();
      Loaded += new RoutedEventHandler(Page_Loaded);
    }

    void Page_Loaded(object sender, RoutedEventArgs e)
    {
      doc = HtmlPage.Document;
      doc.SetProperty("bgColor", GetColor());
      doc.GetElementById("silverlightControlHost").
                   SetStyleAttribute("width", this.Width.ToString());
      doc.GetElementById("silverlightControlHost").
                   SetStyleAttribute("height", this.Height.ToString());

      //Make scriptable type available to JavaScript
      HtmlPage.RegisterScriptableObject("Page", this);
    }

    private string GetColor()
    {
      GradientBrush gb = LayoutRootBorder.Background as GradientBrush;
      //Set background color to second gradient stop
      GradientStop gs = gb.GradientStops[1];
      //Remove alpha component from brush since it doesn't work for html 
      //elements when setting background color.  ToString("X2") formats the
      //byte value as a hexidecimal value forcing 2 digits each if there are
      // not two digits for each component, it will cause an  error.
      return gs.Color.R.ToString("X2") + gs.Color.G.ToString("X2") +
                                         gs.Color.B.ToString("X2");
    }

    private void UpdateDataButton_Click(object sender, RoutedEventArgs e)
    {
      HtmlPage.Window.Invoke("getDataUsingJavaScriptAjaxAsync");
    }

    [ScriptableMember]
    public void SetBookXMLData(string data)
    {
      ApressBooks books = new ApressBooks(data);
      Binding b = new Binding("ApressBookList");
      b.Source = books.ApressBookList;
      BookListBox.ItemsSource = books.ApressBookList;
    }
  }
}
