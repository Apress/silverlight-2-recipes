#pragma checksum "C:\Users\robcamer.NORTHAMERICA\Documents\Book Silverlight Recipes\Code\Ch03_DesigningUX\Recipe3.12\Copy of Page.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D56A77EA02050908D90BBF87364F1FB2"
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
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Hosting;
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


namespace Ch03_DesigningUX.Recipe3_12 {
    
    
    public partial class Page : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.InkPresenter InkEssentials;
        
        internal System.Windows.Controls.Image Picture;
        
        internal System.Windows.Controls.InkPresenter InkPicture;
        
        internal System.Windows.Controls.Button btnResetInk;
        
        internal System.Windows.Controls.Button btnSaveInk;
        
        internal System.Windows.Controls.Button btnLoadInk;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Ch03_DesigningUX.Recipe3_12;component/Copy%20of%20Page.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.InkEssentials = ((System.Windows.Controls.InkPresenter)(this.FindName("InkEssentials")));
            this.Picture = ((System.Windows.Controls.Image)(this.FindName("Picture")));
            this.InkPicture = ((System.Windows.Controls.InkPresenter)(this.FindName("InkPicture")));
            this.btnResetInk = ((System.Windows.Controls.Button)(this.FindName("btnResetInk")));
            this.btnSaveInk = ((System.Windows.Controls.Button)(this.FindName("btnSaveInk")));
            this.btnLoadInk = ((System.Windows.Controls.Button)(this.FindName("btnLoadInk")));
        }
    }
}
