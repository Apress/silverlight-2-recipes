<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls"
    TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" style="height:100%;">
<head runat="server">
    <title>Test Page for Recipe 6.2</title>
    <script type="text/javascript">
    function onSilverlightError(sender, args) {
    
        var appSource = "";
        if (sender != null && sender != 0) {
            appSource = sender.get_source();
        } 
        var errorType = args.ErrorType;
        var iErrorCode = args.ErrorCode;
        
        var errMsg = "Unhandled Error in Silverlight 2 Application " +  appSource + "\n" ;

        errMsg += "Code: "+ iErrorCode + "    \n";
        errMsg += "Category: " + errorType + "       \n";
        errMsg += "Message: " + args.ErrorMessage + "     \n";

        if (errorType == "ParserError")
        {
            errMsg += "File: " + args.xamlFile + "     \n";
            errMsg += "Line: " + args.lineNumber + "     \n";
            errMsg += "Position: " + args.charPosition + "     \n";
        }
        else if (errorType == "RuntimeError")
        {           
            if (args.lineNumber != 0)
            {
                errMsg += "Line: " + args.lineNumber + "     \n";
                errMsg += "Position: " +  args.charPosition + "     \n";
            }
            errMsg += "MethodName: " + args.methodName + "     \n";
        }

        throw new Error(errMsg);
    }
</script>
</head>
<body style="height:100%;margin:0;">
    <form id="form1" runat="server" style="height:100%;">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div  style="height:100%;">
            <asp:Silverlight ID="Xaml1" runat="server" Source="~/ClientBin/Ch06_BrowserIntegration.Recipe6_2.xap" 
              MinimumVersion="2.0.31005.0" Width="100%" Height="100%" 
              onpluginerror="onSilverlightError" />
        </div>
    </form>
</body>
</html>