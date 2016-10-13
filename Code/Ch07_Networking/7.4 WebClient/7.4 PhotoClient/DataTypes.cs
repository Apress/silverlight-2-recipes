using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Ch07_Networking.Recipe7_4.PhotoClient
{
  public class WrappedImage : INotifyPropertyChanged
  {
    //bound to the thumbnail
    public BitmapImage Small { get; set; }
    //bound to the full res image
    public BitmapImage Large { get; set; }
    //Metadata
    private PhotoMetaData _Info = null;
    public PhotoMetaData Info
    {
      get { return _Info; }
      set
      {
        _Info = value;
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs("Info"));
      }
    }
    //Download Progress
    private double _PercentProgress;
    public double PercentProgress
    {
      get { return _PercentProgress; }
      set
      {
        _PercentProgress = value;
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs("PercentProgress"));
      }
    }
    //show the progress bar
    private Visibility _ProgressVisible = Visibility.Collapsed;
    public Visibility ProgressVisible
    {
      get { return _ProgressVisible; }
      set
      {
        _ProgressVisible = value;
        if (PropertyChanged != null) 
          PropertyChanged(this, new PropertyChangedEventArgs("ProgressVisible"));
      }
    }
    //parts removed for brevity

    //download completed - show the image
    private Visibility _ImageVisible = Visibility.Collapsed;
    public Visibility ImageVisible
    {
      get { return _ImageVisible; }
      set
      {
        _ImageVisible = value;
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs("ImageVisible"));
      }
    }
    //name of the thumbnail file
    private string _ThumbName;
    public string ThumbName
    {
      get { return _ThumbName; }
      set
      {
        _ThumbName = value;
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs("ThumbName"));
      }
    }
    //name of the image file
    private string _FileName;
    public string FileName
    {
      get { return _FileName; }
      set
      {
        _FileName = value;
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs("FileName"));
      }
    }


    public event
      PropertyChangedEventHandler PropertyChanged;

  }
  [DataContract]
  public class PhotoMetaData : INotifyPropertyChanged
  {
    //a unique Id for the image file - the file name
    private string _Id;
    [DataMember]
    public string Id
    {
      get { return _Id; }
      set
      {
        _Id = value;
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs("Id"));
      }
    }
    //a user supplied friendly name
    private string _Name;
    [DataMember]
    public string Name
    {
      get { return _Name; }
      set
      {
        _Name = value;
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs("Name"));
      }
    }
    private string _Description;
    [DataMember]
    public string Description
    {
      get { return _Description; }
      set
      {
        _Description = value;
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs("Description"));
      }
    }
    private string _Location;
    [DataMember]
    public string Location
    {
      get { return _Location; }
      set
      {
        _Location = value;
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs("Location"));
      }
    }
    private int? _Rating;
    [DataMember]
    public int? Rating
    {
      get { return _Rating; }
      set
      {
        _Rating = value;
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs("Rating"));
      }
    }
    private DateTime? _DateTaken;
    [DataMember]
    public DateTime? DateTaken
    {
      get { return _DateTaken; }
      set
      {
        _DateTaken = value;
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs("DateTaken"));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

  }
}