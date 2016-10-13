// Title: SilverLight 2 Recipes: A Problem-Solution Approach
//
// Chapter: 1 – Introduction to Silverlight 2
// 
// Written by: Jit Ghosh and Rob Cameron
//
// Copyright © 2008, Apress L.P.

using System.Windows;
using System.Windows.Controls;

namespace Ch01_IntroToSilverlight.Recipe1_5
{
  public partial class Page : UserControl
  {
    public Page()
    {
      InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      SquaretoCircleStoryboard.Begin();
    }
  }
}
