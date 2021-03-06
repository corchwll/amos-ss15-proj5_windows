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

using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace TimeTracker
{
    /*
     * This Class represents the view including all interactions which is shown when
     * a user wants to edit the project information, by selecting the appropriate
     * menu item in the project list.
     */

    public partial class EditProjectPage : PhoneApplicationPage
    {

        private string _id;
        private string _name;
        private int _finalDate;
        private double _latitude;
        private double _longitude;

        public EditProjectPage()
        {
            InitializeComponent();
        }

        
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            CollectProjectData();

            ProjectNameTextBox.Text = _name;
            FinalDateTextBox.Text = _finalDate.ToString();
            LongitudeTextBox.Text = _longitude.ToString();
            LatitudeTextBox.Text = _latitude.ToString();
        }

        //Collects the information send to this page as project name & final date
        private void CollectProjectData()
        {
              string tmp = "";
            if (NavigationContext.QueryString.TryGetValue("projectId", out tmp))
            {
                _id = CollectStringOnNavigation(QueryDictionary.ProjectId);
                _name = CollectStringOnNavigation(QueryDictionary.ProjectName);
                _finalDate = CollectIntOnNavgation(QueryDictionary.ProjectFinalDate);
                _latitude = CollectDoubleOnNavigation(QueryDictionary.ProjectLatitude);
                _longitude = CollectDoubleOnNavigation(QueryDictionary.ProjectLongitude);
            }
        }

        //Following methods are helpers to collect the data received in the 
        //appropriate data format
        #region Collecting Helpers

        private string CollectStringOnNavigation(string key)
        {
            string result = "";
            NavigationContext.QueryString.TryGetValue(key, out result);
            return result;
        }

        private int CollectIntOnNavgation(string key)
        {
            string result = "";
            NavigationContext.QueryString.TryGetValue(key, out result);
            return Int32.Parse(result);

        }

        private double CollectDoubleOnNavigation(string key)
        {
            string result = "";
            NavigationContext.QueryString.TryGetValue(key, out result);

            double fin = Double.Parse(result);
            return fin;
        }

        #endregion

        //Following methods are the click events for th cancel or
        //save interactions
        #region Click Listener
        private void Cancel_click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Save_click(object sender, RoutedEventArgs e)
        {
            _name = ProjectNameTextBox.Text;
            _finalDate = Int32.Parse(FinalDateTextBox.Text);
            _longitude = Double.Parse(LongitudeTextBox.Text);
            _latitude = Double.Parse(LatitudeTextBox.Text);

            string uri = new UriFactory().CreateProjectDataUri(_name, _id,
                _finalDate.ToString(), _latitude.ToString(), _longitude.ToString());

            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        #endregion
    }
}