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
    public partial class RegistrationPage : PhoneApplicationPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private void textBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void registrated_Click(object sender, RoutedEventArgs e)
        {
            String name = textBoxName.Text;
            String surname = textBoxSurname.Text;
            String personalId = textBoxPersonalID.Text;
            String workingTime = textBoxHoursWeek.Text;
            String overtime = textBoxOvertime.Text;
            String vacationDays = textBoxVacation.Text;
            String currentVacation = textBoxCurrentVacation.Text;

            NavigationService.Navigate(new Uri("/MainPage.xaml?"
                + "name=" + name
                + "&" + "surname=" + surname
                + "&" + "personalId=" + personalId
                + "&" + "workingTime=" + workingTime
                + "&" + "overtime=" + overtime
                + "&" + "vacationDays=" + vacationDays
                + "&" + "currentVacation=" + currentVacation
                , UriKind.Relative));

        }
        
    }
}