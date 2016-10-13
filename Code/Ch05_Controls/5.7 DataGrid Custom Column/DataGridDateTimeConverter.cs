using System;
using System.ComponentModel;
using System.Globalization;

namespace Ch05_Controls.Recipe5_7
{
  public class DataGridDateTimeConverter : TypeConverter
  {
    public override bool CanConvertFrom(ITypeDescriptorContext context,
      Type sourceType)
    {
      return (typeof(string) == sourceType);
    }
    public override bool CanConvertTo(ITypeDescriptorContext context,
      Type destinationType)
    {
      return (typeof(DateTime) == destinationType);
    }
    public override object ConvertFrom(ITypeDescriptorContext context,
      CultureInfo culture, object value)
    {
      DateTime target;
      target = DateTime.ParseExact(value as string, "d",
        CultureInfo.CurrentUICulture);
      return target;
    } 
  }
}
