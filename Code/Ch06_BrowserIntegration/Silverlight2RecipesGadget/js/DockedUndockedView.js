function loadGadget() {
    System.Gadget.onDock = dockStateChanged;
    System.Gadget.onUndock = dockStateChanged;

    System.Gadget.Flyout.file = "FlyoutView.html";
    System.Gadget.settingsUI = "SettingsView.html";    
}

function dockStateChanged() {
    //change size depending on state
    if (System.Gadget.docked) {
        document.body.style.width = "130px";
        document.body.style.height = "200px";
        document.getElementById("XamlGadget").Content.GadgetApp.DockGadget(true);
    }
    else {
        document.body.style.width = "400px";
        document.body.style.height = "290px";
        document.getElementById("XamlGadget").Content.GadgetApp.DockGadget(false);
    }
}