using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace Ch08_RichMedia.Recipe8_5
{
  public class MediaMenuData : INotifyPropertyChanged
  {
    private object _Description;
    public object Description
    {
      get { return _Description; }
      set
      {
        _Description = value;
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs("Description"));
      }
    }
    private object _MediaPreview;
    public object MediaPreview
    {
      get { return _MediaPreview; }
      set
      {
        _MediaPreview = value;
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs("MediaPreview"));
      }
    }
    private Uri _MediaLocation;
    public Uri MediaLocation
    {
      get { return _MediaLocation; }
      set
      {
        _MediaLocation = value;
        if (PropertyChanged != null)
        {
          PropertyChanged(this, new PropertyChangedEventArgs("MediaLocation"));
        }
      }
    }
    public event PropertyChangedEventHandler PropertyChanged;

  } 
}
