using System.Windows.Controls;
using System.ComponentModel;

namespace Ch05_Controls.Recipe5_9
{
  public partial class Page : UserControl
  {
    ListBoxPanelOrientation CurrentLbxOrientation = 
      new ListBoxPanelOrientation { CurrentOrientation = Orientation.Horizontal };
    public Page()
    {
      InitializeComponent();
      lbxWrapPanelTest.DataContext = CurrentLbxOrientation;
    }

    private void rbtnHorizontal_Checked(object sender,
      System.Windows.RoutedEventArgs e)
    {
      CurrentLbxOrientation.CurrentOrientation = Orientation.Horizontal;
    }

    private void rbtnVertical_Checked(object sender,
      System.Windows.RoutedEventArgs e)
    {
      CurrentLbxOrientation.CurrentOrientation = Orientation.Vertical;
    }
  }

  public class ListBoxPanelOrientation : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    private Orientation _Current;
    public Orientation CurrentOrientation
    {
      get { return _Current; }
      set
      {
        _Current = value;
        if (PropertyChanged != null) 
          PropertyChanged(this, 
            new PropertyChangedEventArgs("CurrentOrientation"));
      }
    }
  }
}
