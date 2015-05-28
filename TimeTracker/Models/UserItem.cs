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
