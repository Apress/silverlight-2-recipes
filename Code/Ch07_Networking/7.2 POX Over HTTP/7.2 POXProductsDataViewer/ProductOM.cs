using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.ComponentModel;

namespace Ch07_Networking.Recipe7_2.POXProductsDataViewer
{

  public partial class ProductHeader
  {
    private ushort? productIdField;
    private decimal? listPriceField;

    //more fields here - omitted for brevity
    private string nameField;
    private string sellEndDateField;
    private string sellStartDateField;

    //dirty flag
    public bool Dirty { get; set; }

    public ushort? ProductId
    {
      get { return this.productIdField; }
      set { this.productIdField = value; }
    }


    public decimal? ListPrice
    {
      get { return this.listPriceField; }
      set { this.listPriceField = value; }
    }

    //more properties here - omitted for brevity

    public string Name
    {
      get
      {
        return this.nameField;
      }
      set
      {
        this.nameField = value;
      }
    }

    /// <remarks/>

    public string SellEndDate
    {
      get
      {
        return this.sellEndDateField;
      }
      set
      {
        this.sellEndDateField = value;
      }
    }

    /// <remarks/>

    public string SellStartDate
    {
      get
      {
        return this.sellStartDateField;
      }
      set
      {
        this.sellStartDateField = value;
      }
    }
  }

  public partial class ProductDetail
  {

    private ushort? productIdField;
    private string classField;
    private string colorField;
    //more fields here - omitted for brevity
    private byte? daysToManufactureField;
    private string discontinuedDateField;
    private string finishedGoodsFlagField;
    private string makeFlagField;
    private string productLineField;
    private string productNumberField;
    private ushort? reorderPointField;
    private ushort? safetyStockLevelField;
    private string sizeField;
    private decimal? standardCostField;
    private string styleField;
    private string weightField;


    public ushort? ProductId
    {
      get { return this.productIdField; }
      set { this.productIdField = value; }
    }



    public string Class
    {
      get { return this.classField; }
      set { this.classField = value; }
    }

    public string Color
    {
      get { return this.colorField; }
      set { this.colorField = value; }
    }
    //more proerties here - omitted for brevity

    public byte? DaysToManufacture
    {
      get
      {
        return this.daysToManufactureField;
      }
      set
      {
        this.daysToManufactureField = value;
      }
    }

    public string DiscontinuedDate
    {
      get
      {
        return this.discontinuedDateField;
      }
      set
      {
        this.discontinuedDateField = value;
      }
    }

    public string FinishedGoodsFlag
    {
      get
      {
        return this.finishedGoodsFlagField;
      }
      set
      {
        this.finishedGoodsFlagField = value;
      }
    }

    public string MakeFlag
    {
      get
      {
        return this.makeFlagField;
      }
      set
      {
        this.makeFlagField = value;
      }
    }

    public string ProductLine
    {
      get
      {
        return this.productLineField;
      }
      set
      {
        this.productLineField = value;
      }
    }

    public string ProductNumber
    {
      get
      {
        return this.productNumberField;
      }
      set
      {
        this.productNumberField = value;
      }
    }

    public ushort? ReorderPoint
    {
      get
      {
        return this.reorderPointField;
      }
      set
      {
        this.reorderPointField = value;
      }
    }

    public ushort? SafetyStockLevel
    {
      get
      {
        return this.safetyStockLevelField;
      }
      set
      {
        this.safetyStockLevelField = value;
      }
    }

    public string Size
    {
      get
      {
        return this.sizeField;
      }
      set
      {
        this.sizeField = value;
      }
    }



    public decimal? StandardCost
    {
      get
      {
        return this.standardCostField;
      }
      set
      {
        this.standardCostField = value;
      }
    }

    public string Style
    {
      get
      {
        return this.styleField;
      }
      set
      {
        this.styleField = value;
      }
    }

    public string Weight
    {
      get
      {
        return this.weightField;
      }
      set
      {
        this.weightField = value;
      }
    }
  }
}
