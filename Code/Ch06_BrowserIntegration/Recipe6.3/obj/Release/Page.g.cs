#pragma checksum "C:\Users\robcamer.NORTHAMERICA\Documents\Book Silverlight Recipes\Code\Ch06_BrowserIntegration\Recipe6.3\Page.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F4604D011AC8EB5EDFAC165FB96F72BE"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3053
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Ch06_BrowserIntegration.Recipe6_3 {
    
    
    public partial class Page : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBox TextFirstName;
        
        internal System.Windows.Controls.TextBox TextLastName;
        
        internal System.Windows.Controls.TextBox TextFavoriteColor;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Ch06_BrowserIntegration.Recipe6_3;component/Page.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TextFirstName = ((System.Windows.Controls.TextBox)(this.FindName("TextFirstName")));
            this.TextLastName = ((System.Windows.Controls.TextBox)(this.FindName("TextLastName")));
            this.TextFavoriteColor = ((System.Windows.Controls.TextBox)(this.FindName("TextFavoriteColor")));
        }
    }
}
