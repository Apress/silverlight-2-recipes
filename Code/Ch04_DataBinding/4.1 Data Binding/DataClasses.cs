using System.Collections.Generic;

namespace Ch04_DataBinding.Recipe4_1
{
  public class Employee
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public long PhoneNum { get; set; }
  }
  public class Company
  {
    public string Name { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public int ZipCode { get; set; }
    public List<Employee> Employees { get; set; }

    public Company()
    {
      this.Name = "Woodgrove Bank";
      this.Street = "555 Wall Street";
      this.City = "New York";
      this.State = "NY";
      this.ZipCode = 10005;

      this.Employees = new List<Employee>();

      this.Employees.Add(
          new Employee
          {
            FirstName = "Joe",
            LastName = "Duffin",
            PhoneNum = 2125551212
          });
      this.Employees.Add(
          new Employee
          {
            FirstName = "Alex",
            LastName = "Bleeker",
            PhoneNum = 7185551212
          });

      //rest of the initialization code omitted for brevity
      this.Employees.Add(new Employee
      {
        FirstName = "Nelly",
        LastName = "Myers",
        PhoneNum = 7325551212
      });
      this.Employees.Add(new Employee
      {
        FirstName = "Marcus",
        LastName = "Bernard",
        PhoneNum = 7325551414
      });
      this.Employees.Add(new Employee
      {
        FirstName = "Juliette",
        LastName = "Bernard",
        PhoneNum = 7325551414
      });
      this.Employees.Add(new Employee
      {
        FirstName = "Cory",
        LastName = "Winston",
        PhoneNum = 9085551414
      });
      this.Employees.Add(new Employee
      {
        FirstName = "Randall",
        LastName = "Valetta",
        PhoneNum = 2015551414
      });

      this.Employees.Add(new Employee
      {
        FirstName = "Maurice",
        LastName = "Dutronc",
        PhoneNum = 3635551414
      });
      this.Employees.Add(new Employee
      {
        FirstName = "Nathan",
        LastName = "Adams",
        PhoneNum = 3635551414
      });
      this.Employees.Add(new Employee
      {
        FirstName = "Harold",
        LastName = "Anthony",
        PhoneNum = 3745551414
      });
      this.Employees.Add(new Employee
      {
        FirstName = "Paul",
        LastName = "Gomez",
        PhoneNum = 3415551414
      });

      this.Employees.Add(new Employee
      {
        FirstName = "Martha",
        LastName = "Willard",
        PhoneNum = 4795551414
      });
      this.Employees.Add(new Employee
      {
        FirstName = "Barry",
        LastName = "Driver",
        PhoneNum = 4165551414
      });
      this.Employees.Add(new Employee
      {
        FirstName = "Peter",
        LastName = "Martinson",
        PhoneNum = 4165551414
      });
      this.Employees.Add(new Employee
      {
        FirstName = "Mike",
        LastName = "Dempsey",
        PhoneNum = 4165551656
      });
    }
  }
}