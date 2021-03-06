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
using System.Windows.Controls;
using Microsoft.Phone.Controls;

namespace TimeTracker
{
    public partial class RegistrationPage : PhoneApplicationPage
    {
        string _name = "";
        string _surname = "";
        string _personalId = "";
        string _workingTime = "";
        string _overtime = "";
        string _vacationDays = "";
        string _currentVacation = "";
        bool _sortByLocation = false;

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

            ReadFormFields();

            if (!IsFormFilledCompletely())
            {
                ShowErrorMessage("Form not completed", "Please fill out every form field");
                return;
            }

            if (!CheckWorkingTime(_workingTime) || !CheckVacationDays(_vacationDays)
                || !CheckOvertime(_overtime) || !CheckCurrentVacationDays(_currentVacation))
            {
                ShowErrorMessage("Error", "Please check your inputs");
                return;
            }

            if (!CheckPersonalId(_personalId))
            {
                ShowErrorMessage("Error", "Your personal ID must consist of 5 digits");
                return;
            }

           NavigateToMainPivotPage(_name, _surname, _personalId, _workingTime,
                _overtime, _vacationDays, _currentVacation);

        }

        private void ReadFormFields()
        {
            _name = TextBoxName.Text;
            _surname = TextBoxSurname.Text;
            _personalId = TextBoxPersonalId.Text;
            _workingTime = TextBoxHoursWeek.Text;
            _overtime = TextBoxOvertime.Text;
            _vacationDays = TextBoxVacation.Text;
            _currentVacation = TextBoxCurrentVacation.Text;
            _sortByLocation = CheckBoxSort.IsChecked.Value;
        }

        private void NavigateToMainPivotPage(string name,
            string surname, string personalId, string workingTime,
            string overtime, string vacationDays, string currentVacation)
        {
            NavigationService.Navigate(new Uri("/Pages/MainPivotPage.xaml?"
               + "name=" + _name
               + "&" + "surname=" + _surname
               + "&" + "personalId=" + _personalId
               + "&" + "workingTime=" + _workingTime
               + "&" + "overtime=" + _overtime
               + "&" + "vacationDays=" + _vacationDays
               + "&" + "currentVacation=" + _currentVacation
               + "&" + "sortbylocation=" + _sortByLocation
               , UriKind.Relative));
        }

        private void ShowErrorMessage(string titel, string message)
        {
            MessageBoxResult result = MessageBox.Show(message,
                      titel, MessageBoxButton.OK);
        }

        private bool IsFormFilledCompletely()
        {
            if (_name.Length == 0 || _surname.Length == 0 || _personalId.Length == 0 ||
               _workingTime.Length == 0 || _overtime.Length == 0 || _vacationDays.Length == 0
               || _currentVacation.Length == 0)
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