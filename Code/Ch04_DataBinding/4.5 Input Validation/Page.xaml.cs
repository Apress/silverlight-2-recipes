using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Ch04_DataBinding.Recipe4_5
{
  public partial class Page : UserControl
  {
    public Page()
    {
      InitializeComponent();

      //initialize the employee collection with some sample data
      EmployeeCollection empColl = (EmployeeCollection)lbx_Employees.ItemsSource;

      empColl.Add(new Employee
      {
        FirstName = "Joe",
        LastName = "Duffin",
        PhoneNum = 2125551212,
        Address = new Address
        {
          Street = "2000 Mott Street",
          City = "New York",
          State = "NY",
          ZipCode = 10006
        }
      });

      empColl.Add(new Employee
      {
        FirstName = "Alex",
        LastName = "Bleeker",
        PhoneNum = 7185551212,
        Address = new Address
        {
          Street = "11000 Clover Street",
          City = "New York",
          State = "NY",
          ZipCode = 10007
        }
      });

      empColl.Add(new Employee
      {
        FirstName = "Nelly",
        LastName = "Myers",
        PhoneNum = 7325551212,
        Address = new Address
        {
          Street = "12000 Fay Road",
          City = "New York",
          State = "NY",
          ZipCode = 10016
        }
      });
    }

    private void btn_New_Click(object sender, RoutedEventArgs e)
    {
      //get the bound collection
      EmployeeCollection empColl = (EmployeeCollection)lbx_Employees.ItemsSource;
      //create and initialize a new Employee
      Employee newEmp = new Employee();
      newEmp.Address = new Address();
      //add it to the collection
      empColl.Add(newEmp);
      //set the current selection to the newly added employee. 
      //This will cause selection change to fire, and set the 
      //datacontext for the form appropriately
      lbx_Employees.SelectedItem = newEmp;

    }

    private void lbx_Employees_SelectionChanged(object sender,
      SelectionChangedEventArgs e)
    {
      //set the datacontext of the form to the selected Employee
      grid_EmployeeForm.DataContext = (Employee)lbx_Employees.SelectedItem;
      //show the form
      grid_EmployeeForm.Visibility = Visibility.Visible;
      grid_NewButton.Visibility = Visibility.Collapsed;
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
      //hide the form
      grid_EmployeeForm.Visibility = Visibility.Collapsed;
      grid_NewButton.Visibility = Visibility.Visible;
    }

    private void TextBox_BindingValidationError(object sender,
      ValidationErrorEventArgs e)
    {
      if (e.Action == ValidationErrorEventAction.Added)
      {

        if (sender is TextBox)
        {
          (sender as TextBox).Background = new SolidColorBrush(Colors.Red);
        }
        else if (sender is Grid)
        {
          TextBlock errorInd = (sender as Grid).FindName(
            (e.OriginalSource as TextBox).Name + "_ErrorIndicator") as TextBlock;
          errorInd.Visibility = Visibility.Visible;
          errorInd.Text = "*";
          ContentControl tooltipContent = errorInd.FindName(
            (e.OriginalSource as TextBox).Name + "_TooltipContent") as ContentControl;
          tooltipContent.Content = e.Error.Exception.Message;
        }
      }
      else
      {
        if (sender is TextBox)
        {
          (sender as TextBox).Background =
            new SolidColorBrush(Colors.Transparent);
        }
        else if (sender is Grid)
        {
          ((sender as Grid).FindName(
            (e.OriginalSource as TextBox).Name + "_ErrorIndicator") as TextBlock)
            .Visibility = Visibility.Collapsed; ;

        }
      }
    }
  }
}
