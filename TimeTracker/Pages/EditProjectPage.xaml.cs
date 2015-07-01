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
    public partial class EditProjectPage : PhoneApplicationPage
    {
        public EditProjectPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            CollectProjectData();
           
        }

        private void CollectProjectData()
        {
              string tmp = "";
            if (NavigationContext.QueryString.TryGetValue("projectId", out tmp))
            {
                string id = CollectStringOnNavigation(QueryDictionary.ProjectId);
                string name = CollectStringOnNavigation(QueryDictionary.ProjectName);
                int finalDate = CollectIntOnNavgation(QueryDictionary.ProjectFinalDate);
                double latitude = CollectDoubleOnNavigation(QueryDictionary.ProjectLatitude);
                double longitude = CollectDoubleOnNavigation(QueryDictionary.ProjectLongitude);
            }
        }


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
    }
}