using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Ch04_DataBinding.Recipe4_3
{
  public class Employee : INotifyPropertyChanged
  {

    public event PropertyChangedEventHandler PropertyChanged;
    private void RaisePropertyChanged(PropertyChangedEventArgs e)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, e);
    }

    public Employee()
    {
    }

    private string _FirstName;
    public string FirstName
    {
      get { return _FirstName; }
      set
      {
        string OldVal = _FirstName;
        if (OldVal != value)
        {
          _FirstName = value;
          RaisePropertyChanged(new PropertyChangedEventArgs("FirstName"));
        }
      }
    }

    private string _LastName;
    public string LastName
    {
      get { return _LastName; }
      set
      {
        string OldVal = _LastName;
        if (OldVal != value)
        {
          _LastName = value;
          RaisePropertyChanged(new PropertyChangedEventArgs("LastName"));
        }
      }
    }

    private long _PhoneNum;
    public long PhoneNum
    {
      get { return _PhoneNum; }
      set
      {
        long OldVal = _PhoneNum;
        if (OldVal != value)
        {
          _PhoneNum = value;
          RaisePropertyChanged(new PropertyChangedEventArgs("PhoneNum"));
        }
      }
    }

    private Address _Address;
    public Address Address
    {
      get { return _Address; }
      set
      {
        Address OldVal = _Address;
        if (OldVal != value)
        {
          _Address = value;
          RaisePropertyChanged(new PropertyChangedEventArgs("Address"));
        }
      }
    }
  }

  public class Address : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;
    private void RaisePropertyChanged(PropertyChangedEventArgs e)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, e);
    }

    private string _Street;
    public string Street
    {
      get { return _Street; }
      set
      {
        string OldVal = _Street;
        if (OldVal != value)
        {
          _Street = value;
          RaisePropertyChanged(new PropertyChangedEventArgs("Street"));
        }
      }
    }

    private string _City;
    public string City
    {
      get { return _City; }
      set
      {
        string OldVal = _City;
        if (OldVal != value)
        {
          _City = value;
          RaisePropertyChanged(new PropertyChangedEventArgs("City"));
        }
      }
    }

    private string _State;
    public string State
    {
      get { return _State; }
      set
      {
        string OldVal = _State;
        if (OldVal != value)
        {
          _State = value;
          RaisePropertyChanged(new PropertyChangedEventArgs("State"));
        }
      }
    }

    private int _ZipCode;
    public int ZipCode
    {
      get { return _ZipCode; }
      set
      {
        int OldVal = _ZipCode;
        if (OldVal != value)
        {
          _ZipCode = value;
          RaisePropertyChanged(new PropertyChangedEventArgs("ZipCode"));
        }
      }
    }
  }

  public class EmployeeCollection : ICollection<Employee>,
    IList<Employee>,
    INotifyCollectionChanged
  {
    private List<Employee> _internalList;

    public EmployeeCollection()
    {
      _internalList = new List<Employee>();
    }

    public event NotifyCollectionChangedEventHandler CollectionChanged;

    private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      if (CollectionChanged != null)
      {
        CollectionChanged(this, e);
      }
    }
    //Methods/Properties that would possibly change the collection and its content
    //need to raise the CollectionChanged event
    public void Add(Employee item)
    {
      _internalList.Add(item);
      RaiseCollectionChanged(
        new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,
          item, _internalList.Count - 1));
    }
    public void Clear()
    {
      _internalList.Clear();
      RaiseCollectionChanged(
        new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
    public bool Remove(Employee item)
    {
      int idx = _internalList.IndexOf(item);
      bool RetVal = _internalList.Remove(item);
      if (RetVal)
        RaiseCollectionChanged(
          new NotifyCollectionChangedEventArgs(
            NotifyCollectionChangedAction.Remove, item, idx));
      return RetVal;
    }
    public void RemoveAt(int index)
    {
      Employee item = null;
      if (index < _internalList.Count)
        item = _internalList[index];
      _internalList.RemoveAt(index);
      if (index < _internalList.Count)
        RaiseCollectionChanged(
          new NotifyCollectionChangedEventArgs(
            NotifyCollectionChangedAction.Remove, item, index));

    }
    public void Insert(int index, Employee item)
    {
      _internalList.Insert(index, item);
      RaiseCollectionChanged(
        new NotifyCollectionChangedEventArgs(
          NotifyCollectionChangedAction.Add, item, index));
    }
    public Employee this[int index]
    {
      get { return _internalList[index]; }
      set
      {
        _internalList[index] = value;
        RaiseCollectionChanged(
          new NotifyCollectionChangedEventArgs(
            NotifyCollectionChangedAction.Replace, value, index));

      }
    }

    public bool Contains(Employee item)
    {
      return _internalList.Contains(item);
    }
    public void CopyTo(Employee[] array, int arrayIndex)
    {
      _internalList.CopyTo(array, arrayIndex);
    }
    public int Count
    {
      get { return _internalList.Count; }
    }
    public bool IsReadOnly
    {
      get { return ((IList<Employee>)_internalList).IsReadOnly; }
    }
    public IEnumerator<Employee> GetEnumerator()
    {
      return _internalList.GetEnumerator();
    }
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return (System.Collections.IEnumerator)_internalList.GetEnumerator();
    }
    public int IndexOf(Employee item)
    {
      return _internalList.IndexOf(item);
    }
  }

}

