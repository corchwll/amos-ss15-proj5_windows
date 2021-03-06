﻿/*
 *     Mobile Time Accounting
 *     Copyright (C) 2015
 *
 *     This program is free software: you can redistribute it and/or modify
 *     it under the terms of the GNU Affero General Public License as
 *     published by the Free Software Foundation, either version 3 of the
 *     License, or (at your option) any later version.
 *
 *     This program is distributed in the hope that it will be useful,
 *     but WITHOUT ANY WARRANTY; without even the implied warranty of
 *     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *     GNU Affero General Public License for more details.
 *
 *     You should have received a copy of the GNU Affero General Public License
 *     along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */


using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;


namespace TimeTracker
{
    public partial class App : Application
    {


        /// <summary>
        /// Bietet einen einfachen Zugriff auf den Stammframe der Phone-Anwendung.
        /// </summary>
        /// <returns>Der Stammframe der Phone-Anwendung.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Konstruktor für das Application-Objekt.
        /// </summary>
        public App()
        {
            // Globaler Handler für nicht abgefangene Ausnahmen. 
            UnhandledException += Application_UnhandledException;

            // Silverlight-Standardinitialisierung
            InitializeComponent();

            // Phone-spezifische Initialisierung
            InitializePhoneApplication();

            // Während des Debuggens Profilerstellungsinformationen zur Grafikleistung anzeigen.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Zähler für die aktuelle Bildrate anzeigen.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Bereiche der Anwendung hervorheben, die mit jedem Bild neu gezeichnet werden.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Nicht produktiven Visualisierungsmodus für die Analyse aktivieren, 
                // in dem Bereiche einer Seite angezeigt werden, die mit einer Farbüberlagerung an die GPU übergeben wurden.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Deaktivieren Sie die Leerlauferkennung der Anwendung, indem Sie die UserIdleDetectionMode-Eigenschaft des
                // PhoneApplicationService-Objekts der Anwendung auf "Disabled" festlegen.
                // Vorsicht: Nur im Debugmodus verwenden. Eine Anwendung mit deaktivierter Benutzerleerlauferkennung wird weiterhin ausgeführt
                // und verbraucht auch dann Akkuenergie, wenn der Benutzer das Handy nicht verwendet.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

            

        }

        // Code, der beim Starten der Anwendung ausgeführt werden soll (z. B. über "Start")
        // Dieser Code wird beim Reaktivieren der Anwendung nicht ausgeführt
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        // Code, der ausgeführt werden soll, wenn die Anwendung aktiviert wird (in den Vordergrund gebracht wird)
        // Dieser Code wird beim ersten Starten der Anwendung nicht ausgeführt
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        // Code, der ausgeführt werden soll, wenn die Anwendung deaktiviert wird (in den Hintergrund gebracht wird)
        // Dieser Code wird beim Schließen der Anwendung nicht ausgeführt
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code, der beim Schließen der Anwendung ausgeführt wird (z. B. wenn der Benutzer auf "Zurück" klickt)
        // Dieser Code wird beim Deaktivieren der Anwendung nicht ausgeführt
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // Code, der bei einem Navigationsfehler ausgeführt wird
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Navigationsfehler. Unterbrechen und Debugger öffnen
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code, der bei nicht behandelten Ausnahmen ausgeführt wird
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Eine nicht behandelte Ausnahme ist aufgetreten. Unterbrechen und Debugger öffnen
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Initialisierung der Phone-Anwendung

        // Doppelte Initialisierung vermeiden
        private bool _phoneApplicationInitialized = false;

        // Fügen Sie keinen zusätzlichen Code zu dieser Methode hinzu
        private void InitializePhoneApplication()
        {
            if (_phoneApplicationInitialized)
                return;

            // Frame erstellen, aber noch nicht als RootVisual festlegen. Dadurch kann der Begrüßungsbildschirm
            // aktiv bleiben, bis die Anwendung bereit für das Rendern ist.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Navigationsfehler behandeln
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Sicherstellen, dass keine erneute Initialisierung erfolgt
            _phoneApplicationInitialized = true;
        }

        // Fügen Sie keinen zusätzlichen Code zu dieser Methode hinzu
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Visuelle Stammkomponente festlegen, sodass die Anwendung gerendert werden kann
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Dieser Handler wird nicht mehr benötigt und kann entfernt werden
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        

        #endregion
    }

    
}