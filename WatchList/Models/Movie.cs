using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WatchList.Models
{
    public class Movie
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Director { get; set; }

        public int Status { get; set; }

        public int Score { get; set; }

        public string IMDBU { get; set; }
        
        public string IMDBR { get; set; }

        public string IMGU { get; set; }

        public string IMGUP { get; set; }

        public int TitleType { get; set; }

        public int WatchProgress { get; set; }

        public string Notes { get; set; }

        public int Episodes { get; set; }

    }
}
