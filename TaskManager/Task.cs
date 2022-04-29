using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager
{
    public class Task
    {
        public int ID { get; set; }
        public string TaskName { get; set; }
        public bool Important { get; set; }

        //[ForeignKey("Date")]
        public int DateID { get; set; }
        public virtual Date Date { get; set; }


        /// <summary>
        /// Used only to be usable in the list
        /// </summary>
        public string Date_str
        {
            get { return $"{ GetDate().ToString() }"; }
        }


        /// <summary>
        /// Used only to be usable in the list
        /// </summary>
        public string Date_Day
        {
            get { return $"{ GetDay().DayOfWeek.ToString() }"; }
        }

        /// <summary>
        /// Returns the string from Date.date.ToString() assigned to this Task
        /// </summary>
        private string GetDate()
        {
            using (var context = new TaskManagerDataBaseContext())
            {
                Date query = context.Dates.Where(d => d.ID == DateID).FirstOrDefault<Date>();
                return query.date.ToString();
            }
        }
        
        /// <summary>
        /// Returns the Datetime from Date assigned to this Task
        /// </summary>
        private DateTime GetDay()
        {
            using (var context = new TaskManagerDataBaseContext())
            {
                Date query = context.Dates.Where(d => d.ID == DateID).FirstOrDefault<Date>();
                return query.date;
            }
        }

    }

}
