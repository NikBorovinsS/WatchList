using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WatchList.Models
{
    public class TitleListDataAccessLayer
    {
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=F:\\ISD\\WatchList\\App_Data\\DatabaseSQL.mdf;Integrated Security=True;Connect Timeout=30";

        //To View all titles details
        public IEnumerable<TitleList> GetAllTitles()
        {
            try
            {
                  List<TitleList> titles = new List<TitleList>();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("spGetAllTitles", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        TitleList elem = new TitleList();

                        elem.ID = Convert.ToInt32(rdr["Id"]);
                        elem.Name = rdr["Name"].ToString();
                        elem.Director = rdr["Director"].ToString();
                        elem.Description = rdr["Description"].ToString();
                        elem.Status = Convert.ToInt32(rdr["Status"]);
                        elem.Score = Convert.ToInt32(rdr["Score"]);
                        elem.IMDBU = rdr["IMDBU"].ToString();
                        elem.IMDBR = rdr["IMDBR"].ToString();

                        titles.Add(elem);
                    }
                    con.Close();
                }
                return titles;
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<TitleList> GetResultsBy(string name)
        {
            List<TitleList> resultsTL = new List<TitleList>();
            var url = "https://www.imdb.com/find?ref_=nv_sr_fn&q="+ name + "&s=all/";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var links = doc.DocumentNode.SelectNodes("//div[@class = 'findSection']");//the parameter is use xpath see: https://www.w3schools.com/xml/xml_xpath.asp

            resultsTL.Add(new TitleList());
            resultsTL[0].IMDBU = "https://www.imdb.com" + links[0].OuterHtml;

            return null;
        }

        public int AddTitle(TitleList title)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("spAddTitle", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Name", title.Name);
                    cmd.Parameters.AddWithValue("@Director", title.Director);
                    cmd.Parameters.AddWithValue("@Description", title.Description);
                    cmd.Parameters.AddWithValue("@IMDBU", title.IMDBU);
                    cmd.Parameters.AddWithValue("@IMDBR", title.IMDBR);
                    cmd.Parameters.AddWithValue("@Status", title.Status);
                    cmd.Parameters.AddWithValue("@Score", title.Score);
 
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return 1;
            }
            catch
            {
                throw;
            }
        }

        public int UpdateTitle(TitleList title)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("spUpdateTitleList", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Name", title.Name);
                    cmd.Parameters.AddWithValue("@Id", title.ID);
                    cmd.Parameters.AddWithValue("@Director", title.Director);
                    cmd.Parameters.AddWithValue("@Description", title.Description);
                    cmd.Parameters.AddWithValue("@IMDBU", title.IMDBU);
                    cmd.Parameters.AddWithValue("@IMDBR", title.IMDBR);
                    cmd.Parameters.AddWithValue("@Status", title.Status);
                    cmd.Parameters.AddWithValue("@Score", title.Score);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return 1;
            }
            catch
            {
                throw;
            }
        }

        public TitleList GetTitleData(int id)
        {
            try
            {
                TitleList elem = new TitleList();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string sqlQuery = "SELECT * FROM TitlesList WHERE Id= " + id;
                    SqlCommand cmd = new SqlCommand(sqlQuery, con);

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        elem.ID = Convert.ToInt32(rdr["Id"]);
                        elem.Name = rdr["Name"].ToString();
                        elem.Director = rdr["Director"].ToString();
                        elem.Description = rdr["Description"].ToString();
                        elem.Status = Convert.ToInt32(rdr["Status"]);
                        elem.Score = Convert.ToInt32(rdr["Score"]);
                        elem.IMDBU = rdr["IMDBU"].ToString();
                        elem.IMDBR = rdr["IMDBR"].ToString();
                        
                    }
                }
                return elem;
            }
            catch
            {
                throw;
            }
        }

        public int DeleteTitle(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("spDeleteTitles", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", id);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return 1;
            }
            catch
            {
                throw;
            }
        }
    }
}
