using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Ch04_DataBinding.Recipe4_4
{
  public partial class Page : UserControl
  {
    private bool MouseLeftBtnDown = false;
    Point PreviousPos;
    public Page()
    {
      InitializeComponent();
    }

    private void Rectangle_MouseMove(object sender, MouseEventArgs e)
    {
      if (MouseLeftBtnDown)
      {
        Rectangle rect = (Rectangle)sender;
        Point CurrentPos = e.GetPosition(sender as Rectangle);
        double Moved = CurrentPos.X - PreviousPos.X;
        if (rect.Width + Moved >= 0)
        {
          rect.Width += Moved;
        }
        PreviousPos = CurrentPos;
      }
    }

    private void Rectangle_MouseLeftButtonDown(object sender,
      MouseButtonEventArgs e)
    {
      MouseLeftBtnDown = true;
      PreviousPos = e.GetPosition(sender as Rectangle);
    }

    private void Rectangle_MouseLeftButtonUp(object sender,
      MouseButtonEventArgs e)
    {
      MouseLeftBtnDown = false;
    }
  }
}








