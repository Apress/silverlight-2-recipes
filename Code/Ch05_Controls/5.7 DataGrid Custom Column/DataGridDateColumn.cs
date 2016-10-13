using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Ch05_Controls.Recipe5_7
{
  public class DataGridDateColumn : DataGridBoundColumn
  {
    [TypeConverter(typeof(DataGridDateTimeConverter))]
    public DateTime DisplayDateStart { get; set; }

    public Binding DisplayDateEndBinding { get; set; }

    protected override void CancelCellEdit(FrameworkElement editingElement,
      object uneditedValue)
    {
      //get the DatePicker
      DatePicker datepicker = (editingElement as Border).Child as DatePicker;
      if (datepicker != null)
      {
        //rest the relevant properties on the DatePicker to the original value 
        //to reflect cancellation and undo changes made
        datepicker.SelectedDate = (DateTime)uneditedValue;
        datepicker.DisplayDate = (DateTime)uneditedValue;
      }
    }
    //edit mode
    protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
    {
      //create an outside Border
      Border border = new Border();
      border.BorderBrush = new SolidColorBrush(Colors.Blue);
      border.BorderThickness = new Thickness(1);
      border.HorizontalAlignment = HorizontalAlignment.Stretch;
      border.VerticalAlignment = VerticalAlignment.Stretch;
      //create the new DatePicker
      DatePicker datepicker = new DatePicker();
      //bind the DisplayDate to the bound data item
      datepicker.SetBinding(DatePicker.DisplayDateProperty,
        base.Binding);
      //bind the SelectedDate to the same
      datepicker.SetBinding(DatePicker.SelectedDateProperty,
        base.Binding);
      //bind the DisplayDate range
      //start value is provided directly through a property
      datepicker.DisplayDateStart = this.DisplayDateStart;
      //end value is another binding allowing developer to bind 
      datepicker.SetBinding(DatePicker.DisplayDateEndProperty,
        this.DisplayDateEndBinding);
      border.Child = datepicker;
      //return the new control
      return border;
    }

    //view mode
    protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
    {
      //create a TextBlock
      TextBlock block = new TextBlock();
      //bind the displayed text to the bound data item
      block.SetBinding(TextBlock.TextProperty, base.Binding);
      //return the new control
      return block;
    }
    protected override object PrepareCellForEdit(FrameworkElement editingElement,
      RoutedEventArgs editingEventArgs)
    {
      //get the datepicker
      DatePicker datepicker = (editingElement as Border).Child as DatePicker;
      //return the initially displayed date, which is the 
      //same as the unchanged data item value
      return datepicker.DisplayDate;
    }
  }
}