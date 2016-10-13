using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Ch02_ProgrammingModel.Recipe2_3
{
  public partial class Page : UserControl
  {
    public Page()
    {
      InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      TextBlock tb = (TextBlock)LayoutRoot.FindName(ControlName.Text);
      if (tb != null)
        tb.FontSize = 20.0;
      else
      {
        ControlName.Foreground = new SolidColorBrush(Color.FromArgb(255, 200, 124, 124));
        ControlName.Text = "Control not found! Please try again.";
      }
    }

    private void ControlName_KeyDown(object sender, KeyEventArgs e)
    {
      ControlName.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
    }
  }
}