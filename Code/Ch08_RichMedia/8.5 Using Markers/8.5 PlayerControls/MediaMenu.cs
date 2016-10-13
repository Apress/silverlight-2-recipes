using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Ch08_RichMedia.Recipe8_2
{
  public class MediaMenuItem : ListBoxItem
  {
    public static DependencyProperty DescriptionTemplateProperty = DependencyProperty.Register("DescriptionTemplate", typeof(DataTemplate), typeof(MediaMenuItem), null);
    public static DependencyProperty MediaPreviewTemplateProperty = DependencyProperty.Register("MediaPreviewTemplate", typeof(DataTemplate), typeof(MediaMenuItem), null);

    public MediaMenuItem() : base()
    {
      base.DefaultStyleKey = typeof(MediaMenuItem);
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
    }
  }
}
