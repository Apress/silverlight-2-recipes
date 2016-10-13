using System;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Ch06_BrowserIntegration.Recipe6_8
{
  public partial class DockedView : UserControl
  {
    public DockedView()
    {
      InitializeComponent();
    }

    private void BtnShowFlyout_Click(object sender, RoutedEventArgs e)
    {
      HtmlPage.Window.Eval(@"System.Gadget.Flyout.show = true;");
    }    

  }
}
