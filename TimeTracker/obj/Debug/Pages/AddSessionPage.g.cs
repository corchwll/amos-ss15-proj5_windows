﻿#pragma checksum "C:\Users\Daniel\Documents\Visual Studio 2013\Projects\TimeTracker\amos-ss15-proj5_windows\TimeTracker\Pages\AddSessionPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3619A9DEE761099FB0775B36A6599B16"
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
    
    
    public partial class AddSessionPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock ProjectName;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal Microsoft.Phone.Controls.DatePicker WorkingDate;
        
        internal Microsoft.Phone.Controls.TimePicker Startingtime;
        
        internal Microsoft.Phone.Controls.TimePicker EndingTime;
        
        internal System.Windows.Controls.Button Button1;
        
        internal System.Windows.Controls.Button Button2;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/TimeTracker;component/Pages/AddSessionPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ProjectName = ((System.Windows.Controls.TextBlock)(this.FindName("ProjectName")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.WorkingDate = ((Microsoft.Phone.Controls.DatePicker)(this.FindName("WorkingDate")));
            this.Startingtime = ((Microsoft.Phone.Controls.TimePicker)(this.FindName("Startingtime")));
            this.EndingTime = ((Microsoft.Phone.Controls.TimePicker)(this.FindName("EndingTime")));
            this.Button1 = ((System.Windows.Controls.Button)(this.FindName("Button1")));
            this.Button2 = ((System.Windows.Controls.Button)(this.FindName("Button2")));
        }
    }
}

