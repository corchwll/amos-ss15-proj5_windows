/*
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


using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace TimeTracker.Pages
{
    public partial class AddSessionPage : PhoneApplicationPage
    {
        string _projectId;

        //Page initializer (Initializes .xaml layout)
        public AddSessionPage()
        {
            InitializeComponent();
        }

        //OnNavigation event handler - read project id property
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string id = "";

            if (NavigationContext.QueryString.TryGetValue("id", out id))
            {
                ProjectName.Text = id;
                _projectId = id;
            }
        }


        //The following region contains the cancel and save click listeners
        #region Click Listener

        
        //Triggers when user click cancel on navigates back to the previous screen
        private void onCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        //Triggers when the user clicks save
        //Checks if data enters is valid
        //Navigates to MainPivotPage and adds data through URL
        private void onSave_Click(object sender, RoutedEventArgs e)
        {
            int timestampStart = Utils.TotalSeconds((DateTime) Startingtime.Value);
            int timestampEnd = Utils.TotalSeconds((DateTime) EndingTime.Value);

            if (timestampEnd - timestampStart <= 0)
            {
                MessageBoxResult result = MessageBox.Show("Negative times are not allowed",
                    "Error", MessageBoxButton.OKCancel);

                return;
            }

            string uri = new UriFactory().CreateSessionDataUri(timestampStart.ToString(), timestampEnd.ToString(), _projectId);
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        #endregion

        private void WorkingDate_OnValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }   
}