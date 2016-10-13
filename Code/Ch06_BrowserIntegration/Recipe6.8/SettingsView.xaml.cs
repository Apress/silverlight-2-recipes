using System;
using System.Windows.Browser;
using System.Windows.Controls;

namespace Ch06_BrowserIntegration.Recipe6_8
{
  public partial class SettingsView : UserControl
  {
    public SettingsView()
    {
      InitializeComponent();
      HtmlPage.RegisterScriptableObject("SettingsView", this);
      //Load settings for gadget
      LoadGadgetSettings();
    }

    private void LoadGadgetSettings()
    {
      try
      {
        //Textbox control name is also Setting name in this example.
        Setting1.Text = (HtmlPage.Window.Eval("System.Gadget.Settings") as ScriptObject).Invoke("read", Setting1.Name) as string;
        Setting2.Text = (HtmlPage.Window.Eval("System.Gadget.Settings") as ScriptObject).Invoke("read", Setting2.Name) as string;
      }
      catch (Exception err)
      {
        //do something with exception here
      }
    }

    [ScriptableMember]
    public void SaveGadgetSettings()
    {
      try
      {
        (HtmlPage.Window.Eval("System.Gadget.Settings") as ScriptObject).Invoke("write", Setting1.Name, Setting1.Text);
        (HtmlPage.Window.Eval("System.Gadget.Settings") as ScriptObject).Invoke("write", Setting2.Name, Setting2.Text);
      }
      catch (Exception err)
      {
        //do something with exception here
      }
    }
  }
}
