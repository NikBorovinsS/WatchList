using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using WatchList.Models;

namespace WatchList.Controllers
{
    public class MovieController : Controller
    {
        MovieDataAccessLayer objtitle = new MovieDataAccessLayer();

        [HttpGet("[action]")]
        [Route("api/WatchList/Index")]
        public IEnumerable<Movie> Index()
        {
            return objtitle.GetAllTitles();
        }

        [HttpPost]
        [Route("api/WatchList/Create")]
        public int Create([FromBody] Movie title)
        {
            return objtitle.AddTitle(title);
        }

        [HttpGet]
        [Route("api/WatchList/Details/{id}")]
        public Movie Details(int id)
        {
            return objtitle.GetTitleData(id);
        }

        [HttpPut]
        [Route("api/WatchList/Edit")]
        public int Edit([FromBody]Movie title)
        {
            return objtitle.UpdateTitle(title);
        }

        [HttpDelete]
        [Route("api/WatchList/Delete/{id}")]
        public int Delete(int id)
        {
            return objtitle.DeleteTitle(id);
        }

        [HttpGet]
        [Route("api/WatchList/ResultsBy/{name}")]
        public IEnumerable<Movie> ResultsBy(string name)
        {
            return objtitle.GetResultsBy(name);
        }

        [HttpPost]
        [Route("api/WatchList/AddTitleFrom")]
        public int AddTitleFrom([FromBody] Movie title)
        {
            return objtitle.AddTitle(title.IMDBU);
        }
    }
}
