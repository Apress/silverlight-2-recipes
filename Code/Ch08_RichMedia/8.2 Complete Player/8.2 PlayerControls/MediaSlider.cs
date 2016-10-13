using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;

namespace Ch08_RichMedia.Recipe8_2
{
 public class MediaSlider : Slider
  {
    private MediaElement MediaSource;
    private FrameworkElement elemDownloadProgressIndicator;
    private FrameworkElement elemPlayProgressIndicator;
    private FrameworkElement Root;
    private TextBlock textPosition;
    private TextBlock textDuration;
    private TextBlock textDownloadPercent;
    private Thumb HorizontalThumb;
    private DispatcherTimer disptimerPlayProgressUpdate;

   //SourceName dependency property - used to attach 
   //a Media element to this control
    public static DependencyProperty SourceNameProperty =
      DependencyProperty.Register("SourceName", typeof(string),
      typeof(MediaSlider),
      new PropertyMetadata(new PropertyChangedCallback(OnSourceNameChanged)));
    public string SourceName
    {
      get
      {
        return (string)GetValue(SourceNameProperty);
      }
      set
      {
        SetValue(SourceNameProperty, value);
      }
    }
   //SourceName change handler
    private static void OnSourceNameChanged(DependencyObject Source,
      DependencyPropertyChangedEventArgs e)
    {
      MediaSlider thisSlider = Source as MediaSlider;
      if (e.NewValue != null && e.NewValue != e.OldValue 
        && thisSlider.Root != null)
      {
        thisSlider.MediaSource = 
          thisSlider.Root.FindName(e.NewValue as string) as MediaElement;
        //reinitialize
        thisSlider.InitMediaElementConnections();
      }
    }
   
 
    public MediaSlider()
      : base()
    {
      this.DefaultStyleKey = typeof(MediaSlider);
      this.Maximum = 100;
      this.Minimum = 0;
      disptimerPlayProgressUpdate = new DispatcherTimer();
      disptimerPlayProgressUpdate.Interval = new TimeSpan(0, 0, 0, 0, 50);
      disptimerPlayProgressUpdate.Tick += 
        new EventHandler(PlayProgressUpdate_Tick);
    }
    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();

      elemDownloadProgressIndicator =
        GetTemplateChild("elemDownloadProgressIndicator") as FrameworkElement;
      elemPlayProgressIndicator =
       GetTemplateChild("elemPlayProgressIndicator") as FrameworkElement;
      HorizontalThumb = GetTemplateChild("HorizontalThumb") as Thumb;
      if (HorizontalThumb != null)
      {
        HorizontalThumb.DragStarted +=
          new DragStartedEventHandler(HorizontalThumb_DragStarted);
        HorizontalThumb.DragCompleted +=
          new DragCompletedEventHandler(HorizontalThumb_DragCompleted);
      }
      textPosition = GetTemplateChild("textPosition") as TextBlock;
      textDuration = GetTemplateChild("textDuration") as TextBlock;
      textDownloadPercent = GetTemplateChild("textDownloadPercent") as TextBlock;

      Root = Helper.FindRoot(this);
      MediaSource = Root.FindName(SourceName) as MediaElement;
      InitMediaElementConnections();
    }
    //Initialize by wiring up handlers
    private void InitMediaElementConnections()
    {
      if (MediaSource != null)
      {
        MediaSource.MediaOpened += 
          new RoutedEventHandler(MediaSource_MediaOpened);
        MediaSource.MediaEnded +=
          new RoutedEventHandler(MediaSource_MediaEnded);
        MediaSource.MediaFailed +=
          new EventHandler<ExceptionRoutedEventArgs>(MediaSource_MediaFailed);
        MediaSource.CurrentStateChanged +=
          new RoutedEventHandler(MediaSource_CurrentStateChanged);
        MediaSource.DownloadProgressChanged +=
          new RoutedEventHandler(MediaSource_DownloadProgressChanged);

        MediaSource_CurrentStateChanged(this, new RoutedEventArgs());
      }
    }
    
   //tick handler for progress timer
    void PlayProgressUpdate_Tick(object sender, EventArgs e)
    {    
      this.Value = 
        (MediaSource.Position.TotalMilliseconds /
        MediaSource.NaturalDuration.TimeSpan.TotalMilliseconds)
        * (this.Maximum - this.Minimum);

      if (elemPlayProgressIndicator != null)
      {
        elemPlayProgressIndicator.Width = 
          (MediaSource.Position.TotalMilliseconds / 
          MediaSource.NaturalDuration.TimeSpan.TotalMilliseconds)
          * ActualWidth;
      }
      if (textPosition != null)
        textPosition.Text = string.Format("{0:00}:{1:00}:{2:00}:{3:000}",
          MediaSource.Position.Hours, 
          MediaSource.Position.Minutes, 
          MediaSource.Position.Seconds, 
          MediaSource.Position.Milliseconds);
    }
   //plug into the thumb to pause play while it is being dragged
    void HorizontalThumb_DragStarted(object sender, DragStartedEventArgs e)
    {
      if (MediaSource != null && MediaSource.CurrentState ==
        MediaElementState.Playing)
        MediaSource.Pause();
    }
    void HorizontalThumb_DragCompleted(object sender, DragCompletedEventArgs e)
    {
      if (MediaSource != null)
      {
        MediaSource.Position = new TimeSpan(0, 
          0, 0, 0, 
          (int)(this.Value * 
          MediaSource.NaturalDuration.TimeSpan.TotalMilliseconds / (this.Maximum - this.Minimum)));
      }
      MediaSource.Play();
    }
      
    //media element download progress changed
    private void MediaSource_DownloadProgressChanged(object sender,
      RoutedEventArgs e)
    {
      if (elemDownloadProgressIndicator != null)
      {
        elemDownloadProgressIndicator.Width = 
          (MediaSource.DownloadProgress * this.ActualWidth);
        if (textDownloadPercent != null)
          textDownloadPercent.Text = string.Format("{0:##.##} %", 
            MediaSource.DownloadProgress * 100);
      }
    }
    //state changes on the MediaElement
    private void MediaSource_CurrentStateChanged(object sender,
      RoutedEventArgs e)
    {
      switch (MediaSource.CurrentState)
      {          
        case MediaElementState.Playing:
          if (textDuration != null)
            textDuration.Text = string.Format("{0:00}:{1:00}:{2:00}:{3:000}",
              MediaSource.NaturalDuration.TimeSpan.Hours, 
              MediaSource.NaturalDuration.TimeSpan.Minutes, 
              MediaSource.NaturalDuration.TimeSpan.Seconds, 
              MediaSource.NaturalDuration.TimeSpan.Milliseconds);
          if (disptimerPlayProgressUpdate.IsEnabled == false)
          disptimerPlayProgressUpdate.Start();
          break;
        case MediaElementState.Paused:
          if (disptimerPlayProgressUpdate.IsEnabled)
          disptimerPlayProgressUpdate.Stop();
          break;
        case MediaElementState.Stopped:
          if (disptimerPlayProgressUpdate.IsEnabled)
          disptimerPlayProgressUpdate.Stop();
          break;
        case MediaElementState.AcquiringLicense:          
        case MediaElementState.Individualizing:
        case MediaElementState.Opening:         
        case MediaElementState.Buffering:
        case MediaElementState.Closed:
          break;
          
        default:
          break;
      }
    }

   //media ended
    private void MediaSource_MediaEnded(object sender,
      RoutedEventArgs e)
    {
      if (disptimerPlayProgressUpdate.IsEnabled)
      disptimerPlayProgressUpdate.Stop();
    }
    
   //media failed
    private void MediaSource_MediaFailed(object sender, RoutedEventArgs e)
    {
      disptimerPlayProgressUpdate.Stop();
    }

    void MediaSource_MediaOpened(object sender, RoutedEventArgs e)
    {
      //we do nothing here in this sample
    }   
  } 
}
