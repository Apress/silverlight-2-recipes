using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Ch04_DataBinding.Recipe4_4
{
  public class SpendingCollection : ObservableCollection<Spending>,
    INotifyPropertyChanged
  {
    public SpendingCollection()
    {
      this.Add(new Spending
      {
        ParentCollection = this,
        Item = "Utilities",
        Amount = 300
      });
      this.Add(new Spending
      {
        ParentCollection = this,
        Item = "Food",
        Amount = 350
      });
      this.Add(new Spending
      {
        ParentCollection = this,
        Item = "Clothing",
        Amount = 200
      });
      this.Add(new Spending
      {
        ParentCollection = this,
        Item = "Transportation",
        Amount = 75
      });
      this.Add(new Spending
      {
        ParentCollection = this,
        Item = "Mortgage",
        Amount = 3000
      });
      this.Add(new Spending
      {
        ParentCollection = this,
        Item = "Education",
        Amount = 500
      });
      this.Add(new Spending
      {
        ParentCollection = this,
        Item = "Entertainment",
        Amount = 125
      });
      this.Add(new Spending
      {
        ParentCollection = this,
        Item = "Loans",
        Amount = 750
      });
      this.Add(new Spending
      {
        ParentCollection = this,
        Item = "Medical",
        Amount = 80
      });
      this.Add(new Spending
      {
        ParentCollection = this,
        Item = "Miscellaneous",
        Amount = 175
      });
    }

    public double Total
    {
      get
      {
        return this.Sum(spending => spending.Amount);
      }
    }
  }

  public class Spending : INotifyPropertyChanged
  {

    public event PropertyChangedEventHandler PropertyChanged;
    internal void RaisePropertyChanged(PropertyChangedEventArgs e)
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, e);
      }
    }


    SpendingCollection _ParentCollection = null;

    public SpendingCollection ParentCollection
    {
      get { return _ParentCollection; }
      set { _ParentCollection = value; }
    }

    private string _Item;
    public string Item
    {
      get { return _Item; }
      set
      {
        string OldVal = _Item;
        if (OldVal != value)
        {
          _Item = value;
          RaisePropertyChanged(new PropertyChangedEventArgs("Item"));

        }
      }
    }

    private double _Amount;
    public double Amount
    {
      get { return _Amount; }
      set
      {
        double OldVal = _Amount;
        if (OldVal != value)
        {
          _Amount = value;


          foreach (Spending sp in ParentCollection)
            sp.RaisePropertyChanged(new PropertyChangedEventArgs("Amount"));

        }
      }
    }
  }
}