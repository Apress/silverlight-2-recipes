namespace Ch04_DataBinding.Recipe4_2
{
  public class Employee
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public long PhoneNum { get; set; }
    public string ImageUri
    {
      get
      {
        return "/" + FirstName + ".png";
      }
    }
    public Address Address { get; set; }
  }

  public class Address
  {
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public int ZipCode { get; set; }
  }
}

