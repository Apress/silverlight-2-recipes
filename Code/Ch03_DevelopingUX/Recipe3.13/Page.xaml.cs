using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Ch03_DevelopingUX.Recipe3_13
{
  public partial class Page : UserControl
  {
    public Page()
    {
      InitializeComponent();
    }

    private void ApplyMatrix(object sender, RoutedEventArgs e)
    {
      MatrixTransform mt = (MatrixTransform)txtMatrixTransform.RenderTransform;
      try
      {
        Matrix m = new Matrix(Convert.ToDouble(txtM11.Text),
         Convert.ToDouble(txtM12.Text), Convert.ToDouble(txtM21.Text),
         Convert.ToDouble(txtM22.Text), Convert.ToDouble(txtOffsetX.Text),
         Convert.ToDouble(txtOffsetY.Text));
        mt.Matrix = m;
      }
      catch
      {
        txtMatrixTransform.Text = "Invalid-retry:-)";
        ResetMatrix(sender, e);
      }
    }

    private void ResetMatrix(object sender, RoutedEventArgs e)
    {
      txtM11.Text = "1";
      txtM12.Text = "0";
      txtM21.Text = "0";
      txtM22.Text = "1";
      txtOffsetX.Text = "0";
      txtOffsetY.Text = "0";
      MatrixTransform mt = (MatrixTransform)txtMatrixTransform.RenderTransform;
      Matrix m = new Matrix(1, 0, 0, 1, 0, 0);
      mt.Matrix = m;
    }
  }
}