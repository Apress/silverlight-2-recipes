#pragma checksum "D:\RobcamerDocs\Book Silverlight Recipes\Code\Ch02_ProgrammingModel\Recipe2.6\Page.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A220CE86307AB299820A6793A6D98397"
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


namespace Ch02_ProgrammingModel.Recipe2_6 {
    
    
    public partial class Page : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBox settingTextData;
        
        internal System.Windows.Controls.StackPanel FormData;
        
        internal System.Windows.Controls.TextBox Field1;
        
        internal System.Windows.Controls.TextBox Field2;
        
        internal System.Windows.Controls.TextBox Field3;
        
        internal System.Windows.Controls.TextBox Field4;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Ch02_ProgrammingModel.Recipe2_6;component/Page.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.settingTextData = ((System.Windows.Controls.TextBox)(this.FindName("settingTextData")));
            this.FormData = ((System.Windows.Controls.StackPanel)(this.FindName("FormData")));
            this.Field1 = ((System.Windows.Controls.TextBox)(this.FindName("Field1")));
            this.Field2 = ((System.Windows.Controls.TextBox)(this.FindName("Field2")));
            this.Field3 = ((System.Windows.Controls.TextBox)(this.FindName("Field3")));
            this.Field4 = ((System.Windows.Controls.TextBox)(this.FindName("Field4")));
        }
    }
}