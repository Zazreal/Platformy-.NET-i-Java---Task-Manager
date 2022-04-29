using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for TaskInputWindow.xaml
    /// </summary>
    public partial class TaskInputWindow : Window
    {
        /// <summary>
        /// Holds the current Max ID of tasks in the database
        /// </summary>
        int MaxTaskID = 0;

        /// <summary>
        /// Holds the current Max ID of dates in the database
        /// </summary>
        int MaxDateID = 0;

        /// <summary>
        /// The function resposible for managing the task input window
        /// </summary>
        /// <param name="DateIDs">Current Max Date ID</param>
        /// <param name="TaskIDs">Current Max Task ID</param>
        /// <param name="IsEdit">Scrapped</param>
        public TaskInputWindow(int TaskIDs, int DateIDs, bool IsEdit = false)
        {
            InitializeComponent();
            
            MaxTaskID = TaskIDs;
            MaxDateID = DateIDs;
            /*
            if (IsEdit)
            {
                Task task = TaskManagerDataBaseContext.GetTask(TaskIDs);

                /*
                TaskNameText.Text = task.TaskName;
                ImportanceText.Text = task.Important.ToString();
                TaskDateText.Text = task.Date_str;
                TaskTimeText.Text = null;
                
                bool im = false;
                try
                {
                     im = Boolean.Parse(ImportanceText.Text);
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Date date = TaskManagerDataBaseContext.GetDate(TaskDateText.Text);
                if (date == null)
                {
                    TaskManagerDataBaseContext.AddNewDate(MaxDateID, TaskDateText.Text + " " + TaskTimeText.Text);
                }
                else
                {
                    MaxDateID = date.ID;
                }
                Task task_new = new Task { ID = MaxTaskID, TaskName = TaskNameText.Text, Important = im, DateID = DateIDs };
                TaskManagerDataBaseContext.UpdateTask(task, task_new);
                //TaskManagerDataBaseContext.AddNewTask(MaxTaskID, TaskNameText.Text, ImportanceText.Text, MaxDateID);
                this.Close();
            }
            */
            
        
        }


        /// <summary>
        /// Handler for the Add button, adds a task to the database and closes the window
        /// </summary>
        private void InputWindow_AddTask_Click(object sender, RoutedEventArgs e)
        {
            if (ImportanceText.Text != null && TaskDateText.Text != null && TaskTimeText.Text != null)
            {
                //MessageBox.Show(date_t.ToString());
                Date date = TaskManagerDataBaseContext.GetDate(TaskDateText.Text);
                if (date == null)
                {
                    TaskManagerDataBaseContext.AddNewDate(MaxDateID, TaskDateText.Text + " " + TaskTimeText.Text);
                }
                else
                {
                    MaxDateID = date.ID;
                }
                TaskManagerDataBaseContext.AddNewTask(MaxTaskID, TaskNameText.Text, ImportanceText.Text, MaxDateID);

            }
            else
            {
                MessageBox.Show("At least one of the values is empty");
            }
            ImportanceText.Text = null;
            TaskNameText.Text = null;
            TaskDateText.Text = null;
            TaskTimeText.Text = null;
            this.Close();
        }
    }
}
