using System;
using System.Windows;
using System.Windows.Controls;
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

        //Click listener when registration button is clicked
        //All entered contents are checked if they match the requirements
        //If they don't match appropriate message are displayed in the UI
        private void registrated_Click(object sender, RoutedEventArgs e)
        {
            String name = TextBoxName.Text;
            String surname = TextBoxSurname.Text;
            String personalId = TextBoxPersonalId.Text;
            String workingTime = TextBoxHoursWeek.Text;
            String overtime = TextBoxOvertime.Text;
            String vacationDays = TextBoxVacation.Text;
            String currentVacation = TextBoxCurrentVacation.Text;

            if (IsFormFilledCompletely(name, surname, personalId, workingTime,
                overtime, vacationDays, currentVacation))
            {
                MessageBoxResult result = MessageBox.Show("Please fill out every field",
                       "Form not completed", MessageBoxButton.OKCancel);
                return;
            }



            if (!CheckWorkingTime(workingTime) || !CheckVacationDays(vacationDays)
                || !CheckOvertime(overtime) || !CheckCurrentVacationDays(currentVacation))
            {
                MessageBoxResult result = MessageBox.Show("Please check your inputs",
                       "Error", MessageBoxButton.OKCancel);
                return;
                
            }


            if (!CheckPersonalId(personalId))
            {
                MessageBoxResult result = MessageBox.Show("Your personal ID must consist of 5 digits",
                       "Error", MessageBoxButton.OKCancel);
                return;
            }



            NavigationService.Navigate(new Uri("/MainPivotPage.xaml?"
                + "name=" + name
                + "&" + "surname=" + surname
                + "&" + "personalId=" + personalId
                + "&" + "workingTime=" + workingTime
                + "&" + "overtime=" + overtime
                + "&" + "vacationDays=" + vacationDays
                + "&" + "currentVacation=" + currentVacation
                , UriKind.Relative));

        }

        private bool IsFormFilledCompletely(string name,
            string surname, string personalId, string workingTime,
            string overtime, string vacationDays, string currentVacation)
        {
            if (name.Length == 0 || surname.Length == 0 || personalId.Length == 0 ||
               workingTime.Length == 0 || overtime.Length == 0 || vacationDays.Length == 0
               || currentVacation.Length == 0)
            {
               return false;
            }
            return true;
        }

        private bool CheckWorkingTime(string workingTime)
        {
            int workTime;
            if (int.TryParse(workingTime, out workTime))
            {
                if (workTime < 10 || workTime > 50)
                {
                    return false;
                }
                return true;
            }
                return false;
        }

        private bool CheckVacationDays(string vacationDays)
        {
            int vacDays;
            if (int.TryParse(vacationDays, out vacDays))
            {
                if (vacDays < 10 || vacDays > 40)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        private bool CheckOvertime(string overtime)
        {
            int ovtime;
            if (int.TryParse(overtime, out ovtime))
            {
                return true;
            }
            return false;
        }

        private bool CheckCurrentVacationDays(string currentVacation)
        {
            int vacDays;
            if (int.TryParse(currentVacation, out vacDays))
            {
                if (vacDays <= 0)
                {
                    return false;
                }
                return true;
            }
            return false;
            
        }

        private bool CheckPersonalId(string id)
        {
            int idNumber;
            if (int.TryParse(id, out idNumber))
            {
                if (id.Length != 5)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        
    }


}