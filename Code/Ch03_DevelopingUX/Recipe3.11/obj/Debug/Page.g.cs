#pragma checksum "D:\RobcamerDocs\Book Silverlight Recipes\Code\Ch03_DevelopingUX\Recipe3.11\Page.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A0B9635E0F15DB5A17C06AF43FECD35A"
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


namespace Ch03_DevelopingUX.Recipe3_11 {
    
    
    public partial class Page : System.Windows.Controls.UserControl {
        
        internal System.Windows.Media.Animation.Storyboard Rect1MouseMove;
        
        internal System.Windows.Media.Animation.Storyboard EllipseMouseEnter;
        
        internal System.Windows.Media.Animation.Storyboard EllipseMouseLeave;
        
        internal System.Windows.Media.Animation.Storyboard PathClick;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Shapes.Rectangle Rect1;
        
        internal System.Windows.Shapes.Ellipse Ellipse1;
        
        internal System.Windows.Controls.StackPanel stackPanel;
        
        internal System.Windows.Media.ArcSegment animatedArcSegment;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Ch03_DevelopingUX.Recipe3_11;component/Page.xaml", System.UriKind.Relative));
            this.Rect1MouseMove = ((System.Windows.Media.Animation.Storyboard)(this.FindName("Rect1MouseMove")));
            this.EllipseMouseEnter = ((System.Windows.Media.Animation.Storyboard)(this.FindName("EllipseMouseEnter")));
            this.EllipseMouseLeave = ((System.Windows.Media.Animation.Storyboard)(this.FindName("EllipseMouseLeave")));
            this.PathClick = ((System.Windows.Media.Animation.Storyboard)(this.FindName("PathClick")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.Rect1 = ((System.Windows.Shapes.Rectangle)(this.FindName("Rect1")));
            this.Ellipse1 = ((System.Windows.Shapes.Ellipse)(this.FindName("Ellipse1")));
            this.stackPanel = ((System.Windows.Controls.StackPanel)(this.FindName("stackPanel")));
            this.animatedArcSegment = ((System.Windows.Media.ArcSegment)(this.FindName("animatedArcSegment")));
        }
    }
}
