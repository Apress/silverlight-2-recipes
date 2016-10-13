function createEllipse()
{
    var slControl = document.getElementById("SilverlightPlugInID");
    var e = 
        slControl.Content.CreateFromXaml(
            '<Ellipse Height="200" Width="200" Fill="Navy" />');
    var layoutRoot = slControl.content.FindName("LayoutRoot"); 
    layoutRoot.Children.Add(e);
}

function onSilverlightLoad(sender, args)
{
  var btn = document.getElementById("testButton");
  btn.disabled = false;
}