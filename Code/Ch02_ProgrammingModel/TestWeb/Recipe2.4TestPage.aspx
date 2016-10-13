<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls"
    TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

  protected void Page_Load(object sender, EventArgs e)
  {
    testButton.Attributes.Add("onclick", "createEllipse();");
  }
</script>

<html xmlns="http://www.w3.org/1999/xhtml" style="height:100%;">
<head runat="server">
    <title>Test Page For Recipe 2.4</title>
</head>
<body style="height:100%;margin:0;">
    <form id="form1" runat="server" style="height:100%;">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
          <Scripts>
            <asp:ScriptReference Path="js/Recipe2.4.js" />
          </Scripts>
        </asp:ScriptManager>
        <asp:Button ID="testButton" runat="server" Enabled="False" 
                    Text="Click Me!" UseSubmitBehavior="False" />
        <div  style="height:100%;">
            <asp:Silverlight ID="SilverlightPlugInID" runat="server" 
              Source="~/ClientBin/Ch02_ProgrammingModel.Recipe2_4.xap" 
              MinimumVersion="2.0.31005.0" Width="100%" Height="100%" 
              onpluginloaded="onSilverlightLoad" />
        </div>
    </form>
</body>
</html>