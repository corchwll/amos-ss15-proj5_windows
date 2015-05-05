using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;



using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Data.Linq;

namespace TimeTracker
{
    [Table]
    public class UserItem{
        private int _userItemId;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int UserItemId
        {
            get
            {
                return _userItemId;
            }
            set
            {
                if (_userItemId != value)
                {
                    _userItemId = value;
                }
            }
        }


        private string _name;

        [Column]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                }
            }
        }

        private string _surname;

        [Column]
        public string Surname
        {
            get
            {
                return _surname;
            }
            set
            {
                if (_surname != value)
                {
                    _surname = value;
                }
            }
        }

        private string _personalId;

        [Column]
        public string PersonalId
        {
            get
            {
                return _personalId;
            }
            set
            {
                if (_personalId != value)
                {
                    _personalId = value;
                }
            }
        }


        private int _workingTime;

        [Column]
        public int WorkingTime
        {
            get
            {
                return _workingTime;
            }
            set
            {
                if (_workingTime != value)
                {
                    _workingTime = value;
                }
            }
        }

        private int _overtime;

        [Column]
        public int OverTime
        {
            get
            {
                return _overtime;
            }
            set
            {
                if (_overtime != value)
                {
                    _overtime = value;
                }
            }
        }

        private int _vacationDays;

        [Column]
        public int VacationDays
        {
            get
            {
                return _vacationDays;
            }
            set
            {
                if (_vacationDays != value)
                {
                    _vacationDays = value;
                }
            }
        }

        private int _currentVacationDays;

        [Column]
        public int CurrentVacationDays
        {
            get
            {
                return _currentVacationDays;
            }
            set
            {
                if (_currentVacationDays != value)
                {
                    _currentVacationDays = value;
                }
            }
        }







       
    }
}
