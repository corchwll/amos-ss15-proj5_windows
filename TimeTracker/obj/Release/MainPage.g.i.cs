﻿#pragma checksum "C:\Users\Daniel\Documents\Visual Studio 2013\Projects\TimeTracker\amos-ss15-proj5_windows\TimeTracker\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5ABDA33BB18EEAC9F1740EFD095216B9"
//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.36014
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
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


namespace TimeTracker {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock ApplicationTitle;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.ListBox projectItemtListBox;
        
        internal System.Windows.Controls.TextBox textBoxTime;
        
        internal System.Windows.Controls.Button buttonQueryData;
        
        internal System.Windows.Controls.TextBox newProjectNameTextBox;
        
        internal System.Windows.Controls.Button button1;
        
        internal System.Windows.Controls.TextBox newProjectIdTextBox;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/TimeTracker;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ApplicationTitle = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitle")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.projectItemtListBox = ((System.Windows.Controls.ListBox)(this.FindName("projectItemtListBox")));
            this.textBoxTime = ((System.Windows.Controls.TextBox)(this.FindName("textBoxTime")));
            this.buttonQueryData = ((System.Windows.Controls.Button)(this.FindName("buttonQueryData")));
            this.newProjectNameTextBox = ((System.Windows.Controls.TextBox)(this.FindName("newProjectNameTextBox")));
            this.button1 = ((System.Windows.Controls.Button)(this.FindName("button1")));
            this.newProjectIdTextBox = ((System.Windows.Controls.TextBox)(this.FindName("newProjectIdTextBox")));
        }
    }
}

