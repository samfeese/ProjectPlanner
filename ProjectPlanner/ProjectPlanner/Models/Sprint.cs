using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPlanner.Models
{
    public class Sprint
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(2550)]
        public string? Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AssociatedProjectId { get; set; }
    }
}
