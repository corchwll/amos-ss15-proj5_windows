﻿#pragma checksum "C:\Users\Joachim\documents\visual studio 2010\Projects\TimeTracker\TimeTracker\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D155B730DA71FF949F811C58861B4B76"
//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.18063
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
        
        internal System.Windows.Controls.Button buttonStartRecording;
        
        internal System.Windows.Controls.TextBox textBoxTime;
        
        internal System.Windows.Controls.Button buttonSaveData;
        
        internal System.Windows.Controls.Button buttonQueryData;
        
        internal System.Windows.Controls.TextBox newProjectTextBox;
        
        internal System.Windows.Controls.Button button1;
        
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
            this.buttonStartRecording = ((System.Windows.Controls.Button)(this.FindName("buttonStartRecording")));
            this.textBoxTime = ((System.Windows.Controls.TextBox)(this.FindName("textBoxTime")));
            this.buttonSaveData = ((System.Windows.Controls.Button)(this.FindName("buttonSaveData")));
            this.buttonQueryData = ((System.Windows.Controls.Button)(this.FindName("buttonQueryData")));
            this.newProjectTextBox = ((System.Windows.Controls.TextBox)(this.FindName("newProjectTextBox")));
            this.button1 = ((System.Windows.Controls.Button)(this.FindName("button1")));
        }
    }
}

