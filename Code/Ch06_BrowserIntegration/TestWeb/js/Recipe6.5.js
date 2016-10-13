///<reference name="MicrosoftAjax.js"
function getDataUsingJavaScriptAjaxAsync() {
    ///<summary>This method makes a web request to obtain an XML file</summary>
    ///<returns type="String" />
    var req = new Sys.Net.WebRequest();
    req.set_url("http://localhost:9090/xml/ApressBooks.xml");
    req.set_httpVerb("GET");
    req.set_userContext("user's context");
    req.add_completed(OnWebRequestCompleted);
    req.invoke();
}

function OnWebRequestCompleted(executor, eventArgs) {
    if (executor.get_responseAvailable()) {
        var xmlString = executor.get_responseData();
        //Call Managed Code method to pass back data - Covered in Recipe 6.6
        document.getElementById("Xaml1").Content.Page.SetBookXMLData(xmlString);
    }
}
