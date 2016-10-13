using System.Collections.Generic;
using System.Windows.Controls;

namespace Ch04_DataBinding.Recipe4_2
{
  public partial class Page : UserControl
  {
    public Page()
    {
      InitializeComponent();

      List<Employee> EmployeeList = new List<Employee>();

      EmployeeList.Add(new Employee
          {
            FirstName = "Joe",
            LastName = "Duffin",
            PhoneNum = 2125551212,
            Address = new Address { Street = "2000 Mott Street", 
              City = "New York", State = "NY", ZipCode = 10006 }
          });

      EmployeeList.Add(new Employee
          {
            FirstName = "Alex",
            LastName = "Bleeker",
            PhoneNum = 7185551212,
            Address = new Address { Street = "11000 Clover Street", 
              City = "New York", State = "NY", ZipCode = 10007 }
          });

      EmployeeList.Add(new Employee
          {
            FirstName = "Nelly",
            LastName = "Myers",
            PhoneNum = 7325551212,
            Address = new Address { Street = "12000 Fay Road", 
              City = "New York", State = "NY", ZipCode = 10016 }
          });

      cntctrlEmployee.Content = EmployeeList[0];
      itmctrlEmployees.ItemsSource = EmployeeList;
    }
  }
}
