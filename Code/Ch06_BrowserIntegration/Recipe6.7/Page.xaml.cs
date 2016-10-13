using System;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;

namespace Ch06_BrowserIntegration.Recipe6_7
{
  public partial class Page : UserControl
  {
    private string _partnerControlID;
    public Page()
    {
      InitializeComponent();
      Loaded += new RoutedEventHandler(Page_Loaded);
    }

    void Page_Loaded(object sender, RoutedEventArgs e)
    {
      //Make scriptable type available to JavaScript
      //Enable call to ReceiveData from JavaScript
      HtmlPage.RegisterScriptableObject("Page", this);
      //Get the ID of the Silverlight Plug-in Control Parent
      string parentId = HtmlPage.Plugin.Parent.Id;
      //Get the ID of the Silverlight Plug-in Control
      ControlID.Text = HtmlPage.Plugin.Id;
      //Obtain a reference to the DOM
      HtmlDocument doc = HtmlPage.Document;
      //Set height and width on parent div so 
      //that the control displays properly
      doc.GetElementById(parentId).
              SetStyleAttribute("width", this.Width.ToString());
      doc.GetElementById(parentId).
                   SetStyleAttribute("height", this.Height.ToString());
      //Get passed parameter for partner control
      string initParams = 
         HtmlPage.Plugin.GetProperty("initParams").ToString();
      string[] paramsArray = initParams.Split(';');
      string[] KeyValue = paramsArray[0].Split('=');
      _partnerControlID = KeyValue[1];
    }

    [ScriptableMember]
    public void ReceiveData(object receivedData)
    {
      ReceivedData.Text = (string)receivedData;
    }

    [ScriptableMember]
    public string RequestData()
    {
      if (DateTime.Now.Millisecond < 500)
        return "RequestedData" + DateTime.Now.ToString();
      else
        return null;
    }

    private void SendDataButton_Click(object sender, RoutedEventArgs e)
    {
      object[] args = new object[1];
      args[0] = DataToSend.Text ;
      HtmlPage.Window.Invoke(_partnerControlID + "DoReceive",args);
    }

    private void RequestDataButton_Click(object sender, RoutedEventArgs e)
    {
      string str = (string)HtmlPage.Window.
        Invoke(_partnerControlID + "RequestData");
      if (str != null)
        RequestedData.Text = str;
      else
        RequestedData.Text = "no data";
    }
  }
}
