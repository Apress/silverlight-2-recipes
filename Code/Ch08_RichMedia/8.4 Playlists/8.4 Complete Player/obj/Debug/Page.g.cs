#pragma checksum "D:\work\Current Initiatives\Writing\SLBook\WorkInProgress\ChapterSamples\Ch08_RichMedia\8.4 Playlists\8.4 Complete Player\Page.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "809A19CA61B015D751B91E0E2AB96E48"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3053
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Ch08_RichMedia.Recipe8_4;
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


namespace Ch08_RichMedia.Recipe8_4 {
    
    
    public partial class Page : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.MediaElement mediaelemMain;
        
        internal System.Windows.Controls.MediaElement mediaelemPIP;
        
        internal System.Windows.Controls.Border displayMain;
        
        internal System.Windows.Media.VideoBrush vidbrushMain;
        
        internal System.Windows.Controls.Border displayPIP;
        
        internal System.Windows.Media.VideoBrush vidbrushPIP;
        
        internal System.Windows.Controls.Grid buttonsPIP;
        
        internal System.Windows.Controls.Button btnClosePIP;
        
        internal System.Windows.Shapes.Path Path;
        
        internal System.Windows.Controls.Button btnSwitchPIP;
        
        internal Ch08_RichMedia.Recipe8_4.MediaSlider mediaSlider;
        
        internal Ch08_RichMedia.Recipe8_4.MediaButtonsPanel mediaControl;
        
        internal System.Windows.Controls.Slider sliderVolumeControl;
        
        internal System.Windows.Controls.RadioButton rbtnDownloadsMenu;
        
        internal System.Windows.Controls.RadioButton rbtnOnDemandMenu;
        
        internal System.Windows.Controls.RadioButton rbtnBroadcastMenu;
        
        internal System.Windows.Controls.ListBox lbxMediaMenuDownloads;
        
        internal System.Windows.Controls.ListBox lbxMediaMenuOnDemandStreams;
        
        internal System.Windows.Controls.ListBox lbxMediaMenuBroadcastStreams;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Ch08_RichMedia.Recipe8_4.PlaylistPlayer;component/Page.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.mediaelemMain = ((System.Windows.Controls.MediaElement)(this.FindName("mediaelemMain")));
            this.mediaelemPIP = ((System.Windows.Controls.MediaElement)(this.FindName("mediaelemPIP")));
            this.displayMain = ((System.Windows.Controls.Border)(this.FindName("displayMain")));
            this.vidbrushMain = ((System.Windows.Media.VideoBrush)(this.FindName("vidbrushMain")));
            this.displayPIP = ((System.Windows.Controls.Border)(this.FindName("displayPIP")));
            this.vidbrushPIP = ((System.Windows.Media.VideoBrush)(this.FindName("vidbrushPIP")));
            this.buttonsPIP = ((System.Windows.Controls.Grid)(this.FindName("buttonsPIP")));
            this.btnClosePIP = ((System.Windows.Controls.Button)(this.FindName("btnClosePIP")));
            this.Path = ((System.Windows.Shapes.Path)(this.FindName("Path")));
            this.btnSwitchPIP = ((System.Windows.Controls.Button)(this.FindName("btnSwitchPIP")));
            this.mediaSlider = ((Ch08_RichMedia.Recipe8_4.MediaSlider)(this.FindName("mediaSlider")));
            this.mediaControl = ((Ch08_RichMedia.Recipe8_4.MediaButtonsPanel)(this.FindName("mediaControl")));
            this.sliderVolumeControl = ((System.Windows.Controls.Slider)(this.FindName("sliderVolumeControl")));
            this.rbtnDownloadsMenu = ((System.Windows.Controls.RadioButton)(this.FindName("rbtnDownloadsMenu")));
            this.rbtnOnDemandMenu = ((System.Windows.Controls.RadioButton)(this.FindName("rbtnOnDemandMenu")));
            this.rbtnBroadcastMenu = ((System.Windows.Controls.RadioButton)(this.FindName("rbtnBroadcastMenu")));
            this.lbxMediaMenuDownloads = ((System.Windows.Controls.ListBox)(this.FindName("lbxMediaMenuDownloads")));
            this.lbxMediaMenuOnDemandStreams = ((System.Windows.Controls.ListBox)(this.FindName("lbxMediaMenuOnDemandStreams")));
            this.lbxMediaMenuBroadcastStreams = ((System.Windows.Controls.ListBox)(this.FindName("lbxMediaMenuBroadcastStreams")));
        }
    }
}
