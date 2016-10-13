using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Xml.Linq;

namespace Ch08_RichMedia.Recipe8_5
{
  public partial class Page : UserControl
  {
    private const string DownloadsListUri =
      "http://localhost:9494/MediaLocationProvider.svc/GetDownloadsList";
    private const string OnDemandStreamsListUri =
      "http://localhost:9494/MediaLocationProvider.svc/GetOnDemandStreamsList";
    private const string BroadcastStreamsListUri =
      "http://localhost:9494/MediaLocationProvider.svc/GetBroadcastStreamsList";
    private const string CaptionsListUri =
     "http://localhost:9494/MediaLocationProvider.svc/Captions?MediaId={0}";
    private const string CommercialsListUri =
     "http://localhost:9494/MediaLocationProvider.svc/Commercial?Marker={0}";

    private ObservableCollection<MediaMenuData> listDownloads =
      new ObservableCollection<MediaMenuData>();
    private ObservableCollection<MediaMenuData> listOnDemandStreams =
     new ObservableCollection<MediaMenuData>();
    private ObservableCollection<MediaMenuData> listBroadcastStreams =
     new ObservableCollection<MediaMenuData>();

    private Dictionary<string, string> dictCaptions = null;

    DispatcherTimer timerAdManager = null;

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

      //handle marker reached for the main display
      MainVideo.MarkerReached += 
        new System.Windows.Media.TimelineMarkerRoutedEventHandler(MainVideo_MarkerReached);
      //handle both media_opened events
      MainVideo.MediaOpened += new RoutedEventHandler(MainVideo_MediaOpened);
      PIPVideo.MediaOpened += new RoutedEventHandler(PIPVideo_MediaOpened);
      //set up a timer to manage commercials
      timerAdManager = new DispatcherTimer();
      timerAdManager.Interval = new TimeSpan(0, 0, 15);
      timerAdManager.Tick +=new EventHandler(delegate(object timer, EventArgs args)
        {
          //clear
          if (adContainer.Children.Count > 0)
            adContainer.Children.Clear();
          //stop timer
          if ((timer as DispatcherTimer).IsEnabled)
            (timer as DispatcherTimer).Stop();
        });
    }

    void PIPVideo_MediaOpened(object sender, RoutedEventArgs e)
    {
      //we will nevr display commercials in the PIP, but it might get switched with the main - hence this
      AttachClientMarkers(PIPVideo);
    }

    void MainVideo_MediaOpened(object sender, RoutedEventArgs e)
    {
      //attach the client markers for commercials demo
      AttachClientMarkers(MainVideo);
    }

    private void AttachClientMarkers(MediaElement medElem)
    {
      TimeSpan ts = TimeSpan.Zero;
      if (medElem.NaturalDuration.TimeSpan != TimeSpan.Zero)
      {
        int Ctr = 0;
        while (ts <= medElem.NaturalDuration.TimeSpan)
        {
          //Text = unique name, Time 5,40, 75, ...
          medElem.Markers.Add(new TimelineMarker {
            Text = "ClientMarker"+(++Ctr).ToString(),
            Time = ts + new TimeSpan(0, 0, 5), 
            Type="SLMovie" }); 
          ts += new TimeSpan(0, 0, 30);
        }
      }
    }    

    void MainVideo_MarkerReached(object sender, 
      System.Windows.Media.TimelineMarkerRoutedEventArgs e)
    {
      //Captions markers coming from encoded video
      if (dictCaptions != null && dictCaptions.Count > 0 
        && dictCaptions.ContainsKey(e.Marker.Text))
      {
        //clear if we got here before the previous animation completed
        if (CaptionContainer.Children.Count > 0)
          CaptionContainer.Children.Clear();
        //get the caption XAML
        FrameworkElement fe = XamlReader.Load(dictCaptions[e.Marker.Text]) 
          as FrameworkElement;
        //add
        CaptionContainer.Children.Add(fe);
        //get the animation
        Storyboard stbd = fe.Resources["STBD_AnimateCaption"] as Storyboard;
        stbd.Completed +=
          new EventHandler(delegate(object anim, EventArgs args)
          {
            //clear on animation completion
            if (CaptionContainer.Children.Count > 0)
              CaptionContainer.Children.Clear();
          });
        //run animation
        stbd.Begin();
      }
        //commerical marker
      else if (e.Marker.Type == "SLMovie")
      {
        WebClient wcCommercial = new WebClient();
        wcCommercial.DownloadStringCompleted += 
          new DownloadStringCompletedEventHandler(
            delegate(object wc, DownloadStringCompletedEventArgs args)
          {
            if (args.Result == null || args.Result == string.Empty) return;
            if (adContainer.Children.Count > 0)
              adContainer.Children.Clear();
            //parse
            XDocument xDoc = XDocument.Parse(args.Result);
            //add
            adContainer.Children.Add(XamlReader.Load((
              (XCData)xDoc.Root.DescendantNodes().ToList()[0]).Value) 
              as FrameworkElement);
            //start timer
            timerAdManager.Start();
          
          });
        //get commercial for this marker type
        wcCommercial.DownloadStringAsync(
          new Uri(string.Format(CommercialsListUri,e.Marker.Type)));
      }
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
      wcOnDemand.DownloadStringAsync(new Uri(OnDemandStreamsListUri),
        listOnDemandStreams);
      wcBroadcast.DownloadStringAsync(new Uri(BroadcastStreamsListUri),
        listBroadcastStreams);
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

        ObservableCollection<MediaMenuData> target = 
          (e.UserState as ObservableCollection<MediaMenuData>);
        foreach (MediaMenuData medloc in tempList)
          target.Add(medloc);
      }));
    }

    private void PlayFull_Click(object sender, RoutedEventArgs e)
    {
      //get the animations
      Uri mediaUri = ((sender as Button).Tag as MediaMenuData).MediaLocation;
      WebClient wcAnimations = new WebClient();
      wcAnimations.DownloadStringCompleted += 
        new DownloadStringCompletedEventHandler(wcAnimations_DownloadStringCompleted);
      //pass in the mediaelement and the source URI
      wcAnimations.DownloadStringAsync(
        new Uri(string.Format(CaptionsListUri, mediaUri.AbsoluteUri)),
        new object[] { MainVideo, mediaUri });
 
    }

    void wcAnimations_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
    {
      if (e.Result != null && e.Result != string.Empty)
      {
        //parse
        XDocument xDoc = XDocument.Parse(e.Result);
        //get each animation 
        var AnimationUnits = from marker in xDoc.Root.Elements()
                             select new {
                             key = marker.Attribute("Value").Value, 
                             XamlFragment = ((XCData)marker.DescendantNodes().
                             ToList()[0]).Value };

        dictCaptions = new Dictionary<string, string>();
        //store in dictionary
        foreach (var marker in AnimationUnits)
          dictCaptions.Add(marker.key, marker.XamlFragment);
      }
      //start playing the media
      ((e.UserState as object[])[0] as MediaElement).Source = 
        ((e.UserState as object[])[1] as Uri); 
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
