﻿#pragma checksum "C:\Users\Daniel\Documents\Visual Studio 2013\Projects\TimeTracker\amos-ss15-proj5_windows\TimeTracker\Pages\CreateProjectPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "37136CE6B294A7AFB7C0772F95DF3E7F"
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


namespace TimeTracker.Pages {
    
    
    public partial class CreateProjectPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBox TextBoxName;
        
        internal System.Windows.Controls.TextBox TextBoxId;
        
        internal Microsoft.Phone.Controls.DatePicker FinalDate;
        
        internal System.Windows.Controls.TextBox TextBoxLatitude;
        
        internal System.Windows.Controls.TextBox TextBoxLongitude;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/TimeTracker;component/Pages/CreateProjectPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.TextBoxName = ((System.Windows.Controls.TextBox)(this.FindName("TextBoxName")));
            this.TextBoxId = ((System.Windows.Controls.TextBox)(this.FindName("TextBoxId")));
            this.FinalDate = ((Microsoft.Phone.Controls.DatePicker)(this.FindName("FinalDate")));
            this.TextBoxLatitude = ((System.Windows.Controls.TextBox)(this.FindName("TextBoxLatitude")));
            this.TextBoxLongitude = ((System.Windows.Controls.TextBox)(this.FindName("TextBoxLongitude")));
        }
    }
}

