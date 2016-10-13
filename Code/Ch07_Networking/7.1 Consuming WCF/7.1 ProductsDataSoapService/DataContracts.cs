using System.Runtime.Serialization;

namespace Ch07_Networking.Recipe7_1.ProductsDataSoapService
{
  [DataContract]
  public partial class ProductHeader
  {
    private ushort? productIdField;
    private decimal? listPriceField;
    private string nameField;
    private string sellEndDateField;
    private string sellStartDateField;

    [DataMember]
    public ushort? ProductId
    {
      get { return this.productIdField; }
      set { this.productIdField = value; }
    }
    [DataMember]
    public decimal? ListPrice
    {
      get { return this.listPriceField; }
      set { this.listPriceField = value; }
    }
    [DataMember]
    public string Name
    {
      get { return this.nameField; }
      set { this.nameField = value; }
    }
    [DataMember]
    public string SellEndDate
    {
      get { return this.sellEndDateField; }
      set { this.sellEndDateField = value; }
    }
    [DataMember]
    public string SellStartDate
    {
      get { return this.sellStartDateField; }
      set { this.sellStartDateField = value; }
    }
  }

  [DataContract]
  public partial class ProductDetail
  {

    private ushort? productIdField;
    private string classField;
    private string colorField;
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

    [DataMember]
    public ushort? ProductId
    {
      get { return this.productIdField; }
      set { this.productIdField = value; }
    }
    [DataMember]
    public string Class
    {
      get { return this.classField; }
      set { this.classField = value; }
    }
    [DataMember]
    public string Color
    {
      get { return this.colorField; }
      set { this.colorField = value; }
    }
    [DataMember]
    public byte? DaysToManufacture
    {
      get { return this.daysToManufactureField; }
      set { this.daysToManufactureField = value; }
    }
    [DataMember]
    public string DiscontinuedDate
    {
      get { return this.discontinuedDateField; }
      set { this.discontinuedDateField = value; }
    }
    [DataMember]
    public string FinishedGoodsFlag
    {
      get { return this.finishedGoodsFlagField; }
      set { this.finishedGoodsFlagField = value; }
    }
    [DataMember]
    public string MakeFlag
    {
      get { return this.makeFlagField; }
      set { this.makeFlagField = value; }
    }
    [DataMember]
    public string ProductLine
    {
      get { return this.productLineField; }
      set { this.productLineField = value; }
    }
    [DataMember]
    public string ProductNumber
    {
      get { return this.productNumberField; }
      set { this.productNumberField = value; }
    }
    [DataMember]
    public ushort? ReorderPoint
    {
      get { return this.reorderPointField; }
      set { this.reorderPointField = value; }
    }
    [DataMember]
    public ushort? SafetyStockLevel
    {
      get { return this.safetyStockLevelField; }
      set { this.safetyStockLevelField = value; }
    }
    [DataMember]
    public string Size
    {
      get { return this.sizeField; }
      set { this.sizeField = value; }
    }
    [DataMember]
    public decimal? StandardCost
    {
      get { return this.standardCostField; }
      set { this.standardCostField = value; }
    }
    [DataMember]
    public string Style
    {
      get { return this.styleField; }
      set { this.styleField = value; }
    }
    [DataMember]
    public string Weight
    {
      get { return this.weightField; }
      set { this.weightField = value; }
    }
  }
}


