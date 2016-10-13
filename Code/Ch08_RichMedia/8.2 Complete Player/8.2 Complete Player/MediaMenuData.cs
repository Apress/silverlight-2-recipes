using System;
using System.ComponentModel;

namespace Ch08_RichMedia.Recipe8_2
{
  public class MediaMenuData : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;
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
  }
}
