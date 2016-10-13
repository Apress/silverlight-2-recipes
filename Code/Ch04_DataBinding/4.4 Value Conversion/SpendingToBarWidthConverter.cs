using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Shapes;

namespace Ch04_DataBinding.Recipe4_4
{
  public class SpendingToBarWidthConverter : IValueConverter
  {
    public object Convert(object value, Type targetType,
      object parameter, System.Globalization.CultureInfo culture)
    {
      //verify validity of all the parameters
      if (value.GetType() != typeof(double) || targetType != typeof(double)
        || parameter == null
        || parameter.GetType() != typeof(SpendingCollection))
        return null;
      //cast appropriately
      double Spending = (double)value;
      double Total = ((SpendingCollection)parameter).Total;
      //find the xAxis
      Rectangle rectXAxis = (Rectangle)((Page)Application.Current.RootVisual)
        .FindName("rectXAxis");
      //calculate bar width in proportion to the xAxis width
      return (Spending / Total) * rectXAxis.Width;
    }

    public object ConvertBack(object value, Type targetType,
      object parameter, System.Globalization.CultureInfo culture)
    {
      //verify validity of all the parameters
      if (value.GetType() != typeof(double) || targetType != typeof(double)
        || parameter == null
        || parameter.GetType() != typeof(SpendingCollection))
        return null;
      //cast appropriately
      double BarWidth = (double)value;
      double Total = ((SpendingCollection)parameter).Total;
      //find the xAxis
      Rectangle rectXAxis = (Rectangle)((Page)Application.Current.RootVisual)
        .FindName("rectXAxis");
      //calculate new spending keeping total spending constant based on 
      //new bar width to xAxis width ratio
      return (BarWidth / rectXAxis.Width) * Total;
    }
  }
}