<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls"
    TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Test Page for Recipe 6.7</title>
</head>
<body >
    <form id="form1" runat="server" >
        <asp:ScriptManager ID="ScriptManager1" runat="server" >
          <Scripts>
          <asp:ScriptReference Path="/js/Recipe6.7.js" />
          </Scripts>
        </asp:ScriptManager>
        <div  id="silverlightControlHost1"style="margin:20px" >
            <asp:Silverlight ID="Xaml1" runat="server" width="100%" Height="50%"
            Source="~/ClientBin/Ch06_BrowserIntegration.Recipe6_7.xap" 
            MinimumVersion="2.0.31005.0"
              InitParameters="PartnerControl=Xaml2" HtmlAccess="Enabled"/>
        </div>
        <div  id="silverlightControlHost2" style="margin:20px" >
            <asp:Silverlight ID="Xaml2" runat="server" width="100%" Height="50%" 
              Source="~/ClientBin/Ch06_BrowserIntegration.Recipe6_7.xap" 
              MinimumVersion="2.0.31005.0" 
              InitParameters="PartnerControl=Xaml1" HtmlAccess="Enabled"/>
        </div>
    </form>
</body>
</html>