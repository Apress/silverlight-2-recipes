using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Ch06_BrowserIntegration.Recipe6_4
{
  public partial class Page : UserControl
  {
    private double _embeddedWidth;
    private double _embeddedHeight;

    public Page()
    {
      InitializeComponent();
      Application.Current.Host.Content.FullScreenChanged += new EventHandler(Content_FullScreenChanged);
      this.Loaded += new RoutedEventHandler(Page_Loaded);
    }

    void Page_Loaded(object sender, RoutedEventArgs e)
    {
      //Store the embedded with and height so that we can
      //calculate the proper scale factor
      _embeddedWidth = this.Width;
      _embeddedHeight = this.Height;
    }

    void Content_FullScreenChanged(object sender, EventArgs e)
    {
      if (!Application.Current.Host.Content.IsFullScreen)
      {
        ScaleToFullScreen.ScaleX = 1.0d;
        ScaleToFullScreen.ScaleY = 1.0d;
        ((ScaleTransform)this.Resources["ReduceScaleTransform"]).ScaleX = 1.0d;
        ((ScaleTransform)this.Resources["ReduceScaleTransform"]).ScaleY = 1.0d;
        ButtonPanel.HorizontalAlignment = HorizontalAlignment.Center;
      }
      else
      {
        double pluginWidth = Application.Current.Host.Content.ActualWidth;
        double pluginHeight = Application.Current.Host.Content.ActualHeight;
        double scaleX = pluginWidth / _embeddedWidth;
        double scaleY = pluginHeight / _embeddedHeight;

        ScaleToFullScreen.ScaleX = scaleX;
        ScaleToFullScreen.ScaleY = scaleY;

        ((ScaleTransform)this.Resources["ReduceScaleTransform"]).ScaleX = scaleX * .10d;
        ((ScaleTransform)this.Resources["ReduceScaleTransform"]).ScaleY = scaleY * .10d;
        ButtonPanel.HorizontalAlignment = HorizontalAlignment.Right;
      }
    }

    private void FullScreenButton_Click(object sender, RoutedEventArgs e)
    {
      Application.Current.Host.Content.IsFullScreen =
        !Application.Current.Host.Content.IsFullScreen;
      if (Application.Current.Host.Content.IsFullScreen)
      {
        FullScreenButton.Content = "Emb";
      }
      else
      {
        FullScreenButton.Content = "Full";
      }
    }

    private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
    {
      if ((mediaElement.CurrentState == MediaElementState.Stopped) ||
         (mediaElement.CurrentState == MediaElementState.Paused))
      {
        mediaElement.Play();
        PlayPauseButton.Content = "Pause";
      }
      else if (mediaElement.CurrentState == MediaElementState.Playing)
      {
        mediaElement.Pause();
        PlayPauseButton.Content = "Play";
      }
    }

    private void StopButton_Click(object sender, RoutedEventArgs e)
    {
      mediaElement.Stop();
      PlayPauseButton.Content = "Play";
    }

    private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
    {
      mediaElement.Position = new TimeSpan(0);
      PlayPauseButton.Content = "Play";
    }
  }
}
