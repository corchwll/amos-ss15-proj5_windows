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

using System.Data.Linq.Mapping;

namespace TimeTracker
{
    /**
     * Folllowing class defines the model of a User in the local database
     * The model contains name, surname, personal id, overtime, working time,
     * vacation days & vacationdays currently used.
     * The model contains a primary key.
     */
    [Table]
    public class UserItem{

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int UserItemId { get; set; }

        [Column]
        public string Name { get; set; }

        [Column]
        public string Surname { get; set; }

        [Column]
        public string PersonalId { get; set; }

        [Column]
        public int WorkingTime { get; set; }

        [Column]
        public int OverTime { get; set; }

        [Column]
        public int VacationDays { get; set; }

        [Column]
        public int CurrentVacationDays { get; set; }
    }
}
