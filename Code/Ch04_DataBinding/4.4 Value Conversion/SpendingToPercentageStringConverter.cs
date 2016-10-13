using System;
using System.Windows.Data;

namespace Ch04_DataBinding.Recipe4_4
{
  public class SpendingToPercentageStringConverter : IValueConverter
  {
    public object Convert(object value, Type targetType,
      object parameter, System.Globalization.CultureInfo culture)
    {
      //verify validity of all the parameters
      if (value.GetType() != typeof(double) || targetType != typeof(string)
        || parameter == null
        || parameter.GetType() != typeof(SpendingCollection))
        return null;
      //cast appropriately
      double Spending = (double)value;
      double Total = ((SpendingCollection)parameter).Total;
      //calculate the spending percentage and format as string 
      return ((Spending / Total) * 100).ToString("###.##") + " %";
    }

    public object ConvertBack(object value, Type targetType,
      object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}