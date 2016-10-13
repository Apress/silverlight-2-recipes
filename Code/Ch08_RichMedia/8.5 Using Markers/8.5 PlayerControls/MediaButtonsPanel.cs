using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Ch08_RichMedia.Recipe8_5
{
  [TemplateVisualState(GroupName = "SeekStates", Name = "CanSeek")]
  [TemplateVisualState(GroupName = "SeekStates", Name = "CannotSeek")]
  [TemplateVisualState(GroupName = "PauseStates", Name = "CanPause")]
  [TemplateVisualState(GroupName = "PauseStates", Name = "CannotPause")]
  public class MediaButtonsPanel : Control
  {

    private MediaElement MediaSource;
    private FrameworkElement Root;
    private ButtonBase btnPlay, btnPause, btnStop, btnForward, btnRewind;

    public static DependencyProperty SourceNameProperty =
      DependencyProperty.Register("SourceName", typeof(string),
      typeof(MediaButtonsPanel),
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
      MediaButtonsPanel thisPanel = Source as MediaButtonsPanel;

      if (e.NewValue != e.OldValue && thisPanel.Root != null)
      {
        thisPanel.MediaSource = thisPanel.Root.FindName(e.NewValue as string) as MediaElement;
        thisPanel.InitMediaElementConnections();
      }
    }
    public MediaButtonsPanel()
    {
      this.DefaultStyleKey = typeof(MediaButtonsPanel);
    }
    public override void OnApplyTemplate()
    {

      btnPlay = GetTemplateChild("btnPlay") as ButtonBase;
      btnPause = GetTemplateChild("btnPause") as ButtonBase;
      btnStop = GetTemplateChild("btnStop") as ButtonBase;
      btnForward = GetTemplateChild("btnForward") as ButtonBase;
      btnRewind = GetTemplateChild("btnRewind") as ButtonBase;
      Root = Helper.FindRoot(this);
      MediaSource = Root.FindName(SourceName) as MediaElement;
      InitMediaElementConnections();
      WireButtonEvents();
    }
    private void WireButtonEvents()
    {
      if (btnPlay != null)
        btnPlay.Click += new RoutedEventHandler(btnPlay_Click);
      if (btnPause != null)
        btnPause.Click += new RoutedEventHandler(btnPause_Click);
      if (btnStop != null)
        btnStop.Click += new RoutedEventHandler(btnStop_Click);
      if (btnForward != null)
        btnForward.Click += new RoutedEventHandler(btnForward_Click);
      if (btnRewind != null)
        btnRewind.Click += new RoutedEventHandler(btnRewind_Click);
    }

    void btnRewind_Click(object sender, RoutedEventArgs e)
    {
      if (MediaSource != null && MediaSource.Position > TimeSpan.Zero)
      {
        MediaSource.Pause();
        //5th of a second
        MediaSource.Position -= new TimeSpan(0, 0, 0, 0, 200);
        MediaSource.Play();
      }
    }
    void btnForward_Click(object sender, RoutedEventArgs e)
    {
      if (MediaSource != null && MediaSource.Position 
        <= MediaSource.NaturalDuration.TimeSpan)
      {
        MediaSource.Pause();
        MediaSource.Position += new TimeSpan(0, 0, 0, 0, 200);
        MediaSource.Play();
      }
    }
    void btnStop_Click(object sender, RoutedEventArgs e)
    {
      if (MediaSource != null)
        MediaSource.Stop();
    }
    void btnPause_Click(object sender, RoutedEventArgs e)
    {
      if (MediaSource != null &&
        MediaSource.CurrentState == MediaElementState.Playing)
        MediaSource.Pause();
    }
    void btnPlay_Click(object sender, RoutedEventArgs e)
    {
      if (MediaSource != null &&
        MediaSource.CurrentState != MediaElementState.Playing)
        MediaSource.Play();
    }

    private void InitMediaElementConnections()
    {
      if (MediaSource != null)
      {
        MediaSource.MediaOpened +=
          new RoutedEventHandler(MediaSource_MediaOpened);        
        MediaSource.CurrentStateChanged +=
          new RoutedEventHandler(MediaSource_CurrentStateChanged);
        
        MediaSource_CurrentStateChanged(this, new RoutedEventArgs());
      }
    }
    private void MediaSource_CurrentStateChanged(object sender, RoutedEventArgs e)
    {
      switch (MediaSource.CurrentState)
      {        
        case MediaElementState.Playing:
          VisualStateManager.GoToState(this,
      (MediaSource.CanSeek == false) ? "CannotSeek" : "CanSeek", true);
          VisualStateManager.GoToState(this,
      (MediaSource.CanPause == false) ? "CannotPause" : "CanPause", true);  
          break;      
        default:
          break;
      }
    }
     
    private void MediaSource_MediaOpened(object sender, RoutedEventArgs e)
    {
      VisualStateManager.GoToState(this,
      (MediaSource.CanSeek == false) ? "CannotSeek" : "CanSeek", true);
      VisualStateManager.GoToState(this,
      (MediaSource.CanPause == false) ? "CannotPause" : "CanPause", true);  
    }     
  }
}
