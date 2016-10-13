using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Ch05_Controls.Recipe5_11
{
  [TemplateVisualState(Name = "Expanded", GroupName = "ExpanderStates")]
  [TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
  public class Expander : ContentControl
  {

    public static DependencyProperty HeaderContentProperty =
      DependencyProperty.Register("HeaderContent", typeof(object),
      typeof(Expander),null);
    public object HeaderContent
    {
      get
      {
        return GetValue(HeaderContentProperty);
      }
      set
      {
        SetValue(HeaderContentProperty, value);
      }
    }

    public static DependencyProperty HeaderContentTemplateProperty =
      DependencyProperty.Register("HeaderContentTemplate", typeof(DataTemplate),
      typeof(Expander),null);
    public object HeaderContentTemplate
    {
      get
      {
        return (DataTemplate)GetValue(HeaderContentTemplateProperty);
      }
      set
      {
        SetValue(HeaderContentTemplateProperty, value);
      }
    }

    private ToggleButton btnToggler;

    public Expander()
    {
      base.DefaultStyleKey = typeof(Expander);
    }
    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      btnToggler = GetTemplateChild("toggler") as ToggleButton;
      if (btnToggler != null)
      {
        btnToggler.Checked += new RoutedEventHandler(btnToggler_Checked);
        btnToggler.Unchecked += new RoutedEventHandler(btnToggler_Unchecked);
      }
    }

    void btnToggler_Unchecked(object sender, RoutedEventArgs e)
    {
      VisualStateManager.GoToState(this, "Normal", true);
    }

    void btnToggler_Checked(object sender, RoutedEventArgs e)
    {
      VisualStateManager.GoToState(this, "Expanded", true);
    }

  }
}
