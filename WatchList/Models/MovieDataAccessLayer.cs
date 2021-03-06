﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WatchList.Models
{
    public class MovieDataAccessLayer
    {
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=F:\\ISD\\WatchList\\App_Data\\DatabaseSQL.mdf;Integrated Security=True;Connect Timeout=30";

        //To View all titles details
        public IEnumerable<Movie> GetAllTitles()
        {
            try
            {
                List<Movie> titles = new List<Movie>();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("spGetAllTitles", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Movie elem = new Movie();

                        elem.ID = Convert.ToInt32(rdr["Id"]);
                        elem.Name = rdr["Name"].ToString();
                        elem.Director = rdr["Director"].ToString();
                        elem.Status = Convert.ToInt32(rdr["Status"]);
                        elem.Score = Convert.ToInt32(rdr["Score"]);
                        elem.IMDBU = rdr["IMDBU"].ToString();
                        elem.IMDBR = rdr["IMDBR"].ToString();
                        elem.IMGU = rdr["IMGU"].ToString();
                        elem.TitleType = Convert.ToInt32(rdr["TitleType"]);
                        elem.WatchProgress = Convert.ToInt32(rdr["WatchProgress"]);
                        elem.Episodes = Convert.ToInt32(rdr["Episodes"]);
                        elem.Notes = rdr["Notes"].ToString();

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

        public IEnumerable<Movie> GetResultsBy(string name)
        {
            List<Movie> resultsTL = new List<Movie>();
            var url = "https://www.imdb.com/find?ref_=nv_sr_fn&q=" + name + "&s=all/";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var links = doc.DocumentNode.SelectNodes("//div[@class = 'findSection']");
            int t = 0;
            var nodeTable = links[0].ChildNodes["table"];

            foreach (var node in nodeTable.ChildNodes)
            {
                if (node.Name == "tr")
                {
                    resultsTL.Add(new Movie());
                    foreach (var nodetd in node.ChildNodes)
                    {
                        if (nodetd.Name == "td" && (nodetd.OuterHtml.IndexOf("result_text") != -1 || nodetd.OuterHtml.IndexOf("primary_photo") != -1))
                        {

                            if (nodetd.OuterHtml.IndexOf("result_text") != -1)
                            {
                                var film = node.SelectSingleNode(".//a").Attributes["href"].Value;
                                resultsTL[t].IMDBU = "https://www.imdb.com" + film;
                                var resultWebdoc = web.Load(resultsTL[t].IMDBU);
                                resultsTL[t].Name = NodeLink(resultWebdoc, "//div[@class = 'title_wrapper']", ".//h1");
                                resultsTL[t].Director = NodeLink(resultWebdoc, "//div[@class = 'credit_summary_item']", " ");
                                resultsTL[t].Notes = NodeLink(resultWebdoc, "//div[@class = 'summary_text']", " ");
                                resultsTL[t].IMDBR = NodeLink(resultWebdoc, "//div[@class = 'ratingValue']", " ");

                                var photoPoste = resultWebdoc.DocumentNode.SelectNodes("//div[@class = 'poster']");
                                resultsTL[t].IMGUP = photoPoste == null ? " " : photoPoste[0].SelectSingleNode(".//img").Attributes["src"].Value;
                                resultsTL[t].IMGUP = resultsTL[t].IMGUP == null ? " " : resultsTL[t].IMGUP;
                                resultsTL[t].IMGU = resultsTL[t].IMGU == null ? " " : resultsTL[t].IMGU;
                            }
                            if (nodetd.OuterHtml.IndexOf("primary_photo") != -1)
                            {
                                var photo = nodetd.SelectSingleNode(".//img").Attributes["src"].Value;
                                resultsTL[t].IMGU = photo == null ? " " : photo;
                            }
                        }
                    }
                    ++t;
                }

            }
            return resultsTL;
        }

        private string NodeLink(HtmlDocument resultWebdoc, string node, string node1)
        {
            string res;
            var link = resultWebdoc.DocumentNode.SelectNodes(node);
            if (node1 != " ")
                res = link[0].SelectSingleNode(node1).InnerText;
            else
                res = link == null ? " " : link[0].InnerText;
            res = res.Replace("\n", " ").Trim();
            res = res.Replace("&nbsp;", " ").Trim();
            return res;
        }


        public int AddTitle(Movie title)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("spAddTitle", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Name", title.Name);
                    cmd.Parameters.AddWithValue("@Director", title.Director);
                    cmd.Parameters.AddWithValue("@IMDBU", title.IMDBU);
                    cmd.Parameters.AddWithValue("@IMDBR", title.IMDBR);
                    cmd.Parameters.AddWithValue("@Status", title.Status);
                    cmd.Parameters.AddWithValue("@Score", title.Score);
                    cmd.Parameters.AddWithValue("@IMGU", title.IMGU);
                    cmd.Parameters.AddWithValue("@TitleType", title.TitleType);
                    cmd.Parameters.AddWithValue("@WatchProgress", title.WatchProgress);
                    cmd.Parameters.AddWithValue("@Episodes", title.Episodes);
                    cmd.Parameters.AddWithValue("@Notes", title.Notes);

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

        public int AddTitle(string imdbu)
        {
            Movie temp = new Movie();

            temp.IMDBU = imdbu;

            var web = new HtmlWeb();
            var doc = web.Load(imdbu);

            var resultWebdoc = web.Load(imdbu);
            temp.Name = NodeLink(resultWebdoc, "//div[@class = 'title_wrapper']", ".//h1");
            temp.Director = NodeLink(resultWebdoc, "//div[@class = 'credit_summary_item']", " ");
            temp.IMDBR = NodeLink(resultWebdoc, "//div[@class = 'ratingValue']", " ");
            temp.Notes = NodeLink(resultWebdoc, "//div[@class = 'summary_text']", " ");
            temp.IMGU = " ";
            AddTitle(temp);

            return 1;
        }

        public int UpdateTitle(Movie title)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("spUpdateTitleList", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    
                    cmd.Parameters.AddWithValue("@Id", title.ID);
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


        public Movie GetTitleData(int id)
        {
            try
            {
                Movie elem = new Movie();

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
                        elem.Status = Convert.ToInt32(rdr["Status"]);
                        elem.Score = Convert.ToInt32(rdr["Score"]);
                        elem.IMDBU = rdr["IMDBU"].ToString();
                        elem.IMDBR = rdr["IMDBR"].ToString();
                        elem.IMGU = rdr["IMGU"].ToString();
                        elem.TitleType = Convert.ToInt32(rdr["TitleType"]);
                        elem.WatchProgress = Convert.ToInt32(rdr["WatchProgress"]);
                        elem.Episodes = Convert.ToInt32(rdr["Episodes"]);
                        elem.Notes = rdr["Notes"].ToString();

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