﻿using System;
using System.Windows;

namespace Ch04_DataBinding.Recipe4_4
{
  public partial class App : Application
  {

    public App()
    {
      this.Startup += this.Application_Startup;
      this.Exit += this.Application_Exit;
      this.UnhandledException += this.Application_UnhandledException;

      InitializeComponent();
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
      // Load the main control
      this.RootVisual = new Page();
    }

    private void Application_Exit(object sender, EventArgs e)
    {

    }
    private void Application_UnhandledException(object sender,
      ApplicationUnhandledExceptionEventArgs e)
    {

    }
  }
}
