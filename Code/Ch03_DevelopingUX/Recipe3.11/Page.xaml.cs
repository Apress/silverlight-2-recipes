using System.Windows.Controls;
using System.Windows.Input;

namespace Ch03_DevelopingUX.Recipe3_11
{
  public partial class Page : UserControl
  {
    public Page()
    {
      InitializeComponent();
    }

    private void Rect1_MouseEnter(object sender, MouseEventArgs e)
    {
      Rect1MouseMove.Begin();
    }

    private void Rect1_MouseLeave(object sender, MouseEventArgs e)
    {
      Rect1MouseMove.Begin();
    }

    private void Ellipse1_MouseEnter(object sender, MouseEventArgs e)
    {
      EllipseMouseEnter.Begin();
    }

    private void Ellipse1_MouseLeave(object sender, MouseEventArgs e)
    {
      EllipseMouseLeave.Begin();
    }

    private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      PathClick.Begin();
    }
  }
}