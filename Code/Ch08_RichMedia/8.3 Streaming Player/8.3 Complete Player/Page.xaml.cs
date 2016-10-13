using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;

namespace Ch08_RichMedia.Recipe8_3
{
  public partial class Page : UserControl
  {
    private const string DownloadsListUri =
      "http://localhost:9292/MediaLocationProvider.svc/GetDownloadsList";
    private const string OnDemandStreamsListUri =
      "http://localhost:9292/MediaLocationProvider.svc/GetOnDemandStreamsList";
    private const string BroadcastStreamsListUri =
      "http://localhost:9292/MediaLocationProvider.svc/GetBroadcastStreamsList";
    private ObservableCollection<MediaMenuData> listDownloads =
      new ObservableCollection<MediaMenuData>();
    private ObservableCollection<MediaMenuData> listOnDemandStreams =
     new ObservableCollection<MediaMenuData>();
    private ObservableCollection<MediaMenuData> listBroadcastStreams =
     new ObservableCollection<MediaMenuData>();

    public Page()
    {
      InitializeComponent();
      lbxMediaMenuDownloads.ItemsSource = listDownloads;
      lbxMediaMenuOnDemandStreams.ItemsSource = listOnDemandStreams;
      lbxMediaMenuBroadcastStreams.ItemsSource = listBroadcastStreams;
      this.Loaded += new RoutedEventHandler(Page_Loaded);
    }

    void Page_Loaded(object sender, RoutedEventArgs e)
    {
      PopulateMediaMenu();
    }



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

    private void PopulateMediaMenu()
    {
      WebClient wcDownloads = new WebClient();
      wcDownloads.DownloadStringCompleted +=
        new DownloadStringCompletedEventHandler(ListDownloadCompleted);
      WebClient wcOnDemand = new WebClient();
      wcOnDemand.DownloadStringCompleted+=
        new DownloadStringCompletedEventHandler(ListDownloadCompleted);
      WebClient wcBroadcast = new WebClient();
      wcBroadcast.DownloadStringCompleted += 
        new DownloadStringCompletedEventHandler(ListDownloadCompleted);

      wcDownloads.DownloadStringAsync(new Uri(DownloadsListUri), listDownloads);
      wcOnDemand.DownloadStringAsync(new Uri(OnDemandStreamsListUri), listOnDemandStreams);
      wcBroadcast.DownloadStringAsync(new Uri(BroadcastStreamsListUri), listBroadcastStreams);
    }

    void ListDownloadCompleted(object sender, DownloadStringCompletedEventArgs e)
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

        ObservableCollection<MediaMenuData> target = (e.UserState as ObservableCollection<MediaMenuData>);
        foreach (MediaMenuData medloc in tempList)
          target.Add(medloc);
      }));
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
        mediaControl.SourceName = "mediaelemPIP";
        mediaelemMain.IsMuted = true;
        mediaelemPIP.IsMuted = false;
      }
      else
      {
        vidbrushMain.SourceName = "mediaelemMain";
        vidbrushPIP.SourceName = "mediaelemPIP";
        mediaSlider.SourceName = "mediaelemMain";
        mediaControl.SourceName = "mediaelemMain";
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
    private void rbtnDownloadsMenu_Checked(object sender, RoutedEventArgs e)
    {
      if (lbxMediaMenuBroadcastStreams != null &&
        lbxMediaMenuDownloads != null &&
        lbxMediaMenuOnDemandStreams != null)
      {
        lbxMediaMenuBroadcastStreams.Visibility = Visibility.Collapsed;
        lbxMediaMenuOnDemandStreams.Visibility = Visibility.Collapsed;
        lbxMediaMenuDownloads.Visibility = Visibility.Visible;
      }
    }

    private void rbtnOnDemandMenu_Checked(object sender, RoutedEventArgs e)
    {
      if (lbxMediaMenuBroadcastStreams != null &&
        lbxMediaMenuDownloads != null &&
        lbxMediaMenuOnDemandStreams != null)
      {
        lbxMediaMenuBroadcastStreams.Visibility = Visibility.Collapsed;
        lbxMediaMenuOnDemandStreams.Visibility = Visibility.Visible;
        lbxMediaMenuDownloads.Visibility = Visibility.Collapsed;
      }
    }

    private void rbtnBroadcastMenu_Checked(object sender, RoutedEventArgs e)
    {
      if (lbxMediaMenuBroadcastStreams != null &&
        lbxMediaMenuDownloads != null &&
        lbxMediaMenuOnDemandStreams != null)
      {
        lbxMediaMenuBroadcastStreams.Visibility = Visibility.Visible;
        lbxMediaMenuOnDemandStreams.Visibility = Visibility.Collapsed;
        lbxMediaMenuDownloads.Visibility = Visibility.Collapsed;
      }
    }    
  } 
}
