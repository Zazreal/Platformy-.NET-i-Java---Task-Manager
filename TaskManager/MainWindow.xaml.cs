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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using System.ComponentModel;
using System.Net;
using Newtonsoft.Json;


namespace TaskManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

        /// <summary>
        /// Holds the Max ID of all Tasks in the current instance
        /// </summary>
        public int TaskIDs = 0;

        /// <summary>
        /// Holds the Max ID of all Dates in the current instance
        /// </summary>
        public int DateIDs = 11000;


        /// <summary>
        /// The function resposible for managing the main window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            IList<Date> dates;
            IList<Task> tasks;

            using (var context = new TaskManagerDataBaseContext())
            {
                dates = context.Dates.ToList();
                tasks = context.Tasks.ToList();   
            }
            foreach(var task in tasks)
            {
                if(task.TaskName == null)
                {
                    task.TaskName = "empty";
                }
            }

            TaskListView.ItemsSource = tasks;

            Sortby.ItemsSource = new string[] { "Name", "Date", "Importance" };
            SortDir.ItemsSource = Enum.GetNames(typeof(ListSortDirection));
            SelectDay.ItemsSource = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday","Friday", "Saturday", "Sunday"};
            
            SelectDay.SelectionChanged += SelectDay_SelectionChanged;
            Sortby.SelectionChanged += SelectionChanged;
            SortDir.SelectionChanged += SelectionChanged;


            TaskListView.Items.SortDescriptions.Add(new SortDescription("TaskName", ListSortDirection.Ascending));

            Console.WriteLine(tasks);
            if(tasks.Count > 0) TaskIDs = tasks.Max(t => t.ID);
            if(dates.Count > 0) DateIDs = dates.Max(d => d.ID);
            GetForecast();

        }

        /// <summary>
        /// Handler for Add button, opens the TaskInputWindow
        /// </summary>
        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            TaskIDs++;
            DateIDs++;

            TaskInputWindow InputWindow = new TaskInputWindow(TaskIDs, DateIDs);
            InputWindow.Owner = this;
            InputWindow.Show();

            RefreshList();
            
        }
        
        /// <summary>
        /// Handler for Remove button, removes a task from the database
        /// </summary>
        private void RemoveTask_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListView.SelectedItem != null)
            {
                //Task del = (TaskListBox.SelectedItem as Task).ID;
                TaskManagerDataBaseContext.DeleteTask((TaskListView.SelectedItem as Task).ID);
            }
            else
                MessageBox.Show("Item not selected");
            // MessageBox.Show((TaskList.SelectedItem as Task).date.date.ToString());
            RefreshList();
        }

        /*
        /// <summary>
        /// Handler for Edit button, edits a task from the database
        /// </summary>
        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            if(TaskListView.SelectedItem != null)
            {
                Task task = (Task)TaskListView.SelectedItem;
                int TID = task.ID;
                int DID = task.DateID;
                TaskInputWindow InputWindow = new TaskInputWindow(TID, DID);
                InputWindow.Owner = this;
                InputWindow.Show();
            }

            RefreshList();
        }
        */

        /// <summary>
        /// Refreshes the list
        /// </summary>
        public void RefreshList()
        {
            IList<Task> tasks;
            using (var context = new TaskManagerDataBaseContext())
            {
                tasks = context.Tasks.ToList();

            }
            foreach (var task in tasks)
            {
                if (task.TaskName == null)
                {
                    task.TaskName = "empty";
                }
            }
            TaskListView.ItemsSource = tasks;
        }

        /// <summary>
        /// Sorts the list according to what is selected in the combo boxes
        /// </summary>
        public void SortList()
        {
            var SortProperty = Sortby.SelectedItem.ToString();

            if (SortProperty == "Name") SortProperty = "TaskName";
            else if (SortProperty == "Date") SortProperty = "Date_str";
            else if (SortProperty == "Importance") SortProperty = "Important";
            else SortProperty = "TaskName";

            var SortDirection = SortDir.SelectedItem.ToString() == "Ascending" ? ListSortDirection.Ascending : ListSortDirection.Descending;
            TaskListView.Items.SortDescriptions[0] = new SortDescription(SortProperty, SortDirection);
        }

        /// <summary>
        /// Handler for the 2 combo boxes used to sort the list
        /// </summary>
        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortList();
        }


        /// <summary>
        /// Handler for the Refresh button, basically just calls the RefreshList() method
        /// </summary>
        private void RefreshLIst_Click(object sender, RoutedEventArgs e)
        {
            RefreshList();
        }

        /// <summary>
        /// Converts a long to a DateTime format
        /// </summary>
        private DateTime ConvertDayTime(long dt)
        {
            DateTime day = new DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc).ToLocalTime();
            day = day.AddSeconds(dt).ToLocalTime();
            return day;
        }


        /// <summary>
        /// APIKey used to get access to the OpenWeatherMapAPI
        /// </summary>
        string APIKey = "613e7479390c5e6aeb2ecbe31941912e";

        /// <summary>
        /// Logic for the weather forecast combobox
        /// </summary>
        private void GetForecast()
        {
            var SelectedDay = SelectDay.SelectedItem.ToString();

            using(WebClient web = new WebClient())
            {
                string url = string.Format("https://api.openweathermap.org/data/2.5/onecall?lat=51.1079&lon=17.0385&exclude=current,hourly,minutely,alerts&units=metric&appid={0}", APIKey);
                //url = string.Format("https://api.openweathermap.org/data/2.5/onecall?lat=51.1079&lon=17.0385&exclude=current,hourly,minutely,alerts&units=metric&appid=613e7479390c5e6aeb2ecbe31941912e");
                var json = web.DownloadString(url);

                ForecastAPI.ForecastInfo Info = JsonConvert.DeserializeObject<ForecastAPI.ForecastInfo>(json);
                
                for(int i = 0; i < Info.daily.Count; i++)
                {
                    if (ConvertDayTime(Info.daily[i].dt).DayOfWeek.ToString() == SelectedDay)
                    {
                        try
                        {
                            var ImageSource = new ImageSourceConverter().ConvertFromString("https://openweathermap.org/img/w/" + Info.daily[i].weather[0].icon + ".png") as ImageSource;
                            WheatherImage.Source = ImageSource;
                            WeatherText.Text = Info.daily[i].weather[0].main;
                            TempText.Text = string.Format("{0}°C", Info.daily[i].temp.day);
                        }
                        catch (Exception ex) 
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                }

            }
        }


        /// <summary>
        /// Handler for the forecast combobox, basically just calls the GetForecast() method
        /// </summary>
        private void SelectDay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetForecast();
        }
    }
}



