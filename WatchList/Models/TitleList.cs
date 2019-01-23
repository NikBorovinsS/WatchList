using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WatchList.Models
{
    public class TitleList
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Director { get; set; }

        public string Description { get; set; }

        public int Status { get; set; }

        public int Score { get; set; }

        public string IMDBU { get; set; }
        
        public string IMDBR { get; set; }
        
    }
}
