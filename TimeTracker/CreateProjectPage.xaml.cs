using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace TimeTracker
{
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

            NavigationService.Navigate(new Uri("/MainPivotPage.xaml?projectName=" + projectName + "&" + "projectId=" + projectId + "&" + "finalDate=" + FinalDate, UriKind.Relative));

        }
    }
}