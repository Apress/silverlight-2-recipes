function Xaml1DoReceive(data)
{
    document.getElementById("Xaml1").Content.Page.ReceiveData(data);
}

function Xaml1RequestData() {
    return document.getElementById("Xaml1").Content.Page.RequestData();
}

function Xaml2DoReceive(data)
{
    document.getElementById("Xaml2").Content.Page.ReceiveData(data);
}

function Xaml2RequestData(data) 
{
    return document.getElementById("Xaml2").Content.Page.RequestData();
}