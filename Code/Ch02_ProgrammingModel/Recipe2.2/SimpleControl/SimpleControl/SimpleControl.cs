using System.Windows;
using System.Windows.Controls;

namespace SimpleControl
{
  public class SimpleControl : Control
  {
    protected TextBlock _fullNameTextBlock;

    public SimpleControl()
    {
      this.DefaultStyleKey = this.GetType();
    }

    public string FullName
    {
      get { return (string)GetValue(FullNameProperty); }
      set { SetValue(FullNameProperty, value); }
    }

    public static readonly DependencyProperty FullNameProperty =
        DependencyProperty.Register("FullName", typeof(string),
        typeof(SimpleControl), new PropertyMetadata(OnFullNameChanged,));

    protected static void OnFullNameChanged(DependencyObject o,
      DependencyPropertyChangedEventArgs args)
    {
      SimpleControl hc = o as SimpleControl;
      if (hc != null)
      {
        if (hc._fullNameTextBlock != null)
          hc._fullNameTextBlock.Text =
            controlLogic(args.NewValue.ToString());
      }
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      _fullNameTextBlock = 
        GetTemplateChild("fullNameTextBlock") as TextBlock;
      _fullNameTextBlock.Text = 
        controlLogic((string)GetValue(FullNameProperty));
    }

    private static string controlLogic(string targetValue)
    {
      return string.Format("FullName: {0}", targetValue);
    }
  }
}