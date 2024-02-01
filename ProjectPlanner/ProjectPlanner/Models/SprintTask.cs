using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPlanner.Models
{
    public class SprintTask
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(255)]
        public string? Name { get; set; }
        public bool Complete { get; set; }
        public int AssociatedSprintId { get; set; }

        public SprintTask()
        {
            Complete = false;
        }
    }
}
