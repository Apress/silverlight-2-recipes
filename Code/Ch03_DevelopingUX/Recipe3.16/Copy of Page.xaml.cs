using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Text;

namespace Ch03_DesigningUX.Recipe3_12
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

    private void btnResetInk_Click(object sender, RoutedEventArgs e)
    {
      using (var store = IsolatedStorageFile.GetUserStoreForApplication())
      {
        store.DeleteFile(System.IO.Path.Combine(FormDataDirectory, FormDataFileName));
      }
    }

    private void btnSaveInk_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        using (var store = IsolatedStorageFile.GetUserStoreForApplication())
        {
          store.CreateDirectory(FormDataDirectory);
          IsolatedStorageFileStream fileHandle = store.CreateFile(System.IO.Path.Combine(FormDataDirectory, FormDataFileName));
         using (StreamWriter sw = new StreamWriter(fileHandle))
          {
            StrokeSerializer ss = SerializeStrokes(InkPicture.Strokes);
            sw.WriteLine(ss);
            sw.Flush();
            sw.Close();
          }
        }
      }
      catch (IsolatedStorageException ex)
      {
        //Do something with error here
      }
    }

    private StrokeSerializer SerializeStrokes(System.Windows.Ink.StrokeCollection strokes)
    {
      StrokeSerializer ss = new StrokeSerializer();
      foreach (System.Windows.Ink.Stroke s in strokes)
      {

      }
      return ss ;
    }

    private void DeserializeStrokes(StrokeSerializer ss)
    {

    }

    private void btnLoadInk_Click(object sender, RoutedEventArgs e)
    {
      using (var store = IsolatedStorageFile.GetUserStoreForApplication())
      {
        //Load form data using private string values for directory and filename
        string filePath = System.IO.Path.Combine(FormDataDirectory, FormDataFileName);
        //Check to see if file exists before proceeding
        if (store.FileExists(filePath))
        {
          using (StreamReader sr = new StreamReader(
              store.OpenFile(filePath, FileMode.Open, FileAccess.Read)))
          {
            String formData = sr.Read();
            //Split string based on seperator used in SaveFormData method
            string[] fieldValues = formData.Split('|');
            for (int i = 1; i <= fieldValues.Count(); i++)
            {
              //Use the FindName method to loop through TextBoxes
              TextBox tb = FindName("Field" + i.ToString()) as TextBox;
              if (tb != null)
                tb.Text = fieldValues[i - 1];
            }
            sr.Close();
          }
        }
      }
    }
  }
}