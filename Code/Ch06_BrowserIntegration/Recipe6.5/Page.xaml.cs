using System;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Media;

namespace Ch06_BrowserIntegration.Recipe6_6
{
  public partial class Page : UserControl
  {
    public Page()
    {
      InitializeComponent();
      Loaded += new RoutedEventHandler(Page_Loaded);
      SizeChanged += new SizeChangedEventHandler(Page_SizeChanged);
    }

    void Page_Loaded(object sender, RoutedEventArgs e)
    {
      //Make scriptable type available to JavaScript
      HtmlPage.RegisterScriptableObject("Page", this);

      HtmlDocument doc = HtmlPage.Document;
      doc.GetElementById("Button1").AttachEvent("click",
        new EventHandler(this.InvokedFromHtmlButtonClick));

    }

    void Page_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      //Set width of Div containing the SilverlightControl to width
      //of Silverlight content so that the HTML background displays
      //along side of the Silverlight control.  Otherwise the Silverlight
      //control will take up the entire page based on the html in the
      //default test pages that are created.
      HtmlDocument doc = HtmlPage.Document;
      doc.GetElementById("silverlightControlHost").SetStyleAttribute("width",
                                                      this.Width.ToString());
    }

    [ScriptableMember]
    public string GetMyBackgroundColor()
    {
      Brush b = LayoutRoot.Background;
      if (b is SolidColorBrush)
      {
        SolidColorBrush scb = b as SolidColorBrush;
        //Remove alpha component from brush since it doesn't work for 
        //html elements when setting background color.
        //ToString("X2") formats the byte value as a hexidecimal value forcing
        //2 digits each if there are not two digits for each component, it will
        //cause a JavaScript error.
        return scb.Color.R.ToString("X2") + scb.Color.G.ToString("X2") +
               scb.Color.B.ToString("X2");
      }
      else if (b is GradientBrush)
      {
        GradientBrush gb = b as GradientBrush;
        //Arbitrarily pick the color of first gradient stop as the color 
        //to pass back as the returned value.
        GradientStop gs = gb.GradientStops[0];
        //Remove alpha component from brush since it doesn't work for html 
        //elements when setting background color.  ToString("X2") formats the
        //byte value as a hexidecimal value forcing 2 digits each if there are
        // not two digits for each component, it will cause a JavaScript error.
        return gs.Color.R.ToString("X2") + gs.Color.G.ToString("X2") +
                                           gs.Color.B.ToString("X2");
      }
      else
        return "#FFFFFF";
    }

    private void InvokedFromHtmlButtonClick(object o, EventArgs e)
    {
      MessageTextBlock.Text = "HTML button clicked at " +
         DateTime.Now.ToString();
    }
  }
}
