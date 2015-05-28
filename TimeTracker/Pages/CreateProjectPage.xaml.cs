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

namespace TimeTracker
{
    /**
     * This Page shows the user a form which has to be filled with project
     * information. As soon as the information is confirmed it gets send
     * back to the MainPivotPage.xaml
     */
    public partial class CreateProjectPage : PhoneApplicationPage
    {


        public CreateProjectPage()
        {
            InitializeComponent();
        }


        private void Click_CreateProject(object sender, RoutedEventArgs e)
        {

            string projectName = TextBoxName.Text;
            string projectId = TextBoxId.Text;
            int date = (int) FinalDate.Value.Value.Ticks;

            if (!ProjectItem.CheckProjectId(projectId))
            {
                MessageBoxResult result = MessageBox.Show("Your project ID must consist of 5 digits",
                      "Error", MessageBoxButton.OKCancel);
                return;
            }

            UriFactory factory = new UriFactory();
            NavigationService.Navigate(new Uri(factory.CreateDataUri(projectName, projectId,
                                        FinalDate.ToString()), UriKind.Relative));

        }


    }
}