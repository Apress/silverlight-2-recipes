#pragma checksum "D:\RobcamerDocs\Book Silverlight Recipes\Code\Ch04_DataBinding\4.1 Data Binding\Page.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "AE1C6DF15E10DF374C35DBCC474ED70D"
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


namespace Ch04_DataBinding.Recipe4_1 {
    
    
    public partial class Page : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock tbxCompanyName;
        
        internal System.Windows.Controls.ListBox lbxEmployees;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Ch04_DataBinding.Recipe4_1;component/Page.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.tbxCompanyName = ((System.Windows.Controls.TextBlock)(this.FindName("tbxCompanyName")));
            this.lbxEmployees = ((System.Windows.Controls.ListBox)(this.FindName("lbxEmployees")));
        }
    }
}