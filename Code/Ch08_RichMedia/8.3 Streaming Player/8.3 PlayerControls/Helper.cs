using System.Windows;

namespace Ch08_RichMedia.Recipe8_3
{
  public static class Helper
  {
    public static FrameworkElement FindRoot(FrameworkElement CurrentLevel)
    {
      FrameworkElement NextParent = null;
      if (CurrentLevel.Parent is FrameworkElement)
        NextParent = FindRoot(CurrentLevel.Parent as FrameworkElement);
      else
        NextParent = CurrentLevel;
      return NextParent;

    }
  }
}
