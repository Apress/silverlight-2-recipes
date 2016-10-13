using System.Collections.Generic;

namespace Ch02_ProgrammingModel.Recipe2_1
{
  public class Organization
  {
    private List<Person> _people;
    public List<Person> People 
    {
      get
      {
        if (null == _people)
          return Populate();
        else
          return _people;
      }
    }

    private List<Person> Populate()
    {
      _people = new List<Person>
         { //C# 3.0 Object Initializers 
          new Person {FirstName="John",LastName="Smith", Age=20},
          new Person{FirstName="Sean",LastName="Jones", Age=25},
          new Person{FirstName="Kevin",LastName="Smith", Age=30}
         };
      return _people;
    }
  }

   public class Person
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
  }
}
