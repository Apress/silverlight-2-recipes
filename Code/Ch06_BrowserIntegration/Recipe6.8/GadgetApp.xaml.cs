using System;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;

namespace Ch06_BrowserIntegration.Recipe6_8
{
  public partial class GadgetApp : Application
  {
    //Use this as root control so that the user controls
    //can be switched from docked or undocked
    private Grid _rootControl;
    //Hold references to Docked and Undocked Views
    private DockedView _dockedView;
    private UndockedView _unDockedView;

    public GadgetApp()
    {
      this.Startup += this.Application_Startup;
      this.Exit += this.Application_Exit;
      this.UnhandledException += this.Application_UnhandledException;

      InitializeComponent();
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
      //Get passed parameter for partner control
      string initParams =
         HtmlPage.Plugin.GetProperty("initParams").ToString();
      string[] paramsArray = initParams.Split(';');
      string[] KeyValue = paramsArray[0].Split('=');
      switch (KeyValue[1])
      {                 //For DockedUndocked, we set Grid as root
        //so that we can switch user controls later
        //at runtime for docked and undocked states.
        case "DockedUndocked": _rootControl = new Grid();
          this.RootVisual = _rootControl;
          _dockedView = new DockedView();
          _rootControl.Children.Clear();
          _rootControl.Children.Add(_dockedView); break;
        case "Flyout": this.RootVisual = new FlyoutView(); break;
        case "Settings": this.RootVisual = new SettingsView(); break;
      }
      //Make GadgetApp instance avaialble so that script 
      //can call DockGadget method from JavaScript
      HtmlPage.RegisterScriptableObject("GadgetApp", this);
    }

    private void Application_Exit(object sender, EventArgs e)
    {

    }

    private void Application_UnhandledException(object sender,
    ApplicationUnhandledExceptionEventArgs e)
    {
      if (!System.Diagnostics.Debugger.IsAttached)
      {
        e.Handled = true;
        Deployment.Current.Dispatcher.BeginInvoke(
          delegate { ReportErrorToDOM(e); });
      }
    }

    private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
    {
      try
      {
        string errorMsg = e.ExceptionObject.Message +
                              e.ExceptionObject.StackTrace;
        errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

        System.Windows.Browser.HtmlPage.Window.Eval(
        "throw new Error(\"Unhandled Error in Silverlight 2 Application " +
        errorMsg + "\");");
      }
      catch (Exception)
      {
      }
    }

    [ScriptableMember]
    public void DockGadget(Boolean state)
    {
      switch (state)
      {
        case true: _rootControl.Children.Clear();
          _rootControl.Children.Add(_dockedView); break;
        //First time undocking, create undocked view
        case false: if (null == _unDockedView)
            _unDockedView = new UndockedView();
          //Switch to undocked view when gadget undocked
          _rootControl.Children.Clear();
          _rootControl.Children.Add(_unDockedView); break;
      }
    }
  }
}
