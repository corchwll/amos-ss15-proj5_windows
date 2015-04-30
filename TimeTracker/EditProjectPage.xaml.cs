using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace TimeTracker
{
    public partial class EditProjectPage : PhoneApplicationPage
    {
        public EditProjectPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string id = "";

            if (NavigationContext.QueryString.TryGetValue("id", out id))
            {
                ProjectName.Text = id;
            }
        }

        private void onCancel_Click(object sender, RoutedEventArgs e)
        {
        }

        private void onSave_Click(object sender, RoutedEventArgs e)
        {
        }
    }

    
}