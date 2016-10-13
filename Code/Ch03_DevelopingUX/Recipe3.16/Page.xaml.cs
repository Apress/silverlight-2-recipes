using System.IO.IsolatedStorage;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Ch03_DevelopingUX.Recipe3_16
{
  public partial class Page : UserControl
  {
    private System.Windows.Ink.Stroke _currentStroke;
    private System.Windows.Ink.Stroke _currentImageStroke;

    private IsolatedStorageSettings settings =
            IsolatedStorageSettings.ApplicationSettings;
    private string setting = "Ink";
    private string FormDataFileName = "ImageInk.data";
    private string FormDataDirectory = "InkData";

    public Page()
    {
      InitializeComponent();
    }

    private void InkEssentials_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      InkEssentials.CaptureMouse();
      _currentStroke = new System.Windows.Ink.Stroke();
      //Change color of the stroke and stroke outline
      _currentStroke.DrawingAttributes.Color = Colors.Orange;
      _currentStroke.DrawingAttributes.OutlineColor = Colors.Black;
      _currentStroke.StylusPoints.Add(
         e.StylusDevice.GetStylusPoints(InkEssentials));
      InkEssentials.Strokes.Add(_currentStroke);
    }

    private void InkEssentials_MouseMove(object sender, MouseEventArgs e)
    {
      if (null != _currentStroke)
      {
        _currentStroke.StylusPoints.Add(
          e.StylusDevice.GetStylusPoints(InkEssentials));
      }
    }

    private void InkEssentials_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      _currentStroke = null;
      InkEssentials.ReleaseMouseCapture();
    }

    private void InkPicture_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      InkPicture.CaptureMouse();
      _currentImageStroke = new System.Windows.Ink.Stroke();
      _currentImageStroke.StylusPoints.Add(
         e.StylusDevice.GetStylusPoints(InkPicture));
      InkPicture.Strokes.Add(_currentImageStroke);
    }

    private void InkPicture_MouseMove(object sender, MouseEventArgs e)
    {
      if (null != _currentImageStroke)
      {
        _currentImageStroke.StylusPoints.Add(
          e.StylusDevice.GetStylusPoints(InkPicture));
      }
    }

    private void InkPicture_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      _currentImageStroke = null;
      InkPicture.ReleaseMouseCapture();
    }
  }
}