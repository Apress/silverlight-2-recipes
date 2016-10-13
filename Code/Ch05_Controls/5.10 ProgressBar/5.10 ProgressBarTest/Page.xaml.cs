using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using Ch05_Controls.Recipe5_10.AdvWorks;

namespace Ch05_Controls.Recipe5_10
{
  public partial class Page : UserControl
  {
    AdvWorksDataServiceClient client =
      new AdvWorksDataServiceClient();

    private const string DownloadSvcUri =
      "http://localhost:9191/AdvWorksPhotoService.svc/Photos?Id={0}";

    internal TotalDownloadCounter TotalDownloadData = null;

    internal ObservableCollection<ImageData> listImages =
      new ObservableCollection<ImageData>();

    public Page()
    {
      InitializeComponent();
      GetPhotos();
    }

    private void GetPhotos()
    {
      listImages.Clear();
      lbxImages.ItemsSource = listImages;

      client.GetPhotoIdsCompleted +=
        new EventHandler<GetPhotoIdsCompletedEventArgs>(
          delegate(object sender, GetPhotoIdsCompletedEventArgs e)
          {
            TotalDownloadData =
              new TotalDownloadCounter
              {
                ImageCount = 0,
                TotalImages = e.Result.Count
              };
            LayoutRoot.DataContext = TotalDownloadData;

            foreach (int PhotoId in e.Result)
            {
              ImageData TempImageData =
                new ImageData
                {
                  DownloadProgress = 0,
                  ImageVisible = Visibility.Collapsed,
                  ProgBarVisible = Visibility.Visible,
                  PngImage = new BitmapImage()
                };

              listImages.Add(TempImageData);
              DownloadPhoto(PhotoId, TempImageData);
            }
          });
      client.GetPhotoIdsAsync();
    }
    private void DownloadPhoto(int PhotoId, ImageData TempImageData)
    {
      WebClient wc = new WebClient();
      wc.DownloadProgressChanged +=
        new DownloadProgressChangedEventHandler(
          delegate(object sender, DownloadProgressChangedEventArgs e)
          {
            (e.UserState as ImageData).DownloadProgress = e.ProgressPercentage;
            Thread.Sleep(5);
          });
      wc.DownloadStringCompleted +=
        new DownloadStringCompletedEventHandler(
          delegate(object sender, DownloadStringCompletedEventArgs e)
          {
            ImageData ImgSource = e.UserState as ImageData;
            //parse XML formatted response string into an XDocument
            XDocument xDoc = XDocument.Parse(e.Result);
            //grab the root, and decode the deafult base64 
            //representation into the image bytes
            byte[] Buff = Convert.FromBase64String((string)xDoc.Root);
            //wrap in a memory stream,and 
            MemoryStream ms = new MemoryStream(Buff);
            ImgSource.PngImage.SetSource(ms);
            ms.Close();
            (e.UserState as ImageData).ProgBarVisible = Visibility.Collapsed;
            (e.UserState as ImageData).ImageVisible = Visibility.Visible;
            ++TotalDownloadData.ImageCount;
          });

      wc.DownloadStringAsync(new Uri(
        string.Format(DownloadSvcUri, PhotoId)), TempImageData);
    }
  }

  public class TotalDownloadCounter : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;
    private double _TotalImages;
    public double TotalImages
    {
      get { return _TotalImages; }
      set
      {
        _TotalImages = value;
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs("TotalImages"));
      }
    }
    private double _ImageCount;
    public double ImageCount
    {
      get { return _ImageCount; }
      set
      {
        _ImageCount = value;
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs("ImageCount"));
      }
    }
  }

  public class ImageData : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;
    private double _DownloadProgress;
    public double DownloadProgress
    {
      get { return _DownloadProgress; }
      set
      {
        _DownloadProgress = value;
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs("DownloadProgress"));
      }
    }
    private Visibility _ImageVisible;
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
    private Visibility _ProgBarVisible;
    public Visibility ProgBarVisible
    {
      get { return _ProgBarVisible; }
      set
      {
        _ProgBarVisible = value;
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs("ProgBarVisible"));
      }
    }
    private BitmapImage _PngImage;
    public BitmapImage PngImage
    {
      get { return _PngImage; }
      set
      {
        _PngImage = value;
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs("PngImage"));
      }
    }
  }
}
