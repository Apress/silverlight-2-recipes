using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Ch05_Controls.Recipe5_10
{
  [TemplatePart(Name="elemPBar",Type=typeof(FrameworkElement))]
  public class ProgressBar : ContentControl
  {
    public static DependencyProperty CurrentValueProperty =
      DependencyProperty.Register("CurrentValue",
      typeof(double), typeof(ProgressBar),
      new PropertyMetadata(0.0,
        new PropertyChangedCallback(ProgressBar.OnCurrentValueChanged)));
    public double CurrentValue
    {
      get { return (double)GetValue(CurrentValueProperty); }
      set { SetValue(CurrentValueProperty, value); }
    }

    public static DependencyProperty MaximumValueProperty =
      DependencyProperty.Register("MaximumValue",
      typeof(double), typeof(ProgressBar), new PropertyMetadata(100.0));
    public double MaximumValue
    {
      get { return (double)GetValue(MaximumValueProperty); }
      set { SetValue(MaximumValueProperty, value); }
    }

    public static DependencyProperty MinimumValueProperty =
      DependencyProperty.Register("MinimumValue",
      typeof(double), typeof(ProgressBar), new PropertyMetadata(0.0));
    public double MinimumValue
    {
      get { return (double)GetValue(MinimumValueProperty); }
      set { SetValue(MinimumValueProperty, value); }
    }


    
    internal FrameworkElement elemPBar { get; set; }

    public ProgressBar()
    {
      base.DefaultStyleKey = typeof(ProgressBar);
    } 

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      elemPBar = this.GetTemplateChild("elemPBar") as FrameworkElement;
    }

    internal static void OnCurrentValueChanged(DependencyObject Target,
      DependencyPropertyChangedEventArgs e)
    {
      ProgressBar pBar = Target as ProgressBar;
      if (pBar.elemPBar != null)
      {
        pBar.elemPBar.Width = (pBar.ActualWidth * (double)e.NewValue)
          / (pBar.MaximumValue - pBar.MinimumValue);
      }

    }
  }
}
