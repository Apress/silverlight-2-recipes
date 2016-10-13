<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Assembly="System.Web.Silverlight" 
 Namespace="System.Web.UI.SilverlightControls"
    TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
 "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" style="height:100%;">
<head runat="server">
    <title>Test Page for Recipe 6.5</title>
</head>
<body>
    <form id="form1" runat="server" >
        <asp:ScriptManager ID="ScriptManager1" runat="server">
          <Scripts>
            <asp:ScriptReference Path="/js/Recipe6.5.js" />
          </Scripts>
        </asp:ScriptManager>
        <span>Silverlight 2 has technology for interacting between the Silverlight 2 managed code and the hosting browser’s HTML DOM called the HTML Bridge.  The HTML Bridge enables developers to expose entire managed code types or just member functions of types for scripting in JavaScript.  
Developers can enable or disable HTML Bridge functionality by setting the enableHtmlAccess parameter on the Silverlight browser plug-in to a Boolean value, with the default as false or disabled.</span>
        <div id="silverlightControlHost" class="PositionControl" >
            <asp:Silverlight ID="Xaml1" runat="server" HtmlAccess="Enabled"
             Source="~/ClientBin/Ch06_BrowserIntegration.Recipe6_5.xap" 
             MinimumVersion="2.0.31005.0" Width="100%" Height="100%" />
        </div>
    </form>
</body>
</html>