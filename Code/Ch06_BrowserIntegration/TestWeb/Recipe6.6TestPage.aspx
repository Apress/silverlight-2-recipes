<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Assembly="System.Web.Silverlight" 
  Namespace="System.Web.UI.SilverlightControls"
  TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="height: 100%;">
<head runat="server">
  <title>Test Page for Recipe 6.6</title>
</head>
<body style="height: 100%; margin: 0;">
  <form id="form1" runat="server" style="height: 100%;">
  <asp:ScriptManager ID="ScriptManager1" runat="server">
    <Scripts>
      <asp:ScriptReference Path="/js/Recipe6.6.js" />
    </Scripts>
  </asp:ScriptManager>
  <div>
    <br />
    <asp:Label ID="Label1" runat="server" Text="Color from Silverlight:" />
    <asp:TextBox ID="Text1" runat="server"></asp:TextBox><br />
    <input id="Button1" type="button" value="button" />
  </div>
  <div id="silverlightControlHost" style="height: 100%;">
    <asp:Silverlight ID="Xaml1" runat="server" Width="100%" Height="100%" 
     Source="~/ClientBin/Ch06_BrowserIntegration.Recipe6_6.xap"
      MinimumVersion="2.0.31005.0" HtmlAccess="Enabled" OnPluginLoaded=
      "onSilverlightLoaded" />
  </div>
  </form>
</body>
</html>
