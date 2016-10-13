using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;

namespace Ch08_RichMedia.Recipe8_2
{
  public partial class Page : UserControl
  {
    private const string MediaLocatorUri =
      "http://localhost:9191/MediaLocationProvider.svc/GetLocationList";

    private ObservableCollection<MediaMenuData> listMedia =
      new ObservableCollection<MediaMenuData>();

    private MediaElement MainVideo
    {
      get
      {
        return (vidbrushMain.SourceName == "mediaelemMain") ?
        mediaelemMain : mediaelemPIP;
      }
    }
    private MediaElement PIPVideo
    {
      get
      {
        return (vidbrushPIP.SourceName == "mediaelemMain") ?
        mediaelemMain : mediaelemPIP;
      }
    }

    public Page()
    {
      InitializeComponent();
      lbxMediaMenu.ItemsSource = listMedia;
      this.Loaded += new RoutedEventHandler(Page_Loaded);
    }

    void Page_Loaded(object sender, RoutedEventArgs e)
    {
      PopulateMediaMenu();
    }
    

    private void PopulateMediaMenu()
    {
      WebClient wcMediaLocator = new WebClient();
      wcMediaLocator.DownloadStringCompleted +=
        new DownloadStringCompletedEventHandler(
        delegate(object Sender, DownloadStringCompletedEventArgs e)
        {
          this.Dispatcher.BeginInvoke(new Action(delegate
            {
              XDocument xDoc = XDocument.Parse(e.Result);

              List<MediaMenuData> tempList =
                (from medloc in xDoc.Root.Elements()
                 select new MediaMenuData
                 {
                   Description = medloc.Element("Description").Value,
                   MediaLocation = new Uri(medloc.Element("Uri").Value),
                   MediaPreview = medloc.Element("ImageUri").Value
                 }).ToList();
              foreach (MediaMenuData medloc in tempList)
                listMedia.Add(medloc);
            }));
        });
      wcMediaLocator.DownloadStringAsync(new Uri(MediaLocatorUri));
    }

    private void PlayFull_Click(object sender, RoutedEventArgs e)
    {
      MainVideo.Source = ((sender as Button).Tag as MediaMenuData).MediaLocation;
    }

    private void PlayPIP_Click(object sender, RoutedEventArgs e)
    {
      PIPVideo.Source = ((sender as Button).Tag as MediaMenuData).MediaLocation; 
      displayPIP.Visibility = Visibility.Visible;
    }

    private void btnClosePIP_Click(object sender, RoutedEventArgs e)
    {
      PIPVideo.Stop();
      buttonsPIP.Visibility = displayPIP.Visibility = Visibility.Collapsed;
    }

    private void btnSwitchPIP_Click(object sender, RoutedEventArgs e)
    {
      if (vidbrushMain.SourceName == "mediaelemMain")
      {
        vidbrushMain.SourceName = "mediaelemPIP";
        vidbrushPIP.SourceName = "mediaelemMain";
        mediaSlider.SourceName = "mediaelemPIP";
        mediaButtons.SourceName = "mediaelemPIP";
        mediaelemMain.IsMuted = true;
        mediaelemPIP.IsMuted = false;
      }
      else
      {
        vidbrushMain.SourceName = "mediaelemMain";
        vidbrushPIP.SourceName = "mediaelemPIP";
        mediaSlider.SourceName = "mediaelemMain";
        mediaButtons.SourceName = "mediaelemMain";
        mediaelemMain.IsMuted = false;
        mediaelemPIP.IsMuted = true;
      }
      MainVideo.Volume = sliderVolumeControl.Value;
    }

    private void displayPIP_MouseLeftButtonUp(object sender,
      MouseButtonEventArgs e)
    {
      if (displayPIP.Visibility == Visibility.Visible)
      {
        buttonsPIP.Visibility =
          (buttonsPIP.Visibility == Visibility.Visible ?
          Visibility.Collapsed : Visibility.Visible);
      }
    }
    private void sliderVolumeControl_ValueChanged(object sender,
      RoutedPropertyChangedEventArgs<double> e)
    {
      if (vidbrushMain != null)
      {
        MainVideo.Volume = e.NewValue;
      }
    } 
  }  
}
