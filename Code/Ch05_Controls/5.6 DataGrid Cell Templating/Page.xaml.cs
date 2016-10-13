using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Ch05_Controls.Recipe5_6.AdvWorks;

namespace Ch05_Controls.Recipe5_6
{
  public partial class Page : UserControl
  {
    AdvWorksDataServiceClient client = 
      new AdvWorksDataServiceClient();
    bool EditingColor = false;

    public Page()
    {
      InitializeComponent(); 
      GetData();
    }

    private void GetData()
    {
      client.GetProductsCompleted += 
        new EventHandler<GetProductsCompletedEventArgs>(
          delegate(object sender, GetProductsCompletedEventArgs e)
          {           
            dgProducts.ItemsSource = e.Result;
          });

      client.GetProductsAsync();
    } 
  }


  public class ColorNameToBrushConverter : IValueConverter
  {
    //convert from a string Color name to a SolidColorBrush
    public object Convert(object value, Type targetType, 
      object parameter, System.Globalization.CultureInfo culture)
    {
      //substitute a null with Transparent
      if (value == null)
        value = "Transparent";
      //make sure the right types are being converted
      if (targetType != typeof(Brush) || value.GetType() != typeof(string))
        throw new NotSupportedException(
          string.Format("{0} to {1} is not supported by {2}",
          value.GetType().Name,
          targetType.Name,
          this.GetType().Name));

      SolidColorBrush scb = null;
      try
      {
        //get all the static Color properties defined in 
        //System.Windows.Media.Colors
        List<PropertyInfo> ColorProps = typeof(Colors).
          GetProperties(BindingFlags.Public | BindingFlags.Static).ToList();
        //use LINQ to find the property whose name equates 
        //to the string literal we are trying to convert
        List<PropertyInfo> piTarget = (from pi in ColorProps
                                       where pi.Name == (string)value
                                       select pi).ToList();
        //create a SolidColorBrush using the found Color property. 
        //If none was found i.e. the string literal being converted
        //did not match any of the defined Color properties in Colors
        //use Transparent
        scb = new SolidColorBrush(piTarget.Count == 0 ? 
          Colors.Transparent : (Color)(piTarget[0].GetValue(null, null)));
      }
      catch
      {
        //on exception, use Transparent
        scb = new SolidColorBrush(Colors.Transparent);
      }
      return scb;


    }
    //convert from a SolidColorBrush to a string Color name
    public object ConvertBack(object value, Type targetType, 
      object parameter, System.Globalization.CultureInfo culture)
    {
      //make sure the right types are being converted
      if (targetType != typeof(string) || value.GetType() != typeof(Brush))
        throw new NotSupportedException(
          string.Format("{0} to {1} is not supported by {2}",
          value.GetType().Name,
          targetType.Name,
          this.GetType().Name));

      string ColorName = null;
      try
      {
        //get all the static Color properties defined 
        //in System.Windows.Media.Colors
        List<PropertyInfo> ColorProps = typeof(Colors).
          GetProperties(BindingFlags.Public | BindingFlags.Static).ToList();
        //use LINQ to find the property whose value equates to the 
        //Color on the Brush we are trying to convert
        List<PropertyInfo> piTarget = (from pi in ColorProps
                                       where (Color)pi.GetValue(null, null)
                                       == ((SolidColorBrush)value).Color
                                       select pi).ToList();
        //If a match is found get the Property name, if not use "Transparent"
        ColorName = (piTarget.Count == 0 ? "Transparent" : piTarget[0].Name);
      }
      catch
      {
        //on exception use Transparent
        ColorName = "Transparent";
      }
      return ColorName;
    }
  }
}

namespace Ch05_Controls.Recipe5_6.AdvWorks
{
  public partial class Product
  {
    private ObservableCollection<string> _ColorList;
    //color literals defined in System.Windows.Media.Colors
    public ObservableCollection<string> ColorList
    {
      get
      {
        return new ObservableCollection<string> { 
        "Black", 
        "Blue", 
        "Brown", 
        "Cyan", 
        "DarkGray", 
        "Gray", 
        "Green", 
        "LightGray", 
        "Magenta", 
        "Orange", 
        "Purple", 
        "Red", 
        "Transparent", 
        "White", 
        "Yellow" };
      }
    }
  }
}
