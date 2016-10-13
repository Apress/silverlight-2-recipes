function onSilverlightLoaded(sender, args) {
    //Get Color from Page.GetMyColor
    colorRGB = document.getElementById("Xaml1").Content.Page.GetMyBackgroundColor();
    //Obtain a reference to the html input textbox
    txt1 = document.getElementById("Text1");
    txt1.value = colorRGB;
    txt1.style.backgroundColor = colorRGB;
    //Set background of entire page to same color
    document.bgColor = colorRGB;
}