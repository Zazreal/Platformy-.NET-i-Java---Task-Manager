using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Globalization;

namespace TaskManager
{

    /// <summary>
    /// Class responsible for managing the database context
    /// </summary>
    public class TaskManagerDataBaseContext : DbContext
    {
        public TaskManagerDataBaseContext() : base("TaskManagerDatabase")
        { }

        /// <summary>
        /// Collection of all Dates in the database
        /// </summary>
        public DbSet<Date> Dates { get; set; }

        /// <summary>
        /// Collection of all Tasks in the database
        /// </summary>
        public DbSet<Task> Tasks { get; set; }

        /// <summary>
        /// Adds new Task to the database
        /// </summary>
        /// <param name="task">Object of the Task class</param>
        public static void AddNewTask(Task task)
        {
            using (var context = new TaskManagerDataBaseContext())
            {
                context.Tasks.Add(task);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Adds new Task to the database,
        /// </summary>
        /// <param name="Id">ID of new task</param>
        /// <param name="name">Task name</param>
        /// <param name="im">Importance in boolean form</param>
        /// <param name="date_id">ID of date to assign</param>
        public static void AddNewTask(int Id = 0, string name = " ", bool im = false, int date_id = 0 )
        {

            Task task = new Task { ID = Id, TaskName = name, Important = im, DateID = date_id };

            using (var context = new TaskManagerDataBaseContext())
            {
                context.Tasks.Add(task);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Adds new Task to the database
        /// </summary>
        /// /// <param name="Id">ID of new task</param>
        /// <param name="name">Task name</param>
        /// <param name="im">Importance in string, needs to be "true" or "false"</param>
        /// <param name="date_id">ID of date to assign</param>
        public static void AddNewTask(int Id = 0, string name = "empty", string im_str = "false", int date_id = 0)
        {
            if (name != null && im_str != null)
            {
                using (var context = new TaskManagerDataBaseContext())
                {
                    try
                    {
                        bool im = Boolean.Parse(im_str);
                        Task task = new Task { ID = Id, TaskName = name, Important = im, DateID = date_id };


                        context.Tasks.Add(task);
                        context.SaveChanges();

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("ERROR: " + e.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves a Task from the database by TaskName
        /// </summary>
        /// <param name="task">TaskName of wanted Task</param>
        public static Task GetTask(string task)
        {
            try 
            {
            using (var context = new TaskManagerDataBaseContext())
                {
                    var query = context.Tasks.Where(t => t.TaskName == task).FirstOrDefault<Task>();
                    return query;
                }
            }
                catch (Exception e)
                { 
                    Console.WriteLine(e.Message); 
                }
                return null;
        }

        /// <summary>
        /// Retrieves a Task from the database by ID
        /// </summary>
        /// <param name="TaskID">ID of wanted Task</param>
        public static Task GetTask(int TaskID)
        {
            try
            {
                using (var context = new TaskManagerDataBaseContext())
                {
                    var query = context.Tasks.Where(t => t.ID == TaskID).FirstOrDefault<Task>();
                    return query;
                }
            }
            catch (Exception e)
            { 
                Console.WriteLine(e.Message); 
            }
            return null;
        }

        /// <summary>
        /// Retrieves a Task
        /// </summary>
        /// <param name="task">Looks for exact copy of "task" in database</param>
        public static Task GetTask(Task task)
        {
            using (var context = new TaskManagerDataBaseContext())
            {
                var query = context.Tasks.Where(t => t == task).FirstOrDefault<Task>();
                return query;
            }
        }
        
        /// <summary>
        /// Returns a list of tasks from given date
        /// </summary>
        /// <param name="date">date in DateTime format</param>
        public static IList<Task> GetTask(DateTime date)
        {
            using (var context = new TaskManagerDataBaseContext())
            {
                var query = context.Dates.Where(d => d.date == date).FirstOrDefault<Date>();

                IList<Task> tasks = query.Tasks.ToList();
                return tasks;
            }
        }
        
        /// <summary>
        /// Updates task to new task
        /// </summary>
        public static void UpdateTask(Task task_to_update, Task new_task)
        {
            using (var context = new TaskManagerDataBaseContext())
            {
               // context.Entry(task_to_update).Entity.ID = new_task.ID;
                if(new_task.TaskName!=null)context.Entry(task_to_update).Entity.TaskName = new_task.TaskName;
                context.Entry(task_to_update).Entity.Important = new_task.Important;
                if (new_task.TaskName != null) context.Entry(task_to_update).Entity.DateID = new_task.DateID;
                context.SaveChanges();
            }
        }
        
        /// <summary>
        /// Deletes task from database
        /// </summary>
        /// <param name="task">task needs to be 1:1 with the one to be deleted</param>
        public static void DeleteTask(Task task)
        {
            using (var context = new TaskManagerDataBaseContext())
            {
                context.Tasks.Remove(task);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes task from database by ID
        /// </summary>
        public static void DeleteTask(int task_id)
        {
            try
            {
                using (var context = new TaskManagerDataBaseContext())
                {
                    var query = context.Tasks.Where(t => t.ID == task_id).FirstOrDefault<Task>();
                    context.Tasks.Remove(query);
                    context.SaveChanges();
                }
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
        
        /// <summary>
        /// Adds a new Date to database using a Date object
        /// </summary>
        public static void AddNewDate(Date date)
        {
            if (GetDate(date.date) == null)
            {
                using (var context = new TaskManagerDataBaseContext())
                {
                    context.Dates.Add(date);
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Adds a new Date to database
        /// </summary>
        /// <param name="id">ID of new date</param>
        /// <param name="date">date in string format, later to be parsed into DateTime using CultureInfo.CreateSpecificCulture("de-DE")</param>
        public static void AddNewDate(int id, string date)
        {
            if (GetDate(date) == null)
            {
                if (date != null)
                {
                    try
                    {
                        CultureInfo culture = CultureInfo.CreateSpecificCulture("de-DE");
                        DateTime date_t = DateTime.Parse(date, culture);
                        using (var context = new TaskManagerDataBaseContext())
                        {
                            Date date_formated = new Date { ID = id, date = date_t };
                            context.Dates.Add(date_formated);
                            context.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves a Date by date 
        /// </summary>
        /// <param name="date">needs to be in DateTime format</param>
        public static Date GetDate(DateTime date)
        {
            using (var context = new TaskManagerDataBaseContext())
            {
                var query = context.Dates.Where(d => d.date == date).FirstOrDefault<Date>();
                return query;
            }
        }

        /// <summary>
        /// Retrieves a Date by date 
        /// </summary>
        /// <param name="date">date in string format, later to be parsed into DateTime using CultureInfo.CreateSpecificCulture("de-DE")</param>
        public static Date GetDate(string date)
        {
            try
            {
                if (date != null)
                {
                    CultureInfo culture = CultureInfo.CreateSpecificCulture("de-DE");
                    DateTime date_t = DateTime.Parse(date, culture);
                    using (var context = new TaskManagerDataBaseContext())
                    {
                        var query = context.Dates.Where(d => d.date == date_t).FirstOrDefault<Date>();
                        return query;
                    }
                }
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Retrieves a Date by ID
        /// </summary>
        public static Date GetDate(int DateID)
        {
            try
            {
                using (var context = new TaskManagerDataBaseContext())
                {
                    var query = context.Dates.Where(d => d.ID == DateID).FirstOrDefault<Date>();
                    return query;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }
        
        /// <summary>
        /// Updates Date in database
        /// </summary>
        public static void UpdateDate(Date date_to_update, Date new_date)
        {
            using (var context = new TaskManagerDataBaseContext())
            {
                //context.Entry(date_to_update).Entity.ID = new_date.ID;
                context.Entry(date_to_update).Entity.date = new_date.date;
                context.Entry(date_to_update).Entity.Tasks = new_date.Tasks;
                context.SaveChanges();
            }
        }
        
        /// <summary>
        /// Deletes Date from Database by object
        /// </summary>
        /// <param name="date">needs to be an exact copy</param>
        public static void DeleteDate(Date date)
        {
            using (var context = new TaskManagerDataBaseContext())
            {
                context.Dates.Remove(date);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes Date from Database by ID
        /// </summary>
        public static void DeleteDate(int date_id)
        {
            using (var context = new TaskManagerDataBaseContext())
            {
                var query = context.Dates.Where(d => d.ID == date_id).FirstOrDefault<Date>();
                context.Dates.Remove(query);
                context.SaveChanges();
            }
        }
    }

    /// <summary>
    /// Class initializing the database, also used for seeding
    /// </summary>
    public class TaskManagerDbInitializer : DropCreateDatabaseAlways<TaskManagerDataBaseContext>
    {

        /// <summary>
        /// The seed method
        /// </summary>
        protected override void Seed(TaskManagerDataBaseContext context)
        {
            var dates = new List<Date>
                {
                    new Date() { ID=10001, date=new DateTime(2022, 1, 1)},
                    new Date() { ID=10002, date=new DateTime(2022, 2, 2)},
                    new Date() { ID=10003, date=new DateTime(2022, 3, 3)},
                    new Date() { ID=10004, date=new DateTime(2022, 4, 4)},
                };

            dates.ForEach(d => context.Dates.Add(d));
            context.SaveChanges();

            List<Date> datesDb = context.Dates.ToList();

            var tasks = new List<Task>
                {
                    new Task()
                    {
                        ID=1001,
                        TaskName="Take out the trash",
                        Important = false,
                        DateID = datesDb[0].ID,
                        //date = DatesDb[0],
                    },
                    new Task()
                    {
                        ID=1002,
                        TaskName="Dentist",
                        Important = true,
                        DateID = datesDb[1].ID,
                        //date = DatesDb[1],
                    },
                    new Task()
                    {
                        ID=1003,
                        TaskName="Final exams 1",
                        Important= true,
                        DateID = datesDb[2].ID,
                        //date = DatesDb[2],
                    },
                    new Task()
                    {
                        ID=1003,
                        TaskName="Final exams 2",
                        Important= true,
                        DateID = datesDb[1].ID,
                        //date = DatesDb[2],
                    },
                    new Task()
                    {
                        ID=1003,
                        TaskName="Final exams 3",
                        Important= true,
                        DateID = datesDb[0].ID,
                        //date = DatesDb[2],
                    },
                    new Task()
                    {
                        ID=1003,
                        TaskName="Final exams 4",
                        Important= true,
                        DateID = datesDb[3].ID,
                        //date = DatesDb[2],
                    },
                    new Task()
                    {
                        ID=1003,
                        TaskName="Making More",
                        Important= true,
                        DateID = datesDb[0].ID,
                    },
                    new Task()
                    {
                        ID=1003,
                        TaskName="Making More and more",
                        Important= true,
                        DateID = datesDb[2].ID,
                    },

                };
            tasks.ForEach(t => context.Tasks.Add(t));
            context.SaveChanges();
        }
        
    }

}
