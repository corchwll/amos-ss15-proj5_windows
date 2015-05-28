using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker
{
    public class UriFactory
    {

        private const string NavigationBody = "/Pages/MainPivotPage.xaml?";
        private const string LinkElement = "&";

        public string CreateDataUri(string projectName, string projectId, string date)
        {
            return NavigationBody
                   + ProjectNameUri(projectName)
                   + LinkElement
                   + ProjectIdUri(projectId)
                   + LinkElement
                   + ProjectDateUri(date);
        }

        private string ProjectNameUri(string projectName)
        {
            return QueryDictionary.ProjectName + "=" + projectName;
        }

        private string ProjectIdUri(string projectId)
        {
            return QueryDictionary.ProjectId + "=" + projectId;

        }

        private string ProjectDateUri(string date)
        {
            return QueryDictionary.ProjectFinalDate + "=" + date;

        }
    }


        
    
}
