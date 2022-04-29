using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager
{

    /// <summary>
    /// Class holding and handling the data from the API, go to ForecastAPI.ForecastInfo for more information
    /// </summary>
    public class ForecastAPI
    {

        /// <summary>
        /// This is the main class that is meant to be used, the rest is just to help organize the data
        /// </summary>
        public class ForecastInfo
        {
            public List<daily> daily { get; set; }
        }

        public class daily
        {
            public long dt { get; set; }
            public temp temp { get; set; }
            public List<weather> weather { get; set; }
        }
        public class temp
        {
            public double day { get; set; }
        }

        public class weather
        {
            public string main { get; set; }
            public string description { get; set; }
            public string icon { get; set; }
        }

    }
}
