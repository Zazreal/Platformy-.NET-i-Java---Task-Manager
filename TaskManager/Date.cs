using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager
{
    public class Date
    {
        public int ID { get; set; }
        public DateTime date { get; set; }

        /// <summary>
        /// Collection of Tasks assigned to this Date
        /// </summary>
        public virtual ICollection<Task> Tasks { get; set; }

        

    }
}
