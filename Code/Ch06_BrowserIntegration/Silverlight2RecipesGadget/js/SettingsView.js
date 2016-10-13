function loadSettingsView()
{   
    System.Gadget.onSettingsClosed = settingsViewClosed;
    System.Gadget.onSettingsClosing = settingsViewClosing;
}

function settingsViewClosed(event) {
}

function settingsViewClosing(event) {
    document.getElementById("XamlGadget").Content.SettingsView.SaveGadgetSettings();
    if (event.closeAction == event.Action.commit) 
    {
        //call Method to save settings in the SettingsView UserControl
        document.getElementById("XamlGadget").Content.SettingsView.SaveGadgetSettings();
    }
    // Allow the Settings dialog to close.
    event.cancel = false;
}