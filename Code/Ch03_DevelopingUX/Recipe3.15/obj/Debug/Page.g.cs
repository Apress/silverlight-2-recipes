#pragma checksum "D:\RobcamerDocs\Book Silverlight Recipes\Code\Ch03_DevelopingUX\Recipe3.15\Page.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8799733BA02A0F9D9CCEE3931DD0C373"
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


namespace Ch03_DevelopingUX.Recipe3_15 {
    
    
    public partial class Page : System.Windows.Controls.UserControl {
        
        internal System.Windows.Media.Animation.Storyboard SpinGameBallStoryboard;
        
        internal System.Windows.Controls.Canvas GameCanvas;
        
        internal System.Windows.Controls.Border WelcomeMessage;
        
        internal System.Windows.Controls.Border ClickToPlay;
        
        internal System.Windows.Shapes.Ellipse RadioactiveBall;
        
        internal System.Windows.Shapes.Path LeftIceCaveWall;
        
        internal System.Windows.Shapes.Path IceCaveWallFloor;
        
        internal System.Windows.Shapes.Path RightIceCaveWall;
        
        internal System.Windows.Shapes.Path IceCaveCieiling;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Ch03_DevelopingUX.Recipe3_15;component/Page.xaml", System.UriKind.Relative));
            this.SpinGameBallStoryboard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("SpinGameBallStoryboard")));
            this.GameCanvas = ((System.Windows.Controls.Canvas)(this.FindName("GameCanvas")));
            this.WelcomeMessage = ((System.Windows.Controls.Border)(this.FindName("WelcomeMessage")));
            this.ClickToPlay = ((System.Windows.Controls.Border)(this.FindName("ClickToPlay")));
            this.RadioactiveBall = ((System.Windows.Shapes.Ellipse)(this.FindName("RadioactiveBall")));
            this.LeftIceCaveWall = ((System.Windows.Shapes.Path)(this.FindName("LeftIceCaveWall")));
            this.IceCaveWallFloor = ((System.Windows.Shapes.Path)(this.FindName("IceCaveWallFloor")));
            this.RightIceCaveWall = ((System.Windows.Shapes.Path)(this.FindName("RightIceCaveWall")));
            this.IceCaveCieiling = ((System.Windows.Shapes.Path)(this.FindName("IceCaveCieiling")));
        }
    }
}
