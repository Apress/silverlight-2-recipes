using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;

namespace Ch08_RichMedia.Recipe8_4
{
  [TemplateVisualState(GroupName = "SeekStates", Name = "CanSeek")]
  [TemplateVisualState(GroupName = "SeekStates", Name = "CannotSeek")]
  [TemplateVisualState(GroupName = "ContentStates", Name = "Buffering")]
  [TemplateVisualState(GroupName = "ContentStates", Name = "Playing")]
  [TemplateVisualState(GroupName = "DurationStates", Name = "UnknownDuration")]
  [TemplateVisualState(GroupName = "DurationStates", Name = "KnownDuration")]
  public class MediaSlider : Slider
  {
    private MediaElement MediaSource;
    private FrameworkElement elemDownloadProgressIndicator;
    private FrameworkElement elemBufferingProgressIndicator;
    private FrameworkElement elemPlayProgressIndicator;
    private FrameworkElement Root;
    private TextBlock textPosition;
    private TextBlock textDuration;
    private TextBlock textDownloadPercent;
    private TextBlock textBufferingPercent;
    private Thumb HorizontalThumb;
    private DispatcherTimer disptimerPlayProgressUpdate;

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
    private static void OnSourceNameChanged(DependencyObject Source,
      DependencyPropertyChangedEventArgs e)
    {
      MediaSlider thisSlider = Source as MediaSlider;
      if (e.NewValue != null && e.NewValue != e.OldValue
        && thisSlider.Root != null)
      {
        thisSlider.MediaSource =
          thisSlider.Root.FindName(e.NewValue as string) as MediaElement;
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
      elemBufferingProgressIndicator =
        GetTemplateChild("elemBufferingProgressIndicator") as FrameworkElement;
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
      textBufferingPercent = GetTemplateChild("textBufferingPercent") as TextBlock;
      Root = Helper.FindRoot(this);
      MediaSource = Root.FindName(SourceName) as MediaElement;
      InitMediaElementConnections();
    }
    private void InitMediaElementConnections()
    {
      if (MediaSource != null)
      {
        MediaSource.MediaOpened += new RoutedEventHandler(MediaSource_MediaOpened);
        MediaSource.MediaEnded +=
          new RoutedEventHandler(MediaSource_MediaEnded);
        MediaSource.MediaFailed +=
          new EventHandler<ExceptionRoutedEventArgs>(MediaSource_MediaFailed);
        MediaSource.CurrentStateChanged +=
          new RoutedEventHandler(MediaSource_CurrentStateChanged);
        MediaSource.DownloadProgressChanged +=
          new RoutedEventHandler(MediaSource_DownloadProgressChanged);
        MediaSource.BufferingProgressChanged +=
          new RoutedEventHandler(MediaSource_BufferingProgressChanged);

        MediaSource_CurrentStateChanged(this, new RoutedEventArgs());
      }
    }
    void PlayProgressUpdate_Tick(object sender, EventArgs e)
    {
      if (MediaSource.NaturalDuration.TimeSpan == TimeSpan.Zero)
        return;

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
    void HorizontalThumb_DragCompleted(object sender, DragCompletedEventArgs e)
    {
      if (MediaSource != null && MediaSource.CurrentState ==
        MediaElementState.Playing
        && MediaSource.NaturalDuration.TimeSpan != TimeSpan.Zero)
      {
        MediaSource.Position = new TimeSpan(0,
          0, 0, 0,
          (int)(this.Value *
          MediaSource.NaturalDuration.TimeSpan.TotalMilliseconds / 100));
      }
      MediaSource.Play();
    }
    void HorizontalThumb_DragStarted(object sender, DragStartedEventArgs e)
    {
      if (MediaSource != null &&
        MediaSource.CurrentState == MediaElementState.Playing
        && MediaSource.CanPause)
        MediaSource.Pause();
    }

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
    void MediaSource_BufferingProgressChanged(object sender, RoutedEventArgs e)
    {
      if (elemDownloadProgressIndicator != null)
      {

        if (textBufferingPercent != null)
          textBufferingPercent.Text = string.Format("{0:##.##} %",
            MediaSource.BufferingProgress * 100);
      }
    }
    private void MediaSource_CurrentStateChanged(object sender, RoutedEventArgs e)
    {
      switch (MediaSource.CurrentState)
      {
        case MediaElementState.Opening:
          VisualStateManager.GoToState(this, "Normal", true);
          break;
        case MediaElementState.Playing:
          RefreshMediaStates();
          VisualStateManager.GoToState(this, "Playing", true);
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
        case MediaElementState.Buffering:
          VisualStateManager.GoToState(this, "Buffering", true);
          break;
        default:
          break;
      }
    }
    void MediaSource_MediaOpened(object sender, RoutedEventArgs e)
    {
      RefreshMediaStates();
    }

    private void RefreshMediaStates()
    {
      VisualStateManager.GoToState(this,
           (MediaSource.CanSeek) ? "CanSeek" : "CannotSeek", true);
      VisualStateManager.GoToState(this,
        (MediaSource.NaturalDuration.TimeSpan != TimeSpan.Zero) ?
        "KnownDuration" : "UnknownDuration", true);
      VisualStateManager.GoToState(this,
        (MediaSource.DownloadProgress == 1.0) ?
        "NoDownload" : "NeedsDownload", true);
      if (textDuration != null &&
           MediaSource.NaturalDuration.TimeSpan != TimeSpan.Zero)
        textDuration.Text = string.Format("{0:00}:{1:00}:{2:00}:{3:000}",
          MediaSource.NaturalDuration.TimeSpan.Hours,
          MediaSource.NaturalDuration.TimeSpan.Minutes,
          MediaSource.NaturalDuration.TimeSpan.Seconds,
          MediaSource.NaturalDuration.TimeSpan.Milliseconds);
    }
    private void MediaSource_MediaEnded(object sender, RoutedEventArgs e)
    {
      if (disptimerPlayProgressUpdate.IsEnabled)
        disptimerPlayProgressUpdate.Stop();
    }
    private void MediaSource_MediaFailed(object sender, RoutedEventArgs e)
    {
      if (disptimerPlayProgressUpdate.IsEnabled)
        disptimerPlayProgressUpdate.Stop();
    }
  } 
}
